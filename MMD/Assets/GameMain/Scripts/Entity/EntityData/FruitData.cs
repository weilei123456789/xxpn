using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    public class FruitData : TargetableObjectData
    {
        [SerializeField]
        private int m_MaxHP = 0;

        [SerializeField]
        private float m_Speed = 0f;

        [SerializeField]
        private int m_Attack = 0;

        [SerializeField]
        private int m_DeadEffectId = 0;

        [SerializeField]
        private int m_DeadSoundId = 0;

        public FruitData(int entityId, int typeId)
            : base(entityId, typeId, CampType.Enemy)
        {
            HP = m_MaxHP = 1;
            m_Speed = 15;
            m_Attack = 1;
            m_DeadEffectId = 20001;
            m_DeadSoundId = 20001;
        }

        public override int MaxHP
        {
            get
            {
                return m_MaxHP;
            }
        }

        public int Attack
        {
            get
            {
                return m_Attack;
            }
        }

        public float Speed
        {
            get
            {
                return m_Speed;
            }
        }

        public int DeadEffectId
        {
            get
            {
                return m_DeadEffectId;
            }
        }

        public int DeadSoundId
        {
            get
            {
                return m_DeadSoundId;
            }
        }
    }
}
