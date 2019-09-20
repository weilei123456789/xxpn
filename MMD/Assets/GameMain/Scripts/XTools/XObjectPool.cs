using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    public class XObjectPool : MonoBehaviour
    {
        [SerializeField]
        private XObjectPoolDataBase[] m_XObjectPoolDataBaseTemplate = null;
        [SerializeField]
        private int m_EveryCreatePreloadNum = 10;
        [SerializeField]
        private Vector3 m_InitPosition = Vector3.zero;

        private Dictionary<string, List<XObjectPoolDataBase>> m_ObjectPool = new Dictionary<string, List<XObjectPoolDataBase>>();

        private static XObjectPool _xObjectPool = null;
        public static XObjectPool Instance { get { return _xObjectPool; } }

        private void Awake()
        {
            _xObjectPool = this;
            InitObjectPool();
        }

        private void InitObjectPool()
        {
            foreach (XObjectPoolDataBase item in m_XObjectPoolDataBaseTemplate)
            {
                if (!m_ObjectPool.ContainsKey(item.name))
                    m_ObjectPool.Add(item.name, new List<XObjectPoolDataBase>());
                for (int i = 0; i < m_EveryCreatePreloadNum; i++)
                {
                    m_ObjectPool[item.name].Add(CreateTemplate(item));
                }
            }
        }

        public XObjectPoolDataBase FindUnUsed(string name)
        {
            if (!m_ObjectPool.ContainsKey(name))
            {
                Debug.LogWarning("对象池没有这个对象：" + name);
                return null;
            }
            for (int i = 0; i < m_ObjectPool[name].Count; i++)
            {
                if (!m_ObjectPool[name][i].IsUsed)
                {
                    m_ObjectPool[name][i].Used();
                    return m_ObjectPool[name][i];
                }
            }
            XObjectPoolDataBase xObject = FindObjectPoolObject(name);
            if (xObject == null) return null;
            XObjectPoolDataBase cObject = CreateTemplate(xObject);
            cObject.Used();
            m_ObjectPool[name].Add(cObject);
            return cObject;
        }

        private XObjectPoolDataBase FindObjectPoolObject(string name)
        {
            foreach (var item in m_XObjectPoolDataBaseTemplate)
            {
                if (item.name.Equals(name))
                {
                    return item;
                }
            }
            return null;
        }

        private XObjectPoolDataBase CreateTemplate(XObjectPoolDataBase template)
        {
            XObjectPoolDataBase item = Instantiate(template);
            Transform trs = item.GetComponent<Transform>();
            trs.SetParent(transform);
            //trs.localScale = Vector3.one;
            trs.localPosition = Vector3.zero;
            //trs.localRotation = Quaternion.identity;
            item.UnUsed();
            return item;
        }
    }

}