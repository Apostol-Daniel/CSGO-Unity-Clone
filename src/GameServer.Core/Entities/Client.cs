using System;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace GameServer.Core.Entities
{
    public class Client
    {
        public int Id;
        public Tcp Tcp;
        public Udp Udp;
        public Player Player;
        //4 MB's
        public static int DataBufferSize = 4096;

        public Client(int clientId)
        {
            Id = clientId;
            Tcp = new Tcp(Id, DataBufferSize);
            Udp = new Udp(Id);
        }

        public void SendIntoGame(string playerName) 
        {
            Player = new Player(Id, playerName, new Vector3(0, 0, 0));

            //Send alreday connected players info to new player
            foreach(Client client in Server.Clients.Values) 
            {
                if(client.Player != null) 
                {
                    if(client.Id != Id) 
                    {
                        ServerSend.SpawnPlayer(Id, client.Player);
                    }
                }
            }

            //Send new player info to all players and himself
            foreach (Client client in Server.Clients.Values)
            {
                if(client.Player != null) 
                {
                    ServerSend.SpawnPlayer(client.Id, Player);
                }
            }
        }

        public void Disconnect() 
        {
            Console.WriteLine($"{Tcp.Socket.Client.RemoteEndPoint} has disconnected.");

            Player = null;

            Tcp.Disconnect();
            Udp.Disconnect();
        }
    }

}
