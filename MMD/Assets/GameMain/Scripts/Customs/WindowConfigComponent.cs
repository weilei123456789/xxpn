using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;


namespace Penny
{
    public class Config
    {
        public int Serial_BaudRate { get; set; }
        public string Serial_Port { get; set; }
        public double Serial_Scale { get; set; }
        public int Serial_Offset_Width { get; set; }
        public int Serial_Offset_Height { get; set; }

        public string URG_Address { get; set; }
        public int URG_Port { get; set; }
        public double URG_Scale { get; set; }
        public int URG_Offset_Width { get; set; }
        public int URG_Offset_Height { get; set; }

        public int Screen_Wall_Width { get; set; }
        public int Screen_Wall_Height { get; set; }

        public int Screen_Land_Width { get; set; }
        public int Screen_Land_Height { get; set; }

        public string Socket_IP { get; set; }
        public int Socket_Port { get; set; }

        public int DeviceNumber { get; set; }

        public bool IsKinect { get; set; }
        public string Http_IP { get; set; }

    }

    public class WindowConfigComponent : GameFrameworkComponent
    {
        [SerializeField]
        private string m_ConfigTextAssetName = null;

        private Config m_Config = null;

        public Config Config
        {
            get { return m_Config; }
        }

        public void InitCustomConfig(GameFrameworkAction<Config> readFinish)
        {
            string path = StreamingAsserts(m_ConfigTextAssetName);
            Log.Info(path);
            StartCoroutine(ReadData(path, readFinish));
        }

        private IEnumerator ReadData(string path, GameFrameworkAction<Config> readFinish)
        {
            WWW www = new WWW(path);
            yield return www;
            while (!www.isDone)
            {
                yield return new WaitForEndOfFrame();
            }
            string json = www.text;
            if (json.Equals(string.Empty))
            {
                Log.Error("Error: Config文件不存在！！！！");
                yield break;
            }
            Log.Info(json);
            m_Config = Utility.Json.ToObject<Config>(json);
            if (readFinish != null)
                readFinish(m_Config);
        }

        public static string StreamingAsserts(string name)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return Application.streamingAssetsPath + "/" + name;
#elif UNITY_IPHONE && !UNITY_EDITOR
            return "file://" + Application.streamingAssetsPath + "/" + name;
#elif UNITY_STANDLONE_WIN && !UNITY_EDITOR
            return Utility.Path.GetCombinePath("file://", Application.streamingAssetsPath, name);
#else
            return Utility.Path.GetCombinePath("file://", Application.streamingAssetsPath, name);
#endif

        }
    }

}