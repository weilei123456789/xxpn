using GameFramework;
using System.Collections.Generic;

namespace Penny
{
    public class DeviceMap
    {
        public class Ware
        {
            public Dictionary<string, DeviceWareMap> deviceWareMap { get; set; }
        }

        public long timeStamp { get; set; }

        public string classid { get; set; }

        public string belong { get; set; }

        public string lanip { get; set; }

        public string lanport { get; set; }

        public string name { get; set; }

        public int id { get; set; }

        public string lbs { get; set; }

        public Ware deviceWareMap { get; set; }

        public override string ToString()
        {
            return Utility.Json.ToJson(this);
        }

    }
}