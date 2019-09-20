using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{

    public class MindMappingManager : XSingleton<MindMappingManager>
    {
        // 学员数据
        private List<StudentData> m_StudentDatas = new List<StudentData>();
        // 当前选择的课id
        private int m_CurLessonId = 0;
        // 第几节有X课 x=length
        private int m_LessonLength = 0;
        // 学员数
        private int m_StudentLength = 0;

        // 学员数据
        public List<StudentData> StudentDatas { get { return m_StudentDatas; } }
        // 当前选择的课id, 起始为1
        public int CurLessonId { get { return m_CurLessonId; } }
        // 第几节有X课 x=length
        public int LessonLength { get { return m_LessonLength; } }
        // 学员数
        public int StudentLength { get { return m_StudentLength; } }

        // 简单得分上限
        public readonly static int EasyScoreLimit = 10;
        // 困难得分上限
        public readonly static int TroubleScoreLimit = 20;

        /// <summary>
        /// 初始化思维课信息
        /// </summary>
        /// <param name="studentCount">学员数量</param>
        /// <param name="lessonLength">多少课</param>
        public void InitManager(int studentCount, int lessonLength)
        {
            m_StudentLength = studentCount;
            m_LessonLength = lessonLength;
            m_StudentDatas.Clear();
            for (int id = 0; id < studentCount; id++)
            {
                StudentData data = new StudentData(id, "学员:" + id, lessonLength);
                m_StudentDatas.Add(data);
            }
        }

        /// <summary>
        /// 进入下一课
        /// </summary>
        /// <returns></returns>
        public int EnterNextLesson()
        {
            m_CurLessonId++;
            if (m_CurLessonId > m_LessonLength)
            {
                m_CurLessonId = 1;
            }
            switch (m_CurLessonId)
            {
                case 1:
                    return (int)GameEntry.UI.OpenUIForm(UIFormId.TopicOneForm, this);
                case 2:
                    return (int)GameEntry.UI.OpenUIForm(UIFormId.Topic2Form, this);
                case 3:
                    return (int)GameEntry.UI.OpenUIForm(UIFormId.Topic3Form, this);
                case 4:
                    return (int)GameEntry.UI.OpenUIForm(UIFormId.Topic4Form, this);
                default:
                    return -1;
            }

        }

    }

}