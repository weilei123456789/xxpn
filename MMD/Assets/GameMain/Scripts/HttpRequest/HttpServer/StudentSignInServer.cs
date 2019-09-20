using GameFramework;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class StudentSignInServer : HttpBase
    {
        public StudentSignInServer(int _sid, string _tel, int cdid, string cid, byte[] _photo, long _timestamp, SuccessCallBack success, FailedCallBack failure)
            : base(GlobalData.Server_StudentSigninTimestamp, success, failure)
        {
            m_HttpSendData = new StudentSignData(_sid, _timestamp, _tel);

            m_HttpByteData = _photo;
            m_HttpForm.Add("sid", _sid.ToString());
            m_HttpForm.Add("tel", _tel);
            m_HttpForm.Add("timestamp", _timestamp.ToString());
            m_HttpForm.Add("cdid", cdid.ToString());
            m_HttpForm.Add("cid", cid);
        }
    }

}
