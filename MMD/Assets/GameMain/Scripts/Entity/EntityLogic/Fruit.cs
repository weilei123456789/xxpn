//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Penny
{
    /// <summary>
    /// 水果类。
    /// </summary>
    public class Fruit : TargetableObject
    {
        [SerializeField]
        private FruitData m_FruitData = null;

        private Collider m_Collider = null;
        private Rigidbody m_Rigidbody = null;

        private float m_CancalTime = 0;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Collider = GetComponent<Collider>();
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_FruitData = userData as FruitData;
            if (m_FruitData == null)
            {
                Log.Error("Asteroid data is invalid.");
                return;
            }
            m_CancalTime = 0;
            m_Rigidbody.useGravity = false;
            m_Collider.isTrigger = true;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            m_CancalTime += elapseSeconds;

            if (m_CancalTime > 10)
            {
                GameEntry.Entity.HideEntity(this);
                m_CancalTime = 0;
            }

            if (m_FruitData.HP <= 0)
                return;
            CachedTransform.Translate(Vector3.back * m_FruitData.Speed * elapseSeconds, Space.World);


        }

        protected override void OnDead(Entity attacker)
        {
            //base.OnDead(attacker);
            Log.Info("死亡");
            m_Collider.isTrigger = false;
            m_Rigidbody.useGravity = true;
            //GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), m_FruitData.DeadEffectId)
            //{
            //    Position = CachedTransform.localPosition,
            //});

            m_Rigidbody.AddForce(
                Utility.Random.GetRandom(-1000, 1000),
                Utility.Random.GetRandom(-1000, 1000),
                Utility.Random.GetRandom(0, 1000)
            );
            //GameEntry.Sound.PlaySoundAndLength(m_FruitData.DeadSoundId);

        }

        protected override void OnTriggerEnter(Collider other)
        {
            GameObject entity = other.gameObject;
            if (!entity.name.Equals("LeftLine") && !entity.name.Equals("RightLine"))
            {
                return;
            }

            ApplyDamage(this, m_FruitData.Attack);

        }

        public override ImpactData GetImpactData()
        {
            return new ImpactData(m_FruitData.Camp, m_FruitData.HP, m_FruitData.Attack, 0);
        }
    }
}
