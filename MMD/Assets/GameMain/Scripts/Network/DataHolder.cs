using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Penny
{
    public class DataHolder
    {
        public byte[] m_RecvDataCache;//use array as buffer for efficiency consideration
        public byte[] m_RecvData;

        private int mTail = -1;
        private int packLen;
        public void PushData(byte[] data, int length)
        {
            if (m_RecvDataCache == null)
                m_RecvDataCache = new byte[length];

            if (this.Count + length > this.Capacity)//current capacity is not enough, enlarge the cache
            {
                byte[] newArr = new byte[this.Count + length];
                m_RecvDataCache.CopyTo(newArr, 0);
                m_RecvDataCache = newArr;
            }

            Array.Copy(data, 0, m_RecvDataCache, mTail + 1, length);
            mTail += length;
        }

        public bool IsFinished()
        {
            if (this.Count == 0)
            {
                //skip if no data is currently in the cache
                return false;
            }

            if (this.Count >= 4)
            {
                DataStream reader = new DataStream(m_RecvDataCache, true);
                packLen = (int)reader.ReadInt32();
                if (packLen > 0)
                {
                    if (this.Count - 4 >= packLen)
                    {
                        m_RecvData = new byte[packLen];
                        Array.Copy(m_RecvDataCache, 4, m_RecvData, 0, packLen);
                        return true;
                    }

                    return false;
                }
                return false;
            }

            return false;
        }

        public void Reset()
        {
            mTail = -1;
        }

        public void RemoveFromHead()
        {
            int countToRemove = packLen + 4;
            if (countToRemove > 0 && this.Count - countToRemove > 0)
            {
                Array.Copy(m_RecvDataCache, countToRemove, m_RecvDataCache, 0, this.Count - countToRemove);
            }
            mTail -= countToRemove;
        }

        //cache capacity
        public int Capacity
        {
            get
            {
                return m_RecvDataCache != null ? m_RecvDataCache.Length : 0;
            }
        }

        //indicate how much data is currently in cache in bytes
        public int Count
        {
            get
            {
                return mTail + 1;
            }
        }
    }
}