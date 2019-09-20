//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using System;
using UnityGameFramework.Runtime;

namespace Penny
{
    public static class EntityExtension
    {
        // 关于 EntityId 的约定：
        // 0 为无效
        // 正值用于和服务器通信的实体（如玩家角色、NPC、怪等，服务器只产生正值）
        // 负值用于本地生成的临时实体（如特效、FakeObject等）
        private static int s_SerialId = 0;

        public static Entity GetGameEntity(this EntityComponent entityComponent, int entityId)
        {
            UnityGameFramework.Runtime.Entity entity = entityComponent.GetEntity(entityId);
            if (entity == null)
            {
                return null;
            }

            return (Entity)entity.Logic;
        }

        public static void HideEntity(this EntityComponent entityComponent, Entity entity)
        {
            entityComponent.HideEntity(entity.Entity);
        }

        public static void AttachEntity(this EntityComponent entityComponent, Entity entity, int ownerId, string parentTransformPath = null, object userData = null)
        {
            entityComponent.AttachEntity(entity.Entity, ownerId, parentTransformPath, userData);
        }

        public static void ShowFruit(this EntityComponent entityCompoennt, FruitData data)
        {
            entityCompoennt.ShowEntity(typeof(Fruit), "Fruit", Constant.AssetPriority.FruitAsset, data);
        }

        /// <summary>
        /// 自定义的实体显示方法
        /// </summary>
        /// <param name="logicType">实体类型</param>
        /// <param name="entityGroup">实体组</param>
        /// <param name="path">实体路径</param>
        /// <param name="data">实体数据</param>
        public static void ShowCustomEntity(this EntityComponent entityComponent, Type logicType, string entityGroup,string seasonPath, string path, EntityData data)
        {
            entityComponent.ShowEntity(logicType, entityGroup, seasonPath, path, Constant.AssetPriority.GroundModelAsset, data);
        }

        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup, int priority, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetEntityAsset(drEntity.AssetName), entityGroup, priority, data);
        }

        /// <summary>
        /// 课件地址实体
        /// </summary>
        private static void ShowEntity(this EntityComponent entityComponent, Type logicType, string entityGroup,string seasonPath, string path, int priority, EntityData data)
        {
            if (data == null)
            {
                Log.Warning("Data is invalid.");
                return;
            }

            IDataTable<DREntity> dtEntity = GameEntry.DataTable.GetDataTable<DREntity>();
            DREntity drEntity = dtEntity.GetDataRow(data.TypeId);
            if (drEntity == null)
            {
                Log.Warning("Can not load entity id '{0}' from data table.", data.TypeId.ToString());
                return;
            }

            entityComponent.ShowEntity(data.Id, logicType, AssetUtility.GetLessonEntityAsset(seasonPath, path, drEntity.AssetName), entityGroup, priority, data);
        }



        public static int GenerateSerialId(this EntityComponent entityComponent)
        {
            return --s_SerialId;
        }
    }
}
