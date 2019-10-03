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

        }

        private PythonAutoGUIWrapper pyAutoGui;

        public MouseControlApi(PythonAutoGUIWrapper pyAutoGui)
        {
            this.pyAutoGui = pyAutoGui;

        }

        private static Dictionary<ApiComman, string> ApiCommandStringParam = new Dictionary<ApiComman, string>();

        public int MoveMouse(RelativeScreenLocation screenLocation, IntPtr windowHandle)
        {
            // todo fix
            var winPosition = WindowUtils.GetWindowPosition(windowHandle);
            var winPosition2 = WindowUtils.GetApplicationSize2(windowHandle);

            var arg1 = Convert.ToInt32(winPosition.Left + screenLocation.FromLeft.ThePercentage / 100 * winPosition.Width);
            var arg2 = Convert.ToInt32(winPosition.Top + screenLocation.FromTop.ThePercentage / 100 * winPosition.Height);

            return RunCommand(ApiComman.MoveMouse, arg1.ToString(), arg2.ToString());
        }

        public int ClickLeft(RelativeScreenLocation screenLocation)
        {
            var arg1 = screenLocation.FromLeft.ThePercentage.ToString();
            var arg2 = screenLocation.FromTop.ThePercentage.ToString();

            return RunCommand(ApiComman.ClickLeft, arg1,arg2);
        }

        public int LeftDown(RelativeScreenLocation screenLocation)
        {
            var arg1 = screenLocation.FromLeft.ThePercentage.ToString();
            var arg2 = screenLocation.FromTop.ThePercentage.ToString();

            return RunCommand(ApiComman.LeftMouseDown, arg1,arg2);
        }

        public int LeftUp(RelativeScreenLocation screenLocation)
        {
            var arg1 = screenLocation.FromLeft.ThePercentage.ToString();
            var arg2 = screenLocation.FromTop.ThePercentage.ToString();

            return RunCommand(ApiComman.LeftMouseUp/*, arg1,arg2*/);
        }

        public int RunCommand(MouseControlApi.ApiComman command, params string [] args)
        {
            var result = this.pyAutoGui?.executeApiCommand(apiCommandToActualCommand[command], new List<string>(args));
            return -1; //TODO consider fixing
        }

    }
}
