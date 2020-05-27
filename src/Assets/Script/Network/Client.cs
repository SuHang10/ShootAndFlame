using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using Logic;

namespace WebManager
{
    static public class Client
    {
        private static Socket client;
        private static EndPoint sendPoint, recvPoint;
        private static Thread recvThread;
        public static bool recvClosed;
        public static void Initialize()
        {
            /*if(client!=null)
                client.Close();*/
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sendPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            recvPoint = new IPEndPoint(IPAddress.Any, 0);
            recvClosed = false;
            recvThread = new Thread(ReciveMsg);
            recvThread.Start();
            Debug.Log("客户端已经开启");
        }

        public static Exception BindPort(string str)
        {
            try
            {
                int port = Convert.ToInt32(str);
                client.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"),port));
            }
            catch (Exception e)
            {
                return e;
            }
            return null;
        }

        public static void SendMsg(Dictionary<string,string> dict)
        {
            try
            {
                string s = Turner.ToString(dict);
                //Debug.Log(s);
                client.SendTo(Encoding.UTF8.GetBytes(s), sendPoint);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public static void ReciveMsg()
        {
            while (!recvClosed)
            {
                try
                {
                    EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
                    byte[] buffer = new byte[2048];
                    int length = client.ReceiveFrom(buffer, ref point);//接收数据报
                    string message = Encoding.UTF8.GetString(buffer, 0, length);
                    LogicMain.ParseACK(Turner.ToDict(message));
                }
                catch (Exception e){
                    
                }
            }
            Debug.Log("线程关闭");
        }

        public static void Close()
        {
            recvClosed = true;
            client.Close();
        }
    }
}
