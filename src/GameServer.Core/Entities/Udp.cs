using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GameServer.Core.Entities
{
    public class Udp
    {
        public IPEndPoint UdpEndPoint;

        private int UdpClientId;

        public Udp(int clientId)
        {
            UdpClientId = clientId;
        }

        public void Connect(IPEndPoint endPoint)    
        {
            UdpEndPoint = endPoint;           
        }

        public void SendData(Packet packet) 
        {
            Server.SendUdpData(UdpEndPoint, packet);
        }

        public void HandleData(Packet packetData) 
        {
            int packetLengyh = packetData.ReadInt();
            byte[] packetBytes = packetData.ReadBytes(packetLengyh);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(packetBytes))
                {
                    int packetId = packet.ReadInt();
                    Server.PacketHandlers[packetId](UdpClientId, packet);
                }
            });
        }

        public void Disconnect() 
        {
            UdpEndPoint = null;
        }
    }
}
