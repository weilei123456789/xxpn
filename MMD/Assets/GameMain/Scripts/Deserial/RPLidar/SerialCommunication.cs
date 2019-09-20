using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using Assets.SerialPortUtility.Scripts;


public delegate void SerialPortMessageEventHandler(byte[] sendData);
public delegate void SerialPortSendMessageReportHandler(byte[] sendData);

namespace Assets.SerialPortUtility.Interfaces
{
    public class SerialCommunication
    {
        // 从串口发出消息事件
        public event SerialPortMessageEventHandler SerialPortMessageEvent;
        // 给串口发消息事件
        public event SerialPortSendMessageReportHandler SerialPortSendMessageReportEvent;
        private SerialPort m_SerialPort;
        private Thread m_ThreadReceive;
        private LidarType m_LidarType = LidarType.RPLidarA2;
        // 储存接收到的消息
        private List<byte> m_Buffer = new List<byte>(4096);

        public SerialCommunication(string portName, int boudrate, LidarType lidarType)
        {
            m_LidarType = lidarType;
            m_SerialPort = new SerialPort(portName, boudrate, Parity.None, 8, StopBits.One);
        }

        public void OpenSerialPort()
        {
            m_SerialPort.Open();
            m_SerialPort.DiscardInBuffer();
            m_SerialPort.DiscardOutBuffer();
            m_SerialPort.DtrEnable = false;
            //serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            m_SerialPort.ReadTimeout = -1;
            if (m_LidarType == LidarType.RPLidarA2 || m_LidarType == LidarType.RPLidarA3)
            {
                m_ThreadReceive = new Thread(ListenSerialPort_RPLidarAX)
                {
                    IsBackground = true
                };
            }
            else// if (m_LidarType == LidarType.YDLidarG4)
            {
                m_ThreadReceive = new Thread(ListenSerialPort_YDLidarG4)
                {
                    IsBackground = true
                };
            }

            m_ThreadReceive.Start();
        }

        public bool IsSerialPortIsOpen()
        {
            return m_SerialPort.IsOpen;
        }

        public void CloseSerialPort()
        {
            if (m_ThreadReceive != null)
            {
                m_ThreadReceive.Abort();//关闭线程
                m_ThreadReceive = null;
                Debug.Log("Close thread");
            }
            m_SerialPort.Close();//关闭串口
            m_SerialPort.Dispose();//将串口从内存中释放掉，注意如果这里不释放则在同一次运行状态下打不开此关闭的串口
        }

        /// <summary>
        /// 给串口发消息
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public bool SendMessageFromSerialPort(byte[] byteArray)
        {
            if (m_SerialPort != null && m_SerialPort.IsOpen == true)
            {
                m_SerialPort.Write(byteArray, 0, byteArray.Length);

                if (SerialPortSendMessageReportEvent != null && SerialPortSendMessageReportEvent.GetInvocationList().Length > 0) // If somebody is listening
                {
                    SerialPortSendMessageReportEvent(byteArray);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// A2雷达，监听串口,读取串口消息
        /// </summary>
        private void ListenSerialPort_RPLidarAX()
        {
            while (m_SerialPort != null && m_SerialPort.IsOpen)
            {
                try
                {
                    int bufferSize = m_SerialPort.ReadBufferSize;
                    byte[] buf = new byte[bufferSize];
                    int count = m_SerialPort.Read(buf, 0, bufferSize);
                    if (count > 0)
                    {
                        //if (m_IsFristCommond)
                        //{
                        //    m_IsFristCommond = false;
                        //    byte[] splitArray = new byte[count - 1];
                        //    Array.Copy(buf, 1, splitArray, 0, count - 1);
                        //    if (SerialPortMessageEvent != null && SerialPortMessageEvent.GetInvocationList().Length > 0) // If somebody is listening
                        //    {
                        //        SerialPortMessageEvent(splitArray);
                        //    }
                        //}
                        //else
                        //{
                        byte[] splitArray = new byte[count];
                        Array.Copy(buf, 0, splitArray, 0, count);

                        if (SerialPortMessageEvent != null && SerialPortMessageEvent.GetInvocationList().Length > 0) // If somebody is listening
                        {
                            SerialPortMessageEvent(splitArray);
                        }
                        //}
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning(e.Message);
                }
            }
        }

        /// <summary>
        /// G4雷达，监听串口,读取串口消息
        /// </summary>
        private void ListenSerialPort_YDLidarG4()
        {
            while (m_SerialPort != null && m_SerialPort.IsOpen)
            {
                try
                {
                    int bufferSize = m_SerialPort.ReadBufferSize;
                    byte[] buf = new byte[bufferSize];
                    int count = m_SerialPort.Read(buf, 0, bufferSize);

                    byte[] splitArray = new byte[count];
                    Array.Copy(buf, 0, splitArray, 0, count);

                    if (SerialPortMessageEvent != null && SerialPortMessageEvent.GetInvocationList().Length > 0)
                    {
                        SerialPortMessageEvent(splitArray);
                    }

                    //Debug.Log("Buffer.cout:" + count + "  Data: " + SerialCommunicationUtility.ByteToHexString(splitArray));
                }
                catch (Exception e)
                {
                    Debug.LogWarning(e.Message);
                }

                //try
                //{
                //    byte buf = Convert.ToByte(m_SerialPort.ReadByte());
                //    m_Buffer.Add(buf);
                //    while (m_Buffer.Count >= 2)
                //    {
                //        if (m_Buffer[0] == 0xAA && m_Buffer[1] == 0x55)
                //        {
                //            //Debug.Log("内层收到未处理消息");
                //            if (m_Buffer.Count < 4)
                //            {
                //                break;
                //            }
                //            int numLen = m_Buffer[3];
                //            if (m_Buffer.Count < numLen * 2 + 10)
                //            {
                //                break;
                //            }
                //            //Debug.Log("numLen: " + numLen);
                //            Data_Process(numLen, m_Buffer);
                //            //一条完整数据存储进行处理移除前面一条完整数据
                //            m_Buffer.RemoveRange(0, numLen * 2 + 10);
                //        }
                //        else
                //        {
                //            m_Buffer.RemoveAt(0);
                //        }
                //    }
                //    //Debug.Log("buffer.Count: " + buffer.Count + "  " + RunSerial.byteToHexStr(buffer.ToArray()));
                //}
                //catch (Exception e)
                //{
                //    Debug.LogWarning(e.Message);
                //}
            }
        }

        private void Data_Process(int numLen, List<byte> bufferSrc)
        {
            byte[] readBuffer = null;
            readBuffer = new byte[numLen * 2 + 10];
            bufferSrc.CopyTo(0, readBuffer, 0, numLen * 2 + 10);
            SerialPortMessageEvent(readBuffer);// Invoke方法防止主线程拥堵冲突
        }

    }
}