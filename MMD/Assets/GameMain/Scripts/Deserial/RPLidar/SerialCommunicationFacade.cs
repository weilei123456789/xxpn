using Assets.SerialPortUtility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.SerialPortUtility.Scripts
{
    public class SerialData
    {
        public float angle;
        public float distance;
        public float quality;
        public Penny.CabinStruct[] cabinStructs;
    }

    public class SerialCommunicationFacade : ISerialCommunication
    {
        private SerialCommunication m_SerialCom;
        private LidarType m_LidarType = LidarType.RPLidarA2;
        //private bool m_IsScan = false;
        public Action<List<SerialData>> UpdateSerialData = null;

        public void Connect(int baudrate, string portName, LidarType lidarType)
        {
            m_LidarType = lidarType;
            m_SerialCom = new SerialCommunication(portName, baudrate, lidarType);
            m_SerialCom.OpenSerialPort();// 打开串口
            // 绑定方法触发,监听读取串口
            m_SerialCom.SerialPortMessageEvent += SerialCom_SerialPortMessageEvent;
            // 绑定方法触发,给串口发消息
            m_SerialCom.SerialPortSendMessageReportEvent += SerialCom_SerialPortSendMessageReportEvent;
        }

        public void Disconnect()
        {
            if (m_SerialCom != null)
            {
                m_SerialCom.CloseSerialPort();
                Debug.Log("Serial Disconnected");
            }
        }

        public void SendMessage(byte[] byteArray)
        {
            if (m_SerialCom == null) return;
            if (m_SerialCom.IsSerialPortIsOpen())
            {
                //Debug.Log("Message Sended");
                m_SerialCom.SendMessageFromSerialPort(byteArray);

                //m_IsScan = (byteArray == RplidariIstruct.SCAN);
            }
            else
            {
                Debug.Log("Message Send Failed!");
            }
        }

        /// <summary>
        /// 监听给串口发的消息
        /// </summary>
        /// <param name="sendData"></param>
        private void SerialCom_SerialPortSendMessageReportEvent(byte[] sendData)
        {
            string text = SerialCommunicationUtility.ByteToHexString(sendData);
            Debug.Log("Message.. =>  " + text.ToString());
        }

        /// <summary>
        /// 串口发过来的消息
        /// </summary>
        /// <param name="sendData"></param>
        private void SerialCom_SerialPortMessageEvent(byte[] sendData)
        {
            //string reciveStr = SerialCommunicationUtility.ByteToHexString(sendData);
            //Debug.Log("____________收到消息_____________ Message =>  " + reciveStr.ToString());//打印一条数据信息

            if (m_LidarType == LidarType.RPLidarA2 || m_LidarType == LidarType.RPLidarA3)
            {
                //if (m_IsScan)
                RPLidarA3_DataResponseContentProcess_SCAN(sendData);
                //else
                //    RPLidarA3_DataResponseContentProcess_EXPRESS_SCAN(sendData);
            }
        }

        private List<SerialData> SerialDatas = new List<SerialData>(4096);
        private List<byte> CacheBuffer = new List<byte>(4096 * 2);
        private List<byte> TempBuffer = null;
        private int CacheBufferLen = 0;
        private int StructLen = 5;
        private int CalcNum = 0;
        private int _res;
        private float _angle;
        private float _dis;
        private bool isFrist = false;

        private void RPLidarA3_DataResponseContentProcess_SCAN(byte[] responseData)
        {
            CacheBuffer.AddRange(responseData);

            if (CacheBuffer.Count > 2)
            {
                if (CacheBuffer[0] == 0xA5 && CacheBuffer[1] == 0x5A && !isFrist)
                {
                    CacheBuffer.RemoveAt(0);
                    CacheBuffer.RemoveAt(1);
                    isFrist = true;
                }
            }
            CalcNum = CacheBuffer.Count / StructLen - 1;
            //Debug.Log(
            //    "buffer.Count: " + CacheBuffer.Count +
            //    //"  data: " + SerialCommunicationUtility.ByteToHexString(responseData) +
            //    "  all.data:[" + SerialCommunicationUtility.ByteToHexString(CacheBuffer.ToArray()) + "]" +
            //    "  StructLen:" + StructLen +
            //    "  CalcNum:" + CalcNum
            //);
            if (CalcNum > 5)
            {
                CacheBufferLen = CalcNum * StructLen;
                TempBuffer = CacheBuffer.GetRange(0, CacheBufferLen);
                for (int i = 0; i < CalcNum; i++)
                {
                    _res = Convert.ToInt32(SerialCommunicationUtility.BinaryConversion(TempBuffer[i * StructLen + 2], TempBuffer[i * StructLen + 1]), 16);
                    _angle = (_res >> 1) / 64f;
                    _dis = Convert.ToInt32(SerialCommunicationUtility.BinaryConversion(TempBuffer[i * StructLen + 4], TempBuffer[i * StructLen + 3]), 16) / 4f;
                    if (_dis > 10)
                    {
                        SerialDatas.Add(new SerialData() { angle = _angle, distance = _dis });
                    }
                }
                CacheBuffer.RemoveRange(0, CacheBufferLen);
                if (UpdateSerialData != null)
                {
                    Loom.RunAsync(() =>
                    {
                        Loom.QueueOnMainThread(() =>
                        {
                            UpdateSerialData(SerialDatas);
                            SerialDatas.Clear();
                        });
                    });

                }
            }
        }


        private void RPLidarA3_DataResponseContentProcess_EXPRESS_SCAN(byte[] sendData)
        {

        }


    }
}
