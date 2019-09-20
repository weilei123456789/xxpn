using GameFramework;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class QueryCoursewareInServer : HttpBase
    {
        public QueryCoursewareInServer(long _time, string _classesId, SuccessCallBack success, FailedCallBack failure)
            : base(GlobalData.Server_GetCoursewareDailyByClassestime, success, failure, false)
        {
            m_HttpSendData = new QueryCoursewareData(_time, _classesId);
        }
    }
}
