using System;
using GameFramework;
using LitJson;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class GetCoursewareDataRep
    {
        public int id { set; get; }
    }

    public class GetCoursewareDataResponse
    {
        public RespVo respVo { set; get; }
        public CourseWareMap[] courseWareMap { set; get; }
    }

    public class GetCoursewareData : IDictable, ICloneable
    {
        private GetCoursewareDataRep m_GetCoursewareDataReq = new GetCoursewareDataRep();
        public GetCoursewareDataResponse Response { private set; get; }

        public GetCoursewareData(int _id)
        {
            m_GetCoursewareDataReq.id = _id;
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
            Response = Utility.Json.ToObject<GetCoursewareDataResponse>(responseJson);
        }

        #endregion

        #region 克隆,拷贝方法需添加赋值操作

        public object Clone()
        {
            return new GetCoursewareData(m_GetCoursewareDataReq.id);
        }

        #endregion
    }
}