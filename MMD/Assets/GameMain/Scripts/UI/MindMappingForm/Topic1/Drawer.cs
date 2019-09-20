using DG.Tweening;
using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Penny
{
    /// <summary>
    /// 抽屉
    /// </summary>
    public class Drawer : MonoBehaviour
    {
        [SerializeField]
        private DirType m_DrawerDirType = DirType.None;
        [SerializeField]
        private int m_Id = 0;
        [SerializeField]
        private GameObject m_TouchObj = null;
        [SerializeField]
        private float m_CrazyTouchTime = 3;
        [SerializeField]
        private Image m_Mask = null;
        [SerializeField]
        private Transform m_Parent = null;

        private bool m_IsOpen = false;

        private RectTransform m_RectTrs = null;

        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        public GameObject TouchObj
        {
            get
            {
                return m_TouchObj;
            }
        }

        public Transform Parent
        {
            get
            {
                return m_Parent;
            }
        }

        public DirType DirType
        {
            get
            {
                return m_DrawerDirType;
            }
        }

        private bool m_IsCanTouch = true;
        private float m_TouchTime = 0;

        // Use this for initialization
        void Start()
        {
            m_RectTrs = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (!m_IsCanTouch)
            {
                m_TouchTime += Time.deltaTime;
                if (m_TouchTime > m_CrazyTouchTime)
                {
                    m_TouchTime = 0;
                    m_IsCanTouch = true;
                }
            }
        }

        public void Open()
        {
            m_IsOpen = true;
            if (!DOTween.IsTweening(transform))
            {
                transform.DOLocalMoveX(transform.localPosition.x - 90, 0.3f);
            }
        }

        public void Close()
        {
            m_IsOpen = false;
            if (!DOTween.IsTweening(transform))
            {
                transform.DOLocalMoveX(transform.localPosition.x + 90, 0.3f).SetDelay(0.5f);
            }
        }

        public bool Select(DirType dirType, int successID, GameFrameworkAction<Drawer> action)
        {
            if (!m_IsCanTouch) return false;
            m_IsCanTouch = false;
            if (m_IsOpen) return false;

            if (dirType == m_DrawerDirType && m_Id == successID)
            {
                GameEntry.Sound.PlayUISound((int)UISoundId.drawer);
                m_Mask.color = Color.white;
                Open();
                if (action != null)
                {
                    action(this);
                }
                return true;
            }
            else
            {
                GameEntry.Sound.PlayUISound((int)UISoundId.drawer);
                //m_Mask.color = Color.red;
                Shake();
                return false;
            }
        }

        public void Cancel()
        {
            m_Mask.color = Color.white;
        }

        private void Shake()
        {
            transform.DOShakePosition(1, new Vector3(10, 0, 0));
        }

        public void ClearParentNode()
        {
            for (int i = m_Parent.childCount - 1; i >= 0; i--)
            {
                Destroy(m_Parent.GetChild(i).gameObject);
            }
        }
    }
}