using System;
using GameFramework;
using LitJson;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class UpdateClassDailyEndtimeDataReq
    {
        public int id { set; get; }
    }

    public class UpdateClassDailyEndtimeDataResponse
    {
        public RespVo respVo { set; get; }
    }

    public class UpdateClassDailyEndtimeData : IDictable, ICloneable
    {
        private UpdateClassDailyEndtimeDataReq m_Req = new UpdateClassDailyEndtimeDataReq();
        public UpdateClassDailyEndtimeDataResponse m_Response { get; private set; }

        public UpdateClassDailyEndtimeData(int _id)
        {
            m_Req.id = _id;
        }

        #region 序列化

        public string ToJson()
        {
            return Utility.Json.ToJson(m_Req);
        }

        public byte[] ToJsonData()
        {
            return Utility.Json.ToJsonData(m_Req);
        }

        public void fromDict(string responseJson)
        {
            m_Response = Utility.Json.ToObject<UpdateClassDailyEndtimeDataResponse>(responseJson);
        }

        #endregion

        #region 克隆,拷贝方法需添加赋值操作

        public object Clone()
        {
            return new UpdateClassDailyEndtimeData(m_Req.id);
        }

        #endregion
    }
}