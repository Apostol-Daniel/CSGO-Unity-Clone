using System;
using System.Net;
using System.Net.Sockets;

namespace GameServer.Core
{
    public class Server
    {
        public static int MaxPlayers { get; private set; }

        public static int Port { get; private set; }

        private static TcpListener tcpListener;

        public static void Start(int mapPlayes, int portNumber) 
        {
            MaxPlayers = mapPlayes;
            Port = portNumber;

            Console.WriteLine("Starting server...");

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);

            Console.WriteLine($"Server started on {Port}.");
        }

        private static void TcpConnectCallback(IAsyncResult result) 
        {
            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);
        }
    }
}
