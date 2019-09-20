using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Penny
{
    public class SocketDataResp : Resp
    {
        public SocketData RespSocketData
        {
            private set;
            get;
        }

        private int DataProtocol
        {
            get;
            set;
        }

        public override int GetProtocol()
        {
            return DataProtocol;
        }

        public override void Deserialize(SocketData data)
        {
            RespSocketData = data;
            DataProtocol = data.code;
        }
    }
}