using System;
using UnityEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityGameFramework.Runtime;
using Assets.SerialPortUtility.Scripts;
using GameFramework;

namespace Penny
{
    public class CabinStruct
    {
        public int K { get; set; }
        public float distance { get; set; }
        public float d { get; set; }
    }

    public class EthernetInterface
    {
        [SerializeField]
        private readonly IPAddress m_IpAddress;
        public string IpAddress { get { return m_IpAddress.ToString(); } }

        [SerializeField]
        private readonly int m_Port;

        private TcpClient m_TcpClient;
        private Thread m_ListenThread = null;

        private bool isConnected = false;
        public bool IsConnected { get { return isConnected; } }

        private NetworkStream m_NetworkStreamCache = null;
        private GameFrameworkAction<int> m_ConnectSuccessDelegate = null;
        private GameFrameworkAction<int> m_ConnectFailedDelegate = null;
        private int m_index = 0;
        private bool m_IsExpress = false;

        public Action<List<SerialData>> UpdateSerialData = null;

        public EthernetInterface(string _ipAddress, int _port, int index, GameFrameworkAction<int> connectSuccessCallback, GameFrameworkAction<int> connectFailedCallback)
        {
            m_IpAddress = IPAddress.Parse(_ipAddress);
            m_Port = _port;
            m_index = index;
            m_ConnectSuccessDelegate = connectSuccessCallback;
            m_ConnectFailedDelegate = connectFailedCallback;
        }

        public void Open()
        {
            m_TcpClient = new TcpClient();
            IAsyncResult result = m_TcpClient.BeginConnect(m_IpAddress, m_Port, new AsyncCallback(ConnectedSuccessCallback), m_TcpClient);
            bool success = result.AsyncWaitHandle.WaitOne(1000, true);
            if (!success)
            {
                //超时  
                if (m_ConnectFailedDelegate != null)
                {
                    m_ConnectFailedDelegate(m_index);
                }
                Close();
            }
            else
            {
                m_ListenThread = new Thread(new ParameterizedThreadStart(ListenRPLidarA3));
                isConnected = true;
                m_ListenThread.IsBackground = true;
                m_ListenThread.Start(m_TcpClient);
            }
        }

        public void Close()
        {
            if (m_ListenThread != null)
            {
                isConnected = false;
                m_ListenThread.Join();
                m_ListenThread = null;
            }

            if (m_TcpClient != null)
            {
                if (m_TcpClient.Connected)
                {
                    if (m_TcpClient.GetStream() != null)
                    {
                        m_TcpClient.GetStream().Close();
                    }
                }
                m_TcpClient.Close();
                m_TcpClient = null;
            }
        }

        private void ConnectedSuccessCallback(IAsyncResult asyncConnect)
        {
            if (m_TcpClient != null && !m_TcpClient.Connected)
            {
                if (m_ConnectFailedDelegate != null)
                {
                    m_ConnectFailedDelegate(m_index);
                }
                return;
            }

            if (m_ConnectSuccessDelegate != null)
            {
                m_ConnectSuccessDelegate(m_index);
            }
        }

        private byte[] m_SplitBufferData = null;
        private byte[] m_Buffer = new byte[4096];

        private void ListenRPLidarA3(object obj)
        {
            TcpClient client = (TcpClient)obj;
            while (client != null && IsConnected)
            {
                try
                {
                    int bufferSize = client.Client.Receive(m_Buffer);
                    if (bufferSize > 0)
                    {
                        m_SplitBufferData = new byte[bufferSize];
                        Array.Copy(m_Buffer, 0, m_SplitBufferData, 0, bufferSize);
                        if (m_IsExpress)
                        {
                            EthernetAnalysisEXPRESS_SCAN(m_SplitBufferData);
                        }
                        else
                        {
                            EthernetAnalysis(m_SplitBufferData);
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }


        private Queue<List<SerialData>> DataQueueCache = new Queue<List<SerialData>>();
        public Queue<List<SerialData>> DataQueue
        {
            get
            {
                return DataQueueCache;
            }
        }

        private List<byte> CacheBuffer = new List<byte>(4096 * 2);
        private bool IsStart = false;


        private void EthernetAnalysis(byte[] responseData)
        {
            //return;
            if (responseData[0] == 0xA5 && responseData[1] == 0x5A)
            {
                IsStart = true;
                return;
            }
            if (IsStart)
            {
                int _res = 0;
                float _angle = 0;
                float _dis = 0;
                byte origin_q = 0;
                byte angle_q1 = 0;
                byte angle_q2 = 0;
                byte distance_q1 = 0;
                byte distance_q2 = 0;
                int s = 0;
                int _s = 0;
                int quality = 0;
                int check_bit = 0;
                int breakIndex = 0;
                CacheBuffer.AddRange(responseData);
                List<SerialData> SerialDatas = new List<SerialData>();

                for (int i = 0; i < CacheBuffer.Count; i++)
                {
                    if (i + 5 >= CacheBuffer.Count) { breakIndex = i; break; }
                    origin_q = CacheBuffer[i];
                    // 判断该字节的0位和1位,是不是一个是0一个是1，如果判断为假则拉取下一个字节继续做同样的判断，直到判断为真
                    s = SerialCommunicationUtility.GetBit(origin_q, 0);
                    _s = SerialCommunicationUtility.GetBit(origin_q, 1);
                    if (s != 0 || _s != 1)
                    {
                        continue;
                    }
                    quality = origin_q >> 2;
                    angle_q1 = CacheBuffer[i + 1];
                    // 如果字节校验位不为1,那后面就不用了算了,重头再来
                    check_bit = SerialCommunicationUtility.GetBit(angle_q1, 0);
                    if (check_bit != 1)
                    {
                        continue;
                    }
                    angle_q2 = CacheBuffer[i + 2];
                    distance_q1 = CacheBuffer[i + 3];
                    distance_q2 = CacheBuffer[i + 4];

                    // 计算角度和距离
                    _res = Convert.ToInt32(SerialCommunicationUtility.BinaryConversion(angle_q2, angle_q1), 16);
                    _angle = (_res >> 1) / 64f;
                    _dis = Convert.ToInt32(SerialCommunicationUtility.BinaryConversion(distance_q2, distance_q1), 16) / 4f;
                    if (_dis > 10 && quality > 0)
                        SerialDatas.Add(new SerialData() { angle = _angle, distance = _dis });
                }
                DataQueueCache.Enqueue(SerialDatas);
                for (int i = breakIndex - 1; i > 0; i--)
                {
                    CacheBuffer.Remove(CacheBuffer[i]);
                }
            }
        }

        private List<SerialData> SerialDatasCache = new List<SerialData>();
        private List<SerialData> SerialDatasReal = new List<SerialData>();
        private static int CabinElementLength = 5;
        private static int CabinLength = 16;

        private void EthernetAnalysisEXPRESS_SCAN(byte[] responseData)
        {
            //string reciveStr = SerialCommunicationUtility.ByteToHexString(responseData);
            //Debug.Log("____________收到消息_____________ Message =>  " + reciveStr.ToString());//打印一条数据信息

            //A5 5A 54 00 00 40 82
            if (responseData[0] == 0xA5 && responseData[1] == 0x5A && responseData[2] == 0x54 && responseData[3] == 0x00 && responseData[4] == 0x00 && responseData[5] == 0x40 && responseData[6] == 0x82)
            {
                IsStart = true;
                CacheBuffer.AddRange(responseData);
                return;
            }
            if (IsStart)
            {
                CacheBuffer.AddRange(responseData);
                //综合
                float start_angle_q6 = 0;
                int breakIndex = 0;
                SerialData item = new SerialData();
                for (int i = 0; i < CacheBuffer.Count; i++)
                {
                    if (i + 84 > CacheBuffer.Count) { breakIndex = i; break; }
                    byte sync1 = Convert.ToByte(CacheBuffer[i + 0] >> 3);
                    // 0x07 = 00000111 获取右边3位
                    byte ChkSum1 = Convert.ToByte(CacheBuffer[i + 0] & 0x07);
                    byte sync2 = Convert.ToByte(CacheBuffer[i + 1] >> 3);
                    // 0x07 = 00000111 获取右边3位
                    byte ChkSum2 = Convert.ToByte(CacheBuffer[i + 1] & 0x07);
                    if (sync1 != 0xA || sync2 != 0x5)
                    {
                        continue;
                    }
                    byte start_angle_q6_7_0 = CacheBuffer[i + 2];
                    // 起始应答报文标志 当设置为1时，表示当前应答报文时本轮测距采样中的第一个。
                    int s = CacheBuffer[i + 3] >> 7;
                    if (s == 1)
                    {
                        // TODO: 清理缓存,重新计算
                        SerialDatasCache.Clear();
                        DataQueueCache.Clear();
                    }
                    // 0x7f = 01111111 获取右边7位
                    byte start_angle_q6_14_8 = Convert.ToByte(CacheBuffer[i + 3] & 0x7f);
                    // 夹角
                    int _res = Convert.ToInt32(SerialCommunicationUtility.BinaryConversion(start_angle_q6_14_8, start_angle_q6_7_0), 16);
                    start_angle_q6 = (_res >> 1) / 64f;
                    // Cabin 
                    CabinStruct[] Cabin = new CabinStruct[CabinLength * 2];
                    int k = 1;
                    // Cabin 5位1组,共16组
                    for (int ck = i; ck < CabinLength * CabinElementLength; ck += CabinElementLength)
                    {
                        //dis1
                        byte distance1_5_0 = Convert.ToByte(CacheBuffer[ck + 0] >> 2);
                        //0x03 = 00000011 获取右边2位
                        byte d01_5_4 = Convert.ToByte(CacheBuffer[ck + 0] & 0x03);
                        byte distance1_16_0 = CacheBuffer[ck + 1];
                        // dis2
                        byte distance2_5_0 = Convert.ToByte(CacheBuffer[ck + 2] >> 2);
                        //0x03 = 00000011 获取右边2位
                        byte d02_5_4 = Convert.ToByte(CacheBuffer[ck + 2] & 0x03);
                        byte distance2_16_0 = CacheBuffer[ck + 3];
                        // 补偿角2 
                        //取出后四位：10110111 & 00001111(即0x0f)，得到00000111，表达式： data & 0x0f；
                        byte d01_3_0 = Convert.ToByte((CacheBuffer[ck + 4] & 0x0f));
                        //取出前四位：10110111 & 11110000(即0xf0)，得到10110000，再右移四位(>>4)，表达式：(data & 0xf0) >> 4；
                        byte d02_3_0 = Convert.ToByte((CacheBuffer[ck + 4] & 0xf0) >> 4);
                        //
                        int distance1 = Convert.ToInt32(SerialCommunicationUtility.BinaryConversion(distance1_16_0, distance1_5_0), 16);
                        int d1 = Convert.ToInt32(SerialCommunicationUtility.BinaryConversion(d01_5_4, d01_3_0), 16);
                        int distance2 = Convert.ToInt32(SerialCommunicationUtility.BinaryConversion(distance2_16_0, distance2_5_0), 16);
                        int d2 = Convert.ToInt32(SerialCommunicationUtility.BinaryConversion(d02_5_4, d02_3_0), 16);
                        // add c1
                        Cabin[k - 1] = new CabinStruct() { K = k++, distance = distance1, d = d1 };
                        // add c2
                        Cabin[k - 1] = new CabinStruct() { K = k++, distance = distance2, d = d2 };
                    }
                    item.angle = start_angle_q6;
                    item.distance = 0;
                    item.quality = 0;
                    item.cabinStructs = (CabinStruct[])Cabin.Clone();
                    SerialDatasCache.Add(item);
                }
                for (int i = 0; i < SerialDatasCache.Count - 1; i++)
                {
                    float angle1 = SerialDatasCache[i].angle;
                    float angle2 = SerialDatasCache[i + 1].angle;
                    CabinStruct[] cabinStructs = SerialDatasCache[i].cabinStructs;
                    for (int j = 0; j < cabinStructs.Length; j++)
                    {
                        item.angle = angle1 + (AngleDiff(angle1, angle2)) * cabinStructs[j].K - cabinStructs[j].d;
                        item.distance = cabinStructs[j].distance;
                        item.quality = 0;
                        item.cabinStructs = null;
                        if (item.distance > 10)
                            SerialDatasReal.Add(item);
                    }
                }
                DataQueueCache.Enqueue(SerialDatasReal);
                for (int i = SerialDatasCache.Count - 1; i > 0; i--)
                {
                    SerialDatasCache.Remove(SerialDatasCache[i]);
                }
                for (int i = breakIndex - 1; i > 0; i--)
                {
                    CacheBuffer.Remove(CacheBuffer[i]);
                }
                SerialDatasReal.Clear();
            }
        }

        private float AngleDiff(float w_i, float w_i_1)
        {
            if (w_i <= w_i_1)
            {
                return w_i_1 - w_i;
            }
            else
            {
                return 360 + w_i_1 + w_i;
            }
        }

        protected static bool TCPWrite(NetworkStream stream, byte[] data)
        {
            if (stream.CanWrite)
            {
                stream.Write(data, 0, data.Length);
                stream.Flush();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Write data to the device.
        /// </summary>
        /// <param name="data"></param>
        public void Write(byte[] data, bool isExpress = false)
        {
            if (m_TcpClient == null) return;
            if (!m_TcpClient.Connected) return;
            try
            {
                TCPWrite(m_TcpClient.GetStream(), data);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            m_IsExpress = isExpress;
        }
    }
}
