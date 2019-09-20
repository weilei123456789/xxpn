using Assets.SerialPortUtility.Interfaces;
using Assets.SerialPortUtility.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class EthernetComponent : GameFrameworkComponent
    {
        [SerializeField]
        private string m_IpAddress = string.Empty;
        [SerializeField]
        private int m_PortNumber = 0;
        [SerializeField]
        private int m_CollectionNum = 240;
        [SerializeField]
        private CanvasScaler m_CanvasScaler = null;
        [SerializeField]
        private GameObject m_LandDebugImage;
        [SerializeField]
        private Transform m_LandDebugTrs;
        // 等比缩放
        [SerializeField]
        private float m_Scale = 0.2f;
        [SerializeField]
        private bool m_IsDebug = true;

        private List<Image> m_LandDebugObjs = new List<Image>();
        private EthernetInterface[] m_EthernetA3 = null;
        private int m_LandOffsetX = 0;
        private int m_LandOffsetY = 0;
        private Vector3 _position = Vector3.zero;
        private List<SerialData> m_DoubleSerialData = new List<SerialData>();
        // 屏幕的实际尺寸-宽
        private int m_ResolutionWidth = 0;
        // 屏幕的实际尺寸-高
        private int m_ResolutionHeight = 0;
        // 屏幕的实际尺寸-偏移宽
        private int m_ResolutionOffsetWidth = 0;
        // 屏幕的实际尺寸-偏移搞
        private int m_ResolutionOffsetHeight = 0;


        private Vector3 Resolution
        {
            get
            {
                return new Vector3(m_ResolutionWidth, m_ResolutionHeight);
            }
        }

        public Vector3 OriginalOffset
        {
            get
            {
                return new Vector3(m_ResolutionOffsetWidth, m_ResolutionOffsetHeight);
            }
        }

        private void Start()
        {
            CreateDebug();
            //byte src = 239;
            //string twobit = SerialCommunicationUtility.Binary_Ten_To_Two(src);
            //byte ssrc = Convert.ToByte(twobit, 2);
            //byte right = Convert.ToByte(ssrc >> 3);
            //string rightbit = SerialCommunicationUtility.Binary_Ten_To_Two(right);
            //byte left = SerialCommunicationUtility.Binary_HexStr_To_Byte(ssrc, 3);
            //string leftbit = SerialCommunicationUtility.Binary_Ten_To_Two(left);


            //byte origin_q = Convert.ToByte(src);
            //Debug.Log(origin_q);
            //int s = SerialCommunicationUtility.GetBit(origin_q, 0);
            //int _s = SerialCommunicationUtility.GetBit(origin_q, 1);

            //byte sb = SerialCommunicationUtility.GetBit2(origin_q, 0);
            //byte _sb = SerialCommunicationUtility.GetBit2(origin_q, 1);
            //int sb_qufan = ~s;
            //sb_qufan = Math.Abs(sb_qufan);

            //int jz_16_1 = Convert.ToInt32(sb);
            //int jz_16_2 = Convert.ToInt32(_sb);

        }

        private void Update()
        {

            if (m_EthernetA3 != null)
                TcpUpdate();
        }

        private void OnDestroy()
        {
            if (m_EthernetA3 == null) return;
            for (int i = 0; i < m_EthernetA3.Length; i++)
            {
                if (m_EthernetA3[i] != null)
                {
                    m_EthernetA3[i].Write(RplidariIstruct.STOP_MOTOR);
                    m_EthernetA3[i].Write(RplidariIstruct.STOP);
                    m_EthernetA3[i].Close();
                }
            }
        }

        public void StartEthernet(int land_Width, int land_Height, string _ipAddress, int _portNumber, int offsetWidth = 0, int offsetHeight = 0, float scale = 0.2f, int displayId = 1)
        {
            m_LandOffsetX = land_Width;
            m_LandOffsetY = land_Height;
            m_CanvasScaler.referenceResolution = new Vector2(m_LandOffsetX, m_LandOffsetY);
            m_IpAddress = _ipAddress;
            m_PortNumber = _portNumber;
            m_ResolutionOffsetWidth = offsetWidth;
            m_ResolutionOffsetHeight = offsetHeight;
            m_Scale = scale;

            if (Display.displays.Length > 1)
            {
                m_ResolutionWidth = Display.displays[displayId].systemWidth;
                m_ResolutionHeight = Display.displays[displayId].systemHeight;
            }
            else
            {
                m_ResolutionWidth = m_LandOffsetX;
                m_ResolutionHeight = m_LandOffsetY;
            }
            Debug.LogError("设备尺寸: " + Resolution);

            string[] Address = _ipAddress.Split('|');

            m_EthernetA3 = new EthernetInterface[Address.Length];
            for (int i = 0; i < m_EthernetA3.Length; i++)
            {
                m_EthernetA3[i] = new EthernetInterface(Address[i], m_PortNumber, i, ConnectSuccess, ConnectFailed);
                m_EthernetA3[i].Open();
                m_EthernetA3[i].UpdateSerialData = UpdateLandData;
            }
        }

        private void ConnectSuccess(int index)
        {
            //启动电机，适用RPLidar自带的转接板！！！
            if (m_EthernetA3[index] != null)
            {
                m_EthernetA3[index].Write(RplidariIstruct.START_MOTOR);
                m_EthernetA3[index].Write(RplidariIstruct.SCAN);
                //m_EthernetA3[index].Write(RplidariIstruct.EXPRESS_SCAN);

                Log.Info("<color=green>{0}</color>", m_EthernetA3[index].IpAddress + " LiDar Connect Success!!!");

            }

        }
        private void ConnectFailed(int index)
        {
            Log.Info("<color=red>{0}</color>", m_EthernetA3[index].IpAddress + " LiDar Connect Failed!!!");
        }

        private void CreateDebug()
        {
            if (m_IsDebug)
            {
                for (int i = 0; i < m_CollectionNum; i++)
                {
                    GameObject obj = Instantiate(m_LandDebugImage, m_LandDebugTrs);
                    m_LandDebugObjs.Add(obj.GetComponent<Image>());
                }
            }
            else
            {
                m_LandDebugImage.SetActive(false);
            }

        }

        private void TcpUpdate()
        {
            if (m_EthernetA3 == null) return;
            m_DoubleSerialData.Clear();
            for (int i = 0; i < m_EthernetA3.Length; i++)
            {
                if (m_EthernetA3[i].DataQueue.Count > 5)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        m_DoubleSerialData.AddRange(m_EthernetA3[i].DataQueue.Dequeue());
                    }
                }
            }
            if (m_DoubleSerialData.Count > 0)
            {
                UpdateLandData(m_DoubleSerialData);
            }
        }

        private void UpdateLandData(List<SerialData> serialData)
        {
            if (serialData.Count < 0) { return; }

            if (m_IsDebug)
            {
                for (int i = 0; i < m_LandDebugObjs.Count; i++)
                {
                    m_LandDebugObjs[i].transform.localPosition = Vector3.zero;
                }
            }

            for (int i = serialData.Count - 1; i > 0; i--)
            {
                float angle = serialData[i].angle;
                float distance = serialData[i].distance;
                BuildObj(angle, distance, i);
            }

        }

        private void BuildObj(float angle, float distance, int i)
        {
            _position = GetPosition(angle, distance, m_Scale, m_Scale);
            if (_position.x > -m_LandOffsetX && _position.x < 0 && _position.y < 0f && _position.y > -m_LandOffsetY)
            {
                GameEntry.Windows.GroundUICameraRay(_position + Resolution + OriginalOffset);

                if (i < m_LandDebugObjs.Count && m_IsDebug)
                {
                    m_LandDebugObjs[i].transform.localPosition = _position + Resolution + OriginalOffset;
                    m_LandDebugObjs[i].transform.localScale = Vector3.one;
                    m_LandDebugObjs[i].transform.localRotation = Quaternion.identity;
                }
            }
        }

        public Vector2 GetPosition(float angle, float distance, float xScale = 1, float yScale = 1)
        {
            float rad = (angle * Mathf.PI / 180f);
            float endptX = Mathf.Sin(rad) * distance * xScale;
            float endptY = Mathf.Cos(rad) * distance * yScale;
            return new Vector2(-endptY, endptX);
        }
    }
}
