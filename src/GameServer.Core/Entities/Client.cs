using System;
using System.Net;
using System.Net.Sockets;

namespace GameServer.Core.Entities
{
    public class Client
    {
        public int Id;
        public Tcp Tcp;
        public Udp Udp;
        //4 MB's
        public static int DataBufferSize = 4096;

        public Client(int clientId)
        {
            Id = clientId;
            Tcp = new Tcp(Id, DataBufferSize);
            Udp = new Udp(Id);
        }
    }

}
