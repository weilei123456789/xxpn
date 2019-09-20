using URG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class UrgComponent : GameFrameworkComponent
    {
        class DetectObject
        {
            public List<long> distList;
            public List<int> idList;

            public long startDist;

            public DetectObject()
            {
                distList = new List<long>();
                idList = new List<int>();
            }
        }

        [SerializeField]
        private UrgDeviceEthernet m_UrgDevice;
        [SerializeField]
        private string m_IpAddress = "192.168.0.10";
        [SerializeField]
        private int m_Port = 10940;


        [SerializeField]
        private float m_Scale = 0.1f;
        [SerializeField]
        private float m_Limit = 300.0f;//mm
        [SerializeField]
        private int m_NoiseLimit = 5;
        [SerializeField]
        private Color m_DistanceColor = Color.white;
        [SerializeField]
        private Color m_StrengthColor = Color.white;
        [SerializeField]
        private Color[] m_GroupColors;
        [SerializeField]
        private Rect m_AreaRect;
        [SerializeField]
        private bool m_DebugDraw = true;

        private List<long> m_Distances = new List<long>();
        private List<long> m_Strengths = new List<long>();
        private List<DetectObject> m_DetectObjects = new List<DetectObject>();
        private Vector3[] m_Directions;
        private bool m_Cached = false;
        private int m_DrawCount;
        // 屏幕的实际尺寸-宽
        private int m_ResolutionWidth = 0;
        // 屏幕的实际尺寸-高
        private int m_ResolutionHeight = 0;
        // 屏幕的实际尺寸-偏移宽
        private int m_ResolutionOffsetWidth = 0;
        // 屏幕的实际尺寸-偏移搞
        private int m_ResolutionOffsetHeight = 0;

        private List<Vector3> m_DebugLineVector3 = new List<Vector3>();
        private List<UrgGUIData> m_UrgGUI = new List<UrgGUIData>();

        public int ResolutionWidth
        {
            get
            {
                return m_ResolutionWidth;
            }
        }

        public int ResolutionHeight
        {
            get
            {
                return m_ResolutionHeight;
            }
        }

        public List<Vector3> DebugLineVector3
        {
            get
            {
                return m_DebugLineVector3;
            }
        }

        public List<UrgGUIData> UrgGUI
        {
            get
            {
                return m_UrgGUI;
            }
        }

        public Vector3 OriginalOffset
        {
            get
            {
                return new Vector3(m_ResolutionOffsetWidth, m_ResolutionOffsetHeight);
            }
        }

        // 是否绘制
        public bool DebugDraw
        {
            set
            {
                m_DebugDraw = value;
            }
            get
            {
                return m_DebugDraw;
            }
        }
        // IP地址
        public string IpAddress
        {
            set
            {
                m_IpAddress = value;
            }
            get
            {
                return m_IpAddress;
            }
        }
        // 缩放比
        public float Scale
        {
            set
            {
                m_Scale = value;
            }
            get
            {
                return m_Scale;
            }
        }
        // 长度限制
        public float Limit
        {
            set
            {
                m_Limit = value;
            }
            get
            {
                return m_Limit;
            }
        }
        // 屏幕的实际尺寸-偏移宽
        public int ResolutionOffsetWidth
        {
            set
            {
                m_ResolutionOffsetWidth = value;
            }
            get
            {
                return m_ResolutionOffsetWidth;
            }
        }
        // 屏幕的实际尺寸-偏移搞
        public int ResolutionOffsetHeight
        {
            set
            {
                m_ResolutionOffsetHeight = value;
            }
            get
            {
                return m_ResolutionOffsetHeight;
            }
        }

        private void Update()
        {
            // Draw Area
            DrawRect();
            // Calc Urg
            CalcUrgPosition();
        }

        public void StartUrgEthernet(string ipAddress = "192.168.0.10", int port = 10940, int offsetWidth = 0, int offsetHeight = 20, float disScale = 0.34f, int displayId = 0)
        {
            m_IpAddress = ipAddress;
            m_Port = port;
            m_Scale = disScale;
            m_ResolutionOffsetWidth = offsetWidth;
            m_ResolutionOffsetHeight = offsetHeight;

            m_ResolutionWidth = Display.displays[displayId].systemWidth;
            m_ResolutionHeight = Display.displays[displayId].systemHeight;

            m_UrgDevice.StartTCP(m_IpAddress, m_Port);
            m_AreaRect = new Rect(0, 0, m_ResolutionWidth, m_ResolutionHeight);
            StartCoroutine(WaitOneSecondStrat());
        }

        private IEnumerator WaitOneSecondStrat()
        {
            yield return new WaitForSeconds(1);
            m_UrgDevice.Write(URG.SCIP_Writer.MD(0, 1080, 1, 0, 0));
        }

        private void CalcUrgPosition()
        {
            m_DebugLineVector3.Clear();
            m_UrgGUI.Clear();

            if (m_UrgDevice == null) return;

            // cache directions
            if (m_UrgDevice.distances.Count > 0)
            {
                if (!m_Cached)
                {
                    float d = Mathf.PI * 2 / 1440;
                    float offset = d * 540;

                    m_Directions = new Vector3[m_UrgDevice.distances.Count];
                    for (int i = 0; i < m_Directions.Length; i++)
                    {
                        float a = d * i + offset;
                        //directions[i] = new Vector3(-Mathf.Cos(a), -Mathf.Sin(a), 0);
                        m_Directions[i] = new Vector3(Mathf.Cos(a), Mathf.Sin(a), 0);
                    }
                    m_Cached = true;
                }
            }

            // strengths
            try
            {
                if (m_UrgDevice.strengths.Count > 0)
                {
                    m_Strengths.Clear();
                    m_Strengths.AddRange(m_UrgDevice.strengths);
                }
            }
            catch
            {
            }
            // distances
            try
            {
                if (m_UrgDevice.distances.Count > 0)
                {
                    m_Distances.Clear();
                    m_Distances.AddRange(m_UrgDevice.distances);
                }
            }
            catch
            {
            }
            if (m_DebugDraw)
            {
                //// strengths
                //for (int i = 0; i < m_Strengths.Count; i++)
                //{
                //    Vector3 dir = m_Directions[i];
                //    long dist = m_Strengths[i];
                //    //Debug.DrawRay(Vector3.zero, Mathf.Abs(dist) * dir * m_Scale, m_StrengthColor);
                //}

                for (int i = 0; i < m_Distances.Count; i++)
                {
                    Vector3 dir = m_Directions[i];
                    long dist = m_Distances[i];

                    Vector3 rayend = dist * dir * m_Scale;
                    //Debug.DrawRay(Vector3.zero, rayend, m_DistanceColor);


                    m_UrgGUI.Add(new UrgGUIData()
                    {
                        start_pos = new Vector3(m_ResolutionWidth / 2, m_ResolutionHeight),
                        end_pos = rayend + new Vector3(m_ResolutionWidth / 2, m_ResolutionHeight) + OriginalOffset,
                        color = m_DistanceColor,
                    });
                }
            }

            //  group
            m_DetectObjects.Clear();
            //------
            bool endGroup = true;
            float deltaLimit = 100; // 识别阈值仅获取连续值(mm)
            for (int i = 1; i < m_Distances.Count - 1; i++)
            {
                Vector3 dir = m_Directions[i];
                long dist = m_Distances[i];
                float delta = Mathf.Abs((float)(m_Distances[i] - m_Distances[i - 1]));
                float delta1 = Mathf.Abs((float)(m_Distances[i + 1] - m_Distances[i]));

                if (dir.y < 0)
                {
                    DetectObject detect;
                    if (endGroup)
                    {
                        //Vector3 pt = dist * dir * m_Scale;
                        if (dist < m_Limit && (delta < deltaLimit && delta1 < deltaLimit))
                        {
                            //bool isArea = detectAreaRect.Contains(pt);
                            //if(isArea && (delta < deltaLimit && delta1 < deltaLimit)){
                            detect = new DetectObject();
                            detect.idList.Add(i);
                            detect.distList.Add(dist);

                            detect.startDist = dist;
                            m_DetectObjects.Add(detect);

                            endGroup = false;
                        }
                    }
                    else
                    {
                        if (delta1 >= deltaLimit || delta >= deltaLimit)
                        {
                            endGroup = true;
                        }
                        else
                        {
                            detect = m_DetectObjects[m_DetectObjects.Count - 1];
                            detect.idList.Add(i);
                            detect.distList.Add(dist);
                        }
                    }
                }
            }

            //-----------------
            // draw 
            m_DrawCount = 0;
            for (int i = 0; i < m_DetectObjects.Count; i++)
            {
                DetectObject detect = m_DetectObjects[i];

                // noise
                if (detect.idList.Count < m_NoiseLimit)
                {
                    continue;
                }

                int offsetCount = detect.idList.Count / 3;
                int avgId = 0;
                for (int n = 0; n < detect.idList.Count; n++)
                {
                    avgId += detect.idList[n];
                }
                avgId = avgId / (detect.idList.Count);

                long avgDist = 0;
                for (int n = offsetCount; n < detect.distList.Count - offsetCount; n++)
                {
                    avgDist += detect.distList[n];
                }
                avgDist = avgDist / (detect.distList.Count - offsetCount * 2);

                Vector3 dir = m_Directions[avgId];
                long dist = avgDist;

                if (m_DebugDraw)
                {
                    int id0 = detect.idList[offsetCount];
                    Vector3 dir0 = m_Directions[id0];
                    long dist0 = detect.distList[offsetCount];

                    int id1 = detect.idList[detect.idList.Count - 1 - offsetCount];
                    Vector3 dir1 = m_Directions[id1];
                    long dist1 = detect.distList[detect.distList.Count - 1 - offsetCount];

                    Color gColor;
                    if (m_DrawCount < m_GroupColors.Length)
                    {
                        gColor = m_GroupColors[m_DrawCount];
                    }
                    else
                    {
                        gColor = Color.green;
                    }
                    for (int j = offsetCount; j < detect.idList.Count - offsetCount; j++)
                    {
                        int _id = detect.idList[j];
                        Vector3 _dir = m_Directions[_id];
                        long _dist = detect.distList[j];

                        Vector3 rayend = _dist * _dir * m_Scale;
                        //Debug.DrawRay(Vector3.zero, rayend, gColor);

                        m_UrgGUI.Add(new UrgGUIData()
                        {
                            start_pos = new Vector3(m_ResolutionWidth / 2, m_ResolutionHeight),
                            end_pos = rayend + new Vector3(m_ResolutionWidth / 2, m_ResolutionHeight) + OriginalOffset,
                            color = gColor,
                        });
                    }
                    Vector3 start = dist0 * dir0 * m_Scale;
                    Vector3 end = dist1 * dir1 * m_Scale;
                    //Debug.DrawLine(start, end, gColor);
                    m_UrgGUI.Add(new UrgGUIData()
                    {
                        start_pos = start + new Vector3(m_ResolutionWidth / 2, m_ResolutionHeight) + OriginalOffset,
                        end_pos = end + new Vector3(m_ResolutionWidth / 2, m_ResolutionHeight) + OriginalOffset,
                        color = gColor,
                    });
                }

                Vector3 point = dist * dir * m_Scale;
                // 筛选的中间点
                //Debug.DrawRay(Vector3.zero, point, Color.red);

                Vector3 ScreenPoint = point + new Vector3(m_ResolutionWidth / 2, m_ResolutionHeight) + OriginalOffset;
                m_DebugLineVector3.Add(ScreenPoint);

                GameEntry.Windows.WallUICameraRay(ScreenPoint);

                m_DrawCount++;
            }
        }

        private void DrawRect()
        {
            // center offset rect
            Rect detectAreaRect = m_AreaRect;
            //detectAreaRect.x *= scale;
            //detectAreaRect.y *= scale;
            //detectAreaRect.width *= scale;
            //detectAreaRect.height *= scale;
            detectAreaRect.x = -detectAreaRect.width / 2;
            detectAreaRect.y = -detectAreaRect.height;
            DrawRect(detectAreaRect, Color.green);
        }

        private void DrawRect(Rect rect, Color color)
        {
            Vector3 p0 = new Vector3(rect.x, rect.y, 0);
            Vector3 p1 = new Vector3(rect.x + rect.width, rect.y, 0);
            Vector3 p2 = new Vector3(rect.x + rect.width, rect.y + rect.height, 0);
            Vector3 p3 = new Vector3(rect.x, rect.y + rect.height, 0);
            Debug.DrawLine(p0, p1, color);
            Debug.DrawLine(p1, p2, color);
            Debug.DrawLine(p2, p3, color);
            Debug.DrawLine(p3, p0, color);
        }

    }

}