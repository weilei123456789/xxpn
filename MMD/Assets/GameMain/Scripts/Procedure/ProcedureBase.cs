//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace Penny
{
    public abstract class ProcedureBase : GameFramework.Procedure.ProcedureBase
    {
        public abstract bool UseNativeDialog
        {
            get;
        }

        /// <summary>
        /// 是否进入下一环节
        /// </summary>
        protected bool IsEnterNextProduce = false;
        /// <summary>
        /// 进入下一环节
        /// </summary>
        public void NextProduce()
        {
            IsEnterNextProduce = true;
        }

        /// <summary>
        /// 是否有老师抢登陆
        /// </summary>
        protected bool IsGrabLoginTeacher = false;
        /// <summary>
        /// 老师抢登陆
        /// </summary>
        public void GrabLoginTeacher()
        {
            IsGrabLoginTeacher = true;
        }

        /// <summary>
        /// 是否返回初始化流程
        /// </summary>
        protected bool IsBackInitProceduce = false;
        /// <summary>
        /// 老师抢登陆
        /// </summary>
        public void BackInitProceduce()
        {
            IsBackInitProceduce = true;
        }

    }
}
