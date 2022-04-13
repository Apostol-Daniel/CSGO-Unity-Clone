using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GameServer.Core.Entities
{
    public class ServerHandle
    {
        public static void WelcomeReceived(int clientId, Packet packet) 
        {
            int clientIdControl = packet.ReadInt();
            string userName = packet.ReadString();

            Console.WriteLine($"{Server.Clients[clientId].Tcp.Socket.Client.RemoteEndPoint} connected successfully and is now player with Id:{clientId} and UserName:{userName }.");
            if(clientId != clientIdControl) 
            {
                //This message should never get printed; if it is, something went horribly wrong
                Console.WriteLine($"Playerr \"{userName}\" (Id:{clientId} has assumed the wrong client Id ({clientIdControl})!");
            }
            Server.Clients[clientId].SendIntoGame(userName);
        }
        
        public static void PlayerMovement(int clientId, Packet packet) 
        {
            bool[] inputs = new bool[packet.ReadInt()];
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = packet.ReadBool();
            }

            Quaternion rotation = packet.ReadQuaternion();

            Server.Clients[clientId].Player.SetInput(inputs, rotation);
        }
    }
}
