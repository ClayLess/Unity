using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Assets
{
    public class SocketServer
    {
        //以下默认都是私有的成员  
        Socket serverSocket; //服务器端socket  
        Socket clientSocket; //客户端socket  
        IPEndPoint ipEnd; //侦听端口  
        string recvStr; //接收的字符串  
        string sendStr; //发送的字符串  
        byte[] recvData = new byte[1024]; //接收的数据，必须为字节  
        byte[] sendData = new byte[1024]; //发送的数据，必须为字节  
        int recvLen; //接收的数据长度  
        Thread connectThread; //连接线程  
        public int mode;
        public string database_info;
        public int handid;
        public bool showfakehand;
        //初始化  
        public void InitSocket()
        {
            //定义侦听端口,侦听任何IP  
            ipEnd = new IPEndPoint(IPAddress.Any, 5566);
            //定义套接字类型,在主线程中定义  
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //连接  
            serverSocket.Bind(ipEnd);
            //开始侦听,最大10个连接  
            serverSocket.Listen(10);
            mode = 0;
            database_info = "";
            handid = 0;
            showfakehand = false;
            //开启一个线程连接，必须的，否则主线程卡死  
            connectThread = new Thread(new ThreadStart(SocketReceive));
            connectThread.Start();
        }

        //连接  
        public void SocketConnect()
        {
            if (clientSocket != null)
                clientSocket.Close();
            //控制台输出侦听状态  
            Console.WriteLine("Waiting for a client");
            //一旦接受连接，创建一个客户端  
            clientSocket = serverSocket.Accept();
            //获取客户端的IP和端口  
            IPEndPoint ipEndClient = (IPEndPoint)clientSocket.RemoteEndPoint;
            //输出客户端的IP和端口  
            Console.WriteLine("Connect with " + ipEndClient.Address.ToString() + ":" + ipEndClient.Port.ToString());
            //连接成功则发送数据  
            sendStr = "Welcome to my server";
            SocketSend(sendStr);
        }

        public void SocketSend(string sendStr)
        {
            //清空发送缓存  
            sendData = new byte[1024];
            //数据类型转换  
            sendData = Encoding.ASCII.GetBytes(sendStr);
            //发送  
            clientSocket.Send(sendData, sendData.Length, SocketFlags.None);
        }

        //服务器接收  
        public void SocketReceive()
        {
            //连接  
            SocketConnect();
            //进入接收循环  
            while (true)
            {
                //对data清零  
                recvData = new byte[1024];
                //获取收到的数据的长度  
                try
                {
                    recvLen = clientSocket.Receive(recvData);
                }
                catch
                {
                    Console.WriteLine(clientSocket.RemoteEndPoint.ToString() + " Offline");
                    SocketConnect();
                }
                //如果收到的数据长度为0，则重连并进入下一个循环  
                if (recvLen == 0)
                {
                    SocketConnect();
                    continue;
                }
                //输出接收到的数据  
                recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
                //打印
                //Console.WriteLine(recvStr);
                mode = Int32.Parse(recvStr);
                //根据显示模式处理数据
                switch(mode)
                {
                    case 0:
                        recvData = new byte[1000];
                        recvLen = clientSocket.Receive(recvData);
                        database_info = Encoding.ASCII.GetString(recvData, 0, recvLen);
                        break;
                    case 1:
                        recvData = new byte[1000];
                        recvLen = clientSocket.Receive(recvData);
                        handid = Int32.Parse(Encoding.ASCII.GetString(recvData, 0, recvLen));
                        showfakehand = true;
                        break;
                    case 2:
                        showfakehand = false;
                        break;
                    case 3:
                        showfakehand = false;
                        break;
                    default:
                        break;
                }
                //返回处理成功  
                sendStr = "Receive data in mode: "+mode;
                SocketSend(sendStr);
            }
        }

        //连接关闭  
        public void SocketQuit()
        {
            //先关闭客户端  
            if (clientSocket != null)
                clientSocket.Close();
            //再关闭线程  
            if (connectThread != null)
            {
                connectThread.Interrupt();
                connectThread.Abort();
            }
            //最后关闭服务器  
            serverSocket.Close();
            Console.WriteLine("diconnect");
        }
    }
}
