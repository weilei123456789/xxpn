using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    public enum GameStateType 
    {
        /// <summary>    
        /// 重连
        /// </summary>
        ReConnect = -1,
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 等待web选择课件
        /// </summary>
        WaitWebChoiceCourseware = 1,
        /// <summary>
        /// 进入课件
        /// </summary>
        EnterCourseware = 2,

        /// <summary>
        /// 等待web选择游戏
        /// </summary>
        WaitWebChoiceGame = 3,
        /// <summary>
        /// 进入游戏
        /// </summary>
        EnterGame = 4,
    }

}