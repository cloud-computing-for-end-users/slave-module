using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using NUnit.Framework;
using screen_capture_api;

namespace Tests
{
    public class Tests
    {
        ScreenCapture sc;

        [SetUp]
        public void Setup()
        {
            sc = new ScreenCapture();
        }

        //[Test]
        public void Test2()
        {
            var path = sc.CaptureScreen("Notepad++");
            Console.WriteLine(path);
        }

        //[Test]
        public void StreamingTest()
        {
            /*
            var bytes = new byte[] {127, 0, 0, 1};
            IPAddress ipAddress = new IPAddress(bytes);
            Console.WriteLine("IP Address:" + ipAddress);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, FFMPEGStreamingPort);

            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(localEndPoint);
            listener.Listen(100);

            while (true)
            {
                var connection = listener.Accept();

                new Thread(

                    () => { HandleConnection(connection); }
                ).Start();
            }

            /*
            Socket clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(IPAddress.Loopback, FFMPEGStreamingPort);

            if (false == clientSocket.Connected)
            {
                throw new Exception("Could not connect to the server");
            }
            */
        }

        private void HandleConnection(Socket connection)
        {
            Console.WriteLine("Established");
            byte[] buffer = new byte[1];

            while (true)
            {
                connection.Receive(buffer);
                Console.Write(buffer[0]);
            }
            connection.Close();
        }

    }
}