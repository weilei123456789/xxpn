using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Penny
{
    public class MapPuzzleData
    {
        private Image m_MainKey = null;
        private GameObject[] m_NeedTouchs = null;
        private bool[] m_TouchFlag = null;
        private bool m_IsComplete = false;

        public Image MainKey
        {
            get { return m_MainKey; }
        }
        public GameObject[] TouchObjs
        {
            get { return m_NeedTouchs; }
        }
        public bool[] TouchFlag
        {
            get { return m_TouchFlag; }
        }

        public bool IsComplete
        {
            get { return m_IsComplete; }
        }

        public MapPuzzleData(Image mainKey, GameObject[] needTouchs)
        {
            m_MainKey = mainKey;
            m_NeedTouchs = needTouchs;
            m_TouchFlag = new bool[needTouchs.Length];
            Clear();
        }

        public void Clear()
        {
            for (int i = 0; i < m_TouchFlag.Length; i++)
            {
                m_TouchFlag[i] = false;
            }
            m_IsComplete = false;
        }

        public bool Select(GameObject go)
        {
            if (m_IsComplete) return false;
            for (int i = 0; i < m_NeedTouchs.Length; i++)
            {
                if (m_NeedTouchs[i] == go && !m_TouchFlag[i])
                {
                    m_TouchFlag[i] = true;
                }
            }
            if (IsAllComplete())
            {
                m_IsComplete = true;
                m_MainKey.enabled = true;
                return true;
            }
            return false;
        }

        private bool IsAllComplete()
        {
            if (m_TouchFlag == null) return false;
            for (int i = 0; i < m_TouchFlag.Length; i++)
            {
                if (!m_TouchFlag[i])
                {
                    return false;
                }
            }
            return true;
        }
    }

}