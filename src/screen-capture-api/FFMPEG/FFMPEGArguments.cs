using System;
using screen_capture_api.WindowUtilities;

namespace screen_capture_api.FFMPEG
{
    public class FFMPEGArguments
    {
        private int DefaultFramerate = 24;
        
        internal string GetArguments(OS os, WindowPosition position, string imgPath)
        {
            string input;
            switch (os)
            {
                case OS.Windows:
                    input = "gdigrab";
                    break;
                case OS.Ubuntu:
                    throw new NotImplementedException();
                default:
                    input = "";
                    break;
            }

            return "-y " + // Overwrite output files without asking
                   "-f " + // Force input or output file format.
                   input + " " +
                   "-framerate " + DefaultFramerate + " " +
                   "-offset_x " + position.Left + " " +
                   "-offset_y " + position.Top + " " +
                   "-video_size " + position.Width + "x" + position.Height + " " +
                   "-i desktop " +
                   "-frames:v 1 " +
                   "-q 0 " + // Quality, 0 = best quality, 52? = worst quality
                   "\"" + imgPath + "\"";
        }
    }
}