using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    public class CoursesTargetMap
    {
        public class Inside
        {
            public int cdid { set; get; }

            public int id { set; get; }

            public int aid { set; get; }
        }

        public Dictionary<string, Inside> coursesTargetMap { set; get; }

    }
}