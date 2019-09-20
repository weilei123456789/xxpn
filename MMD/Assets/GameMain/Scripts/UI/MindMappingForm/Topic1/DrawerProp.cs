using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Penny
{
    public class DrawerProp : MonoBehaviour
    {
        [SerializeField]
        private Image m_Mask;
        [SerializeField]
        private Image m_Icon;
        [SerializeField]
        private PropType m_DrawerPropType = PropType.None;
        [SerializeField]
        private int m_IconPropId = 0;
        [SerializeField]
        private float m_CrazyTouchTime = 6;
        [SerializeField]
        private UISpriteAnimation m_UISpriteAnimation = null;

        private bool m_IsCanTouch = true;
        private float m_TouchTime = 0;

        private bool m_IsShow = true;

        public Image Icon
        {
            get
            {
                return m_Icon;
            }
        }

        public bool IsShow
        {
            get                                                                                                      
            {
                return m_IsShow;
            }
        }

        public PropType PropType
        {
            get
            {
                return m_DrawerPropType;
            }
        }

        public bool IsCanTouch
        {
            get
            {
                return m_IsCanTouch;
            }

           
        }

        private Vector3 m_InitPos = Vector3.zero;

        private void Awake()
        {
            m_InitPos = transform.localPosition;
            ResetProp();
        }

        private void Start()
        {
            m_UISpriteAnimation.FirstFrame();
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
            AutoScale();
            RandomJump();


        }

        private float m_JumpTimeCur = 0;
        private float m_JumpTimeMax = 1;
        private bool m_IsJumpTime = false;
        private void RandomJump()
        {
            if (!m_IsJumpTime) return;
            m_JumpTimeMax -= Time.deltaTime;
            if (m_JumpTimeMax <= m_JumpTimeCur)
            {
                m_JumpTimeMax = 1;
                m_IsJumpTime = false;
                Shake(0.5f, new Vector3(0, -10, 0));
            }
           
        }
        public void StartJump()
        {
            m_IsJumpTime = true;
        }

        private float m_ScaleTime = 0;
        private float m_ScaleMaxTime = 0.3f;
        private bool m_IsFilp = true;
        private bool m_AutoScale = false;

        private void AutoScale()
        {
            if (!m_AutoScale) return;
            //由小变大再由大变小
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

        public void Scale()
        {
            //transform.localScale = Vector3.zero;
            transform.localPosition = m_InitPos;
            transform.DOScale(Vector3.one, 0.2f);
        }

        public void SetSprite(PropType drawerPropType, Sprite sprite, int id)
        {
            m_DrawerPropType = drawerPropType;
            m_Icon.sprite = sprite;
            m_IconPropId = id;
            ResetProp();
            m_IsShow = true;
        }

        public void ResetProp()
        {
            StopAutoScale();
            transform.localPosition = m_InitPos;
            transform.localScale = Vector3.zero;
            ClearColor();
        }

        public void OutTime()
        {
            m_IsShow = true;
        }

        public void ClearScale()
        {
            if (m_IsShow)
                transform.localScale = Vector3.one;
            m_UISpriteAnimation.FirstFrame();
        }

        public void ClearColor()
        {
            m_Mask.color = Color.white;
            m_Icon.color = Color.white;
        }

        public void StartAutoScale()
        {
            m_AutoScale = true;
        }

        public void StopAutoScale()
        {
            m_AutoScale = false;
        }

        public void Clone()
        {
            //transform.DOKill();
            m_AutoScale = false;
            ClearColor();
            m_Mask.color = new Color(1, 1, 1, 0);
            //transform.localScale = Vector3.one * 1.2f;
        }

        public bool Select(PropType subProp)
        {
            if (!m_IsCanTouch) return false;
            m_IsCanTouch = false;
            //true
            if (subProp == m_DrawerPropType)
            {
                GameEntry.Sound.StopSound(TopicTouchBase.s_SoundSerialId);
                GameEntry.Sound.PlayUISound((int)UISoundId.qipao_s);
                //transform.DOScale(Vector3.one * 1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
                m_AutoScale = true;
                //m_Mask.color = Color.yellow;
                //m_Icon.color = Color.yellow;
                m_IsShow = false;
                m_UISpriteAnimation.Play();
                return true;
            }
            // false
            else
            {
                //GameEntry.XFTTS.MultiSpeak("再仔细想想! "/*+ speek*/);
                //if (m_Sound_XX != -1)
                GameEntry.Sound.StopSound(TopicTouchBase.s_SoundSerialId);
                TopicTouchBase.s_SoundSerialId = (int)GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_xiangxiang,ref TopicOneForm.m_ClipMaxLength);
                GameEntry.Sound.PlayUISound((int)UISoundId.SelectErr);
                Debug.Log("选错了");
                //m_Mask.color = Color.red;
                //m_Icon.color = Color.red;
                Shake();
                m_IsShow = true;

                return false;
            }
        }


        /// <summary>
        /// 指定时间与方向的抖动
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="strength"></param>
        public void Shake(float duration,Vector3 strength)
        {

            transform.DOShakePosition(duration, strength);

        }

        private void Shake()
        {
            transform.DOShakePosition(1, new Vector3(10, 0, 0));
        }
        //变灰
        public void Gary()
        {
            if (!m_IsShow) return;
            //transform.DOKill();
            m_AutoScale = false;
            transform.localScale = Vector3.one;
            m_Mask.color = Color.gray;
            m_Icon.color = Color.gray;
        }

    }

}