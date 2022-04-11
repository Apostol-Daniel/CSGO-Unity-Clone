using UnityEngine;

namespace Assets.Server
{
    public class Client : MonoBehaviour
    {
        public static Client Instance;
        public static int DataBufferSize = 4096;

        public string Ip = "127.0.0.1";

        public int Port = 26950;
        public int ClientId = 0;
        public Tcp TcpClient;

        private void Awake()
        {
            if(Instance == null) 
            {
                Instance = this;
            }

            else if (Instance != this) 
            {
                Debug.Log("Instance already exists, detroying object.");
                Destroy(this);
            }
        }

        private void Start()
        {
            TcpClient = new Tcp(Instance, DataBufferSize);
        }

        public void ConnectToServer()
        {
            TcpClient.Connect();
        }
    }
}
