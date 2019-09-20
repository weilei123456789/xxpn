using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameFramework;

namespace Penny
{
    public abstract class TopicTouchBase : MonoBehaviour
    {
        public static int s_SoundSerialId = -1;

        [SerializeField]
        private Image[] m_Discoloration = null;
        [SerializeField]
        private PropType m_PropType = PropType.None;
        [SerializeField]
        private int m_Id = 0;
        [SerializeField]
        private float m_CrazyTouchTime = 3;
        [SerializeField]
        private UISpriteAnimation m_UISpriteAnimation = null;

        private bool m_IsCanTouch = true;
        private float m_TouchTime = 0;
        private float m_ScaleTime = 0;
        private float m_ScaleMaxTime = 0.3f;
        private bool m_IsFilp = true;
        private bool m_AutoScale = false;
        private Vector3 m_InitPos = Vector3.zero;

        protected abstract int TouchSuccessUISoundId { get; }
        protected abstract int TouchFailedUISoundId { get; }

        public abstract GameObject TouchObj { get; }

        public PropType PropType
        {
            get { return m_PropType; }
        }

        public int Id
        {
            protected set { m_Id = value; }
            get { return m_Id; }
        }

        public bool IsCanTouch
        {
            protected set { m_IsCanTouch = value; }
            get { return m_IsCanTouch; }
        }

        protected virtual void Awake()
        {
            m_InitPos = transform.localPosition;
        }

        protected virtual void Start()
        {
            if (m_UISpriteAnimation != null)
                m_UISpriteAnimation.FirstFrame();
        }

        protected virtual void Update()
        {
            UpdateCrazyTouch();
            UpdateAutoScale();
        }

        private void UpdateCrazyTouch()
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

        private void UpdateAutoScale()
        {
            if (!m_AutoScale) return;
            if (m_IsFilp)
            {
                m_ScaleTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, m_ScaleTime / m_ScaleMaxTime);
                if (m_ScaleTime > m_ScaleMaxTime)
                {
                    m_ScaleTime = 0;
                    m_IsFilp = false;
                }
            }
            else
            {
                m_ScaleTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, m_ScaleTime / m_ScaleMaxTime);
                if (m_ScaleTime > m_ScaleMaxTime)
                {
                    m_ScaleTime = 0;
                    m_IsFilp = true;
                }
            }
        }

        public virtual bool Select(PropType subProp, int id)
        {
            if (!m_IsCanTouch) return false;
            m_IsCanTouch = false;
            //true
            if (subProp == PropType && m_Id == id)
            {
                SelectSuccess();
                return true;
            }
            // false
            else
            {
                SelectFailed();
                return false;
            }
        }

        public virtual bool Select(PropType subProp, TeamType teamType, int id, PaintDrum paint_drum, GameFrameworkAction finish)
        {
            throw new System.Exception("调用请实现虚函数!!");
        }

        public virtual bool Select(GameObject go, GameFrameworkAction finish)
        {
            throw new System.Exception("调用请实现虚函数!!");
        }

        protected virtual void SelectSuccess()
        {
            GameEntry.Sound.StopSound(s_SoundSerialId);
            GameEntry.Sound.PlayUISound(TouchSuccessUISoundId);
            if (m_UISpriteAnimation != null)
                m_UISpriteAnimation.Play();
        }

        protected virtual void SelectFailed()
        {
            Shake();
            GameEntry.Sound.StopSound(s_SoundSerialId);
            s_SoundSerialId = (int)GameEntry.Sound.PlaySound((int)SoundId.s1_xiangxiang);
            GameEntry.Sound.PlayUISound(TouchFailedUISoundId);
        }

        private void Shake()
        {
            transform.DOShakePosition(1, new Vector3(10, 0, 0));
        }

        public void StopAutoScale()
        {
            m_AutoScale = false;
        }

        public void StartAutoScale()
        {
            m_AutoScale = true;
        }

        /// <summary>
        /// 变色
        /// </summary>
        /// <param name="color"></param>
        public void Discoloration(Color color)
        {
            for (int i = 0; i < m_Discoloration.Length; i++)
            {
                m_Discoloration[i].color = color;
            }
        }

        /// <summary>
        /// 指定次数播放指定间隔时间的指定声音
        /// </summary>
        /// <returns></returns>
        protected IEnumerator IEnumeratorSoundShake(int count, int soundID, float interval)
        {
            for (int i = 0; i < count; i++)
            {
                GameEntry.Sound.PlayUISound(soundID);
                yield return new WaitForSeconds(interval);
            }

        }

        /// <summary>
        /// 灰化
        /// </summary>
        public void Gary()
        {
            m_AutoScale = false;
            transform.localScale = Vector3.one;
            Discoloration(Color.gray);
        }

        public virtual void Clear()
        {
            m_AutoScale = false;
            ClearColor();
            transform.localScale = Vector3.one;
            if (m_UISpriteAnimation != null)
                m_UISpriteAnimation.FirstFrame();
        }

        public void ClearColor()
        {
            Discoloration(Color.white);
        }
    }

}