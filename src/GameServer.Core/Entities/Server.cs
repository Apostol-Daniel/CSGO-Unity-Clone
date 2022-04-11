using GameServer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GameServer.Core
{
    public class Server
    {
        public static int MaxPlayers { get; private set; }

        public static int Port { get; private set; }

        public static Dictionary<int, Client> Clients = new Dictionary<int, Client>();

        private static TcpListener tcpListener;

        public static void Start(int mapPlayes, int portNumber) 
        {
            MaxPlayers = mapPlayes;
            Port = portNumber;

            InitializeServerData();

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
            Console.WriteLine($"Incoming connection from {tcpClient.Client.RemoteEndPoint}...");

            for (int i = 1; i <= MaxPlayers; i++)
            {
                //If client socket == null => slot is empty
                if(Clients[i].Tcp.Socket == null)               
                {
                    //connect client
                    Clients[i].Tcp.Connect(tcpClient);
                    //break method to make sure that 1 client only occupies 1 slot
                    return;
                }
            }

            Console.WriteLine($"{tcpClient.Client.RemoteEndPoint} failed to connect: Server is full.");
        }

        private static void InitializeServerData() 
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                Clients.Add(i,new Client(i));
            }
        }
    }
}
