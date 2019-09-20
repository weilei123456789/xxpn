using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace Penny
{
    public class SocketHelper
    {
        private DataHolder m_DataHolder = new DataHolder();

        private GameFrameworkAction m_ConnectSuccessDelegate = null;
        private GameFrameworkAction m_ConnectFailedDelegate = null;
        private Queue<byte[]> m_DataQueue = new Queue<byte[]>();
        private Socket m_Socket = null;
        private bool m_IsStopReceive = true;

        private GameFrameworkAction m_RegisterResp = null;
        public GameFrameworkAction RegisterResp
        {
            set { m_RegisterResp = value; }
            get { return m_RegisterResp; }
        }

        public Thread m_ThreadReceive = null;

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <returns>The connect.</returns>
        /// <param name="serverIp">Server ip.</param>
        /// <param name="serverPort">Server port.</param>
        /// <param name="connectSuccessCallback">Connect callback.</param>
        /// <param name="connectFailedCallback">Connect failed callback.</param>
        public void Connect(string serverIp, int serverPort, GameFrameworkAction connectSuccessCallback, GameFrameworkAction connectFailedCallback)
        {
            m_ConnectSuccessDelegate = connectSuccessCallback;
            m_ConnectFailedDelegate = connectFailedCallback;

            //采用TCP方式连接  
            m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //服务器IP地址  
            IPAddress address = IPAddress.Parse(serverIp);

            //服务器端口  
            IPEndPoint endpoint = new IPEndPoint(address, serverPort);

            //异步连接,连接成功调用connectCallback方法  
            IAsyncResult result = m_Socket.BeginConnect(endpoint, new AsyncCallback(ConnectedSuccessCallback), m_Socket);

            //这里做一个超时的监测，当连接超过5秒还没成功表示超时  
            bool success = result.AsyncWaitHandle.WaitOne(5000, true);
            if (!success)
            {
                //超时  
                if (m_ConnectFailedDelegate != null)
                {
                    m_ConnectFailedDelegate();
                }
                Closed();
            }
            else
            {
                //与socket建立连接成功，开启线程接受服务端数据。  
                m_IsStopReceive = false;
                m_ThreadReceive = new Thread(ReceiveSocket)
                {
                    IsBackground = true
                };
                m_ThreadReceive.Start();
            }
            if (m_RegisterResp != null)
                m_RegisterResp();
        }

        private void ConnectedSuccessCallback(IAsyncResult asyncConnect)
        {
            if (!m_Socket.Connected)
            {
                if (m_ConnectFailedDelegate != null)
                {
                    m_ConnectFailedDelegate();
                }
                return;
            }

            if (m_ConnectSuccessDelegate != null)
            {
                m_ConnectSuccessDelegate();
            }
        }

        private void ReceiveSocket()
        {
            m_DataHolder.Reset();
            while (m_Socket != null && !m_IsStopReceive)
            {
                if (!m_Socket.Connected)
                {
                    //与服务器断开连接跳出循环  
                    Log.Warning("Failed To Client Socket Server.");
                    m_Socket.Close();
                    break;
                }

                try
                {
                    //接受数据保存至bytes当中  
                    byte[] bytes = new byte[4096];
                    //Receive方法中会一直等待服务端回发消息  
                    //如果没有回发会一直在这里等着。  

                    int i = m_Socket.Receive(bytes);

                    if (i <= 0)
                    {
                        m_Socket.Close();
                        break;
                    }
                    m_DataHolder.PushData(bytes, i);

                    while (m_DataHolder.IsFinished())
                    {
                        m_DataQueue.Enqueue(m_DataHolder.m_RecvData);

                        m_DataHolder.RemoveFromHead();
                    }

                }
                catch (Exception e)
                {
                    Log.Info("Failed to clientSocket error." + e);
                    if (m_Socket != null)
                        m_Socket.Close();
                    break;
                }
            }
        }

        //接收到数据放入数据队列，按顺序取出
        public void SocketUpdate(ProtoManager protoManager)
        {
            if (protoManager == null) return;

            if (m_DataQueue.Count > 0)
            {
                protoManager.TryDeserialize(m_DataQueue.Dequeue());
            }
        }

        //关闭Socket  
        public void Closed()
        {
            m_IsStopReceive = true;
            if (m_ThreadReceive != null)
            {
                m_ThreadReceive.Abort();//关闭线程
                m_ThreadReceive = null;
            }

            if (m_Socket != null && m_Socket.Connected)
            {
                m_Socket.Shutdown(SocketShutdown.Both);
                m_Socket.Close();
            }
            m_Socket = null;

        }

        public bool IsConnect()
        {
            return m_Socket != null && m_Socket.Connected;
        }

        private byte[] m_BufferData = null;

        public void SendMessage(Request req)
        {
            if (m_Socket == null)
            {
                return;
            }
            if (!m_Socket.Connected)
            {
                Closed();
                return;
            }
            try
            {
                DataStream m_BufferWriter = new DataStream(true);
                req.Serialize(m_BufferWriter);
                byte[] msg = m_BufferWriter.ToByteArray();

                byte[] buffer = new byte[msg.Length + 4];
                DataStream writer = new DataStream(buffer, true);

                writer.WriteInt32((uint)msg.Length);//增加数据长度
                writer.WriteRaw(msg);

                m_BufferData = writer.ToByteArray();

                IAsyncResult asyncSend = m_Socket.BeginSend(m_BufferData, 0, m_BufferData.Length, SocketFlags.None, new AsyncCallback(SendCallback), m_Socket);
                bool success = asyncSend.AsyncWaitHandle.WaitOne(5000, true);
                if (!success)
                {
                    Closed();
                }
            }
            catch (Exception e)
            {
                Log.Info("send error : " + e.ToString());
            }
        }

        private void SendCallback(IAsyncResult asyncConnect)
        {
            //Log.Info("Send Success");
        }


    }
}