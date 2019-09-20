using Assets.SerialPortUtility.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using URG;
using System.IO;
using Assets.SerialPortUtility.Interfaces;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class SerialPortComponent : GameFrameworkComponent
    {
        [Header("----------------地屏----------------")]
        [SerializeField]
        private SerialCommunicationFacade m_Facade_Land = new SerialCommunicationFacade();
        [SerializeField]
        private LidarType m_LidarType = LidarType.RPLidarA2;
        [SerializeField]
        private string m_LandPortName = "com6";
        [SerializeField]//波特率
        [Header("115200 / 256000")]
        private int m_LandBaudRate = 115200;
        [SerializeField]
        private CanvasScaler m_CanvasScaler = null;
        // 等比缩放
        [SerializeField]
        private float m_Scale = 0.2f;

        [SerializeField]
        [Header("----------------地屏DEBUG----------------")]
        private bool m_IsLandFacadeOpen = true;
        [SerializeField]
        private GameObject m_LandDebugImage;
        [SerializeField]
        private Transform m_LandDebugTrs;
        [SerializeField]
        private List<Image> m_LandDebugObjs = new List<Image>();

        private int m_LandOffsetX = 0;
        private int m_LandOffsetY = 0;
        private float m_Rad;
        private float m_EndptX;
        private float m_EndptY;
        private Vector2 m_Vector2 = Vector2.zero;
        private Vector3 _position = Vector3.zero;
        private List<long> distances = new List<long>();

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
        }

        public void StartSerialPort(int baudRate, string serialPort, int land_Width, int land_Height, int offsetWidth = 0, int offsetHeight = 0, float scale = 0.2f, int displayId = 1)
        {
            m_LandBaudRate = baudRate;
            m_LandPortName = serialPort;

            m_LandOffsetX = land_Width;
            m_LandOffsetY = land_Height;
            m_CanvasScaler.referenceResolution = new Vector2(m_LandOffsetX, m_LandOffsetY);
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
            ConncetLandFacade();
        }

        /// <summary>
        /// 连接地屏
        /// </summary>
        private void ConncetLandFacade()
        {
            try
            {
                m_Facade_Land.Connect(m_LandBaudRate, m_LandPortName, m_LidarType);
                if (m_LidarType == LidarType.RPLidarA2 || m_LidarType == LidarType.RPLidarA3)
                {
                    //启动电机，适用RPLidar自带的转接板！！！
                    m_Facade_Land.SendMessage(RplidariIstruct.START_MOTOR);
                    m_Facade_Land.SendMessage(RplidariIstruct.SCAN);
                    //m_Facade_Land.SendMessage(RplidariIstruct.EXPRESS_SCAN);
                }
                m_Facade_Land.UpdateSerialData = UpdateLandData;
            }
            catch (Exception e)
            {
                Log.Info("<color=red>{0}</color>", "链接地屏串口失败: " + e.Message);
            }
        }


        private void CreateDebug()
        {
            if (m_IsLandFacadeOpen)
            {
                for (int i = 0; i < 150; i++)
                {
                    GameObject obj = Instantiate(m_LandDebugImage, m_LandDebugTrs);
                    m_LandDebugObjs.Add(obj.GetComponent<Image>());
                }
            }
            else
            {
                m_LandDebugImage.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (m_Facade_Land != null)
            {
                if (m_LidarType == LidarType.RPLidarA2 || m_LidarType == LidarType.RPLidarA3)
                {
                    m_Facade_Land.SendMessage(RplidariIstruct.STOP_MOTOR);
                    m_Facade_Land.SendMessage(RplidariIstruct.STOP);
                }
                m_Facade_Land.Disconnect();
            }
        }

        private void UpdateLandData(List<SerialData> serialData)
        {
            for (int i = 0; i < m_LandDebugObjs.Count; i++)
            {
                m_LandDebugObjs[i].enabled = false;
            }
            m_LandOffsetX = GameEntry.WindowsConfig.Config.Screen_Land_Width;
            m_LandOffsetY = GameEntry.WindowsConfig.Config.Screen_Land_Height;

            for (int i = 0; i < serialData.Count; i++)
            {
                BuildObj(serialData[i].angle, serialData[i].distance, i);
            }
        }

        private void BuildObj(float angle, float distance, int i)
        {
            _position = GetPosition(angle, distance, 0.2f, 0.2f);
            if (m_LidarType == LidarType.RPLidarA2 || m_LidarType == LidarType.RPLidarA3)
            {
                if (_position.x > -m_LandOffsetX && _position.x < 0 && _position.y < 0f && _position.y > -m_LandOffsetY)
                {
                    GameEntry.Windows.GroundUICameraRay(_position + Resolution + OriginalOffset);

                    if (m_IsLandFacadeOpen && i < m_LandDebugObjs.Count)
                    {
                        m_LandDebugObjs[i].transform.localPosition = _position + Resolution + OriginalOffset;
                        m_LandDebugObjs[i].transform.localScale = Vector3.one;
                        m_LandDebugObjs[i].transform.localRotation = Quaternion.identity;
                        m_LandDebugObjs[i].enabled = (true);
                    }
                }
            }
        }

        public Vector2 GetPosition(float angle, float distance, float xScale = 1, float yScale = 1)
        {
            m_Rad = (angle * Mathf.PI / 180f);
            m_EndptX = Mathf.Sin(m_Rad) * distance * xScale;
            m_EndptY = Mathf.Cos(m_Rad) * distance * yScale;
            m_Vector2.Set(-m_EndptY, m_EndptX);
            return m_Vector2;
        }

    }

}
