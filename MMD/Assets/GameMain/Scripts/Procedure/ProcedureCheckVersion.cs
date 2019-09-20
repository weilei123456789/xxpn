//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Event;
using GameFramework.Resource;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace Penny
{
    public class ProcedureCheckVersion : ProcedureBase
    {
        private bool m_VersionListUpdataComplete = false;
        private bool m_InitResourcesComplete = false;

        private VersionInfo m_VersionInfo = null;
        private UpdateVersionListCallbacks m_UpdateVersionListCallbacks = null;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("<color=lime>进入<检查version资源>流程.</color>");

            m_VersionListUpdataComplete = false;
            m_InitResourcesComplete = false;

            m_UpdateVersionListCallbacks = new UpdateVersionListCallbacks(OnVersionListUpdateSuccess, OnVersionListUpdateFailure);
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

            //是否是编辑器模式
            if (GameEntry.Base.EditorResourceMode)
            {
                m_InitResourcesComplete = true;
            }
            else
            {
                //单机模式直接初始化资源
                if (GameEntry.Resource.ResourceMode == GameFramework.Resource.ResourceMode.Package)
                {
                    GameEntry.Resource.InitResources(OnInitResourcesComplete);
                }
                else
                {
                    RequestVersion();
                }
            }
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            //是否是编辑器模式
            if (GameEntry.Base.EditorResourceMode)
            {
                if (!m_InitResourcesComplete)
                {
                    return;
                }
                ChangeState<ProcedurePreload>(procedureOwner);
            }
            else
            {
                if (GameEntry.Resource.ResourceMode == GameFramework.Resource.ResourceMode.Package)
                {
                    if (!m_InitResourcesComplete)
                    {
                        return;
                    }
                }
                else
                {
                    if (!m_VersionListUpdataComplete)
                    {
                        return;
                    }
                }
                //单机模式直接初始化资源
                if (GameEntry.Resource.ResourceMode == GameFramework.Resource.ResourceMode.Package)
                {
                    ChangeState<ProcedurePreload>(procedureOwner);
                }
                else
                {
                    //TODO: 进入资源跟新界面
                    //ChangeState<ProcedureUpdateGame>(procedureOwner);
                }
            }
        }

        private void RequestVersion()
        {
            GameEntry.WebRequest.AddWebRequest(GameEntry.BuiltinData.BuildInfo.CheckVersionUrl, this);
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            string responseJson = Utility.Converter.GetString(ne.GetWebResponseBytes());
            m_VersionInfo = Utility.Json.ToObject<VersionInfo>(responseJson);
            if (m_VersionInfo == null)
            {
                Log.Error("Parse VersionInfo failure.");
                return;
            }

            Log.Info("<color=lime>最新版本号： '{0}', 本地版本号 '{1}'.</color>", m_VersionInfo.LatestGameVersion, GameFramework.Version.GameVersion);
            if (!m_VersionInfo.GameNeedUpdate)
            {
                return;
            }
            GameEntry.Resource.UpdatePrefixUri = Utility.Path.GetCombinePath(m_VersionInfo.GameUpdateUrl, GetResourceVersionName());
            Log.Info("<color=lime>更新的URL:  '{0}/{1}'.</color>", m_VersionInfo.GameUpdateUrl, GetResourceVersionName());
            Log.Info("<color=lime>检查版本列表:  '{0}'.</color>", GameEntry.Resource.CheckVersionList(m_VersionInfo.InternalResourceVersion));
            if (GameEntry.Resource.CheckVersionList(m_VersionInfo.InternalResourceVersion) == GameFramework.Resource.CheckVersionListResult.Updated)
            {
                m_VersionListUpdataComplete = true;
            }
            else
            {
                GameEntry.Resource.UpdateVersionList(m_VersionInfo.VersionListLength, m_VersionInfo.VersionListHashCode, m_VersionInfo.VersionListZipLength, m_VersionInfo.VersionListZipHashCode, m_UpdateVersionListCallbacks);
            }
        }

        private void OnWebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            Log.Warning("Check version failure, error message： '{0}'.", ne.ErrorMessage);
        }

        private void OnInitResourcesComplete()
        {
            m_InitResourcesComplete = true;

            Log.Info("Init resources complete.");
        }

        //资源版本更新成功回调
        private void OnVersionListUpdateSuccess(string downloadPath, string downloadUri)
        {
            m_VersionListUpdataComplete = true;
            Log.Info("<color=lime>Download latest resource version list from '{0} || {1}' success.</color>", downloadPath, downloadUri);
        }

        //资源版本失败成功回调
        private void OnVersionListUpdateFailure(string downloadUri, string errorMessage)
        {
            Log.Warning("Download latest resource version list from '{0}' failure, error message '{1}'.", downloadUri, errorMessage);
            m_VersionListUpdataComplete = false;
        }

        private string GetResourceVersionName()
        {
            return string.Format("{0}.{1}", GameFramework.Version.GameVersion, m_VersionInfo.InternalResourceVersion.ToString());
        }

    }
}
