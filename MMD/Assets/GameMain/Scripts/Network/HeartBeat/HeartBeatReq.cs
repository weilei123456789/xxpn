using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class HeartBeatReq : Request
    {
        private SocketData m_SocketData = new SocketData();

        public override int GetProtocol()
        {
            return NetProtocols.CSHeartBeatProtocol;
        }

        public HeartBeatReq(string msg, string data)
        {
            m_SocketData.code = GetProtocol();
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
