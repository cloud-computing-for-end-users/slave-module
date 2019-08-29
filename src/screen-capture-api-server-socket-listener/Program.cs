using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace screen_capture_api_server_socket_listener
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Streaming test started");
            const int FFMPEGStreamingPort = 60607;

            TcpListener server = new TcpListener(IPAddress.Any, FFMPEGStreamingPort);
            // we set our IP address as server's address, and we also set the port: 9999

            server.Start();  // this will start the server
            Console.WriteLine("STARTED");

            while (true) //we wait for a connection
            {
                TcpClient client = server.AcceptTcpClient(); //if a connection exists, the server will accept it
                Console.WriteLine("ACCEPTED");

                NetworkStream ns = client.GetStream(); //networkstream is used to send/receive messages

                byte[] hello = new byte[100]; //any message must be serialized (converted to byte array)
                hello = Encoding.Default.GetBytes("hello world"); //conversion string => byte array

                ns.Write(hello, 0, hello.Length); //sending the message

                while (client.Connected) //while the client is connected, we look for incoming messages
                {
                    byte[] msg = new byte[1024]; //the messages arrive as byte array
                    ns.Read(msg, 0, msg.Length); //the same networkstream reads the message sent by the client
                    Console.WriteLine(Encoding.Default.GetString(msg).Trim()); //now , we write the message as string
                }
            }
        }
    }
}
