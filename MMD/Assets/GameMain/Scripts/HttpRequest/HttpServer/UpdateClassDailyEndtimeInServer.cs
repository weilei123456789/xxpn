using GameFramework;
using UnityGameFramework.Runtime;

namespace Penny
{

    public class UpdateClassDailyEndtimeInServer : HttpBase
    {
        public UpdateClassDailyEndtimeInServer(int _id, SuccessCallBack success, FailedCallBack failure)
            : base(GlobalData.Server_UpdateClassDailyEndtime, success, failure)
        {
            m_HttpSendData = new UpdateClassDailyEndtimeData(_id);
        }

    }

}
