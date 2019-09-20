using UnityEngine;
using System.Collections;
namespace Penny
{
    public class NetProtocols
    {
        //----------------------------------------- client->server --------------------------------------------------------
        /// <summary>
        /// 心跳
        /// </summary>
        public static int CSHeartBeatProtocol = 10000;
        /// <summary>
        /// 心跳链接
        /// </summary>
        public static int CSHeartConnectProtocol = 10200;
        /// <summary>
        /// 老师扫脸登陆成功
        /// </summary>
        public static int CSTeacherLoginSuccessProtocol = 20100;
        /// <summary>
        /// 告知服务器本地课件列表,服务器不回
        /// </summary>
        public static int CSCallServerCoursewareListProtocol = 40000;
        /// <summary>
        /// 更改状态,服务器不回
        /// </summary>
        public static int CSSetGameStateSuccessProtocol = 30500;
        /// <summary>
        /// 大屏发起重启
        /// </summary>
        public static int CSGameRestartSuccessProtocol = 10400;
        /// <summary>
        /// 大屏接受老师顶替，清除数据后返回
        /// </summary>
        public static int CSChangeTeacherSuccessProtocol = 30600;
        /// <summary>
        /// 下课成功（未用上）
        /// </summary>
        public static int CSClassIsOverSuccessProtocols = 10600;
        /// <summary>
        /// 告诉遥控器游戏列表，服务器不回
        /// </summary>
        public static int CSGameListSuccessProtocols = 999990;

        //----------------------------------------- server->client --------------------------------------------------------
        /// <summary>
        /// 可以登陆
        /// </summary>
        public static int SCLoginProtocols = 20000;
        /// <summary>
        /// Web选择大课件
        /// </summary>
        public static int SCChoiceCoursewareProtocols = 30000;
        /// <summary>
        /// Web切换小课件
        /// </summary>
        public static int SCChangeCoursewareProtocols = 30100;
        /// <summary>
        /// Web切换学员登陆
        /// </summary>
        public static int SCChangeStudentLoginProtocols = 20200;
        /// <summary>
        /// Web返回课件列表
        /// </summary>
        public static int SCBackCoursewareListProtocols = 30200;
        /// <summary>
        /// Web难度按钮
        /// </summary>
        public static int SCOperationDifficultyProtocols = 30300;
        /// <summary>
        /// Web音量按钮
        /// </summary>
        public static int SCOperationVolumeProtocols = 30400;
        /// <summary>
        /// Web发起重启
        /// </sum0mary>
        public static int SCGameRestartProtocols = 10300;
        /// <summary>
        /// Web切换教师顶替
        /// </summary>
        public static int SCChangeTeacherProtocols = 10500;
        /// <summary>
        /// Web下课（未用上）
        /// </summary>
        public static int SCClassIsOverProtocols = 10700;
        /// <summary>
        /// Web选择主题曲
        /// </summary>
        public static int SCChoiceThemeSongProtocols = 30700;
        /// <summary>
        /// Web选择热身
        /// </summary>
        public static int SCChoiceWarmUpProtocols = 30800;
        /// <summary>
        /// Web选择屏保
        /// </summary>
        public static int SCChoiceScreenSaverProtocols = 30900;
        /// <summary>
        /// Web选择放松
        /// </summary>
        public static int SCChoiceRelaxProtocols = 31000;
        /// <summary>
        /// Web进入选择游戏
        /// </summary>
        public static int SCEnterGameProtocols = 999999;
        /// <summary>
        /// 选择游戏
        /// </summary>
        public static int SCChoiceGameProtocols = 999900;

    }
}