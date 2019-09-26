using System;
using System.Drawing;
using System.Text;

namespace window_utility
{
    public class WindowUtils
    {
        public static IntPtr GetWindowByWindowTitle(string windowTitle)
        {
            var window = ExternDLLUtilities.FindWindow(null, windowTitle);
            if (window != IntPtr.Zero)
            {
                return window;
            }
            throw new ArgumentException("No window for " + windowTitle + " exists.");
        }
        public static IntPtr GetWindowByClass(string className) //e.g. notepad
        {
            var window = ExternDLLUtilities.FindWindow(className, null);
            if (window != IntPtr.Zero)
            {
                return window;
            }
            throw new ArgumentException("No window for " + className + " exists.");
        }

        public static WindowPosition GetWindowPosition(IntPtr window)
        {
            var rect = new Rect();
            ExternDLLUtilities.GetWindowRect(window, ref rect);
            return new WindowPosition(rect, GetScalingFactor());
        }

        public static void PutWindowOnTop(IntPtr window)
        {
            // Set window as topmost
            ExternDLLUtilities.SetWindowPos(window, new IntPtr(-1), 0, 0, 0, 0, (uint)(PositioningFlags.SWP_NOSIZE | PositioningFlags.SWP_DRAWFRAME | PositioningFlags.SWP_SHOWWINDOW));
            // Reset window as non-topmost so it stays on top but does not stay on top when non activate
            ExternDLLUtilities.SetWindowPos(window, new IntPtr(-2), 0, 0, 0, 0, (uint)(PositioningFlags.SWP_NOSIZE | PositioningFlags.SWP_DRAWFRAME | PositioningFlags.SWP_SHOWWINDOW));
        }

        /// <summary>
        /// returns width as item 1 and height as item 2
        /// </summary>
        /// <returns></returns>
        public static Tuple<int,int> GetApplicationSize(IntPtr window)
        {
            int screenHeight = ExternDLLUtilities.GetDeviceCaps(window, (int)DeviceCap.VERTRES);
            int screenWidth  = ExternDLLUtilities.GetDeviceCaps(window, (int)DeviceCap.HORZRES);

            return new Tuple<int, int>(screenWidth, screenHeight);
        }

        public static string GetClassName(IntPtr window)
        {
            // Pre-allocate 256 characters, since this is the maximum class name length.
            var className = new StringBuilder(256);
            //Get the window class name
            var nRet = ExternDLLUtilities.GetClassName(window, className, className.Capacity);
            return nRet != 0 ? className.ToString() : "No Class Name";
        }

        private static float GetScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            var logicalScreenHeight = ExternDLLUtilities.GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            var physicalScreenHeight = ExternDLLUtilities.GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            return physicalScreenHeight / (float)logicalScreenHeight;
        }


    }
}
