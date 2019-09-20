using GameFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class ProtoManager
    {
        private Dictionary<int, Func<SocketData, Resp>> m_ProtocolMapping;

        public delegate void ResponseDelegate(Resp resp);
        private Dictionary<int, List<ResponseDelegate>> m_DelegateMapping;

        private string m_RecvCommand = string.Empty;
        private SocketData m_RecvHeartBeatData = null;
        private SocketData m_SocketDataCache = new SocketData();
        private Dictionary<string, object> m_JsonKeyValues = null;

        public ProtoManager()
        {
            m_ProtocolMapping = new Dictionary<int, Func<SocketData, Resp>>();
            m_DelegateMapping = new Dictionary<int, List<ResponseDelegate>>();
        }

        public void AddProtocol<T>(int protocol) where T : Resp, new()
        {
            if (m_ProtocolMapping.ContainsKey(protocol))
            {
                m_ProtocolMapping.Remove(protocol);
            }

            m_ProtocolMapping.Add(protocol,
                (heartBeat) =>
                {
                    T data = new T();
                    data.Deserialize(heartBeat);
                    return data;
                });
        }

        /// <summary>
        /// 添加代理，在接受到服务器数据时会下发数据
        /// </summary>
        /// <param name="protocol">Protocol.</param>
        /// <param name="d">D.</param>
        public void AddRespDelegate(int protocol, ResponseDelegate d)
        {
            List<ResponseDelegate> dels;
            if (m_DelegateMapping.ContainsKey(protocol))
            {
                dels = m_DelegateMapping[protocol];
                for (int i = 0; i < dels.Count; i++)
                {
                    if (dels[i] == d)
                    {
                        return;
                    }
                }
            }
            else
            {
                dels = new List<ResponseDelegate>();
                m_DelegateMapping.Add(protocol, dels);
            }
            dels.Add(d);

        }

        public void DelRespDelegate(int protocol, ResponseDelegate d)
        {
            if (m_DelegateMapping.ContainsKey(protocol))
            {
                m_DelegateMapping[protocol].Remove(d);
            }
        }

        public Resp TryDeserialize(byte[] buffer)
        {
            m_RecvCommand = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            m_RecvHeartBeatData = GetHeartBeatData(m_RecvCommand);
            Resp ret = null;
            if (m_ProtocolMapping.ContainsKey(m_RecvHeartBeatData.code))
            {
                ret = m_ProtocolMapping[m_RecvHeartBeatData.code](m_RecvHeartBeatData);
                if (ret != null)
                {
                    if (m_DelegateMapping.ContainsKey(m_RecvHeartBeatData.code))
                    {
                        List<ResponseDelegate> dels = m_DelegateMapping[m_RecvHeartBeatData.code];
                        for (int i = 0; i < dels.Count; i++)
                        {
                            dels[i](ret);
                        }
                    }
                }
            }
            else
            {
                Log.Warning("no register protocol : " + m_RecvHeartBeatData.code + "!please reg to RegisterResp.");
            }

            return ret;
        }

        private SocketData GetHeartBeatData(string responseJson)
        {

            //Debug.Log(responseJson);

            m_JsonKeyValues = MiniJson.Deserialize(responseJson) as Dictionary<string, object>;

            m_SocketDataCache.code = Convert.ToInt32(m_JsonKeyValues["code"]);
            m_SocketDataCache.msg = Convert.ToString(m_JsonKeyValues["msg"]);
            m_SocketDataCache.data = m_JsonKeyValues["data"];

            return m_SocketDataCache;

            //return Utility.Json.ToObject<SocketData>(responseJson);
        }

    }


}