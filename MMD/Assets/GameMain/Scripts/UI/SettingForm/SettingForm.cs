using GameFramework.Localization;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class SettingForm : UGuiForm
    {
        [SerializeField]
        private Slider m_VolumeSlider = null;

        private float m_MusicVolume = 0;
        private float m_SoundVolume = 0;
        private float m_UISoundVolume = 0;

        private float m_Volume = 0;

        private bool m_IsCutDown = false;
        private float m_CutDownTime = 0;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            //m_MusicVolume = GameEntry.Sound.GetVolume("Music");
            //m_SoundVolume = GameEntry.Sound.GetVolume("Sound");
            //m_UISoundVolume = GameEntry.Sound.GetVolume("UISound");
            //m_Volume = (m_MusicVolume + m_SoundVolume + m_UISoundVolume) / 3;

            m_Volume = (float)userData;
            m_VolumeSlider.value = m_Volume;
            Volume(m_Volume);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_IsCutDown)
            {
                m_CutDownTime += elapseSeconds;
                if (m_CutDownTime > 2f)
                {
                    m_IsCutDown = false;
                    m_CutDownTime = 0;
                    Close();
                }
            }
        }

        public void Volume(float value)
        {
            //gameObject.SetActive(true);
            if (value > 1) value = 1;
            else if (value < 0) value = 0;
            VolumeChanged(value);
            m_IsCutDown = true;
            m_CutDownTime = 0;
        }

        //public void MuteChanged(bool isOn)
        //{
        //    GameEntry.Sound.Mute("Music", !isOn);
        //    GameEntry.Sound.Mute("Sound", !isOn);
        //    GameEntry.Sound.Mute("UISound", !isOn);
        //}

        private void VolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("Music", volume);
            GameEntry.Sound.SetVolume("Sound", volume);
            GameEntry.Sound.SetVolume("UISound", volume);
            GameEntry.VideoPlayer.Volume = volume;
            m_VolumeSlider.value = volume;
        }

    }
}
