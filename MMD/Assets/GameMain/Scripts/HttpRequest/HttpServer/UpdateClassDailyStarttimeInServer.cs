using GameFramework;
using UnityGameFramework.Runtime;

namespace Penny
{

    public class UpdateClassDailyStarttimeInServer : HttpBase
    {
        public UpdateClassDailyStarttimeInServer(int _id, SuccessCallBack success, FailedCallBack failure)
            :base(GlobalData.Server_UpdateClassDailyStarttime,success,failure)
        {
            m_HttpSendData = new UpdateClassDailyStarttimeData(_id);
        }
    }

}
