using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    public class WaitDestroyImmediate : MonoBehaviour
    {
        [SerializeField]
        private float m_Time = 1.0f;

        void Start()
        {
            Destroy(gameObject, m_Time);
        }
    }

}
