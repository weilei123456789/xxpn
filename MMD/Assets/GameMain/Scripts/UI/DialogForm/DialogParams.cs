//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace Penny
{
    /// <summary>
    /// 对话框显示数据。
    /// </summary>
    public class DialogParams
    {
        /// <summary>
        /// 关闭时间
        /// </summary>
        public float CloseTime
        {
            get;
            set;
        }

        /// <summary>
        /// 标题。
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 消息内容。
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 弹出窗口时是否暂停游戏。
        /// </summary>
        public bool PauseGame
        {
            get;
            set;
        }

        /// <summary>
        /// 确定按钮回调。
        /// </summary>
        public GameFrameworkAction<object> OnFinish
        {
            get;
            set;
        }

        /// <summary>
        /// 用户自定义数据。
        /// </summary>
        public string UserData
        {
            get;
            set;
        }
    }
}
