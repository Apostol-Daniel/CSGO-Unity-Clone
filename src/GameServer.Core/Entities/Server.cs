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

        public delegate void PacketHandler(int clientId, Packet packet);
        public static Dictionary<int, PacketHandler> PacketHandlers;

        private static TcpListener TcpListener;
        private static UdpClient UdpListener;

        public static void Start(int mapPlayes, int portNumber) 
        {
            MaxPlayers = mapPlayes;
            Port = portNumber;

            InitializeServerData();

            Console.WriteLine("Starting server...");

            TcpListener = new TcpListener(IPAddress.Any, Port);
            TcpListener.Start();
            TcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);

            UdpListener = new UdpClient(Port);
            UdpListener.BeginReceive(UdpReceiveCallback, null);

            Console.WriteLine($"Server started on {Port}.");
        }

        private static void TcpConnectCallback(IAsyncResult result) 
        {
            TcpClient tcpClient = TcpListener.EndAcceptTcpClient(result);
            TcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);
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

        private static void UdpReceiveCallback(IAsyncResult result) 
        {
            try
            {
                IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[]  data = UdpListener.EndReceive(result, ref clientEndPoint);
                UdpListener.BeginReceive(UdpReceiveCallback, null);

                if(data.Length < 4) 
                {
                    return;
                }

                using(Packet packet = new Packet(data)) 
                {
                    int clientId = packet.ReadInt();

                    //Safety check; if clientId == 0 then the server crashes
                    if(clientId == 0) 
                    {
                        return;
                    }

                    //If UdpEndPoint == null => new connection from new client;package is empty => Connect to server and then not handle the data
                    if(Clients[clientId].Udp.UdpEndPoint == null) 
                    {
                        Clients[clientId].Udp.Connect(clientEndPoint);
                        return;
                    }

                    //Check if endpoints match; hacker could send another enpoint
                    //ToString() because just comparing Endpoint to Endpoint will return false
                    if(Clients[clientId].Udp.UdpEndPoint.ToString() == clientEndPoint.ToString()) 
                    {
                        //Finally handle data
                        Clients[clientId].Udp.HandleData(packet);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving UDP data: {ex}.");
            }
        }

        public static void SendUdpData(IPEndPoint clientEndPoint, Packet packet) 
        {
            try
            {
                if (clientEndPoint != null) 
                {
                    UdpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending UDP data: {ex} to {clientEndPoint}.");
            }
        }

        private static void InitializeServerData() 
        {
            for (int i = 1; i <= MaxPlayers; i++)
            {
                Clients.Add(i,new Client(i));
            }

            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                {(int)ClientPackets.udpTestReceived, ServerHandle.UdpTestReceived }
            };
            Console.WriteLine("Init packets.");
        }
    }
}
