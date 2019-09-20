using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace Penny
{
    public class ConnectForm : UGuiForm
    {
        [SerializeField]
        private Transform m_Transform = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Transform.DOLocalRotateQuaternion(Quaternion.Euler(360, 360, 360), 1).SetLoops(-1);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}
