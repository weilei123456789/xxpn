//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace Penny
{
    public static class AssetUtility
    {
        public static string GetConfigAsset(string assetName, LoadType loadType)
        {
            return Utility.Text.Format("Assets/GameMain/Configs/{0}.{1}", assetName, loadType == LoadType.Text ? "txt" : "bytes");
        }

        public static string GetDataTableAsset(string assetName, LoadType loadType)
        {
            return Utility.Text.Format("Assets/GameMain/DataTables/{0}.{1}", assetName, loadType == LoadType.Text ? "txt" : "bytes");
        }

        public static string GetDictionaryAsset(string assetName, LoadType loadType)
        {
            return Utility.Text.Format("Assets/GameMain/Localization/{0}/Dictionaries/{1}.{2}", GameEntry.Localization.Language.ToString(), assetName, loadType == LoadType.Text ? "xml" : "bytes");
        }

        public static string GetFontAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Fonts/{0}.ttf", assetName);
        }

        public static string GetSceneAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Scenes/{0}.unity", assetName);
        }

        public static string GetMusicAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Music/{0}.wav", assetName);
        }

        public static string GetSoundAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Sounds/{0}.wav", assetName);
        }

        public static string GetEntityAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Entities/{0}.prefab", assetName);
        }

        public static string GetUIFormAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/UI/UIForms/{0}.prefab", assetName);
        }

        public static string GetUISoundAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/UI/UISounds/{0}.wav", assetName);
        }

        public static string GetMoviceAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/Movice/{0}.mp4", assetName);
        }

        public static string GetUISpriteAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/UI/UISprites/{0}.png", assetName);
        }

        public static string GetUIItemAsset(string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/UI/UIItems/{0}.prefab", assetName);
        }

        public static string GetGameSenceAsset(string seasonPath, string path ,string assetName) {
            return Utility.Text.Format("Assets/GameMain/{0}/{1}/GameScene/{2}.prefab", seasonPath, path, assetName);
        }

        public static string GetLessonEntityAsset(string seasonPath, string path,string assetName)
        {
            return Utility.Text.Format("Assets/GameMain/{0}/{1}/Entities/{2}.prefab", seasonPath, path, assetName);
        }

        public static string GetTTSSoundAsset(string seasonPath, string path, string assetName) {
            return Utility.Text.Format("Assets/GameMain/{0}/{1}/Sounds/{2}.wav", seasonPath, path, assetName);
        }

        public static string GetLuaAsset(string assetName)
        {
            return string.Format("Assets/GameMain/LuaScripts/{0}.lua", assetName);
        }

        public static string GetLuaListAsset(string assetName)
        {
            return string.Format("Assets/GameMain/LuaScripts/{0}.bytes", assetName);
        }
    }
}
