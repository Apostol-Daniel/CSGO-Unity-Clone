using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Server
{
    public class Tcp
    {
        public TcpClient Socket;

        private NetworkStream Stream;
        private byte[] ReceiveBuffer;
        private readonly int DataBufferSize;
        private readonly Client Instance;

        public Tcp(Client instance,int dataBufferSize)
        {
            DataBufferSize = dataBufferSize;
            Instance = instance;
        }

        public void Connect(TcpClient socket) 
        {
            Socket = socket;

            Socket.ReceiveBufferSize = DataBufferSize;
            Socket.SendBufferSize = DataBufferSize;

            ReceiveBuffer = new byte[DataBufferSize];
            Socket.BeginConnect(Instance.Ip, Instance.Port, ConnectCallback, Socket);
        }

        private void ConnectCallback(IAsyncResult result) 
        {
            Socket.EndConnect(result);

            if (!Socket.Connected)
            {
                return;
            }

            Stream = Socket.GetStream();

            Stream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLenght = Stream.EndRead(result);
                if (byteLenght <= 0)
                {
                    return;
                }

                byte[] data = new byte[byteLenght];
                Array.Copy(ReceiveBuffer, data, byteLenght);

                Stream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error receiving TCP data : {ex}");
            }
        }
    }


}
