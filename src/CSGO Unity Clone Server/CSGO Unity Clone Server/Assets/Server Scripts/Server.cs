using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server
{
    public static int MaxPlayers { get; private set; }

    public static int Port { get; private set; }

    public static Dictionary<int, Client> Clients = new Dictionary<int, Client>();

    public delegate void PacketHandler(int clientId, Packet packet);
    public static Dictionary<int, PacketHandler> PacketHandlers;

    private static TcpListener TcpListener;
    private static UdpClient UdpListener;

    public static void StartOnLocalhost(int mapPlayes, int portNumber)
    {
        MaxPlayers = mapPlayes;
        Port = portNumber;

        InitializeServerData();

        Debug.Log("Starting server...");

        TcpListener = new TcpListener(IPAddress.Any, Port);
        TcpListener.Start();
        TcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);

        UdpListener = new UdpClient(Port);
        UdpListener.BeginReceive(UdpReceiveCallback, null);

        Debug.Log($"Server started on localhost, port: {Port}.");
    }

    public static void StartOnIPV4(int mapPlayes, int portNumber)
    {
        MaxPlayers = mapPlayes;
        Port = portNumber;
        var IPaddress = IPAddress.Parse(GetLocalIPAddress());

        InitializeServerData();

        Debug.Log("Starting server...");

        TcpListener = new TcpListener(IPaddress, Port);
        TcpListener.Start();
        TcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);

        UdpListener = new UdpClient(Port);
        UdpListener.BeginReceive(UdpReceiveCallback, null);

        Debug.Log($"Server on IP: {IPaddress} and on port: {Port}.");
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    private static void TcpConnectCallback(IAsyncResult result)
    {
        TcpClient tcpClient = TcpListener.EndAcceptTcpClient(result);
        TcpListener.BeginAcceptTcpClient(new AsyncCallback(TcpConnectCallback), null);
        Debug.Log($"Incoming connection from {tcpClient.Client.RemoteEndPoint}...");

        for (int i = 1; i <= MaxPlayers; i++)
        {
            //If client socket == null => slot is empty
            if (Clients[i].Tcp.Socket == null)
            {
                //connect client
                Clients[i].Tcp.Connect(tcpClient);
                //break method to make sure that 1 client only occupies 1 slot
                return;
            }
        }

        Debug.Log($"{tcpClient.Client.RemoteEndPoint} failed to connect: Server is full.");
    }

    private static void UdpReceiveCallback(IAsyncResult result)
    {
        try
        {
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = UdpListener.EndReceive(result, ref clientEndPoint);
            UdpListener.BeginReceive(UdpReceiveCallback, null);

            if (data.Length < 4)
            {
                return;
            }

            using (Packet packet = new Packet(data))
            {
                int clientId = packet.ReadInt();

                //Safety check; if clientId == 0 then the server crashes
                if (clientId == 0)
                {
                    return;
                }

                //If UdpEndPoint == null => new connection from new client;package is empty => Connect to server and then not handle the data
                if (Clients[clientId].Udp.UdpEndPoint == null)
                {
                    Clients[clientId].Udp.Connect(clientEndPoint);
                    return;
                }

                //Check if endpoints match; hacker could send another enpoint
                //ToString() because just comparing Endpoint to Endpoint will return false
                if (Clients[clientId].Udp.UdpEndPoint.ToString() == clientEndPoint.ToString())
                {
                    //Finally handle data
                    Clients[clientId].Udp.HandleData(packet);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error receiving UDP data: {ex}.\n" +
                      $"This error will be thrown by every disconnect, don't worry about this one.");
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
            Debug.Log($"Error sending UDP data: {ex} to {clientEndPoint}.");
        }
    }

    private static void InitializeServerData()
    {
        for (int i = 1; i <= MaxPlayers; i++)
        {
            Clients.Add(i, new Client(i));
        }

        PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ClientPackets.WelcomeReceived, ServerHandle.WelcomeReceived },
                {(int)ClientPackets.PlayerMovement, ServerHandle.PlayerMovement },
                {(int)ClientPackets.PlayerShoot, ServerHandle.PlayerShoot },
                {(int)ClientPackets.PlayerThrowItem, ServerHandle.PlayerThrowItem }

            };
        Debug.Log("Init packets.");
    }

    public static void Stop() 
    {
        TcpListener.Stop();
        UdpListener.Close();
    }
}
