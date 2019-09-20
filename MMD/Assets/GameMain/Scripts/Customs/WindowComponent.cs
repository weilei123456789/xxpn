using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class WindowComponent : GameFrameworkComponent
    {
        [SerializeField]
        private Vector2Int m_WallResolution = Vector2Int.zero;

        [SerializeField]
        private Vector2Int m_LandResolution = Vector2Int.zero;

        [SerializeField]
        private Display m_WallDisplay = null;
        [SerializeField]
        private Camera m_WallUICamera = null;
        [SerializeField]
        private Canvas m_WallUICanvas = null;

        [SerializeField]
        private Display m_GroundDisplay = null;
        [SerializeField]
        private Camera m_GroundUICamera = null;
        [SerializeField]
        private Canvas m_GroundUICanvas = null;

        [SerializeField]
        [Header("是否开启地面屏幕")]
        private bool m_IsOpenLand = false;

        [SerializeField]
        [Header("是否开启UI鼠标模拟")]
        private bool m_IsOpenMouseDebug = false;

        private GameFrameworkAction<GameObject, Vector3> WallRayCallBack = null;
        private GameFrameworkAction<GameObject, Vector3> GroundRayCallBack = null;

        public bool IsOpenLand
        {
            get { return m_IsOpenLand; }
        }

        public Camera WallUICamera
        {
            get { return m_WallUICamera; }
        }

        public Camera GroundUICamera
        {
            get { return m_GroundUICamera; }
        }

        public Canvas WallUICanvas
        {
            get { return m_WallUICanvas; }
        }

        public Canvas GroundUICanvas
        {
            get { return m_GroundUICanvas; }
        }

        private void Update()
        {
            if (m_IsOpenMouseDebug)
            {
                GameEntry.Windows.WallUICameraRay(Input.mousePosition);
                GameEntry.Windows.GroundUICameraRay(Input.mousePosition);
            }
        }

        public void InitWindowSetting(int screen_Wall_Width, int screen_Wall_Height, int screen_Land_Width, int screen_Land_Height)
        {
            SetResolution(screen_Wall_Width, screen_Wall_Height, screen_Land_Width, screen_Land_Height);
            InitCanvas();
        }

        /// <summary>
        /// 读取完配置调用
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private void SetResolution(int x1, int y1, int x2, int y2)
        {
            m_WallResolution = new Vector2Int(x1, y1);
            m_LandResolution = new Vector2Int(x2, y2);

            m_WallDisplay = Display.displays[0];
            m_WallDisplay.SetRenderingResolution(m_WallResolution.x, m_WallResolution.y);

            if (Display.displays.Length > 1 && m_IsOpenLand)
            {
                m_GroundDisplay = Display.displays[1];
                //激活第二面屏
                m_GroundDisplay.Activate();
                m_GroundDisplay.SetRenderingResolution(m_LandResolution.x,m_LandResolution.y);
            }
        }

        private void InitCanvas()
        {
            foreach (var item in GameEntry.UI.GetAllUIGroups())
            {
                if (item.Depth == 0)
                {
                    CanvasScaler wall = m_WallUICanvas.GetComponent<CanvasScaler>();
                    wall.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    wall.referenceResolution = new Vector2(m_WallResolution.x, m_WallResolution.y);
                }
                else if (item.Depth == 1)
                {
                    CanvasScaler land = m_GroundUICanvas.GetComponent<CanvasScaler>();
                    land.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    land.referenceResolution = new Vector2(m_LandResolution.x, m_LandResolution.y);
                }
            }
        }

        public void InitWallCanvas(Canvas _WallUICanvas)
        {
            m_WallUICanvas = _WallUICanvas;
        }

        public void InitLandCanvas(Canvas _landUICanvas)
        {
            m_GroundUICanvas = _landUICanvas;
        }

        #region UI相机照射的射线
        private Vector2 m_Origin = Vector2.zero;
        private RaycastHit2D m_RaycastHit;
        private GameObject m_ColliderObj = null;

        public void WallUICameraRay(Vector3 ve)
        {
            m_Origin = WallUICamera.ScreenToWorldPoint(ve);
            m_RaycastHit = Physics2D.Raycast(m_Origin, Vector2.zero, 100);
            if (m_RaycastHit.collider != null)
            {
                m_ColliderObj = m_RaycastHit.collider.gameObject;
                //抛出事件
                //GameEntry.Event.Fire(this, new LeiDaGameObjectEventArgs(m_ColliderObj, m_RaycastHit.point));
                if (WallRayCallBack != null)
                    WallRayCallBack(m_ColliderObj, ve);
            }

        }

        public void GroundUICameraRay(Vector3 ve)
        {
            m_Origin = GroundUICamera.ScreenToWorldPoint(ve);
            m_RaycastHit = Physics2D.Raycast(m_Origin, Vector2.zero, 100);
            if (m_RaycastHit.collider != null)
            {
                m_ColliderObj = m_RaycastHit.collider.gameObject;
                //抛出事件
                //GameEntry.Event.Fire(this, new LeiDaGameObjectEventArgs(m_ColliderObj, m_RaycastHit.point));
                if (GroundRayCallBack != null)
                    GroundRayCallBack(m_ColliderObj, ve);
            }
        }

        /// <summary>
        /// 注册墙屏射线回调
        /// </summary>
        /// <param name="_WallRayCallBack"></param>
        public void SubscribeUIWallEvent(GameFrameworkAction<GameObject, Vector3> _WallRayCallBack)
        {
            WallRayCallBack += _WallRayCallBack;
        }
        /// <summary>
        /// 取消墙屏射线回调
        /// </summary>
        /// <param name="_WallRayCallBack"></param>
        public void UnSubscribeUIWallEvent(GameFrameworkAction<GameObject, Vector3> _WallRayCallBack)
        {
            WallRayCallBack -= _WallRayCallBack;
        }
        /// <summary>
        /// 注册地屏射线回调
        /// </summary>
        /// <param name="_GroundRayCallBack"></param>
        public void SubscribeUIGroundEvent(GameFrameworkAction<GameObject, Vector3> _GroundRayCallBack)
        {
            GroundRayCallBack += _GroundRayCallBack;
        }
        /// <summary>
        /// 取消地屏射线回调
        /// </summary>
        /// <param name="_GroundRayCallBack"></param>
        public void UnSubscribeUIGroundEvent(GameFrameworkAction<GameObject, Vector3> _GroundRayCallBack)
        {
            GroundRayCallBack -= _GroundRayCallBack;
        }

        #endregion
    }

}
