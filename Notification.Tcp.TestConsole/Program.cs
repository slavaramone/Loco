using System;
using System.Net.Sockets;

namespace Notification.Tcp.TestConsole
{
    class Program
    {
        private const int Port = 8082;
        private const string Server = "127.0.0.1";

        static void Main(string[] args)
        {
            Console.WriteLine("Sending TCP packet...");

            SendPacket();
        }

        public static void SendPacket()
        {
            string message = "{\"query\" : \"realtime-event\",\"time\" : \"2021-12-18 00:00:00\", \"cams\" : [ \"IP1\", \"IP2\"],\"types\" : [ 1, 2, 3]}";

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
