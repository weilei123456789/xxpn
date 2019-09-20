//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------
// 此文件由工具自动生成，请勿直接修改。
// 生成时间：2019-08-19 11:09:23.350
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Penny
{
    /// <summary>
    /// �������ñ�。
    /// </summary>
    public class DRSound : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 获取������。
        /// </summary>
        public override int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取��Դ����。
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取���ȼ���Ĭ��0��128��ߣ�-128��ͣ�。
        /// </summary>
        public int Priority
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取�Ƿ�ѭ��。
        /// </summary>
        public bool Loop
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取������0~1��。
        /// </summary>
        public float Volume
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取����。
        /// </summary>
        public int Length
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取����ռ�������0Ϊ2D��1Ϊ3D���м�ֵ���Ч����。
        /// </summary>
        public float SpatialBlend
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取����������。
        /// </summary>
        public float MaxDistance
        {
            get;
            private set;
        }

        public override bool ParseDataRow(GameFrameworkSegment<string> dataRowSegment)
        {
            // Star Force 示例代码，正式项目使用时请调整此处的生成代码，以处理 GCAlloc 问题！
            string[] columnTexts = dataRowSegment.Source.Substring(dataRowSegment.Offset, dataRowSegment.Length).Split(DataTableExtension.DataSplitSeparators);
            for (int i = 0; i < columnTexts.Length; i++)
            {
                columnTexts[i] = columnTexts[i].Trim(DataTableExtension.DataTrimSeparators);
            }

            int index = 0;
            index++;
            m_Id = int.Parse(columnTexts[index++]);
            index++;
            AssetName = columnTexts[index++];
            Priority = int.Parse(columnTexts[index++]);
            Loop = bool.Parse(columnTexts[index++]);
            Volume = float.Parse(columnTexts[index++]);
            Length = int.Parse(columnTexts[index++]);
            SpatialBlend = float.Parse(columnTexts[index++]);
            MaxDistance = float.Parse(columnTexts[index++]);

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(GameFrameworkSegment<byte[]> dataRowSegment)
        {
            // Star Force 示例代码，正式项目使用时请调整此处的生成代码，以处理 GCAlloc 问题！
            using (MemoryStream memoryStream = new MemoryStream(dataRowSegment.Source, dataRowSegment.Offset, dataRowSegment.Length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    m_Id = binaryReader.ReadInt32();
                    AssetName = binaryReader.ReadString();
                    Priority = binaryReader.ReadInt32();
                    Loop = binaryReader.ReadBoolean();
                    Volume = binaryReader.ReadSingle();
                    Length = binaryReader.ReadInt32();
                    SpatialBlend = binaryReader.ReadSingle();
                    MaxDistance = binaryReader.ReadSingle();
                }
            }

            GeneratePropertyArray();
            return true;
        }

        public override bool ParseDataRow(GameFrameworkSegment<Stream> dataRowSegment)
        {
            Log.Warning("Not implemented ParseDataRow(GameFrameworkSegment<Stream>)");
            return false;
        }

        private void GeneratePropertyArray()
        {

        }
    }
}
