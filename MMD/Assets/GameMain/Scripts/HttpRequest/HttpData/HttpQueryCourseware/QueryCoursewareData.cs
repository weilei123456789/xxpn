using System;
using GameFramework;
using LitJson;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class QueryCoursewareRep
    {
        public long time { set; get; }
        public string classesId { set; get; }
    }

    public class QueryCoursewareResponse
    {
        public RespVo respVo { set; get; }
        public CoursesDailyMap[] coursesDailyMap { set; get; }
    }

    public class QueryCoursewareData : IDictable, ICloneable
    {
        private QueryCoursewareRep m_GetCoursewareDataReq = new QueryCoursewareRep();
        public QueryCoursewareResponse Response { private set; get; }

        public QueryCoursewareData(long _time, string _classesId)
        {
            m_GetCoursewareDataReq.time = _time;
            m_GetCoursewareDataReq.classesId = _classesId;
        }

        #region 序列化

        public string ToJson()
        {
            return Utility.Json.ToJson(m_GetCoursewareDataReq);
        }

        public byte[] ToJsonData()
        {
            return Utility.Json.ToJsonData(m_GetCoursewareDataReq);
        }

        public void fromDict(string responseJson)
        {
            Response = Utility.Json.ToObject<QueryCoursewareResponse>(responseJson);
        }

        #endregion

        #region 克隆,拷贝方法需添加赋值操作

        public object Clone()
        {
            return new QueryCoursewareData(m_GetCoursewareDataReq.time, m_GetCoursewareDataReq.classesId);
        }

        #endregion
    }
}