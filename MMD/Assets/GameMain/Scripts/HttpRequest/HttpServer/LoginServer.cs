using GameFramework;
using UnityGameFramework.Runtime;

namespace Penny
{

    public class LoginServer : HttpBase
    {
        public LoginServer(string _facetoken, string _auid, int _flag, SuccessCallBack success, FailedCallBack failure)
            :base(GlobalData.Server_Getuserbyfacetoken, success, failure)
        {
            m_HttpSendData = new LoginData(_facetoken, _auid, _flag);
        }
    }

}
