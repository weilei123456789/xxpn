using UnityEngine;
using System.Collections;

namespace Penny
{
    public abstract class Resp
    {

        public virtual int GetProtocol()
        {
            Debug.LogError("can't get Protocol");
            return -1;
        }

        public virtual void Serialize(DataStream writer)
        {
            //no need to implement as this is a response
        }

        public virtual void Deserialize(SocketData data)
        {
        }

    }
}