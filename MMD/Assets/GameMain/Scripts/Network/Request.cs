using UnityEngine;
using System.Collections;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Penny
{

    public abstract class Request
    {
        public virtual int GetProtocol()
        {
            Log.Error("can't get Protocol");
            return 0;
        }

        public virtual void Serialize(DataStream writer)
        {
        }

        public void Send()
        {
            //GameEntry.Socket.SendMessage(this);
        }
    }
}
