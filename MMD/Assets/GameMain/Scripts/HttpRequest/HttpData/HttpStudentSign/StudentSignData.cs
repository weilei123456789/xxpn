using System;
using GameFramework;
using LitJson;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class StudentSignReq
    {
        public int sid { set; get; }
        public long timestamp { set; get; }
        public string tel { set; get; }
    }

    public class StudentSignResponse
    {
        public int resultCode { get; set; }
        public string resultDesc { get; set; }
    }

    public class StudentSignData : IDictable, ICloneable
    {
        private StudentSignReq m_Req = new StudentSignReq();
        public StudentSignResponse Response { get; private set; }

        public StudentSignData(int _sid, long _timestamp, string _tel)
        {
            m_Req.sid = _sid;
            m_Req.timestamp = _timestamp;
            m_Req.tel = _tel;
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
            Response = Utility.Json.ToObject<StudentSignResponse>(responseJson);
        }

        #endregion

        #region 克隆,拷贝方法需添加赋值操作

        public object Clone()
        {
            return new StudentSignData(m_Req.sid, m_Req.timestamp, m_Req.tel);
        }

        #endregion
    }
}