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

        private void Start()
        {
            TcpClient = new Tcp(ClientInstance, DataBufferSize);
            UdpClient = new Udp(ClientInstance);
        }

        public void ConnectToServer()
        {
            InitClientData();
            TcpClient.Connect();
        }

        private void InitClientData()
        {
            PacketHandlers = new Dictionary<int, PacketHandler>()
            {
                {(int)ServerPackets.Welcome, ClientHandle.Welcome }               
            };
            Debug.Log("Init Data");
        }
    }
}
