using System;
using GameFramework;
using LitJson;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class UpdateClassDailyStarttimeDataReq
    {
        public int id { set; get; }
    }

    public class UpdateClassDailyStarttimeDataResponse
    {
        public RespVo respVo { set; get; }
    }

    public class UpdateClassDailyStarttimeData : IDictable, ICloneable
    {
        public UpdateClassDailyStarttimeDataReq m_Req = new UpdateClassDailyStarttimeDataReq();
        public UpdateClassDailyStarttimeDataResponse Response { private set; get; }

        public UpdateClassDailyStarttimeData(int _id)
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
            Response = Utility.Json.ToObject<UpdateClassDailyStarttimeDataResponse>(responseJson);
        }

        #endregion

        #region 克隆,拷贝方法需添加赋值操作

        public object Clone()
        {
            return new UpdateClassDailyStarttimeData(m_Req.id);
        }

        #endregion
    }
}