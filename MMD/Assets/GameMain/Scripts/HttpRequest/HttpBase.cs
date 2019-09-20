using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class HttpBase
    {
        public delegate void SuccessCallBack(string response, IDictable userData);
        public delegate void FailedCallBack(string error, IDictable userData);

        protected string m_EventName { set; get; }
        protected SuccessCallBack m_SuccessAction { set; get; }
        protected FailedCallBack m_FailureAction { set; get; }
        protected int m_FailureCount = 0;
        protected int m_SerialId = -1;
        protected IDictable m_HttpSendData = null;
        protected Dictionary<string, string> m_HttpForm = new Dictionary<string, string>();
        protected byte[] m_HttpByteData = null;

        private string m_TempJsonResponse = string.Empty;
        private bool m_IsSendHttpForm = false;

        protected HttpBase(string name, SuccessCallBack success, FailedCallBack failure, bool needUI = true)
        {
            m_EventName = name;
            m_SuccessAction = success;
            m_FailureAction = failure;
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
            m_SerialId = needUI ? (int)GameEntry.UI.OpenUIForm(UIFormId.ConnectForm, this) : -1;
        }

        /// <summary>
        /// 移出注册的事件
        /// </summary>
        protected void RemoveEvent()
        {
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        public void SendMsg()
        {
            m_IsSendHttpForm = false;
            if (m_HttpSendData == null)
            {
                Log.Warning("Send data is null !");
                return;
            }
            GameEntry.WebRequest.AddWebRequest(GlobalData.Url + m_EventName, m_HttpSendData.ToJsonData(), this);
        }

        /// <summary>
        /// 发送表单请求
        /// </summary>
        public virtual void SendMsgForm()
        {
            m_IsSendHttpForm = true;
            if (m_HttpForm == null)
            {
                Log.Warning("Send Form is null !");
                return;
            }
            // GameEntry.WebRequest.AddWebRequest(GlobalData.Url + m_EventName, m_WWWForm, this);
            m_TempJsonResponse = HttpUploadFileHelper.HttpUploadFile(GlobalData.Url + m_EventName, m_HttpByteData, "Test.png", m_HttpForm);
            if (m_SuccessAction != null && !string.IsNullOrEmpty(m_TempJsonResponse))
            {
                m_HttpSendData.fromDict(m_TempJsonResponse);
                m_SuccessAction(m_TempJsonResponse, m_HttpSendData);
                if (m_SerialId != -1) GameEntry.UI.CloseUIForm(m_SerialId);
            }
            else
            {
                TimeOut("Connect Time out, Check your network or whether the server is on !");
            }
            RemoveEvent();
        }

        /// <summary>
        /// 接收服务端的返回消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;
            if (ne.UserData != this) return;
            m_TempJsonResponse = Utility.Converter.GetString(ne.GetWebResponseBytes());
            if (m_SuccessAction != null && !string.IsNullOrEmpty(m_TempJsonResponse))
            {
                m_HttpSendData.fromDict(m_TempJsonResponse);
                m_SuccessAction(m_TempJsonResponse, m_HttpSendData);
                if (m_SerialId != -1) GameEntry.UI.CloseUIForm(m_SerialId);
            }
            else
            {
                if (string.IsNullOrEmpty(m_TempJsonResponse))
                    Log.Warning("Return string json is null ！");
            }
            RemoveEvent();
        }

        /// <summary>
        /// 响应失败
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;
            if (ne.UserData != this) return;
            if (m_FailureCount < GlobalData.ConnectMaxCount)
            {
                Reconnect();
            }
            else
            {
                TimeOut("Connect Time out, Check your network or whether the server is on !");
            }
        }

        /// <summary>
        /// 重新连接
        /// </summary>
        /// <param name="error"></param>
        /// <param name="userData"></param>
        private void Reconnect()
        {
            m_FailureCount++;
            if (!m_IsSendHttpForm)
            {
                SendMsg();
            }
            else
            {
                SendMsgForm();
            }
            Log.Warning("ReConnect，Current times:{0} !", m_FailureCount);
        }

        private void TimeOut(string error)
        {
            if (m_FailureAction != null)
            {
                m_FailureAction(error, m_HttpSendData);
            }
            RemoveEvent();
        }

        /// <summary>
        /// 重新登录
        /// </summary>
        /// <param name="response"></param>
        /// <param name="userData"></param>
        public void ReLogin()
        {
            RemoveEvent();
            if (m_SerialId != -1) GameEntry.UI.CloseUIForm(m_SerialId);
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
        }
    }

}
