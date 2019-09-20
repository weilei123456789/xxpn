//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace Penny
{
    /// <summary>
    /// 界面编号。
    /// </summary>
    public enum UIFormId
    {
        Undefined = 0,

        /// <summary>
        /// 弹出框。
        /// </summary>
        DialogForm = 1,

        /// <summary>
        /// 主菜单。
        /// </summary>
        MenuForm = 100,

        /// <summary>
        /// 设置。
        /// </summary>
        SettingForm = 101,

        /// <summary>
        /// 关于。
        /// </summary>
        AboutForm = 102,

        /// <summary>
        /// 链接
        /// </summary>
        ConnectForm = 103,

        /// <summary>
        /// 初始化界面
        /// </summary>
        InitializationForm = 104,

        /// <summary>
        /// 初始化副界面
        /// </summary>
        InitializationViceForm = 105,

        /// <summary>
        /// 老师刷脸登陆。
        /// </summary>
        TeacherFaceLoginForm = 1000,

        /// <summary>
        /// 选择课件。
        /// </summary>
        SelCoursewareForm = 1001,

        /// <summary>
        /// 学生刷脸登陆。
        /// </summary>
        StudentFaceLoginForm = 1002,

        /// <summary>
        /// 老师刷脸副登陆。
        /// </summary>
        TeacherFaceLoginViceForm = 1003,

        /// <summary>
        /// 学生刷脸副登陆。
        /// </summary>
        StudentFaceLoginViceForm = 1004,

        /// <summary>
        /// 选择副课件。
        /// </summary>
        SelCoursewareViceForm = 1005,

        /// <summary>
        /// 视频播放界面
        /// </summary>
        VideoPlayerForm = 2001,

        /// <summary>
        /// 视频播放界面
        /// </summary>
        VideoPlayerGroundForm = 2002,

        /// <summary>
        /// 第一题
        /// </summary>
        TopicOneForm = 2003,

        /// <summary>
        /// 通用地板
        /// </summary>
        TopicViceForm = 2004,

        /// <summary>
        /// 第二题
        /// </summary>
        Topic2Form = 2005,

        /// <summary>
        /// 第三题
        /// </summary>
        Topic3Form = 2006,

        /// <summary>
        /// 第四题
        /// </summary>
        Topic4Form = 2007,

        /// <summary>
        /// Lua测试
        /// </summary>
        LuaTestForm = 999,
    }
}
