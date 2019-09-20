using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class StudentRoot : MonoBehaviour
    {
        [SerializeField]
        private List<Student> m_Students = null;

        [SerializeField]
        private RectTransform m_Arrow = null;

        private int m_CurIndex = -1;

        public int TotalScore
        {
            get
            {
                return FindStuden(m_CurIndex).TotalScore;
            }
        }

        public Student CurStudent
        {
            get
            {
                return FindStuden(m_CurIndex);
            }
        }

        private void Start()
        {
            Clear();
        }

        public void Clear()
        {
            m_CurIndex = -1;
            for (int i = 0; i < m_Students.Count; i++)
            {
                if (i < MindMappingManager.Instance.StudentLength)
                {
                    m_Students[i].InitStudent(MindMappingManager.Instance.StudentDatas[i]);
                }
                else
                {
                    Log.Error("Error:学员头像少于登录学员数!!");
                }
            }
            InitArrow();
        }

        public void SetScore(DifficultyType difficultyType, int score)
        {
            CurStudent.SetScore(MindMappingManager.Instance.CurLessonId, difficultyType, score);
        }

        public void Flash(float time = 0.2f)
        {
            CurStudent.Flash(time);
        }

        public void NextStudent()
        {
            m_CurIndex++;
            if (m_CurIndex >= m_Students.Count)
                m_CurIndex = 0;
            SetArrowY(m_CurIndex);
        }

        private void InitArrow()
        {
            Vector2 anchoredPosition = FindStuden(0).AnchorPosition;
            anchoredPosition.x = anchoredPosition.x + 135;
            m_Arrow.anchoredPosition = anchoredPosition;
            m_Arrow.DOAnchorPosX(m_Arrow.anchoredPosition.x + 35, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }

        private void SetArrowY(int index)
        {
            m_CurIndex = index;
            Vector2 anchoredPosition = CurStudent.AnchorPosition;
            anchoredPosition.x = m_Arrow.anchoredPosition.x;
            m_Arrow.anchoredPosition = anchoredPosition;
        }

        private Student FindStuden(int idnex)
        {
            foreach (var item in m_Students)
            {
                if (item.Id == idnex)
                {
                    return item;
                }
            }
            return m_Students[0];
        }
    }

}