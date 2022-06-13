using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Assets.Server
{
    public class Tcp
    {
        public TcpClient TcpSocket;
        private NetworkStream Stream;
        private Packet ReceivedData;
        private Client ClientInstance;       
                
        private byte[] ReceiveBuffer;
        private readonly int DataBufferSize;

        public Tcp(Client instance, int dataBufferSize)
        {
            DataBufferSize = dataBufferSize;
            ClientInstance = instance;
        }

        public void ConnectToLocalhost()
        {          
            TcpSocket = new TcpClient()
            {
                ReceiveBufferSize = DataBufferSize,
                SendBufferSize = DataBufferSize
            };


            ReceiveBuffer = new byte[DataBufferSize];
            TcpSocket.BeginConnect(ClientInstance.LocalhostIp, ClientInstance.Port, ConnectCallback, TcpSocket);            
        }

        public void ConnectToGivenIp(string ip)
        {
            TcpSocket = new TcpClient()
            {
                ReceiveBufferSize = DataBufferSize,
                SendBufferSize = DataBufferSize
            };


            ReceiveBuffer = new byte[DataBufferSize];
            TcpSocket.BeginConnect(ip, ClientInstance.Port, ConnectCallback, TcpSocket);          
        }

        private void ConnectCallback(IAsyncResult result)
        {
            TcpSocket.EndConnect(result);

            if (!TcpSocket.Connected)
            {
                return;
            }

            Stream = TcpSocket.GetStream();

            ReceivedData = new Packet();

            Stream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }

        public void SendData(Packet packet) 
        {
            try
            {
                if(TcpSocket != null) 
                {
                    Stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
            }
            catch (Exception ex)
            {

                Debug.Log($"Error sending data to server via TCP:{ex}.");
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLenght = Stream.EndRead(result);
                if (byteLenght <= 0)
                {
                    ClientInstance.Disconnect();
                    return;
                }

                byte[] data = new byte[byteLenght];
                Array.Copy(ReceiveBuffer, data, byteLenght);

                ReceivedData.Reset(HandleData(data));
                Stream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Disconnect();
                Console.WriteLine($"Error receiving TCP data : {ex}");
            }
        }

        private bool HandleData(byte[] data) 
        {
            int packetLenght = 0;

            ReceivedData.SetBytes(data);

            if(ReceivedData.UnreadLength() >= 4) 
            {
                packetLenght = ReceivedData.ReadInt();
                if(packetLenght <= 0) 
                {
                    return true;
                }
            }

            while(packetLenght > 0 && packetLenght <= ReceivedData.UnreadLength()) 
            {
                byte[] packetBytes = ReceivedData.ReadBytes(packetLenght);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet packet = new Packet(packetBytes))
                    {
                        int packetId = packet.ReadInt();
                        ClientInstance.PacketHandlers[packetId](packet);
                    }
                });

                packetLenght = 0;
                if (ReceivedData.UnreadLength() >= 4)
                {
                    packetLenght = ReceivedData.ReadInt();
                    if (packetLenght <= 0)
                    {
                        return true;
                    }
                }
            }

            if(packetLenght <= 1) 
            {
                return true;
            }

            return false;
        }
        
        private void Disconnect() 
        {
            ClientInstance.Disconnect();

            Stream = null;
            ReceivedData = null;
            ReceiveBuffer = null;
            TcpSocket = null;
        }
    }


}
