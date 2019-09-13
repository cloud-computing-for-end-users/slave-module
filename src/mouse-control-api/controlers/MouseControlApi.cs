using client_slave_message_communication.model;
using mouse_control_api.ConnectionWrapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace slave_control_api.controlers
{
    public class MouseControlApi
    {
        public enum ApiComman
        {
            MoveMouse,
            ClickLeft,
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

        }



        private PythonAutoGUIWrapper pyAutoGui;

        public MouseControlApi(PythonAutoGUIWrapper pyAutoGui)
        {
            this.pyAutoGui = pyAutoGui;
        }


        //private static Dictionary<ApiComman, string> ApiCommandStringParam = new Dictionary<ApiComman, string>();



        public int MoveMouse(RelativeScreenLocation screenLocation)
        {

            var arg1 = screenLocation.FromLeft.ThePercentage.ToString();
            var arg2 = screenLocation.FromTop.ThePercentage.ToString();

            return RunCommand(ApiComman.MoveMouse, arg1, arg2);
        }

        public int ClickLeft(RelativeScreenLocation screenLocation)
        {
            var arg1 = screenLocation.FromLeft.ThePercentage.ToString();
            var arg2 = screenLocation.FromTop.ThePercentage.ToString();

            return RunCommand(ApiComman.ClickLeft, arg1,arg2);
        }

        private int RunCommand(MouseControlApi.ApiComman command, params string [] args)
        {
            var result = this.pyAutoGui?.executeApiCommand(apiCommandToActualCommand[command], new List<string>(args));
            return -1; //TODO consider fixing
        }

    }
}
