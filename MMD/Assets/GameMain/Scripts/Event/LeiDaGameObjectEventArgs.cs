using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Event;


namespace Penny
{

    public sealed class LeiDaGameObjectEventArgs : GameEventArgs
    {

        /// <summary>
        /// 雷达产生的GameObject事件编号。
        /// </summary>
        public static readonly int EventId = typeof(LeiDaGameObjectEventArgs).GetHashCode();


        /// <summary>
        /// 获取由于雷达产生的GameObject。
        /// </summary>
        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        /// <summary>
        /// 雷达产生新的GameObj。
        /// </summary>
        public GameObject NewGo
        {
            get;
            private set;
        }

        /// <summary>
        /// 挂载的实体组件
        /// </summary>
        public Entity entity {
            get;
            private set;
        }


        /// <summary>
        /// 射线碰到的坐标
        /// </summary>
        public  Vector3 WorldPoint
        {
            get;
            private set;
        }

        public GameObject ChangeGo
        {
            get
            {
                return NewGo;
            }
        }



        /// <summary>
        /// 清理事件
        /// </summary>
        public override void Clear()
        {
            NewGo = default(GameObject);
            WorldPoint = default(Vector3);
            entity = default(Entity);
            // throw new System.NotImplementedException();
        }


        public LeiDaGameObjectEventArgs(GameObject go, Vector3 colliderPint)
        {
            entity = go.GetComponent<Entity>();

            NewGo = go;
            WorldPoint = colliderPint;
        }

        public void Set(GameObject go, Vector3 colliderPint)
        {
            NewGo = go;
            WorldPoint = colliderPint;
        }
    }
}
