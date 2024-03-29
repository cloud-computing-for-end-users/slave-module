﻿using System.Runtime.InteropServices;
using System.Drawing;
using System.Text.RegularExpressions;
using System;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace Getting_window_location_and_size
{
    class ApplicationDimensionFinder
    {
        //    private static bool PRINT = false;

        //    // Win32 constants.
        //    const int WM_GETTEXT = 0x000D;
        //    const int WM_GETTEXTLENGTH = 0x000E;

        //    // Win32 functions that have all been used in previous blogs.
        //    [DllImport("User32.Dll")]
        //    private static extern void GetClassName(int hWnd, StringBuilder s, int nMaxCount);

        //    [DllImport("User32.dll")]
        //    private static extern Int32 SendMessage(int hWnd, int Msg, int wParam, StringBuilder lParam);

        //    [DllImport("User32.dll")]
        //    private static extern Int32 SendMessage(int hWnd, int Msg, int wParam, int lParam);

        //    // The GetWindowRect function takes a handle to the window as the first parameter. The second parameter
        //    // must include a reference to a Rectangle object. This Rectangle object will then have it's values set
        //    // to the window rectangle properties.
        //    [DllImport("user32.dll")]
        //    public static extern long GetWindowRect(int hWnd, ref Rectangle lpRect);





        //    private static Tuple<int, int> _widthHeight = null;
        //    /// <summary>
        //    /// returns application width as item 1, returns application height as item 2
        //    /// This method uses polling to make the call synchronous
        //    /// </summary>
        //    /// <returns></returns>
        //    public static Tuple<int, int> GetApplicationWidthAndHeight(string textInWindowTitle)
        //    {
        //        _widthHeight = null;
        //        WindowFinder wf = new WindowFinder();
        //        wf.FindWindows(0, null, new Regex(textInWindowTitle), null, FoundWindow);

        //        int waitTimePerCycle = 100;
        //        int millSecondInFiveSeconds = 5 * 1000;
        //        for(int i = 0; i < millSecondInFiveSeconds/waitTimePerCycle; i++)
        //        {
        //            if (null == _widthHeight)
        //            {
        //                Thread.Sleep(100);
        //            }
        //            else
        //            {
        //                return _widthHeight;
        //            }
        //        }
        //        throw new TimeoutException("Timed out in GetApplicationWidthAndHeight");
        //    }

        //    private static IntPtr _windowHandle = IntPtr.Zero;
        //    /// <summary>
        //    /// this application uses polling to serialise the call to this method
        //    /// </summary>
        //    /// <param name="windowTitleText"></param>
        //    /// <param name="className"></param>
        //    /// <returns></returns>
        //    public static IntPtr FindWindow(Regex windowTitleText = null, Regex className = null, Regex process = null)
        //    {
        //        _windowHandle = IntPtr.Zero;
        //        var finder = new WindowFinder();
        //        finder.FindWindows(0,className,windowTitleText,process,FoundWindow);

        //        int waitTimePerCycle = 100;
        //        int millSecondInFiveSeconds = 5 * 1000;
        //        //varriable polling
        //        for (int i = 0; i < millSecondInFiveSeconds / waitTimePerCycle; i++)
        //        {
        //            if (IntPtr.Zero == _windowHandle)
        //            {
        //                Thread.Sleep(100);
        //            }
        //            else
        //            {
        //                var result = _windowHandle;
        //                _windowHandle = IntPtr.Zero;
        //                return result;
        //            }
        //        }
        //        throw new Exception("Window not found exception");
        //    }


        static void Main(string[] args)
    {
        // Introduced in the "Finding specific windows" blog, we use the WindowFinder class to find all Internet Explorer main window instances.
        //WindowFinder wf = new WindowFinder();

        // Find all visual studio instances
        //var appDimensions = GetApplicationWidthAndHeight("Paint");
        //Console.WriteLine("Width: " + appDimensions.Item1 + ", Height: " + appDimensions.Item2);

        Console.ReadKey();
    }

        //    static bool FoundWindow(int handle)
        //    {
        //        // First we intialize an empty Rectangle object.
        //        Rectangle rect = new Rectangle();

        //        // Then we call the GetWindowRect function, passing in a reference to the rect object.
        //        GetWindowRect(handle, ref rect);

        //        // And then we get the resulting rectangle. The tricky part here is that this rectangle includes
        //        // not only the location of the window, but also the size, but not in the form we're used to.
        //        Console.WriteLine(rect.ToString());

        //        // If the window is 100 x 100 pixels and is located at (10,10), then the rectangle would look like this:
        //        // rect.X = 10;
        //        // rect.Y = 10;
        //        // rect.Width = 110;
        //        // rect.Height = 110;
        //        // We simply have to subtract the rect.X value from the rect.Width value to obtain the "real" width of
        //        // the window, similarly we have to subtract the Y value from the Height value to obtain the real height.
        //        // After this we have the real window properties through the X, Y, Width and Height values.
        //        rect.Width = rect.Width - rect.X;
        //        rect.Height = rect.Height - rect.Y;
        //        _widthHeight = new Tuple<int, int>(rect.Width, rect.Height);
        //        // Lets print the rectangle after we've fixed it so we can confirm it's correct.
        //        Console.WriteLine(rect.ToString());

        //        // As used earlier, we print the basic properties of the window.
        //        PrintWindowInfo(handle);

        //        return true;
        //    }

        //    //Prints basic properties of a window, uses function already used in previous blogs.
        //    private static void PrintWindowInfo(int handle)
        //    {
        //        if (false == PRINT)
        //        {
        //            return;
        //        }

        //        // Get the class.
        //        StringBuilder sbClass = new StringBuilder(256);
        //        GetClassName(handle, sbClass, sbClass.Capacity);

        //        // Get the text.
        //        int txtLength = SendMessage(handle, WM_GETTEXTLENGTH, 0, 0);
        //        StringBuilder sbText = new StringBuilder(txtLength + 1);
        //        SendMessage(handle, WM_GETTEXT, sbText.Capacity, sbText);

        //        // Now we can write out the information we have on the window.
        //        Console.WriteLine("Handle: " + handle);
        //        Console.WriteLine("Class : " + sbClass);
        //        Console.WriteLine("Text  : " + sbText);
        //        Console.WriteLine();
        //    }
        //}

        ///// <summary>
        ///// A class used for finding windows based upon their class, title, process and parent window handle.
        ///// </summary>
        //internal class WindowFinder
        //{
        //    // Win32 constants.
        //    const int WM_GETTEXT = 0x000D;
        //    const int WM_GETTEXTLENGTH = 0x000E;

        //    // Win32 functions that have all been used in previous blogs.
        //    [DllImport("User32.Dll")]
        //    private static extern void GetClassName(int hWnd, StringBuilder s, int nMaxCount);

        //    [DllImport("User32.dll")]
        //    private static extern int GetWindowText(int hWnd, StringBuilder text, int count);

        //    [DllImport("User32.dll")]
        //    private static extern Int32 SendMessage(int hWnd, int Msg, int wParam, StringBuilder lParam);

        //    [DllImport("User32.dll")]
        //    private static extern Int32 SendMessage(int hWnd, int Msg, int wParam, int lParam);

        //    [DllImport("user32")]
        //    private static extern int GetWindowThreadProcessId(int hWnd, out int lpdwProcessId);

        //    // EnumChildWindows works just like EnumWindows, except we can provide a parameter that specifies the parent
        //    // window handle. If this is NULL or zero, it works just like EnumWindows. Otherwise it'll only return windows
        //    // whose parent window handle matches the hWndParent parameter.
        //    [DllImport("user32.Dll")]
        //    private static extern Boolean EnumChildWindows(int hWndParent, PChildCallBack lpEnumFunc, int lParam);

        //    // The PChildCallBack delegate that we used with EnumWindows.
        //    private delegate bool PChildCallBack(int hWnd, int lParam);

        //    // This is an event that is run each time a window was found that matches the search criterias. The boolean
        //    // return value of the delegate matches the functionality of the PChildCallBack delegate function.
        //    private event FoundWindowCallback foundWindow;
        //    public delegate bool FoundWindowCallback(int hWnd);

        //    // Members that'll hold the search criterias while searching.
        //    private int parentHandle;
        //    private Regex className;
        //    private Regex windowText;
        //    private Regex process;

        //    // The main search function of the WindowFinder class. The parentHandle parameter is optional, taking in a zero if omitted.
        //    // The className can be null as well, in this case the class name will not be searched. For the window text we can input
        //    // a Regex object that will be matched to the window text, unless it's null. The process parameter can be null as well,
        //    // otherwise it'll match on the process name (Internet Explorer = "iexplore"). Finally we take the FoundWindowCallback
        //    // function that'll be called each time a suitable window has been found.
        //    public void FindWindows(int parentHandle, Regex className, Regex windowText, Regex process, FoundWindowCallback fwc)
        //    {
        //        this.parentHandle = parentHandle;
        //        this.className = className;
        //        this.windowText = windowText;
        //        this.process = process;

        //        // Add the FounWindowCallback to the foundWindow event.
        //        foundWindow = fwc;

        //        // Invoke the EnumChildWindows function.
        //        EnumChildWindows(parentHandle, new PChildCallBack(enumChildWindowsCallback), 0);
        //    }

        //    // This function gets called each time a window is found by the EnumChildWindows function. The foun windows here
        //    // are NOT the final found windows as the only filtering done by EnumChildWindows is on the parent window handle.
        //    private bool enumChildWindowsCallback(int handle, int lParam)
        //    {
        //        // If a class name was provided, check to see if it matches the window.
        //        if (className != null)
        //        {
        //            StringBuilder sbClass = new StringBuilder(256);
        //            GetClassName(handle, sbClass, sbClass.Capacity);

        //            // If it does not match, return true so we can continue on with the next window.
        //            if (!className.IsMatch(sbClass.ToString()))
        //                return true;
        //        }

        //        // If a window text was provided, check to see if it matches the window.
        //        if (windowText != null)
        //        {
        //            int txtLength = SendMessage(handle, WM_GETTEXTLENGTH, 0, 0);
        //            StringBuilder sbText = new StringBuilder(txtLength + 1);
        //            SendMessage(handle, WM_GETTEXT, sbText.Capacity, sbText);

        //            // If it does not match, return true so we can continue on with the next window.
        //            if (!windowText.IsMatch(sbText.ToString()))
        //                return true;
        //        }

        //        // If a process name was provided, check to see if it matches the window.
        //        if (process != null)
        //        {
        //            int processID;
        //            GetWindowThreadProcessId(handle, out processID);

        //            // Now that we have the process ID, we can use the built in .NET function to obtain a process object.
        //            Process p = Process.GetProcessById(processID);

        //            // If it does not match, return true so we can continue on with the next window.
        //            if (!process.IsMatch(p.ProcessName))
        //                return true;
        //        }

        //        // If we get to this point, the window is a match. Now invoke the foundWindow event and based upon
        //        // the return value, whether we should continue to search for windows.
        //        return foundWindow(handle);
        //    }
    }
}