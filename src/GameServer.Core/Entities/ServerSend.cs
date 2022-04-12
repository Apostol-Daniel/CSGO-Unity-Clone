using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Core.Entities
{
    public class ServerSend
    {
        public static void Welcome(int toClient, string message) 
        {
            using (Packet packet = new Packet((int)ServerPackets.welcome)) 
            {
                packet.Write(message);
                packet.Write(toClient);

                SendTcpData(toClient, packet);
            }
        }

        private static void SendTcpData(int toClient, Packet packet) 
        {
            packet.WriteLength();
            Server.Clients[toClient].Tcp.SendData(packet);
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
    }
}
