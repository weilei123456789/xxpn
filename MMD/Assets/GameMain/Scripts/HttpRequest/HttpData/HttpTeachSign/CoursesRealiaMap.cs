using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{

    public class CoursesRealiaMap
    {
        public class Inside
        {
            public int realiacount { set; get; }

            public int cdid { set; get; }

            public int id { set; get; }

            public int rid { set; get; }
        }

        public Dictionary<string, Inside> coursesRealiaMap { set; get; }
    }

}