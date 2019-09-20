using GameFramework;
using System;

namespace Penny
{
    public class GetDeviceWareRep
    {
        public int id { set; get; }
    }

    public class GetDeviceWareResponse
    {
        public RespVo respVo { set; get; }
        public DeviceMap[] devicesMap { set; get; }
    }

    public class GetDeviceWareData : IDictable, ICloneable
    {
        private GetDeviceWareRep m_Rep = new GetDeviceWareRep();
        public GetDeviceWareResponse Response { get; private set; }

        public GetDeviceWareData(int _id)
        {
            m_Rep.id = _id;
        }

        #region 序列化

        public string ToJson()
        {
            return Utility.Json.ToJson(m_Rep);
        }

        public byte[] ToJsonData()
        {
            return Utility.Json.ToJsonData(m_Rep);
        }

        public void fromDict(string responseJson)
        {
            Response = Utility.Json.ToObject<GetDeviceWareResponse>(responseJson);
        }

        #endregion

        #region 克隆,拷贝方法需添加赋值操作

        public object Clone()
        {
            return new GetDeviceWareData(m_Rep.id);
        }

        #endregion
    }
}