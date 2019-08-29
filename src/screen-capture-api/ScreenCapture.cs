using System.IO;
using screen_capture_api.FFMPEG;
using screen_capture_api.Model;
using screen_capture_api.WindowUtilities;

namespace screen_capture_api
{
    public class ScreenCapture
    {
        const string imgPath = @"img.png";

        public PathToScreen CaptureScreen(string appName)
        {
            var windowUtils = new WindowUtils();
            var window = windowUtils.GetWindow(appName);
            windowUtils.PositionWindow(window);
            // TODO OS.Windows only now
            new FFMPEGRunner().RunFFMPEG(OS.Windows, windowUtils.GetWindowPosition(window), imgPath);
            return new PathToScreen(Directory.GetCurrentDirectory() + "\\" + imgPath);
        }
    }
}
