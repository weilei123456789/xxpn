using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SerialPortUtility.Scripts
{
    public static class SerialCommunicationUtility
    {

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StrintToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        public static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ByteToHexString(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }


        private static string returnStr;
        private static string oneStr;
        private static string twoStr;
        /// <summary>
        /// 进制转换以及字符串重组
        /// </summary>
        public static string BinaryConversion(byte one, byte two)
        {
            if (one < 16)
                oneStr = "0" + Convert.ToString(one, 16);//十进制转十六进制  byte 出来的直接是十进制
            else
                oneStr = Convert.ToString(one, 16);
            if (two < 16)
                twoStr = "0" + Convert.ToString(two, 16);
            else
                twoStr = Convert.ToString(two, 16);
            returnStr = oneStr + twoStr;
            return returnStr;
        }

        /// <summary>
        /// 10进制转2进制
        /// </summary>
        /// <param name="_byte"></param>
        /// <returns></returns>
        public static string Binary_Ten_To_Two(byte _byte)
        {
            string jz_2 = Convert.ToString(_byte, 2);
            return jz_2;
        }

        /// <summary>
        /// 16进制转2进制
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static string Binary_Hex_To_Two(string hexString)
        {
            int jz_16 = Convert.ToInt32(hexString, 16);
            string jz_2 = Convert.ToString(jz_16, 2);
            return jz_2;
        }

        ///// <summary>
        ///// startIndex 从右往左[7,6,5,4,3,2,1,0]
        ///// </summary>
        ///// <param name="hexString"></param>
        ///// <param name="startIndex"></param>
        ///// <param name="end"></param>
        ///// <returns></returns>
        //public static byte Binary_HexStr_To_Byte(byte hex, int startIndex)
        //{
        //    string jz_2 = Convert.ToString(hex, 2);
        //    int length = jz_2.Length;
        //    jz_2 = jz_2.Substring(length - startIndex, startIndex);
        //    for (int i = 0; i < length - startIndex; i++)
        //    {
        //        jz_2 = "0" + jz_2;
        //    }
        //    return Convert.ToByte(jz_2, 2);
        //}

        /// <summary>
        /// 对byte按位取反后得到byte
        /// </summary>
        /// <param name="bData"></param>
        /// <returns></returns>
        public static byte GetOppData(byte bData)
        {
            string s = Convert.ToString(bData, 16);
            int d = Convert.ToUInt16(s, 16);
            d = ~d;
            string sdata = Convert.ToString(d, 16);
            string realSData = sdata.Substring(6);
            byte[] bHex = HexToByte(realSData);
            return bHex[0];
        }

        /// <summary>
        /// 获取位
        /// </summary>
        /// <param name="b"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetBit(byte b, int index)
        {
            return ((b & (1 << index)) > 0) ? 1 : 0;
        }

        //public static byte GetBit2(byte b, int index)
        //{
        //    return Convert.ToByte((b & (1 << index)));
        //}
    }
}