using System;
using GameFramework;
using LitJson;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class LoginReq
    {
        public string facetoken { set; get; }
        public string auid { set; get; }
        public int flag { set; get; }
    }

    public class LoginResponse
    {
        public RespVo respVo { set; get; }
        public UserData[] userMap { set; get; }
    }

    public class LoginData : IDictable, ICloneable
    {
        private LoginReq m_LoginReq = new LoginReq();
        public LoginResponse Response { get; private set; }

        public LoginData(string _facetoken, string _auid, int _flag)
        {
            m_LoginReq.facetoken = _facetoken;
            m_LoginReq.auid = _auid;
            m_LoginReq.flag = _flag;
        }

        #region 序列化

        public string ToJson()
        {
            return Utility.Json.ToJson(m_LoginReq);
        }

        public byte[] ToJsonData()
        {
            return Utility.Json.ToJsonData(m_LoginReq);
        }

        public void fromDict(string responseJson)
        {
            Response = Utility.Json.ToObject<LoginResponse>(responseJson);
        }

        public override string ToString()
        {
            return ToJson();
        }

        #endregion

        #region 克隆,拷贝方法需添加赋值操作

        public object Clone()
        {
            return new LoginData(m_LoginReq.facetoken, m_LoginReq.auid, m_LoginReq.flag);
        }

        #endregion
    }
}