﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ConsoleApp2
{
    class Program
    {
        //private const int images_to_recive_before_breakup = 500;
        private static int MAX_REVICE_BUFFER_SIZE = 100000;
        static void Main(string[] args)
        {
            Console.WriteLine("Should recive images from a python process");
            StartRecivingImages();
            Console.WriteLine(DateTime.Now);
        }

        public static void StartRecivingImages()
        {
            IPAddress ipAddr = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 30303);

            var reciver = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            reciver.Bind(localEndPoint);
            reciver.Listen(10);
            while (true)
            {
                var connection = reciver.Accept();
                Console.WriteLine("Connection established with: " + connection.ToString());
                int counter = 0;
                string fileName = string.Empty;
                string filePath = @"C:\Users\kryst\Downloads\imagesFromPython\";

                var startTime = DateTime.Now;
                Console.WriteLine("Starting to recive images at: " + startTime);

                while (true)
                {
                    //if(images_to_recive_before_breakup == counter)
                    //{
                    //    Console.WriteLine("Recived " + images_to_recive_before_breakup + " images in : " + DateTime.Now.Subtract(startTime));
                    //    break;
                    //}
                    var imageSizeBuffer = new byte[sizeof(int)];

                    connection.Receive(imageSizeBuffer);

                    var imageDataSize = BitConverter.ToInt32(imageSizeBuffer, 0);
                    Console.WriteLine("Image size in byte from python: " + imageSizeBuffer.ToString());
                    Console.WriteLine("Expecting image of: " + imageDataSize + "bytes");
                    if (0 == imageDataSize)
                    {
                        Console.WriteLine("Recived an image size of 0 so am skipping save to file");
                        continue;
                    }

                    //fileName = ++counter + ".jpg";
                    fileName = "img.jpg";
                    using (var fs = new FileStream(filePath + fileName, FileMode.Create, FileAccess.Write))
                    {
                        while ((imageDataSize - MAX_REVICE_BUFFER_SIZE) > 0)
                        {
                            Console.WriteLine("Remaining data to recive: " + imageDataSize);

                            byte[] fileBuffer = new byte[MAX_REVICE_BUFFER_SIZE];

                            imageDataSize -= MAX_REVICE_BUFFER_SIZE;
                            //read maxReciveSize
                            connection.Receive(fileBuffer);
                            fs.Write(fileBuffer);
                        }
                        if (imageDataSize > 0)
                        {

                            var imageBuffer = new byte[imageDataSize];
                            connection.Receive(imageBuffer);
                            imageDataSize = 0;
                            fs.Write(imageBuffer, 0, imageBuffer.Length);
                        }
                        Console.WriteLine("Remaining data to recive: " + imageDataSize);

                        fs.Flush();
                        fs.Close();
                    }
                    Console.WriteLine("Saved a file that was recived from python");
                }
            }
        }
    }
}
