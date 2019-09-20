using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;


namespace Penny
{
    public class Controller : MonoBehaviour
    {
        [SerializeField]
        private GameObject PangBai = null;
        [SerializeField]
        private GameObject Reset = null;
        [SerializeField]
        private GameObject Change = null;
        [SerializeField]
        private GameObject Next = null;

        [SerializeField]
        private float m_CrazyTouchTime = 3;


        private bool m_IsCanTouch = false;
        private float m_TouchTime = 0;

        public GameFrameworkAction SpeekAgainCallBack = null;
        public GameFrameworkAction ResetCallBack = null;
        public GameFrameworkAction ChangeCallBack = null;
        public GameFrameworkAction NextCallBack = null;

        private void OnEnable()
        {
            GameEntry.Windows.SubscribeUIWallEvent(OnLidarHitEvent);
            m_IsCanTouch = false;
        }

        private void OnDisable()
        {
            //SpeekAgainCallBack = null;
            //ResetCallBack = null;
            //ChangeCallBack = null;
            //NextCallBack = null;
            GameEntry.Windows.UnSubscribeUIWallEvent(OnLidarHitEvent);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="go"></param>
        /// <param name="vec"></param>
        private void OnLidarHitEvent(GameObject go, Vector3 vec)
        {
            if (go == PangBai)
            {
                if (!m_IsCanTouch) return;
                if (SpeekAgainCallBack != null)
                    SpeekAgainCallBack();
                m_IsCanTouch = false;
            }
            else if (go == Reset)
            {
                if (!m_IsCanTouch) return;
                if (ResetCallBack != null)
                    ResetCallBack();
                m_IsCanTouch = false;
            }
            else if (go == Change)
            {
                if (!m_IsCanTouch) return;
                if (ChangeCallBack != null)
                    ChangeCallBack();
                m_IsCanTouch = false;
            }
            else if (go == Next)
            {
                if (!m_IsCanTouch) return;
                if (NextCallBack != null)
                    NextCallBack();
                m_IsCanTouch = false;
            }
        }
    }

}