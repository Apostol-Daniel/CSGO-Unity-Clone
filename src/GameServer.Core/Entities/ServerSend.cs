using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Core.Entities
{
    public class ServerSend
    {
        #region Packets
        public static void Welcome(int toClient, string message) 
        {
            using (Packet packet = new Packet((int)ServerPackets.Welcome)) 
            {
                packet.Write(message);
                packet.Write(toClient);

                SendTcpData(toClient, packet);
            }
        }

        public static void SpawnPlayer(int clientId, Player player) 
        {
            using(Packet packet = new Packet((int)ServerPackets.SpawnPlayer)) 
            {
                packet.Write(player.Id);
                packet.Write(player.UserName);
                packet.Write(player.Position);
                packet.Write(player.Rotation);
                SendTcpData(clientId, packet);
            }

        }
        #endregion  

        #region UdpData
        private static void SendUdpData(int clientId, Packet packet) 
        {
            packet.WriteLength();
            Server.Clients[clientId].Udp.SendData(packet);
        }

        public static void SendUdpDataToAll(Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.Clients[i].Udp.SendData(packet);
            }
        }

        public static void SendUdpDataToAllExceptOneClient(int exceptClient, Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != exceptClient)
                {
                    Server.Clients[i].Udp.SendData(packet);
                }
            }
        }

        #endregion

        #region TcpData

        private static void SendTcpData(int clientId, Packet packet) 
        {
            packet.WriteLength();
            Server.Clients[clientId].Tcp.SendData(packet);
        }

        public static void SendTcpDataToAll(Packet packet) 
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.Clients[i].Tcp.SendData(packet);
            }
        }

        public static void SendTcpDataToAllExceptOneClient(int exceptClient ,Packet packet)
        {
            packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if(i!= exceptClient) 
                {
                    Server.Clients[i].Tcp.SendData(packet);
                }
            }
        }

        #endregion
    }
}
