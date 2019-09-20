using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace Penny
{
    public class TopicViceForm : UGuiForm
    {
        [SerializeField]
        private Image[] m_ArrowImage = null;
        [SerializeField]
        private Transform[] m_ArrowPath = null;

        [SerializeField]
        private Image m_ZiMogu = null;
        [SerializeField]
        private Sprite m_ZiMoguNormal = null;
        [SerializeField]
        private Sprite m_ZiMoguVariant = null;

        [SerializeField]
        private Image m_HongMogu = null;
        [SerializeField]
        private Sprite m_HongMoguNormal = null;
        [SerializeField]
        private Sprite m_HongMoguVariant = null;

        private bool m_IsStepOnZiMogu = false;
        private bool m_IsStepOnHongMogu = false;

        private Vector3[] m_PathVector3 = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_PathVector3 = new Vector3[m_ArrowPath.Length];
            for (int i = 0; i < m_ArrowPath.Length; i++)
            {
                m_PathVector3[i] = m_ArrowPath[i].localPosition;
            }
            float time = 20;
            float len = m_ArrowImage.Length;
            for (int i = 0; i < m_ArrowImage.Length; i++)
            {
                m_ArrowImage[i].rectTransform.localPosition = m_PathVector3[0];
                m_ArrowImage[i].rectTransform.DOLocalPath(m_PathVector3, time, PathType.CatmullRom, PathMode.TopDown2D)
                    .SetLoops(-1, LoopType.Restart)
                    .SetLookAt(-1)
                    .SetEase(Ease.Linear)
                    .SetDelay(20 / len * i);
            }
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Windows.SubscribeUIGroundEvent(OnLidarHitEvent);
            m_IsStepOnZiMogu = false;
            m_IsStepOnHongMogu = false;
        }

        protected override void OnClose(object userData)
        {
            GameEntry.Windows.UnSubscribeUIGroundEvent(OnLidarHitEvent);
            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_IsStepOnZiMogu && m_ZiMogu.sprite.name.Equals(m_ZiMoguVariant.name))
            {
                m_ZiMogu.sprite = m_ZiMoguNormal;
                m_IsStepOnZiMogu = false;
                GameFrameworkLog.Debug("<color=cyan>变色触发</color>");
            }
            else if (m_IsStepOnHongMogu && m_HongMogu.sprite.name.Equals(m_HongMoguVariant.name))
            {
                m_HongMogu.sprite = m_HongMoguNormal;
                m_IsStepOnHongMogu = false;
            }

        }

        private void OnLidarHitEvent(GameObject arg1, Vector3 arg2)
        {
            if (arg1 == m_ZiMogu.gameObject)
            {
                m_ZiMogu.sprite = m_ZiMoguVariant;
                m_IsStepOnZiMogu = true;
                GameFrameworkLog.Debug("<color=lime>变色触发</color>");
                
            }
            else if (arg1 == m_HongMogu.gameObject)
            {
                m_HongMogu.sprite = m_HongMoguVariant;
                m_IsStepOnHongMogu = true;
            }
        }

    }
}