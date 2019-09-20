using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Penny
{
    public class UrgGUI : MonoBehaviour
    {
        [SerializeField]
        private bool m_DebugGUI = true;

        [SerializeField]
        private float m_CanvasReandingWeidth;
        [SerializeField]
        private float m_CanvasReandingHeight;

        private CanvasScaler m_WallCanvas;

        void OnGUI()
        {
            if (!m_DebugGUI) return;

            m_WallCanvas = GameEntry.Windows.WallUICanvas.GetComponent<CanvasScaler>();
            m_CanvasReandingWeidth = m_WallCanvas.referenceResolution.x;
            m_CanvasReandingHeight = m_WallCanvas.referenceResolution.y;
            
            if (GUILayout.Button("隐藏/显示"))
            {
                GameEntry.Urg.DebugDraw = !GameEntry.Urg.DebugDraw;
            }

            //GameEntry.Urg.IpAddress = GUILayout.TextField(GameEntry.Urg.IpAddress);
            if (GUILayout.Button("连接URG"))
            {
                GameEntry.Urg.StartUrgEthernet(GameEntry.Urg.IpAddress);
            }

            string readingW = GUILayout.TextField(m_CanvasReandingWeidth.ToString());
            m_CanvasReandingWeidth = float.Parse(readingW);
            string readingH = GUILayout.TextField(m_CanvasReandingHeight.ToString());
            m_CanvasReandingHeight = float.Parse(readingH);
            if (GUILayout.Button("修改画布渲染尺寸"))
            {
                m_WallCanvas.referenceResolution = new Vector2(m_CanvasReandingWeidth, m_CanvasReandingHeight);
            }

            //GameEntry.Urg.Scale = GUILayout.HorizontalSlider(GameEntry.Urg.Scale, 0f, 1f);
            //GUILayout.Label("缩放值: " + GameEntry.Urg.Scale);

            //GameEntry.Urg.Limit = (int)GUILayout.HorizontalSlider(GameEntry.Urg.Limit, 1000, 10000);
            //GUILayout.Label("距离限制(mm): " + GameEntry.Urg.Limit);

            //GameEntry.Urg.ResolutionOffsetWidth = (int)GUILayout.HorizontalSlider(GameEntry.Urg.ResolutionOffsetWidth, 0, 100);
            //GUILayout.Label("偏移(X): " + GameEntry.Urg.ResolutionOffsetWidth);

            //GameEntry.Urg.ResolutionOffsetHeight = (int)GUILayout.HorizontalSlider(GameEntry.Urg.ResolutionOffsetHeight, 0, 100);
            //GUILayout.Label("偏移(Y): " + GameEntry.Urg.ResolutionOffsetHeight);
        }
    }
}