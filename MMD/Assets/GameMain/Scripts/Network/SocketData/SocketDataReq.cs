using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class SocketDataReq : Request
    {
        private SocketData m_SocketData = new SocketData();

        private int DataProtocol
        {
            get;
            set;
        }

        public override int GetProtocol()
        {
            return DataProtocol;
        }

        public SocketDataReq(int code, string msg, object data)
        {
            DataProtocol = code;
            m_SocketData.code = DataProtocol;
            m_SocketData.msg = msg;
            m_SocketData.data = data;
        }

        public override void Serialize(DataStream writer)
        {
            string json = Utility.Json.ToJson(m_SocketData);
            writer.WriteString8(json);
        }
    }
}
