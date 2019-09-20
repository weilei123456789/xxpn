using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{
    /// <summary>
    /// 为真时获取10位时间戳,为假时获取13位时间戳
    /// </summary>
    public class TimeUtility
    {
        public static long GetTimeStamp(bool bflag = false)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long ret;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds);
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds);
            return ret;
        }

        public static DateTime GetDateTime(long milliSecond)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0)); 
            DateTime dt = startTime.AddMilliseconds(milliSecond);
            return dt;
        }
    }
}
