using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Penny
{
    public class Student : MonoBehaviour
    {
        [SerializeField]
        private RectTransform m_RectTransform = null;

        [SerializeField]
        private Image m_Head = null;

        [SerializeField]
        private Text m_Score = null;

        private int m_Id = 0;
        private int m_TotalScore = 0;
        // 当前题目得分
        private int m_CurSubjectScore = 0;

        public Vector2 AnchorPosition
        {
            get
            {
                return m_RectTransform.anchoredPosition;
            }
        }

        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        public int TotalScore
        {
            get
            {
                return m_TotalScore;
            }
        }

        /// <summary>
        /// 当前题目得分, 用于结算星星数
        /// </summary>
        public int CurSubjectScore
        {
            get
            {
                return m_CurSubjectScore;
            }
        }

        private StudentData m_StudentData = null;

        public void InitStudent(StudentData studentData)
        {
            m_StudentData = studentData;
            m_Score.text = studentData.CurScore.ToString();
            m_TotalScore = studentData.CurScore;
            m_Id = m_StudentData.Id;
            //ResourceUtility.LoadUISprite("Topic/head/touxiang", m_Head);
        }

        public void SetScore(int lessonId, DifficultyType difficultyType, int score)
        {
            if (m_StudentData != null)
            {
                m_CurSubjectScore = m_StudentData.SetScore(lessonId, difficultyType, score);
                m_TotalScore = m_StudentData.TotalScore(lessonId);
            }
        }

        public void Flash(float time = 0.2f)
        {
            if (!m_IsSplash)
            {
                m_CurTime = 0;
                m_MaxTime = time;
                m_IsSplash = true;
            }
        }

        private float m_MaxTime = 0;
        private float m_CurTime = 0;
        private bool m_IsSplash = false;

        private void Update()
        {
            if (m_IsSplash)
            {
                m_CurTime += Time.deltaTime;
                int _score = (int)(m_TotalScore * (m_CurTime / m_MaxTime));
                m_Score.text = (_score).ToString();
                if (m_CurTime > m_MaxTime)
                {
                    m_IsSplash = false;
                    m_Score.text = m_TotalScore.ToString();
                }
            }
        }
    }

}