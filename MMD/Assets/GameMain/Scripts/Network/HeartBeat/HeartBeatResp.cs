using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Penny
{
    public class HeartBeatResp : Resp
    {
        public SocketData RespSocketData
        {
            private set;
            get;
        }

        public override int GetProtocol()
        {
            return NetProtocols.CSHeartBeatProtocol;
        }

        public override void Deserialize(SocketData data)
        {
            RespSocketData = data;
        }
    }
}