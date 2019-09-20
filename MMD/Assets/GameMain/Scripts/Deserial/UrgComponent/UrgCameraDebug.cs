using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Penny
{
    public class UrgCameraDebug : MonoBehaviour
    {
        //材质球
        private Material lineMaterial;

        private void Awake()
        {
            if (!lineMaterial)
            {
                lineMaterial = new Material(Shader.Find("GUI/Text Shader"));
                lineMaterial.hideFlags = HideFlags.HideAndDontSave;
                lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
            }
        }

        void OnPostRender()
        {
            if (!GameEntry.Urg) return;
            if (!GameEntry.Urg.DebugDraw) return;
            //for (int i = 0; i < urgComponent.DebugLineVector3.Count; i++)
            //{
            //    GL.PushMatrix();
            //    lineMaterial.SetPass(0);
            //    GL.LoadOrtho();
            //    GL.Begin(GL.LINES);
            //    GL.Color(Color.black);
            //    GL.Vertex3(urgComponent.ResolutionWidth / 2, urgComponent.ResolutionHeight, 0);
            //    GL.Vertex(urgComponent.DebugLineVector3[i]);
            //    GL.End();
            //    GL.PopMatrix();
            //}

            GL.PushMatrix();
            lineMaterial.SetPass(0);
            GL.LoadPixelMatrix();
            GL.LoadOrtho();
            GL.Begin(GL.LINES);

            for (int i = 0; i < GameEntry.Urg.UrgGUI.Count; i++)
            {
                GL.Color(GameEntry.Urg.UrgGUI[i].color);
                GL.Vertex3(
                    GameEntry.Urg.UrgGUI[i].start_pos.x / GameEntry.Urg.ResolutionWidth,
                    GameEntry.Urg.UrgGUI[i].start_pos.y / GameEntry.Urg.ResolutionHeight,
                    0
                );
                GL.Vertex3(
                 GameEntry.Urg.UrgGUI[i].end_pos.x / GameEntry.Urg.ResolutionWidth,
                 GameEntry.Urg.UrgGUI[i].end_pos.y / GameEntry.Urg.ResolutionHeight,
                 0
                );
            }

            for (int i = 0; i < GameEntry.Urg.DebugLineVector3.Count; i++)
            {
                GL.Color(Color.red);
                GL.Vertex3(GameEntry.Urg.ResolutionWidth / 2f / GameEntry.Urg.ResolutionWidth, GameEntry.Urg.ResolutionHeight / GameEntry.Urg.ResolutionHeight, 0);
                GL.Vertex3(GameEntry.Urg.DebugLineVector3[i].x / GameEntry.Urg.ResolutionWidth, GameEntry.Urg.DebugLineVector3[i].y / GameEntry.Urg.ResolutionHeight, 0);
            }

            GL.End();
            GL.PopMatrix();
        }

    }
}