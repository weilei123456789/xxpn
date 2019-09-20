using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityGameFramework.Runtime;
using GameFramework;

namespace Penny
{
    public abstract class GuideBase : MonoBehaviour
    {
        [SerializeField]
        private UISpriteAnimation m_SlapEffect = null;
        [SerializeField]
        private Transform m_Finger = null;
        [SerializeField]
        private Image m_DottedLine_Point = null;
        [SerializeField]
        private Image m_DottedLine_Arrow = null;
        [SerializeField]
        private Transform m_LineParent = null;

        protected int m_ClipMaxLength = 0;
        protected bool m_IsComplateGuide = false;
        public static int s_GuideSoundId = 0;

        public UISpriteAnimation Slap
        {
            get
            {
                return m_SlapEffect;
            }
        }

        public Transform Finger
        {
            get
            {
                return m_Finger;
            }
        }

        private Image DottedLine_Point
        {
            get
            {
                return m_DottedLine_Point;
            }
        }

        private Image DottedLine_Arrow
        {
            get
            {
                return m_DottedLine_Arrow;
            }
        }

        public bool IsComplateGuide
        {
            get
            {
                return m_IsComplateGuide;
            }
            protected set
            {
                m_IsComplateGuide = value;
            }
        }

        public void ClearPoint()
        {
            for (int i = m_LineParent.childCount - 1; i >= 0; i--)
            {
                Destroy(m_LineParent.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Enter_Teaching
        /// </summary>
        public void Open()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Level_Teaching
        /// </summary>
        public void Close()
        {
            GameEntry.Sound.StopSound(s_GuideSoundId);
            End();
            StopCoroutine("IEnumeratorGuide");
        }

        public void StartGuide(Transform fingerTransform, Transform slapTransform, GameFrameworkAction firstEvent, GameFrameworkAction secondEvent,Animator penyAnimator)
        {
            StartCoroutine(IEnumeratorGuide(fingerTransform, slapTransform, firstEvent, secondEvent,penyAnimator));
        }

        protected abstract IEnumerator IEnumeratorGuide(Transform fingerTransform, Transform slapTransform, GameFrameworkAction firstEvent, GameFrameworkAction secondEvent,Animator penyAnimator);

        protected IEnumerator Line(Transform fingerTransform, Transform slapTransform, float time = 3.0f)
        {
            yield return new WaitForSeconds(0.3f);
            m_Finger.gameObject.SetActive(false);
            Vector3 start = fingerTransform.localPosition;
            Vector3 end = slapTransform.localPosition;
            Vector3 dir = (end - start).normalized;
            float spliteLen = Mathf.Sqrt(m_DottedLine_Point.rectTransform.sizeDelta.x * m_DottedLine_Point.rectTransform.sizeDelta.x);
            float dis = Vector3.Distance(start, end);
            int pointCount = (int)(dis / spliteLen);
            //Log.Info(pointCount);
            m_DottedLine_Point.enabled = true;
            m_DottedLine_Point.transform.localPosition = start;
            for (int i = 1; i < pointCount - 1; i++)
            {
                ClonePoint(start + dir * spliteLen * i);
                yield return new WaitForSeconds(time / (pointCount - 1));
            }
            m_DottedLine_Arrow.enabled = true;
            m_DottedLine_Arrow.transform.localPosition = start + dir * spliteLen * (pointCount - 1);
            float angle = Vector3.Angle(Vector3.up, dir);
            m_DottedLine_Arrow.transform.localRotation = Quaternion.Euler(0, 0, angle);
            yield return new WaitForSeconds(0.3f);
            ClearPoint();
            m_DottedLine_Point.enabled = false;
            m_DottedLine_Arrow.enabled = false;
        }

        protected void FirstFinger(GameFrameworkAction firstEvent)
        {
            m_Finger.gameObject.SetActive(true);
            Vector3 pos = m_Finger.localPosition + Vector3.down * 20;
            m_Finger.localPosition = pos;
            //Finger.DOLocalMoveY(pos.y + 20, 0.5f).SetLoops(3, LoopType.Yoyo);
            Sequence sequence = DOTween.Sequence();
            for (int i = 0; i < 3; i++)
            {
                Tween move_up = m_Finger.DOLocalMoveY(pos.y + 20, 0.5f);
                Tween move_down = m_Finger.DOLocalMoveY(pos.y - 20, 0.5f);
                sequence.Append(move_up);
                sequence.Append(move_down);
            }
            Tween move = m_Finger.DOLocalMoveY(pos.y + 20, 0.5f).OnComplete(() =>
            {
                m_Finger.gameObject.SetActive(false);
                Slap.gameObject.SetActive(true);
                Slap.Rewind(() =>
                {
                    Slap.Rewind(() =>
                    {
                        Slap.Rewind(() =>
                        {
                            Slap.Stop();
                            Slap.gameObject.SetActive(false);
                            if (firstEvent != null)
                                firstEvent();
                        });
                    });
                });
            });
            sequence.Append(move);
        }

        protected void SecondFinger(GameFrameworkAction secondEvent)
        {
            m_SlapEffect.gameObject.SetActive(true);
            m_SlapEffect.Rewind(() =>
            {
                m_SlapEffect.Rewind(() =>
                {
                    m_SlapEffect.Rewind(() =>
                    {
                        m_SlapEffect.Stop();
                        m_SlapEffect.gameObject.SetActive(false);
                        if (secondEvent != null)
                            secondEvent();
                    });
                });
            });
        }

        protected void Init()
        {
            m_SlapEffect.gameObject.SetActive(false);
            m_Finger.gameObject.SetActive(false);
            m_DottedLine_Point.enabled = false;
            m_DottedLine_Arrow.enabled = false;
            m_IsComplateGuide = false;
        }

        protected void SetFinger(Vector3 position)
        {
            m_Finger.position = position;
        }

        protected void SetSlap(Vector3 position)
        {
            m_SlapEffect.transform.position = position;
        }

        protected void End()
        {
            m_IsComplateGuide = true;
            gameObject.SetActive(false);
        }

        protected void ClonePoint(Vector3 position)
        {
            Image item = Instantiate(m_DottedLine_Point);
            Transform transform = item.GetComponent<Transform>();
            transform.SetParent(m_LineParent);
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
            transform.localPosition = position;
        }
    }
}