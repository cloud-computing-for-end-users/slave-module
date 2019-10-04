using client_slave_message_communication.model;
using slave_control_api.ConnectionWrapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using client_slave_message_communication.model.mouse_action;
using window_utility;

namespace slave_control_api.controlers
{
    public class MouseControlApi
    {
        public enum ApiComman
        {
            MoveMouse,
            ClickLeft,
            LeftMouseDown,
            LeftMouseUp,
            RightMouseDown,
            RightMouseUp,
            ClickRight,
            ClickDouble,
            ScrollDown,
            ScrollUp,
            LocationOfMouse
        }

        protected static Dictionary<ApiComman, string> apiCommandToActualCommand = new Dictionary<ApiComman, string>();



        static MouseControlApi()
        {
            apiCommandToActualCommand.Add(ApiComman.MoveMouse, "-mo");
            apiCommandToActualCommand.Add(ApiComman.ClickLeft, "-cl");
            apiCommandToActualCommand.Add(ApiComman.LeftMouseDown, "-ld");
            apiCommandToActualCommand.Add(ApiComman.LeftMouseUp, "-lu");
            apiCommandToActualCommand.Add(ApiComman.RightMouseUp, "-ru");
            apiCommandToActualCommand.Add(ApiComman.RightMouseDown, "-rd");

        }

        private PythonAutoGUIWrapper pyAutoGui;

        public MouseControlApi(PythonAutoGUIWrapper pyAutoGui)
        {
            this.pyAutoGui = pyAutoGui;

        }

        private static Dictionary<ApiComman, string> ApiCommandStringParam = new Dictionary<ApiComman, string>();

        public int MoveMouse(RelativeScreenLocation screenLocation, IntPtr windowHandle)
        {
            var winPosition = WindowUtils.GetWindowPosition(windowHandle);
            var arg1 = Convert.ToInt32(winPosition.Left + screenLocation.FromLeft.ThePercentage / 100 * winPosition.Width);
            var arg2 = Convert.ToInt32(winPosition.Top + screenLocation.FromTop.ThePercentage / 100 * winPosition.Height);

            return RunCommand(ApiComman.MoveMouse, arg1.ToString(), arg2.ToString());
        }

        public int ClickLeft(RelativeScreenLocation screenLocation, IntPtr windowHandle)
        {
            var winPosition = WindowUtils.GetWindowPosition(windowHandle);
            var arg1 = Convert.ToInt32(winPosition.Left + screenLocation.FromLeft.ThePercentage / 100 * winPosition.Width);
            var arg2 = Convert.ToInt32(winPosition.Top + screenLocation.FromTop.ThePercentage / 100 * winPosition.Height);

            return RunCommand(ApiComman.ClickLeft, arg1.ToString(),arg2.ToString());
        }

        public int LeftDown(RelativeScreenLocation screenLocation, IntPtr windowHandle)
        {
            var winPosition = WindowUtils.GetWindowPosition(windowHandle);
            var arg1 = Convert.ToInt32(winPosition.Left + screenLocation.FromLeft.ThePercentage / 100 * winPosition.Width);
            var arg2 = Convert.ToInt32(winPosition.Top + screenLocation.FromTop.ThePercentage / 100 * winPosition.Height);

            return RunCommand(ApiComman.LeftMouseDown, arg1.ToString(),arg2.ToString());
        }

        public int LeftUp(RelativeScreenLocation screenLocation, IntPtr windowHandle)
        {
            var winPosition = WindowUtils.GetWindowPosition(windowHandle);
            var arg1 = Convert.ToInt32(winPosition.Left + screenLocation.FromLeft.ThePercentage / 100 * winPosition.Width);
            var arg2 = Convert.ToInt32(winPosition.Top + screenLocation.FromTop.ThePercentage / 100 * winPosition.Height);

            return RunCommand(ApiComman.LeftMouseUp, arg1.ToString(),arg2.ToString());
        }

        public int RightDown(RelativeScreenLocation screenLocation, IntPtr windowHandle)
        {
            var winPosition = WindowUtils.GetWindowPosition(windowHandle);
            var arg1 = Convert.ToInt32(winPosition.Left + screenLocation.FromLeft.ThePercentage / 100 * winPosition.Width);
            var arg2 = Convert.ToInt32(winPosition.Top + screenLocation.FromTop.ThePercentage / 100 * winPosition.Height);

            return RunCommand(ApiComman.RightMouseDown, arg1.ToString(), arg2.ToString());
        }

        public int RightUp(RelativeScreenLocation screenLocation, IntPtr windowHandle)
        {
            var winPosition = WindowUtils.GetWindowPosition(windowHandle);
            var arg1 = Convert.ToInt32(winPosition.Left + screenLocation.FromLeft.ThePercentage / 100 * winPosition.Width);
            var arg2 = Convert.ToInt32(winPosition.Top + screenLocation.FromTop.ThePercentage / 100 * winPosition.Height);

            return RunCommand(ApiComman.RightMouseUp, arg1.ToString(),arg2.ToString());
        }


        public int RunCommand(MouseControlApi.ApiComman command, params string [] args)
        {
            var result = this.pyAutoGui?.executeApiCommand(apiCommandToActualCommand[command], new List<string>(args));
            return -1; //TODO consider fixing
        }

    }
}
