using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace Penny
{
    public class TopicCale : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_RectTransform = null;
        [SerializeField]
        private Image[] m_CaleStar = null;
        [SerializeField]
        private Text m_TimeText = null;
        [SerializeField]
        private Font m_TimeFont = null;

        private Vector3 m_InitScale = Vector3.zero;
        private Vector2 m_InitPosition = Vector3.zero;

        private Vector3[] m_StarPosition = null;
        private int m_LastTime = 0;

        public bool IsFinish
        {
            get
            {
                return !DOTween.IsTweening(m_RectTransform);
            }
        }

        public int Length
        {
            get { return m_CaleStar.Length; }
        }

        private void Start()
        {
            m_TimeText.font = m_TimeFont;
            m_InitScale = m_RectTransform.localScale;
            m_InitPosition = m_RectTransform.anchoredPosition;
            m_StarPosition = new Vector3[m_CaleStar.Length];
            for (int i = 0; i < m_CaleStar.Length; i++)
            {
                m_StarPosition[i] = m_CaleStar[i].transform.localPosition;
            }
            HideStar();
            m_TimeText.transform.localScale = Vector3.one * 0.8f;
        }

        private void HideStar()
        {
            for (int i = 0; i < m_CaleStar.Length; i++)
            {
                m_CaleStar[i].color = new Color(1, 1, 1, 0);
                m_CaleStar[i].transform.localScale = Vector3.one * 2;
                m_CaleStar[i].transform.localPosition = m_StarPosition[i];
            }
        }

        public void SetTime(int time, bool needJump = true)
        {
            m_TimeText.text = time.ToString();
            if (needJump)
            {
                if (m_LastTime != time)
                {
                    m_TimeText.transform.DOScale(Vector3.one, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.Linear);
                    m_LastTime = time;
                }
            }
            else
            {
                m_LastTime = time;
            }
        }

        public void Bigger(StudentRoot studentRoot, int starNum)
        {
            Sequence ms = DOTween.Sequence();
            Tween move = m_RectTransform.DOAnchorPosY(-320, 0.2f).SetEase(Ease.Linear);
            Tween scale = m_RectTransform.DOScale(Vector3.one, 0.2f);
            ms.Append(move);
            ms.Join(scale);

            for (int i = 0; i < starNum; i++)
            {
                Tween fade = m_CaleStar[i].DOFade(1, 0.5f);
                Tween starscale = m_CaleStar[i].transform.DOScale(Vector3.one, 0.5f);
                ms.Append(fade);
                ms.Join(starscale);
                
            }

            for (int i = 0; i < starNum; i++)
            {
                Tween fade = m_CaleStar[i].DOFade(0, 0.2f).OnComplete(() =>
                {
                    studentRoot.Flash(0.2f * starNum);

                });
                Tween fly = m_CaleStar[i].transform.DOMove(studentRoot.CurStudent.transform.position, 0.2f);
                ms.Append(fade);
                ms.Join(fly);
            }

            switch (starNum)
            {
                case 0:
                    GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_calc_0star,ref TopicOneForm.m_ClipMaxLength);
                    break;
                case 1:
                    GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_calc_1star,ref TopicOneForm.m_ClipMaxLength);
                    break;
                case 2:
                    GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_calc_2star, ref TopicOneForm.m_ClipMaxLength);
                    break;
                case 3:
                    GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_calc_3star, ref TopicOneForm.m_ClipMaxLength);
                    break;
                case 4:
                    GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_calc_4star, ref TopicOneForm.m_ClipMaxLength);
                    break;
                case 5:
                    GameEntry.Sound.PlaySoundAndLength((int)SoundId.s1_calc_5star, ref TopicOneForm.m_ClipMaxLength);
                    break;
            }
        }

        public void Small()
        {
            Sequence sequence = DOTween.Sequence();
            Tween move = m_RectTransform.DOAnchorPosY(m_InitPosition.y, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                HideStar();
            }); ;
            Tween scale = m_RectTransform.DOScale(m_InitScale, 0.2f);
            sequence.Append(move);
            sequence.Join(scale);
        }
    }

}