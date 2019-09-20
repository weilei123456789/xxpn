using GameFramework;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Penny
{

    public class TeachSignInServer : HttpBase
    {
        public TeachSignInServer(int _teacher, string _tel, long _timestamp,int cdid, string cid, byte[] _photo, SuccessCallBack success, FailedCallBack failure)
            : base(GlobalData.Server_TeacherSigninTimestamp, success, failure)
        {
            m_HttpSendData = new TeachSignData(_teacher, _timestamp, _tel);

            m_HttpByteData = _photo;
            m_HttpForm.Add("teacher", _teacher.ToString());
            m_HttpForm.Add("tel", _tel);
            m_HttpForm.Add("timestamp", _timestamp.ToString());
            m_HttpForm.Add("cdid", cdid.ToString());
            m_HttpForm.Add("cid", cid);
        }
        
    }

}
