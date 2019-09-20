using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Penny
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class X2DLidarButton : MonoBehaviour
    {
        [SerializeField]
        private bool m_AdjustBoxCollider = true;

        private BoxCollider2D m_BoxCollider2D = null;
        private RectTransform m_RectTransform = null;

        // Use this for initialization
        private void Start()
        {
            m_RectTransform = this.GetComponent<RectTransform>();
            m_BoxCollider2D = this.GetComponent<BoxCollider2D>();

            Init();
        }

        private void Init()
        {
            if (m_BoxCollider2D == null)
            {
                Log.Error("Error: Can't Find BoxCollider2D!!!!");
                return;
            }
            else
            {
                if (m_AdjustBoxCollider == true)
                {
                    m_BoxCollider2D.offset =m_RectTransform.rect.center;      //把box collider设置到物体的中心
                    m_BoxCollider2D.size = new Vector2(m_RectTransform.rect.width, m_RectTransform.rect.height);    //改变collider大小
                }
            }
        }
    }
}
