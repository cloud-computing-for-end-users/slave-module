using System;
using System.Drawing;
using System.Runtime.InteropServices;
using static screen_capture_api.WindowUtilities.ExternDLLUtilities;
using static screen_capture_api.WindowUtilities.PositioningFlags;

namespace screen_capture_api.WindowUtilities
{
    public class WindowUtils
    {
        public static IntPtr GetWindow(string windowName)
        {
            /* One way of doing it:
            Process[] processes = Process.GetProcessesByName(windowName);
            Process p = processes[0];
            return p.MainWindowHandle;
            */

            var window = FindWindow(windowName, null);
            if (window != IntPtr.Zero)
            {
                return window;
            }
            throw new ArgumentException("No window for " + windowName + " exists.");
        }

        public static WindowPosition GetWindowPosition(IntPtr window)
        {
            var rect = new Rect();
            GetWindowRect(window, ref rect);
            return new WindowPosition(rect, GetScalingFactor());
        }

        public static void PositionWindow(IntPtr window)
        {
            // Set window as topmost
            SetWindowPos(window, new IntPtr(-1), 0, 0, 0, 0, (uint) (SWP_NOSIZE | SWP_DRAWFRAME | SWP_SHOWWINDOW));
            // Reset window as non-topmost so it stays on top but does not stay on top when non activate
            SetWindowPos(window, new IntPtr(-2), 0, 0, 0, 0, (uint) (SWP_NOSIZE | SWP_DRAWFRAME | SWP_SHOWWINDOW));
        }

        private static float GetScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            return (float)PhysicalScreenHeight / (float)LogicalScreenHeight;
        }
    }
}