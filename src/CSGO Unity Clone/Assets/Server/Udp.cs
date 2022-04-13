using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Server
{
    public class Udp
    {
        public UdpClient UdpSocket;
        public IPEndPoint UdpEndPoint;
        private readonly Client ClientInstance;

        public Udp(Client instance)
        {
            ClientInstance = instance;
            UdpEndPoint = new IPEndPoint(IPAddress.Parse(ClientInstance.Ip), ClientInstance.Port);
        }

        public void Connect(int localPort) 
        {
            UdpSocket = new UdpClient(localPort);
            UdpSocket.Connect(UdpEndPoint);
            UdpSocket.BeginReceive(ReceiveCallback, null);

            using(Packet packet = new Packet()) 
            {
                SendData(packet);
            }
        }

        public void SendData(Packet packet) 
        {
            try
            {
                //There can be only one UdpClient per server, ClientId needed to know which client sends data
                packet.InsertInt(ClientInstance.ClientId);
                if(UdpSocket!= null) 
                {
                    UdpSocket.BeginSend(packet.ToArray(), packet.Length(), null, null);
                }
            }
            catch (Exception ex)
            {

                Debug.Log($"Error sending data to server via UDP:{ex}.");
            }
        }

        private void ReceiveCallback(IAsyncResult result) 
        {
            try
            {
                byte[] data = UdpSocket.EndReceive(result, ref UdpEndPoint);
                UdpSocket.BeginReceive(ReceiveCallback, null);

                if (data.Length < 4) 
                {
                    ClientInstance.Disconnect();
                    return;
                }

                HandleData(data);
            }
            catch (Exception ex)
            {
                Disconnect();
                Console.WriteLine($"Error receiving UDP data : {ex}");                
            }
        }


        private void HandleData(byte[] data) 
        {
            using(Packet packet = new Packet(data)) 
            {
                int packetLength = packet.ReadInt();
                data = packet.ReadBytes(packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using(Packet packet = new Packet(data)) 
                {
                    int packetId = packet.ReadInt();
                    ClientInstance.PacketHandlers[packetId](packet);
                }
            });
        }

        public void Disconnect() 
        {
            ClientInstance.Disconnect();
            UdpEndPoint = null;
            UdpSocket = null;
        }
    }
}
