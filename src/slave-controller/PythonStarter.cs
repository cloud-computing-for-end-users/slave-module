using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using window_utility;

namespace slave_controller
{
    /// <summary>
    /// starting the python scripts from here will muffel their standard output
    /// </summary>
    public class PythonStarter
    {
        private const string PATH_TO_PYTHON_EXE = "python";

        private static readonly string PATH_TO_PYTHON_MOUSE_CONTROL_API = AppContext.BaseDirectory + @"Resources\PyAutoGuiMouseController.py";
        private const string ARGS_FOR_PYTHON_MOUSE_CONTROL_API = "";

        private const string PATH_TO_PYTHON_KEYBOARD_CONTROL_API = @"Resources\PyAutoGuiKeyboardController.py";
        private const string ARGS_FOR_PYTHON_KEYBOARD_CONTROL_API = "";

        private static readonly string PATH_TO_PYTHON_SCREEN_CAPTURE = AppContext.BaseDirectory + @"Resources\ScreenCapturing.py";
        //private const string ARGS_FOR_PYTHON_SCREEN_CAPTURE = ""; // gets these at runtime

        private static List<Process> _startedProcesses = new List<Process>();
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

                    var process = Process.Start(start);
                    _startedProcesses.Add(process);

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
            var t = new Thread(
                () =>
                {
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = PATH_TO_PYTHON_EXE;
                    start.Arguments = string.Format("{0} {1}", PATH_TO_PYTHON_KEYBOARD_CONTROL_API, ARGS_FOR_PYTHON_MOUSE_CONTROL_API);
                    start.UseShellExecute = false;
                    start.RedirectStandardOutput = false;

                    var process = Process.Start(start);
                    _startedProcesses.Add(process);

                });
            t.IsBackground = true;
            t.Start();
        }

        public static void StartPythonScreenCapture(IntPtr windowHandle)
        {
            var applicationPosition = WindowUtils.GetWindowPosition(windowHandle);

            string ARGS_FOR_PYTHON_SCREEN_CAPTURE = applicationPosition.Left + " "
                                                        + applicationPosition.Top + " "
                                                        + applicationPosition.Width + " "
                                                        + applicationPosition.Height
                ;

            var t = new Thread(
                () =>
                {
                    ProcessStartInfo start = new ProcessStartInfo();
                    start.FileName = PATH_TO_PYTHON_EXE;
                    start.Arguments = string.Format("{0} {1}", PATH_TO_PYTHON_SCREEN_CAPTURE, ARGS_FOR_PYTHON_SCREEN_CAPTURE);
                    start.UseShellExecute = false;
                    start.RedirectStandardOutput = false;

                    var process = Process.Start(start);
                    _startedProcesses.Add(process);

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

        public static void KillAllStartedProcesses()
        {
            lock (_startedProcesses)
            {
                while(0 != _startedProcesses.Count)
                {
                    var process = _startedProcesses[0];
                    _startedProcesses.Remove(process);
                    //process.Close();
                    process.Kill();
                }
            }
        }
    }
}
