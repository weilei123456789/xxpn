using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    public class CoursesDailyMap
    {
        public int teacher { set; get; }

        public long? sign_in_time { set; get; }

        public string name { set; get; }

        public string description { set; get; }

        public long endtime { set; get; }

        public int id { set; get; }

        public long starttime { set; get; }

        public int classDaily { set; get; }

        public int seq { set; get; }

        public int cid { set; get; }

        public CoursesRealiaMap coursesRealiaMap { set; get; }
        public CoursesTargetMap coursesTargetMap { set; get; }

        public override string ToString()
        {
            return Utility.Json.ToJson(this);
        }

    }
}