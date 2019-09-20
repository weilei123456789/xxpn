using GameFramework;
using UnityGameFramework.Runtime;

namespace Penny
{

    public class GetCoursewareInServer : HttpBase
    {
        public GetCoursewareInServer(int _sid, SuccessCallBack success, FailedCallBack failure)
            : base(GlobalData.Server_GetCoursewareById, success, failure, false)
        {
            m_HttpSendData = new GetCoursewareData(_sid);
        }

    }

}
