using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace screen_capture_api
{
    public class ScreenCapture
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        const uint SWP_NOSIZE = 0x0001;
        const uint SWP_NOZORDER = 0x0004;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public void GetWindowPosition()
        {
            /*
            var prs = Process.GetProcesses();
            foreach (var pr in prs)
            {
                Console.WriteLine(pr.ProcessName);
            }
            */
            Process[] processes = Process.GetProcessesByName("notepad++");
            Console.WriteLine("Processes found: " + processes.Length);
            Process lol = processes[0];
            Console.WriteLine("Handles opened: " + lol.HandleCount);
            IntPtr ptr = lol.MainWindowHandle;
            Rect NotepadRect = new Rect();
            GetWindowRect(ptr, ref NotepadRect);
            Console.WriteLine(NotepadRect.Bottom + " " + NotepadRect.Left + " " + NotepadRect.Top + " " +
                              NotepadRect.Right);
        }

        public void PositionWindow()
        {
            // Find (the first-in-Z-order) Notepad window.
            IntPtr hWnd = FindWindow("Notepad++", null);
            Rect NotepadRect = new Rect();
            GetWindowRect(hWnd, ref NotepadRect);
            Console.WriteLine(NotepadRect.Bottom + " " + NotepadRect.Left + " " + NotepadRect.Top + " " +
                              NotepadRect.Right);

            // If found, position it.
            if (hWnd != IntPtr.Zero)
            {
                // Move the window to (0,0) without changing its size or position
                // in the Z order.
                SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOZORDER);
            }
            else
            {
                Console.WriteLine("Not found");
            }

            NotepadRect = new Rect();
            GetWindowRect(hWnd, ref NotepadRect);
            Console.WriteLine(NotepadRect.Bottom + " " + NotepadRect.Left + " " + NotepadRect.Top + " " +
                              NotepadRect.Right);

        }

        public void LaunchCommandLineApp()
        {
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = Directory.GetCurrentDirectory() + @"\ffmpeg\ffmpeg.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments =
                    "-y -f gdigrab " +
                    "-framerate 24 " +
                    "-offset_x 0 " +
                    "-offset_y 0 " +
                    "-video_size 1920x1080 " +
                    "-i desktop " +
                    "-frames:v 24 " +
                    "-q 0 " +
                    "\"captured\\img%2d.jpg\""
            };
            
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
