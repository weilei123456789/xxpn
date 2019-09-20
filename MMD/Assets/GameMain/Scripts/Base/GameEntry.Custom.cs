//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace Penny
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry : MonoBehaviour
    {
        public static BuiltinDataComponent BuiltinData
        {
            get;
            private set;
        }

        public static HPBarComponent HPBar
        {
            get;
            private set;
        }

        public static WindowComponent Windows
        {
            get;
            private set;
        }

        public static SerialPortComponent SerialPort
        {
            get;
            private set;
        }

        public static WindowConfigComponent WindowsConfig
        {
            get;
            private set;
        }

        public static UrgComponent Urg
        {
            get;
            private set;
        }

        public static EthernetComponent Ethernet
        {
            get;
            private set;
        }

        public static VideoPlayerComponent VideoPlayer
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            HPBar = UnityGameFramework.Runtime.GameEntry.GetComponent<HPBarComponent>();
            Windows = UnityGameFramework.Runtime.GameEntry.GetComponent<WindowComponent>();
            SerialPort = UnityGameFramework.Runtime.GameEntry.GetComponent<SerialPortComponent>();
            WindowsConfig = UnityGameFramework.Runtime.GameEntry.GetComponent<WindowConfigComponent>();
            Urg = UnityGameFramework.Runtime.GameEntry.GetComponent<UrgComponent>();
            Ethernet = UnityGameFramework.Runtime.GameEntry.GetComponent<EthernetComponent>();
            VideoPlayer = UnityGameFramework.Runtime.GameEntry.GetComponent<VideoPlayerComponent>();
        }
    }
}
