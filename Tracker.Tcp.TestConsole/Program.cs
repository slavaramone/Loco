using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tracker.Tcp.TestConsole
{
    class Program
    {
        private const int Port = 8081;
        private const string Server = "127.0.0.1";

        static void Main(string[] args)
        {
            Console.WriteLine("Sending 10 TCP packets simultaneously...");

            Parallel.Invoke(
                () => SendPacket(),
                () => SendPacket(),
                () => SendPacket(),
                () => SendPacket(),
                () => SendPacket(),
                () => SendPacket(),
                () => SendPacket(),
                () => SendPacket(),
                () => SendPacket(),
                () => SendPacket()
            );
        }

        public static void SendPacket()
        {
            string message = "Test message";

            TcpClient client = new TcpClient(Server, Port);

            byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            NetworkStream stream = client.GetStream();

            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", message);

            stream.Close();
            client.Close();
        }
    }
}
