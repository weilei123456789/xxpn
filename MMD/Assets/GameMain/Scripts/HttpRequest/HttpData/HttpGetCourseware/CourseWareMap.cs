using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    public class CourseWareMap
    {
        public class Ware
        {
            public Dictionary<string, CourseWareDetailMap> courseWareDetailMap { get; set; }
        }

        public long timestamp { set; get; }

        public string imgurl { set; get; }

        public string wareversion { set; get; }

        public string name { set; get; }

        public string description { set; get; }

        public int id { set; get; }

        public string url { set; get; }

        public int cid { set; get; }

        public string resname { set; get; }

        public Ware courseWareDetailMap { set; get; }

        public override string ToString()
        {
            return Utility.Json.ToJson(this);
        }
    }
}