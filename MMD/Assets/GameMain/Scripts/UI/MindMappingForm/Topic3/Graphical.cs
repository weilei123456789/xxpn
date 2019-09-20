using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Penny
{
    public class Graphical : TopicTouchBase
    {
        protected override int TouchSuccessUISoundId
        {
            get { return (int)UISoundId.qipao_s; }
        }

        protected override int TouchFailedUISoundId
        {
            get { return (int)UISoundId.qipao_f; }
        }

        public override GameObject TouchObj
        {
            get { return gameObject; }
        }

        [SerializeField]
        private GraphicalType m_GraphicalType = GraphicalType.None;
        [SerializeField]
        private ColorType m_ColorType = ColorType.None;
        [SerializeField]
        private GameObject[] m_TouchObjs = null;
        [SerializeField]
        private Image[] m_EdgeImages = null;

        private Topic3Form m_UserDate = null;
        //private Dictionary<int, GameObject> NeedTouch = new Dictionary<int, GameObject>();
        private GameObject[] m_NeedTouchs = null;
        private bool[] m_Toucheds = null;
        private Sprite[] m_EdgeSprites = null;

        private bool m_IsComplete = false;

        public GraphicalType GraphicalType
        {
            get { return m_GraphicalType; }
        }

        public bool IsComplete
        {
            get { return m_IsComplete; }
        }

        protected override void Awake()
        {
            base.Awake();
            //Id = (int)m_GraphicalType;
            Clear();
        }

        protected override void Start()
        {
        }

        protected override void Update()
        {
            base.Update();
        }

        public void Topic(ColorType color, GraphicalType graphical, object userDate = null)
        {
            if (typeof(Topic3Form) == userDate.GetType())
                m_UserDate = (Topic3Form)userDate;
            m_ColorType = color;
            m_GraphicalType = graphical;
            m_IsComplete = false;
            SetSplitEdge();
            SetNeedTouch();
            for (int i = 0; i < m_EdgeImages.Length; i++)
            {
                m_EdgeImages[i].enabled = false;
            }
        }

        public override bool Select(GameObject go, GameFrameworkAction finish)
        {
            if (!IsCanTouch) return false;
            IsCanTouch = false;
            //true
            if (m_NeedTouchs == null || m_IsComplete)
            {
                return false;
            }

            for (int i = 0; i < m_NeedTouchs.Length; i++)
            {
                if (m_NeedTouchs[i] == go && !m_Toucheds[i])
                {
                    m_EdgeImages[i].enabled = true;
                    m_EdgeImages[i].sprite = m_EdgeSprites[i];
                    m_Toucheds[i] = true;
                    SelectSuccess();
                    GameEntry.Sound.PlayUISound((int)UISoundId.Struck);
                }
            }

            if (!m_IsComplete && IsAllComplete())
            {
                m_UserDate.CloneProp(gameObject.transform.position);
                StartCoroutine(IEnumeratorSoundShake(3, (int)UISoundId.Shovel, 0.5f));
                SetSplitRimLight();
                
                m_IsComplete = true;
            }

            return false;
        }
        
       
        protected override void SelectSuccess()
        {
            base.SelectSuccess();
            //StartAutoScale();
        }

        public override void Clear()
        {
            for (int i = 0; i < m_EdgeImages.Length; i++)
            {
                m_EdgeImages[i].enabled = false;
            }
            m_GraphicalType = GraphicalType.None;
            m_ColorType = ColorType.None;
            m_NeedTouchs = null;
            m_Toucheds = null;
            m_IsComplete = false;
        }

        private bool IsAllComplete()
        {
            if (m_Toucheds == null) return false;
            for (int i = 0; i < m_Toucheds.Length; i++)
            {
                if (!m_Toucheds[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void SetSplitEdge()
        {
            m_EdgeSprites = m_UserDate.GetSplitGraphicalSprite(m_GraphicalType);
        }

        private void SetSplitRimLight()
        {
            for (int i = 0; i < m_EdgeImages.Length; i++)
            {
                m_EdgeImages[i].enabled = false;
            }
            m_EdgeImages[m_EdgeImages.Length - 1].enabled = true;
            m_EdgeImages[m_EdgeImages.Length - 1].sprite = m_UserDate.GetGraphicalSprite_RimLight(m_GraphicalType);
        }

        private void SetNeedTouch()
        {
            switch (m_GraphicalType)
            {
                case GraphicalType.Triangle:
                    {
                        m_NeedTouchs = new GameObject[] { m_TouchObjs[3], m_TouchObjs[5], m_TouchObjs[7], };
                        m_Toucheds = new bool[] { false, false, false, };
                    }
                    break;
                case GraphicalType.Diamond:
                    {
                        m_NeedTouchs = new GameObject[] { m_TouchObjs[0], m_TouchObjs[2], m_TouchObjs[6], m_TouchObjs[8] };
                        m_Toucheds = new bool[] { false, false, false, false, };
                    }
                    break;
                case GraphicalType.Cross:
                    {
                        m_NeedTouchs = new GameObject[] { m_TouchObjs[3], m_TouchObjs[1] };
                        m_Toucheds = new bool[] { false, false, };
                    }
                    break;
                case GraphicalType.Fork:
                    {
                        m_NeedTouchs = new GameObject[] { m_TouchObjs[0], m_TouchObjs[2] };
                        m_Toucheds = new bool[] { false, false, };
                    }
                    break;
                case GraphicalType.Square:
                    {
                        m_NeedTouchs = new GameObject[] { m_TouchObjs[1], m_TouchObjs[3], m_TouchObjs[7], m_TouchObjs[5] };
                        m_Toucheds = new bool[] { false, false, false, false, };
                    }
                    break;
                case GraphicalType.Circular:
                    {
                        m_NeedTouchs = new GameObject[] { m_TouchObjs[0], m_TouchObjs[2], m_TouchObjs[6], m_TouchObjs[8] };
                        m_Toucheds = new bool[] { false, false, false, false, };
                    }
                    break;
            }
        }
    }
}