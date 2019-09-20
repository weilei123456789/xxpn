using GameFramework;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.Video;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class VideoPlayerComponent : GameFrameworkComponent
    {
        [SerializeField]
        private RenderTexture m_RenderTexture = null;
        [SerializeField]
        private VideoPlayer m_VideoPlayer = null;
        [SerializeField]
        private AudioSource m_AudioSource = null;

        public RenderTexture Texture
        {
            get
            {
                return m_RenderTexture;
            }
        }

        public float Volume
        {
            set
            {
                m_AudioSource.volume = value;
            }
            get
            {
                return m_AudioSource.volume;
            }
        }

        public VideoPlayer.EventHandler VideoOpenHandler = null;
        public VideoPlayer.EventHandler VideoPlayEndHandler = null;


        private void Start()
        {
            m_VideoPlayer.playOnAwake = false;
            //视频准备完成时被执行。
            m_VideoPlayer.prepareCompleted += OpenVideoCompleted;
            //播放结束或播放到循环的点时被执行。
            m_VideoPlayer.loopPointReached += PlayVideoCompleted;
            m_VideoPlayer.Prepare();
        }

        private void OnDestroy()
        {
            m_VideoPlayer.prepareCompleted -= OpenVideoCompleted;
            m_VideoPlayer.loopPointReached -= PlayVideoCompleted;
            m_RenderTexture = null;
            m_VideoPlayer = null;
            VideoOpenHandler = null;
            VideoPlayEndHandler = null;
        }

        private void OpenVideoCompleted(VideoPlayer source)
        {
            if (VideoOpenHandler != null)
            {
                VideoOpenHandler(source);
            }
        }

        private void PlayVideoCompleted(VideoPlayer source)
        {
            if (VideoPlayEndHandler != null)
            {
                VideoPlayEndHandler(source);
            }
        }

        public void Pause()
        {
            if (m_VideoPlayer == null)
            {
                Log.Debug("Video Player is Null !!!!!");
                return;
            }
            m_VideoPlayer.Pause();
        }

        public void Stop()
        {
            if (m_VideoPlayer == null)
            {
                Log.Debug("Video Player is Null !!!!!");
                return;
            }
            m_VideoPlayer.isLooping = false;
            m_VideoPlayer.Stop();
            m_VideoPlayer.clip = null;
            m_RenderTexture.Release();
        }

        public void SetRenderMode(VideoRenderMode mode)
        {
            if (m_VideoPlayer == null)
            {
                Log.Debug("Video Player is Null !!!!!");
                return;
            }
            m_VideoPlayer.renderMode = mode;
        }

        public void PlayLoadMovice(string moviceName,bool isloop = true)
        {
            GameEntry.Resource.LoadAsset(AssetUtility.GetMoviceAsset(moviceName), Constant.AssetPriority.Highest, new LoadAssetCallbacks(
              (assetName, asset, duration, userData) =>
              {
                  m_VideoPlayer.clip = (VideoClip)asset;
                  m_VideoPlayer.isLooping = isloop;
                  m_VideoPlayer.SetTargetAudioSource(0, m_AudioSource);
                  m_VideoPlayer.Play();
              },

              (assetName, status, errorMessage, userData) =>
              {
              }));
        }
    }
}

