using System;
using System.IO;
using UnityEngine;

namespace Penny
{
    public class DataStream
    {
        private BinaryReader m_BinReader;
        private BinaryWriter m_BinWriter;
        private MemoryStream m_MemStream;
        private bool m_BEMode;//big endian mode

        public DataStream(bool isBigEndian)
        {
            m_MemStream = new MemoryStream();
            InitMemoryStream(m_MemStream, isBigEndian);
        }

        public DataStream(byte[] buffer, bool isBigEndian)
        {
            m_MemStream = new MemoryStream(buffer);
            InitMemoryStream(m_MemStream, isBigEndian);
        }

        public DataStream(byte[] buffer, int index, int count, bool isBigEndian)
        {
            m_MemStream = new MemoryStream(buffer, index, count);
            InitMemoryStream(m_MemStream, isBigEndian);
        }

        private void InitMemoryStream(MemoryStream ms, bool isBigEndian)
        {
            m_BinReader = new BinaryReader(ms);
            m_BinWriter = new BinaryWriter(ms);
            m_BEMode = isBigEndian;
        }

        public void Close()
        {
            m_MemStream.Close();
            m_BinReader.Close();
            m_BinWriter.Close();
        }

        public void SetBigEndian(bool isBigEndian)
        {
            m_BEMode = isBigEndian;
        }

        public bool IsBigEndian()
        {
            return m_BEMode;
        }

        public long Position
        {
            get
            {
                return m_MemStream.Position;
            }
            set
            {
                m_MemStream.Position = value;
            }
        }

        public long Length
        {
            get
            {
                return m_MemStream.Length;
            }
        }

        public byte[] ToByteArray()
        {
            //return mMemStream.GetBuffer();
            return m_MemStream.ToArray();
        }



        public long Seek(long offset, SeekOrigin loc)
        {
            return m_MemStream.Seek(offset, loc);
        }

        public void WriteRaw(byte[] bytes)
        {
            m_BinWriter.Write(bytes);
        }

        public void WriteRaw(byte[] bytes, int offset, int count)
        {
            m_BinWriter.Write(bytes, offset, count);
        }

        public void WriteByte(byte value)
        {
            m_BinWriter.Write(value);
        }

        public byte ReadByte()
        {
            return m_BinReader.ReadByte();
        }

        public void WriteInt16(UInt16 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public UInt16 ReadInt16()
        {
            UInt16 val = m_BinReader.ReadUInt16();
            if (m_BEMode)
                return BitConverter.ToUInt16(FlipBytes(BitConverter.GetBytes(val)), 0);
            return val;
        }

        public void WriteInt32(UInt32 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public UInt32 ReadInt32()
        {
            UInt32 val = m_BinReader.ReadUInt32();
            if (m_BEMode)
                return BitConverter.ToUInt32(FlipBytes(BitConverter.GetBytes(val)), 0);
            return val;
        }

        public void WriteInt64(UInt64 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public UInt64 ReadInt64()
        {
            UInt64 val = m_BinReader.ReadUInt64();
            if (m_BEMode)
                return BitConverter.ToUInt64(FlipBytes(BitConverter.GetBytes(val)), 0);
            return val;
        }

        //public void WriteString8(string value)
        //{
        //    WriteInteger(BitConverter.GetBytes((byte)value.Length));
        //    System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        //    //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        //    mBinWriter.Write(encoding.GetBytes(value));
        //}

        public void WriteString8(string value)
        {
            // System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] bytes = encoding.GetBytes(value);
            //m_BinWriter.Write((byte)bytes.Length);
            m_BinWriter.Write(bytes);
        }

        public string ReadString8()
        {
            int len = ReadByte();
            byte[] bytes = m_BinReader.ReadBytes(len);
            // System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetString(bytes);
        }

        public void WriteString16(string value)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] data = encoding.GetBytes(value);
            WriteInteger(BitConverter.GetBytes((Int16)data.Length));
            //System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            m_BinWriter.Write(data);
        }

        public string ReadString16()
        {
            ushort len = ReadInt16();
            byte[] bytes = m_BinReader.ReadBytes(len);
            //  System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetString(bytes);
        }

        private void WriteInteger(byte[] bytes)
        {
            if (m_BEMode)
                FlipBytes(bytes);
            m_BinWriter.Write(bytes);
        }

        private byte[] FlipBytes(byte[] bytes)
        {
            //Array.Reverse(bytes); 
            for (int i = 0, j = bytes.Length - 1; i < j; ++i, --j)
            {
                byte temp = bytes[i];
                bytes[i] = bytes[j];
                bytes[j] = temp;
            }
            return bytes;
        }


        /// <summary>
        /// signed型数据读写
        /// </summary>
        public void WriteSByte(sbyte value)
        {
            m_BinWriter.Write(value);
        }

        public sbyte ReadSByte()
        {
            return m_BinReader.ReadSByte();
        }

        public void WriteSInt16(Int16 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public Int16 ReadSInt16()
        {
            Int16 val = m_BinReader.ReadInt16();
            if (m_BEMode)
                return BitConverter.ToInt16(FlipBytes(BitConverter.GetBytes(val)), 0);
            return val;
        }

        public void WriteSInt32(Int32 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public Int32 ReadSInt32()
        {
            Int32 val = m_BinReader.ReadInt32();
            if (m_BEMode)
                return BitConverter.ToInt32(FlipBytes(BitConverter.GetBytes(val)), 0);
            return val;
        }

        public void WriteSInt64(Int64 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public Int64 ReadSInt64()
        {
            Int64 val = m_BinReader.ReadInt64();
            if (m_BEMode)
                return BitConverter.ToInt64(FlipBytes(BitConverter.GetBytes(val)), 0);
            return val;
        }


        /// <summary>
        /// Unsigned型数据读写
        /// </summary>
        public void WriteUByte(byte value)
        {
            m_BinWriter.Write(value);
        }

        public byte ReadUByte()
        {
            return m_BinReader.ReadByte();
        }

        public void WriteUInt16(UInt16 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public UInt16 ReadUInt16()
        {
            UInt16 val = m_BinReader.ReadUInt16();
            if (m_BEMode)
                return BitConverter.ToUInt16(FlipBytes(BitConverter.GetBytes(val)), 0);
            return val;
        }

        public void WriteUInt32(UInt32 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public UInt32 ReadUInt32()
        {
            UInt32 val = m_BinReader.ReadUInt32();
            if (m_BEMode)
                return BitConverter.ToUInt32(FlipBytes(BitConverter.GetBytes(val)), 0);
            return val;
        }

        public void WriteUInt64(UInt64 value)
        {
            WriteInteger(BitConverter.GetBytes(value));
        }

        public UInt64 ReadUInt64()
        {
            UInt64 val = m_BinReader.ReadUInt64();
            if (m_BEMode)
                return BitConverter.ToUInt64(FlipBytes(BitConverter.GetBytes(val)), 0);
            return val;
        }
    }
}