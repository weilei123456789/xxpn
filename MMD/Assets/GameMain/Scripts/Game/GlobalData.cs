using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    internal class GlobalData
    {
        internal static readonly int ConnectMaxCount = 3;

        // 配置到config.json
        internal static string Url = "";

        //查询用户信息(根据faceToken)
        internal static readonly string Server_Getuserbyfacetoken = "getuserbyfacetoken";
        //教师签到
        internal static readonly string Server_TeacherSigninTimestamp = "getcoursesdailybyteachertimestamp";
        //学生签到
        internal static readonly string Server_StudentSigninTimestamp = "addcoursesdetail";
        //修改结束课程时间
        internal static readonly string Server_UpdateClassDailyEndtime = "updateclassdailyendtime";
        //修改开始课程时间
        internal static readonly string Server_UpdateClassDailyStarttime = "updateclassdailystarttime";
        //查询服务器课件信息
        internal static readonly string Server_GetCoursewareById = "getcoursewarebyid";
        //查询设备课件信息
        internal static readonly string Server_GetDevicewareById = "getdevicesbyid";
        //查询全部课程
        internal static readonly string Server_GetCoursewareDailyByClassestime = "getcoursesdailybyclassestime";

        internal static List<UserData> LoginTeacherDatas = new List<UserData>();
        internal static List<UserData> LoginStudentDatas = new List<UserData>();

        /// <summary>
        /// 老师登录获取的课程
        /// </summary>
        internal static CoursesDailyMap CoursesDailyMap = null;
        /// <summary>
        /// 当前游戏状态
        /// </summary>
        internal static GameStateType GameStateType = GameStateType.Unknown;

        /// <summary>
        /// 游戏最大难度
        /// </summary>
        internal static readonly int GameDifficultyMax = 3;

        /// <summary>
        /// Teacher-UID
        /// </summary>
        internal static string SC_TeacherUid = string.Empty;

        /// <summary>
        /// Teacher-ID
        /// </summary>
        internal static int? SC_TeacherClassDaily = 0;

        /// <summary>
        /// uid
        /// </summary>
        internal static string Web_Uid = string.Empty;

        /// <summary>
        /// 清除老师信息
        /// </summary>
        internal static void ClearTeacher()
        {
            LoginTeacherDatas.Clear();
        }
        /// <summary>
        /// 清除学员信息
        /// </summary>
        internal static void ClearStudents()
        {
            LoginStudentDatas.Clear();
        }

        /// <summary>
        /// 是否有学员了
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        internal static bool HasStudent(UserData userData)
        {
            foreach (var item in LoginStudentDatas)
            {
                if (item.name == userData.name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 重启游戏
        /// </summary>
        internal static void RestartGame()
        {
            ClearTeacher();
            ClearStudents();
            SC_TeacherUid = string.Empty;
            Web_Uid = string.Empty;
            GameStateType = GameStateType.Unknown;
        }

    }
}