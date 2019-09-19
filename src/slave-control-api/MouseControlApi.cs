using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace slave_control_api
{
    //[Obsolete]
    //public class MouseControlApi
    //{
    //    //non static start
    //    private const int pythonMouseControlApiPort = 60606;

    //    Socket clientSocket = null;
    //    public MouseControlApi()
    //    {
    //        clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
    //        clientSocket.Connect(IPAddress.Loopback, pythonMouseControlApiPort);

    //        if(false == clientSocket.Connected)
    //        {
    //            throw new Exception("Could not connect to the python server");
    //        }

    //    }


    //    //non static end



    //    public bool throwExceptionOnFailedCommandExecution = false;

    //    private const string pathToPyFile = @"C:\Users\MSI\PycharmProjects\HelloWorld\HelloWorld.py";
    //    private const string pathToPythonDotExe = @"C:\Users\MSI\AppData\Local\Programs\Python\Python37-32\python.exe";

    //    private enum ApiComman
    //    {
    //        MoveMouse,
    //        ClickLeft,
    //        ClickRight,
    //        ClickDouble,
    //        ScrollDown,
    //        ScrollUp,
    //        LocationOfMouse
    //    }

    //    private static Dictionary<ApiComman, string> ApiCommandStringParam = new Dictionary<ApiComman, string>();
    //    static MouseControlApi()
    //    {
    //        // Adding the protocol defined cmd parameters to the dictionary
    //        {
    //            ApiCommandStringParam.Add(ApiComman.ClickLeft, "-cl");
    //            ApiCommandStringParam.Add(ApiComman.ClickRight, "-cr");
    //            ApiCommandStringParam.Add(ApiComman.ClickDouble, "-cd");
    //            ApiCommandStringParam.Add(ApiComman.LocationOfMouse, "-lo");
    //            ApiCommandStringParam.Add(ApiComman.ScrollDown, "-sd");
    //            ApiCommandStringParam.Add(ApiComman.ScrollUp, "-su");
    //            ApiCommandStringParam.Add(ApiComman.MoveMouse, "-mo");
    //        }
    //    }

    //    public void moveMouse(int x, int y)
    //    {
    //        List<string> args = new List<string>();
    //        args.Add(x.ToString());
    //        args.Add(y.ToString());
    //        var result = executeApiCommand(ApiComman.MoveMouse, args);
    //        throwExceptionIfNeeded(result);
    //    }

    //    public void MouseLeftClick()
    //    {
    //        var result = executeApiCommand(ApiComman.ClickLeft);
    //        throwExceptionIfNeeded(result);
    //    }


    //    public void MouseRightClick()
    //    {
    //        var result = executeApiCommand(ApiComman.ClickRight);
    //        throwExceptionIfNeeded(result);
    //    }
    //    public void MouseDoubleClick()
    //    {
    //        var result = executeApiCommand(ApiComman.ClickDouble);
    //        throwExceptionIfNeeded(result);
    //    }
    //    public void MouseScrollDown()
    //    {
    //        var result = executeApiCommand(ApiComman.ScrollDown);
    //        throwExceptionIfNeeded(result);
    //    }
    //    public void MouseScrollUp()
    //    {
    //        var result = executeApiCommand(ApiComman.ScrollUp);
    //        throwExceptionIfNeeded(result);
    //    }
    //    /// <summary>
    //    /// returns a tuple with the x coordinate as the first element and y as the second element
    //    /// </summary>
    //    /// <returns></returns>
    //    public Tuple<int, int> MouseLocation()
    //    {
    //        var result = executeApiCommand(ApiComman.ClickDouble);
    //        throwExceptionIfNeeded(result);

    //        var location = result.ReturnValue;
    //        var data = location.Split(' ');
    //        int x = int.Parse(data[0]);
    //        int y = int.Parse(data[1]);
    //        return new Tuple<int, int>(x, y);
    //    }


    //    /// <summary>
    //    /// throws an exception if  @throwExceptionOnFailedCommandExecution is true and the exit code from the command call is not 0
    //    /// </summary>
    //    /// <param name="result"></param>
    //    private void throwExceptionIfNeeded(CommandCallResult result)
    //    {
    //        if (throwExceptionOnFailedCommandExecution)
    //        {
    //            if (0 != result.ExitCode)
    //            {
    //                throw new ExecutionException();
    //            }
    //        }
    //    }


    //    /// <summary>
    //    /// returns the exit code of the python script e.g. 0 == everything is good, anything else means something is not so good
    //    /// </summary>
    //    /// <param name="command"></param>
    //    /// <param name="args"></param>
    //    /// <returns></returns>
    //    private CommandCallResult executeApiCommand(ApiComman command, List<string> argsList = null)
    //    {
    //        string arg = ApiCommandStringParam[command];

    //        //appending extra arguments when needed
    //        switch (command)
    //        {
    //            case ApiComman.MoveMouse:
    //                //check that there is the right number of arguments
    //                if (2 != argsList.Count)
    //                {
    //                    //something went bad
    //                    return CommandCallResult.ExecutionError;
    //                }

    //                foreach (var item in argsList)
    //                {
    //                    arg += " " + item;
    //                }
    //                break;
    //        }

    //        var encodedParams = System.Text.Encoding.UTF8.GetBytes(arg);
    //        var encodedParamLength = Convert.ToByte(encodedParams.Length);
    //        var byteArray = new byte[1];
    //        byteArray[0] = encodedParamLength;
    //        clientSocket.Send(byteArray);
    //        clientSocket.Send(encodedParams);

    //        return null;


    //        // will no longer be needed if socket solution works
    //        if (false)
    //        {

    //            ProcessStartInfo start = new ProcessStartInfo();
    //            start.FileName = pathToPythonDotExe;
    //            start.Arguments = string.Format("\"{0}\" \"{1}\"", pathToPyFile, arg);
    //            start.UseShellExecute = false;// Do not use OS shell
    //            start.CreateNoWindow = true; // We don't need new window
    //            start.RedirectStandardOutput = true;// Any output, generated by application will be redirected back
    //            start.RedirectStandardError = true; // Any error in standard output will be redirected back (for example exceptions)
    //            using (Process process = Process.Start(start))
    //            {
    //                using (StreamReader reader = process.StandardOutput)
    //                {
    //                    string stderr = process.StandardError.ReadToEnd(); // Here are the exceptions from our Python script
    //                    string result = reader.ReadToEnd(); // Here is the result of StdOut(for example: print "test")

    //                    return new CommandCallResult(process.ExitCode, result); //TODO add the result if any
    //                }
    //            }
    //        }

    //    }

    //    public class ExecutionException : Exception
    //    {
    //        public ExecutionException() : base("The command was not sucessful in executing the command")
    //        {

    //        }
    //    }
    //    private class CommandCallResult
    //    {
    //        private static CommandCallResult dummy = new CommandCallResult(1);
    //        public static CommandCallResult ExecutionError { get { return dummy; } }


    //        public int ExitCode { get; set; }
    //        public string ReturnValue { get; set; }

    //        public CommandCallResult(int ExitCode)
    //        {
    //            this.ExitCode = ExitCode;
    //        }
    //        public CommandCallResult(int ExitCode, string ReturnValue) : this(ExitCode)
    //        {
    //            this.ReturnValue = ReturnValue;
    //        }


    //    }
    //}



}
