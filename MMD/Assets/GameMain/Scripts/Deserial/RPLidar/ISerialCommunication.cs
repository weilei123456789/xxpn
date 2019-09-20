using Assets.SerialPortUtility.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.SerialPortUtility.Interfaces
{
    public static class RplidariIstruct
    {
        /// <summary>
        /// 开始扫描采样
        /// </summary>
        public static byte[] SCAN
        {
            get
            {
                return SerialCommunicationUtility.StrintToHexByte("A520");
            }
        }
        /// <summary>
        /// 停止扫描采样
        /// </summary>
        public static byte[] STOP
        {
            get
            {
                return SerialCommunicationUtility.StrintToHexByte("A525");
            }
        }
        /// <summary>
        /// 启动电机
        /// </summary>
        public static byte[] START_MOTOR
        {
            get
            {
                return SerialCommunicationUtility.StrintToHexByte("A5F0029402C1");
            }
        }
        /// <summary>
        /// 停止电机
        /// </summary>
        public static byte[] STOP_MOTOR
        {
            get
            {
                return SerialCommunicationUtility.StrintToHexByte("A5F002000057");
            }
        }

        /// <summary>
        /// 开始高速采样.传统版本
        /// </summary>
        public static byte[] EXPRESS_SCAN
        {
            get
            {
                return SerialCommunicationUtility.StrintToHexByte("A58205000000000022");
            }
        }
    }

    public enum LidarType
    {
        RPLidarA2 = 0,
        RPLidarA3 = 1,
        //YDLidarG4 = 2,
    }

    public interface ISerialCommunication
    {
        void Connect(int baudrate, string portName, LidarType LidarType);

        void Disconnect();
        void SendMessage(byte[] byteArray);
    }
}
