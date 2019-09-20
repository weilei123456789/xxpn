using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penny
{

    public class GetDeviceWareInServer : HttpBase
    {
        public GetDeviceWareInServer(int _id, SuccessCallBack success, FailedCallBack failure)
            : base(GlobalData.Server_GetDevicewareById, success, failure, false)
        {
            m_HttpSendData = new GetDeviceWareData(_id);
        }
      
    }

}
