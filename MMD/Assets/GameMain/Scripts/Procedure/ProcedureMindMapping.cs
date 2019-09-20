using GameFramework.DataTable;
using GameFramework.Event;
using System;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Penny
{
    public class ProcedureMindMapping : ProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public static int s_OpenFormSerialId = 0;
        private int m_ViceFormSerialId = -1;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            IsEnterNextProduce = false;
            IsGrabLoginTeacher = false;
            IsBackInitProceduce = false;

            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);

            LoadGameScene();

            MindMappingManager.Instance.InitManager(7, 4);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!IsEnterNextProduce)
            {
                return;
            }
            //ChangeState<ProcedureInit>(procedureOwner);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            if (GameEntry.UI.HasUIForm(s_OpenFormSerialId))
                GameEntry.UI.CloseUIForm(s_OpenFormSerialId);
            if (GameEntry.UI.HasUIForm(m_ViceFormSerialId))
                GameEntry.UI.CloseUIForm(m_ViceFormSerialId);
            base.OnLeave(procedureOwner, isShutdown);
        }

        /// <summary>
        /// 加载游戏场景
        /// </summary>
        public void LoadGameScene()
        {
            //加载场景
            int sceneId = GameEntry.Config.GetInt("Scene.Mind");
            IDataTable<DRScene> dtScene = GameEntry.DataTable.GetDataTable<DRScene>();
            DRScene drScene = dtScene.GetDataRow(sceneId);
            if (drScene == null)
            {
                Log.Warning("Can not load scene '{0}' from data table.", sceneId.ToString());
                return;
            }
            GameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), Constant.AssetPriority.SceneAsset, this);

        }

        private void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);
            // 打开UI
            s_OpenFormSerialId = MindMappingManager.Instance.EnterNextLesson();

            if (GameEntry.Windows.IsOpenLand)
                m_ViceFormSerialId = (int)GameEntry.UI.OpenUIForm(UIFormId.TopicViceForm, this);

        }

        private void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
        }

    }
}
