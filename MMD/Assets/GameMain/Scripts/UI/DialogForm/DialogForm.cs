//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class DialogForm : UGuiForm
    {
        [SerializeField]
        private Text m_TitleText = null;

        [SerializeField]
        private Text m_MessageText = null;

        private float m_CloseTime = 1;
        private bool m_PauseGame = false;
        private bool m_RunOnce = false;
        private object m_UserData = null;
        private GameFrameworkAction<object> m_OnFinish = null;

        public float CloseTime
        {
            get
            {
                return m_CloseTime;
            }
        }

        public bool PauseGame
        {
            get
            {
                return m_PauseGame;
            }
        }

        public object UserData
        {
            get
            {
                return m_UserData;
            }
        }

        public void OnOK()
        {
            if (m_OnFinish != null)
            {
                m_OnFinish(m_UserData);
            }
            Close(true);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            DialogParams dialogParams = (DialogParams)userData;
            if (dialogParams == null)
            {
                Log.Warning("DialogParams is invalid.");
                return;
            }

            m_CloseTime = dialogParams.CloseTime;
            m_TitleText.text = dialogParams.Title;
            m_MessageText.text = dialogParams.Message;
            m_PauseGame = dialogParams.PauseGame;
            RefreshPauseGame();
            m_UserData = dialogParams.UserData;
            m_OnFinish = dialogParams.OnFinish;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            m_CloseTime -= elapseSeconds;
            //Log.Info(m_CloseTime);
            if (m_CloseTime < 0)
            {
                if (!m_RunOnce)
                {
                    m_RunOnce = true;
                    OnOK();
                }
            }
        }

        protected override void OnClose(object userData)
        {
            if (m_PauseGame)
            {
                GameEntry.Base.ResumeGame();
            }

            m_CloseTime = 0;
            m_TitleText.text = string.Empty;
            m_MessageText.text = string.Empty;
            m_PauseGame = false;
            m_RunOnce = false;
            m_UserData = null;

            m_OnFinish = null;

            base.OnClose(userData);
        }

        private void RefreshPauseGame()
        {
            if (m_PauseGame)
            {
                GameEntry.Base.PauseGame();
            }
        }

    }
}
