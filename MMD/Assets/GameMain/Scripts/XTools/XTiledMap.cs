using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    public class XTiledMap
    {
        /// <summary>
        /// 背景层
        /// </summary>
        public static readonly string LayerName_BackGround = "BackGround";
        /// <summary>
        /// 背景层
        /// </summary>
        public static readonly string LayerName_Brige = "Brige";
        /// <summary>
        /// 障碍物层
        /// </summary>
        public static readonly string LayerName_Obstacle = "Obstacle";
        /// <summary>
        /// 背景层
        /// </summary>
        public static readonly string LayerName_Star = "Star";
        /// <summary>
        /// 标签层
        /// </summary>
        public static readonly string LayerName_Flag = "Flag";

        /// <summary>
        /// 地图高
        /// </summary>
        private int m_Height = 0;
        /// <summary>
        /// 地图宽
        /// </summary>
        private int m_Width = 0;
        /// <summary>
        /// 所有层级数据
        /// </summary>
        private TiledLayer[] m_TiledLayers = null;
        /// <summary>
        /// 所有瓦片数据
        /// </summary>
        private TileData[] m_TiledDatas = null;

        private TMX m_TMX = null;
        private string m_TiledName = string.Empty;
        private Dictionary<string, TileData[][]> m_AllTiledMaps = null;

        public XTiledMap(string name, string jsonData)
        {
            m_TiledName = name;
            if (string.IsNullOrEmpty(jsonData)) { Debug.Log("Tiled Map Data Is Null"); return; }
            m_TMX = JsonMapper.ToObject<TMX>(jsonData);
            if (m_TMX == null) { Debug.Log(".TMX Is Null"); return; }
            //宽高
            m_Height = m_TMX.height;
            m_Width = m_TMX.width;
            //层
            m_TiledLayers = m_TMX.layers;
            //瓦片
            m_TiledDatas = m_TMX.tilesets;
            //合成
            Combination(m_Height, m_Width);
        }

        private void Combination(int _Height, int _Weight)
        {
            m_AllTiledMaps = new Dictionary<string, TileData[][]>();
            string _name = string.Empty;
            int _index = 0;
            foreach (TiledLayer item in m_TiledLayers)
            {
                _name = item.name;
                _index = 0;
                TileData[][] tileDatas = new TileData[_Height][];
                for (int h = 0; h < _Height; h++)
                {
                    tileDatas[h] = new TileData[_Weight];
                    for (int w = 0; w < _Weight; w++)
                    {
                        _index = w + h * _Weight;
                        tileDatas[h][w] = GetTileByGID(item.data[_index]);
                    }
                }

                if (m_AllTiledMaps.ContainsKey(_name))
                    m_AllTiledMaps.Add(_name, tileDatas);
                else
                    m_AllTiledMaps[_name] = tileDatas;
            }
        }

        private TileData GetTileByGID(int gid)
        {
            foreach (var item in m_TiledDatas)
            {
                if (item.firstgid == gid)
                    return item;
            }
            return null;
        }

        //************************ 开放数据 *********************
        /// <summary>
        /// 地图高
        /// </summary>
        public int Height { get { return m_Height; } }
        /// <summary>
        /// 地图宽
        /// </summary>
        public int Width { get { return m_Width; } }
        /// <summary>
        /// 解析的数据
        /// </summary>
        public Dictionary<string, TileData[][]> TiledMaps { get { return m_AllTiledMaps; } }
        /// <summary>
        /// 地图名字
        /// </summary>
        public string TiledName { get { return m_TiledName; } }

        //************************ TMX数据 *********************
        public class TMX
        {
            public int height { get; set; }
            public int width { get; set; }
            public TiledLayer[] layers { get; set; }
            public TileData[] tilesets { get; set; }
            public int version { get; set; }
            public int tileheight { get; set; }
            public int tilewidth { get; set; }
        }

        //************************ 瓦片数据 *********************
        public class TileData
        {
            public int firstgid { get; set; }
            public string image { get; set; }
            public int imageheight { get; set; }
            public int imagewidth { get; set; }
            public int margin { get; set; }
            public string name { get; set; }
            public int spacing { get; set; }
            public int tileheight { get; set; }
            public int tilewidth { get; set; }
        }

        //************************ 图层数据 *********************
        public class TiledLayer
        {
            public int[] data { get; set; }
            public int height { get; set; }
            public string name { get; set; }
            public int opacity { get; set; }
            public string type { get; set; }
            public bool visible { get; set; }
            public int width { get; set; }
            public int x { get; set; }
            public int y { get; set; }
        }
    }

}
