using System;
using System.Diagnostics;
using System.IO;
using screen_capture_api.WindowUtilities;

namespace screen_capture_api.FFMPEG
{
    public class FFMPEGRunner
    {
        public void RunFFMPEG(OS os, WindowPosition windowPosition, string imgPath)
        {
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = false,
                UseShellExecute = false,
                FileName = Directory.GetCurrentDirectory() + @"\ffmpeg\ffmpeg.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = new FFMPEGArguments().GetArguments(os, windowPosition, imgPath)
            };

            #if DEBUG
            Console.WriteLine(startInfo.Arguments);
            #endif
            
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