using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    public class StudentData
    {
        private int m_Id = 0;
        private int[] m_EasyScore = null;
        private int[] m_TroubleScore = null;
        private string m_StudentName = string.Empty;
        // 课的长度
        private int m_LessonLength = 0;
        // 累计的总分
        private int m_CurScore = 0;

        public StudentData(int id, string name, int lessonLength)
        {
            InitStudentData(id, name, lessonLength);
        }

        private void InitStudentData(int id, string name, int lessonLength)
        {
            this.m_Id = id;
            this.m_StudentName = name;
            this.m_LessonLength = lessonLength;
            this.m_EasyScore = new int[m_LessonLength];
            this.m_TroubleScore = new int[m_LessonLength];
        }

        public int SetScore(int lessonId, DifficultyType difficultyType, int score)
        {
            if (difficultyType == DifficultyType.Easy)
            {
                if (score > m_EasyScore[lessonId - 1])
                {
                    m_EasyScore[lessonId - 1] = score;
                }
            }
            else if (difficultyType == DifficultyType.Trouble)
            {
                if (score > m_TroubleScore[lessonId - 1])
                    m_TroubleScore[lessonId - 1] += score;
            }
            return score;
        }

        public int TotalScore(int lessonId)
        {
            if (lessonId > m_LessonLength)
                return -9999;
            m_CurScore = 0;
            for (int i = 0; i < lessonId; i++)
            {
                m_CurScore += (m_EasyScore[i] + m_TroubleScore[i]);
            }
            //m_CurScore /= 10.0f;
            return m_CurScore;
        }

        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        public string StudentName
        {
            get
            {
                return m_StudentName;
            }
        }

        public int CurScore
        {
            get
            {
                return m_CurScore;
            }
        }

    }

}