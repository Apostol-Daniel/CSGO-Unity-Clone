using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace GameServer.Core.Entities
{
    public class Tcp
    {
        public TcpClient Socket;

        private readonly int Id;
        private readonly int DataBufferSize;
        private NetworkStream Stream;
        private byte[] ReceiveBuffer;

        public Tcp(int id, int dataBufferSize)
        {
            Id = id;
            DataBufferSize = dataBufferSize;
        }

        public void Connect(TcpClient socket) 
        {
            Socket = socket;
            Socket.ReceiveBufferSize = DataBufferSize;
            Socket.SendBufferSize = DataBufferSize;

            Stream = Socket.GetStream();
            ReceiveBuffer = new byte[DataBufferSize];

            Stream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult result) 
        {
            try
            {
                int byteLenght = Stream.EndRead(result);
                if(byteLenght <= 0) 
                {
                    return;
                }

                byte[] data = new byte[byteLenght];
                Array.Copy(ReceiveBuffer, data, byteLenght);

                Stream.BeginRead(ReceiveBuffer,0,DataBufferSize,ReceiveCallback, null);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error receiving TCP data : {ex}");
            }
        }
    }
}
