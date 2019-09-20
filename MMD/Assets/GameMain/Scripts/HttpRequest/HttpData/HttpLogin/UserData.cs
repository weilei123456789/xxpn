using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    public class UserData
    {
        public long timeStamp { get; set; }

        public string auid { get; set; }

        public string facetoken { get; set; }

        public string birthday { get; set; }

        public string faceimg { get; set; }

        public string belong { get; set; }

        public string contacts { get; set; }

        public int channel { get; set; }

        /// <summary>
        /// 1是学员，2是老师
        /// </summary>
        public int flag { get; set; }

        public int sex { get; set; }

        public string name { get; set; }

        public string tel { get; set; }

        public int id { get; set; }

        public override string ToString()
        {
            return Utility.Json.ToJson(this);
        }

    }
}