namespace Penny
{
    public enum UpdateType
    {
        /// <summary>
        /// 准备更新
        /// </summary>
        ReadyUpdate = 0,

        /// <summary>
        /// 检查版本
        /// </summary>
        CheackVersion = 1,

        /// <summary>
        /// 检查版本完毕
        /// </summary>
        CheackVersionFinish,

        /// <summary>
        /// 更新
        /// </summary>
        UpdateResource,

        /// <summary>
        /// 检查课件
        /// </summary>
        CheackCourseware,

        /// <summary>
        /// 下载课件
        /// </summary>
        UpdateCourseware,

        /// <summary>
        /// 更新完成
        /// </summary>
        UpdateCoursewareFinish,
    }

}