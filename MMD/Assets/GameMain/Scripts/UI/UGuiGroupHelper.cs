//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Penny
{
    /// <summary>
    /// uGUI 界面组辅助器。
    /// </summary>
    public class UGuiGroupHelper : UIGroupHelperBase
    {
        //画布宽度
        public int Screen_width = 1920;
        //画布高度
        public int Screen_height = 1080;
        //深度因子
        public const int DepthFactor = 1;
        //深度
        public int m_Depth = 0;
        //画布
        private Canvas m_CachedCanvas = null;

        public override void SetDepth(int depth)
        {
            string format = "初始化自定义UI界面辅助器, 界面深度 '{0}'.";
            Log.Info("<color=lime>" + format + "</color>", depth);
            m_Depth = depth;
            m_CachedCanvas.overrideSorting = true;
            m_CachedCanvas.sortingOrder = DepthFactor * depth;
        }

        private void Awake()
        {
            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            gameObject.GetOrAddComponent<GraphicRaycaster>();
        }

        private void Start()
        {
            InitCanvas();
            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;
        }
        private void InitCanvas()
        {
            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            if (m_Depth == 0)
            {
                m_CachedCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                m_CachedCanvas.sortingLayerName = name;
                m_CachedCanvas.sortingOrder = DepthFactor * m_Depth;
                SetWorldCamera(GameEntry.Windows.WallUICamera, 0);

                GameEntry.Windows.InitWallCanvas(m_CachedCanvas);
            }
            else if (m_Depth == 1)
            {
                m_CachedCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                m_CachedCanvas.sortingLayerName = name;
                m_CachedCanvas.sortingOrder = DepthFactor * m_Depth;
                SetWorldCamera(GameEntry.Windows.GroundUICamera, 0);

                GameEntry.Windows.InitLandCanvas(m_CachedCanvas);
            }

            m_CachedCanvas.planeDistance = 20;

            CanvasScaler csc = gameObject.GetOrAddComponent<CanvasScaler>();
            csc.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            csc.referenceResolution = new Vector2(Screen_width, Screen_height);
            csc.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            csc.matchWidthOrHeight = 1;
            gameObject.GetOrAddComponent<GraphicRaycaster>();

        }

        public void SetWorldCamera(Camera camera, int distance = 100)
        {
            m_CachedCanvas.worldCamera = camera;
            m_CachedCanvas.planeDistance = distance;
        }

        public void SetLayer(string nameLayer)
        {
            gameObject.layer = LayerMask.NameToLayer(nameLayer);
        }
    }
}
