using GameFramework;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Penny
{
    public static class ResourceUtility
    {
        /// <summary>
        /// 加载UI图片
        /// </summary>
        /// <param name="fileName">图片路径+图片名</param>
        /// <param name="image">加载完成后显示位置</param>
        public static void LoadUISprite(string fileName, Image image, bool isNativeSize = false)
        {
            GameEntry.Resource.LoadAsset(AssetUtility.GetUISpriteAsset(fileName), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    Texture2D t2d = asset as Texture2D;
                    image.sprite = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
                    if (isNativeSize)
                        image.SetNativeSize();
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load image '{0}' error message '{1}'.", assetName, errorMessage);
                }));
        }

        /// <summary>
        ///  加载UI图片
        /// </summary>
        /// <param name="fileName">图片路径+图片名</param>
        /// <param name="action">加载完成后回调方法</param>
        public static void LoadUISprite(string fileName, System.Action<Texture2D> action)
        {
            GameEntry.Resource.LoadAsset(AssetUtility.GetUISpriteAsset(fileName), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    //Log.Info("Load image '{0}' OK.", assetName);
                    action(asset as Texture2D);
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load image '{0}' error message '{1}'.", assetName, errorMessage);
                    action(null);
                }));
        }

        public static void LoadSpriteRenderer(string fileName, SpriteRenderer image)
        {
            GameEntry.Resource.LoadAsset(AssetUtility.GetUISpriteAsset(fileName), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    Texture2D t2d = asset as Texture2D;
                    //Debug.Log(t2d.width + "    " + t2d.height);
                    //Debug.Log(image.size);
                    image.sprite = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), new Vector2(0.5f, 0.5f));
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load image '{0}' error message '{1}'.", assetName, errorMessage);
                }));
        }

        public static void LoadUIItem(string fileName, GameFrameworkAction<GameObject> action)
        {
            GameEntry.Resource.LoadAsset(AssetUtility.GetUIItemAsset(fileName), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    Log.Info("Load UIItem prefab '{0}' OK.", assetName);

                    action(asset as GameObject);
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load UIItem prefab '{0}' error message '{1}'.", assetName, errorMessage);
                    action(null);
                }));
        }

        public static void LoadUIItem(string fileName, GameFrameworkAction<GameObject, string> action, string str)
        {
            GameEntry.Resource.LoadAsset(AssetUtility.GetUIItemAsset(fileName), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    Log.Info("Load UIItem prefab '{0}' OK.", assetName);

                    action(asset as GameObject, str);
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load UIItem prefab '{0}' error message '{1}'.", assetName, errorMessage);
                    action(null, null);
                }));
        }

        public static void LoadText(string fileName, GameFrameworkAction<string> action)
        {
            GameEntry.Resource.LoadAsset(AssetUtility.GetDataTableAsset(fileName,LoadType.Bytes), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    Log.Info("Load UIItem prefab '{0}' OK.", assetName);
                    action(asset.ToString());
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load UIItem prefab '{0}' error message '{1}'.", assetName, errorMessage);
                    action("");
                }));
        }

        public static void LoadGameSence(string seasonPath, string path, string fileName, GameFrameworkAction<GameObject> action)
        {
            GameEntry.Resource.LoadAsset(AssetUtility.GetGameSenceAsset(seasonPath, path,fileName), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    Log.Info("Load GameSence prefab '{0}' OK.", assetName);

                    action(asset as GameObject);
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load GameSence prefab '{0}' error message '{1}'.", assetName, errorMessage);
                    action(null);
                }));
        }

        public static void LoadGameSence(string seasonPath, string path, string fileName, GameFrameworkAction<GameObject, string> action, string str)
        {
            GameEntry.Resource.LoadAsset(AssetUtility.GetGameSenceAsset(seasonPath, path,fileName), new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    Log.Info("Load GameSence prefab '{0}' OK.", assetName);

                    action(asset as GameObject, str);
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load GameSence prefab '{0}' error message '{1}'.", assetName, errorMessage);
                    action(null, null);
                }));
        }



    }
}