using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class XAutoSize : MonoBehaviour
    {
        [SerializeField]
        private Camera m_Camera = null;

        private SpriteRenderer m_SpriteRenderer = null;

        private float m_WideScale = 0;
        private float m_HighScale = 0;

        private float m_CameraSize;
        private Vector2 m_SpriteSize;
        private void Awake()
        {
            m_SpriteRenderer = this.GetComponent<SpriteRenderer>();
            m_SpriteSize = m_SpriteRenderer.sprite.bounds.size;
        }

        private void Update()
        {
            if (m_Camera == null)
            {
                Log.Warning("Camera is null!");
                return;
            }
            m_CameraSize = m_Camera.orthographicSize * 2;

            m_WideScale = (m_Camera.aspect * m_CameraSize) / m_SpriteSize.x;
            m_HighScale = m_CameraSize / m_SpriteSize.y;

            m_SpriteRenderer.transform.localScale = new Vector3(m_WideScale, m_HighScale, 1);
        }
    }
}
