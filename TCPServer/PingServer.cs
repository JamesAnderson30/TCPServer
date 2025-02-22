using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    class PingServer
    {
        static void Main()
        {
            const int port = 5000;
            TcpListener listener = new TcpListener(IPAddress.Any, port);

            listener.Start();

            Console.WriteLine($"Server started on port {port}, waiting for connection...");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();

                Console.WriteLine("Client Connected");

                Task.Run(() => HandleClient(client));
            }
        }

        static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            Console.WriteLine($"Request Received: {request}");

            string response = request.Trim().ToLower() == "ping" ? "pong" : "unknown command";
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            stream.Write(responseBytes, 0, responseBytes.Length);

            Console.WriteLine($"Response Sent: {response}");

            client.Close();
        }
    }
}
