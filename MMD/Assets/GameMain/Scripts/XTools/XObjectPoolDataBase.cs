using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    public class XObjectPoolDataBase : MonoBehaviour
    {
        public bool IsUsed { get; private set; }

        protected virtual void Awake()
        {
            IsUsed = false;
            name = name.Replace("(Clone)", "");
        }

        public void Used()
        {
            IsUsed = true;
        }

        public void UnUsed()
        {
            IsUsed = false;
        }

        public void UnUsed(Transform parent)
        {
            UnUsed();
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
        }

    }
}
