using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


namespace Penny
{

    public class VideoPlayerForm : UGuiForm
    {
        [SerializeField]
        private RawImage Video = null;

        private int m_SerialId = -1;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            Video.texture = GameEntry.VideoPlayer.Texture;
            PlayVideo((string)userData);

            m_SerialId = (int)GameEntry.UI.OpenUIForm(UIFormId.VideoPlayerGroundForm);
        }

        private void PlayVideo(string videoName)
        {

            Video.gameObject.SetActive(true);

            GameEntry.VideoPlayer.PlayLoadMovice(videoName, true);

        }


        protected override void OnClose(object userData)
        {
            if (GameEntry.UI.HasUIForm(m_SerialId))
                GameEntry.UI.CloseUIForm(m_SerialId);
            GameEntry.VideoPlayer.Stop();
            base.OnClose(userData);

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}