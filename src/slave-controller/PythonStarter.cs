﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace slave_controller
{
    /// <summary>
    /// starting the python scripts from here will muffel their standard output
    /// </summary>
    public class PythonStarter
    {
        private const string PATH_TO_PYTHON_EXE = @"C:\Users\MSI\PycharmProjects\untitled\venv\Scripts\python.exe";

        private const string PATH_TO_PYTHON_MOUSE_CONTROL_API = @"C:\sourcetree\slave-module\src\py-auto-gui-socket-wrapper\PyAutoGuiSocketWrapper.py";
        private const string ARGS_FOR_PYTHON_MOUSE_CONTROL_API = "";


        private const string PATH_TO_PYTHON_KEYBOARD_CONTROL_API = "";
        private const string ARGS_FOR_PYTHON_KEYBOARD_CONTROL_API = "";


        private const string PATH_TO_PYTHON_SCREEN_CAPTURE = @"C:\sourcetree\slave-module\src\pythonScreenCapture\ScreenCapturing.py";
        private const string ARGS_FOR_PYTHON_SCREEN_CAPTURE = "";



        public static void StartPythonMouseControlApi()
        {
            var t = new Thread(
                () =>
                {
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = PATH_TO_PYTHON_EXE;
                    start.Arguments = string.Format("{0} {1}", PATH_TO_PYTHON_MOUSE_CONTROL_API, ARGS_FOR_PYTHON_MOUSE_CONTROL_API);
                    start.UseShellExecute = false;
                    start.RedirectStandardOutput = false;
                    Process.Start(start);

                    //using (Process process = Process.Start(start))
                    //{
                    //    using (StreamReader reader = process.StandardOutput)
                    //    {
                    //        string result = reader.ReadToEnd();
                    //        Console.Write(result);
                    //    }
                    //}
                });
            t.IsBackground = true;
            t.Start();
        }

        public static void StartPythonKeyboardControlApi()
        {
            throw new Exception("Path to python file needs to be specified first");
            var t = new Thread(
                () =>
                {
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = PATH_TO_PYTHON_EXE;
                    start.Arguments = string.Format("{0} {1}", PATH_TO_PYTHON_KEYBOARD_CONTROL_API, ARGS_FOR_PYTHON_KEYBOARD_CONTROL_API);
                    start.UseShellExecute = false;
                    start.RedirectStandardOutput = false;
                    Process.Start(start);

                    //using (Process process = Process.Start(start))
                    //{
                    //    using (StreamReader reader = process.StandardOutput)
                    //    {
                    //        string result = reader.ReadToEnd();
                    //        Console.Write(result);
                    //    }
                    //}
                });
            t.IsBackground = true;
            t.Start();
        }

        public static void StartPythonScreenCapture()
        {
            var t = new Thread(
                () =>
                {
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = PATH_TO_PYTHON_EXE;
                    start.Arguments = string.Format("{0} {1}", PATH_TO_PYTHON_SCREEN_CAPTURE, ARGS_FOR_PYTHON_SCREEN_CAPTURE);
                    start.UseShellExecute = false;
                    start.RedirectStandardOutput = false;
                    Process.Start(start);
                    //using (Process process = Process.Start(start))
                    //{
                    //    using (StreamReader reader = process.StandardOutput)
                    //    {
                    //        string result = reader.ReadToEnd(); // not used
                    //        Console.Write(result); // not used
                    //    }
                    //}
                });
            t.IsBackground = true;
            t.Start();

        }
    }
}