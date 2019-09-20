using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Resource
{
    public struct CoursewareReadWriteResourceInfo
    {
        private readonly int m_LoadType;
        private readonly int m_Length;
        private readonly int m_HashCode;

        public CoursewareReadWriteResourceInfo(int loadType, int length, int hashCode)
        {
            m_LoadType = loadType;
            m_Length = length;
            m_HashCode = hashCode;
        }

        public int LoadType
        {
            get
            {
                return m_LoadType;
            }
        }

        public int Length
        {
            get
            {
                return m_Length;
            }
        }

        public int HashCode
        {
            get
            {
                return m_HashCode;
            }
        }
    }
}
