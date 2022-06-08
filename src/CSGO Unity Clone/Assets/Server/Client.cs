using System.Collections.Generic;
using UnityEngine;

namespace Assets.Server
{
    public class Client : MonoBehaviour
    {
        public static Client ClientInstance;
        public static int DataBufferSize = 4096;
        public delegate void PacketHandler(Packet packet);
        public Dictionary<int, PacketHandler> PacketHandlers;

        public string Ip = "127.0.0.1";
        public int Port = 26950;
        public int ClientId = 0;
        public Tcp TcpClient;
        public Udp UdpClient;

        bool IsConnected = false;

        private void Awake()
        {
            if(ClientInstance == null) 
            {
                ClientInstance = this;
            }

            else if (ClientInstance != this) 
            {
                Debug.Log("Instance already exists, detroying object.");
                Destroy(this);
            }
        }

        public static Client Instance()
        {
            return ClientInstance;
        }

        private void OnApplicationQuit()
        {
            Disconnect();
        }       

        public void ConnectToLocalhost()
        {
            TcpClient = new Tcp(ClientInstance, DataBufferSize);
            UdpClient = new Udp(ClientInstance);
            InitClientData();
            IsConnected = true;
            TcpClient.ConnectToLocalhost();
        }

        public void ConnectToGivenIp(string ip)
        {
            TcpClient = new Tcp(ClientInstance, DataBufferSize);
            UdpClient = new Udp(ClientInstance);
            InitClientData();
            IsConnected = true;
            TcpClient.ConnectToGivenIp(ip);
        }

        private void InitClientData()
        {
            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ServerPackets.Welcome, ClientHandle.Welcome },
                {(int)ServerPackets.SpawnPlayer, ClientHandle.SpawnPlayer },
                {(int)ServerPackets.PlayerPosition, ClientHandle.PlayerPosition },
                {(int)ServerPackets.PlayerRotation, ClientHandle.PlayerRotation },
                {(int)ServerPackets.PlayerDisconnected, ClientHandle.PlayerDisconnected },
                {(int)ServerPackets.PlayerHealth, ClientHandle.PlayerHealth },
                {(int)ServerPackets.PlayerRespawned, ClientHandle.PlayerRespwaned },
                {(int)ServerPackets.CreateItemSpawner, ClientHandle.CreateItemSpawner },
                {(int)ServerPackets.ItemSpawned, ClientHandle.ItemSpawned },
                {(int)ServerPackets.ItemPickedUp, ClientHandle.ItemPickedUp },
                {(int)ServerPackets.SpawnProjectile, ClientHandle.SpawnProjectile },
                {(int)ServerPackets.ProjectilePositon, ClientHandle.ProjectilePosition },
                {(int)ServerPackets.ProjectileExploded, ClientHandle.ProjectileExploded },
                {(int)ServerPackets.SpawnEnemy, ClientHandle.SpawnEnemy },
                {(int)ServerPackets.EnemyPosition, ClientHandle.EnemyPosition },
                {(int)ServerPackets.EnemyHealth, ClientHandle.EnemyHealth }
            };
            Debug.Log("Init Data");
        }

        public void Disconnect() 
        {
            if (IsConnected) 
            {
                IsConnected = false;
                TcpClient.TcpSocket.Close();
                UdpClient.UdpSocket.Close();

                Debug.Log("Disconnected from server.");
            }
        }
    }
}
