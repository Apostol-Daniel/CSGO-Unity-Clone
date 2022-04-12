using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer.Core.Entities
{
    public class ServerHandle
    {
        public static void WelcomeReeived(int clientId, Packet packet) 
        {
            int clientIdControl = packet.ReadInt();
            string userName = packet.ReadString();

            Console.WriteLine($"{Server.Clients[clientId].Tcp.Socket.Client.RemoteEndPoint} connected successfully and is now player {userName}.");
            if(clientId != clientIdControl) 
            {
                //This message should never get printed; if it is, something went horribly wrong
                Console.WriteLine($"Playerr \"{userName}\" (Id:{clientId} has assumed the wrong client Id ({clientIdControl})!");
            }
        }
    }
}
