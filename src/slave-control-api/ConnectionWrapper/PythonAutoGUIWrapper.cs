using message_based_communication.model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace slave_control_api.ConnectionWrapper
{
    public class PythonAutoGUIWrapper
    {
        //private const string pathToPyFile = @"C:\Users\MSI\PycharmProjects\HelloWorld\HelloWorld.py";
        //private const string pathToPythonDotExe = @"C:\Users\MSI\AppData\Local\Programs\Python\Python37-32\python.exe";




        //private const int pythonMouseControlApiPort = 60606;

        Socket clientSocket = null;
        /// <summary>
        /// will always look for the connection to python on the loopback IP
        /// </summary>
        /// <param name="portWherePythonIs"></param>
        public PythonAutoGUIWrapper(Port portWherePythonIs)
        {
            //start the python process

            clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(IPAddress.Loopback, portWherePythonIs.ThePort);

            if (false == clientSocket.Connected)
            {
                throw new Exception("Could not connect to the python server");
            }

        }

        public CommandCallResult executeApiCommand(string command, List<string> argsList = null)
        {
            string fullCommand = command;

            foreach (var item in argsList)
            {
                fullCommand += " " + item;
            }

            var encodedParams = System.Text.Encoding.UTF8.GetBytes(fullCommand);
            var encodedParamLength = Convert.ToByte(encodedParams.Length);
            var byteArray = new byte[1];
            byteArray[0] = encodedParamLength;
            clientSocket.Send(byteArray);
            clientSocket.Send(encodedParams);

            return null; //TODO Look if this need fixing
        }
        public class ExecutionException : Exception
        {
            public ExecutionException() : base("The command was not sucessful in executing the command")
            {

            }
        }
        public class CommandCallResult
        {
            private static CommandCallResult dummy = new CommandCallResult(1);
            public static CommandCallResult ExecutionError { get { return dummy; } }


            public int ExitCode { get; set; }
            public string ReturnValue { get; set; }

            public CommandCallResult(int ExitCode)
            {
                this.ExitCode = ExitCode;
            }
            public CommandCallResult(int ExitCode, string ReturnValue) : this(ExitCode)
            {
                this.ReturnValue = ReturnValue;
            }


        }
    }
}
