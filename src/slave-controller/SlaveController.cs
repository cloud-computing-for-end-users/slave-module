﻿using client_slave_message_communication.custom_requests;
using client_slave_message_communication.interfaces;
using client_slave_message_communication.model.keyboard_action;
using client_slave_message_communication.model.mouse_action;
using custom_message_based_implementation.model;
using message_based_communication.model;
using message_based_communication.module;
using slave_control_api.ConnectionWrapper;
using System;
using System.Threading;
using window_utility;

namespace slave_controller
{
    public class SlaveController : BaseRouterModule, ISlave
    {
        public override string CALL_ID_PREFIX => "SLAVE_CALL_ID_";
        protected override string MODULE_ID_PREFIXES => "SLAVE_GIVEN_MODULE_ID_";

        //protected slave_control_api.controlers.MouseControlApi mouseController;
        //protected slave_control_api.controlers.KeyboardController keyboardController;
        protected MouseActionHandler mouseActionHandler;


        private static readonly string APP_NAME = "Unavngivet - Paint";
        //private static readonly string APP_NAME = "Paint";
        private IntPtr appWindow;


        public SlaveController(Port forMouseControlApi, Port portForRegistrationToRouter, ModuleType moduleType, message_based_communication.encoding.Encoding customEncoding) : base(portForRegistrationToRouter, moduleType, customEncoding)
        {

            PythonStarter.StartPythonMouseControlApi();
            Thread.Sleep(10); // the mouse control api must be running before the mouseActionHandler can be instanciated

            var pyAutoGui = new PythonAutoGUIWrapper(forMouseControlApi);//TODO FIX, not sure what I mean here anymore

            this.mouseActionHandler = new MouseActionHandler(new slave_control_api.controlers.MouseControlApi(pyAutoGui));
            //this.keyboardController = new slave_control_api.controlers.KeyboardController(pyAutoGuiWrapper);

            //create instances of image capture
            //mouse and keyboard controller
            //other nessesary helper objects

            //do the setup with the application before capturing images
            appWindow = WindowUtils.GetWindowByWindowTitle(APP_NAME);
            WindowUtils.PutWindowOnTop(appWindow);


            PythonStarter.StartPythonScreenCapture();

        }





        public void DoKeyboardAction(BaseKeyboardAction action)
        {
            throw new NotImplementedException();
        }

        public void DoMouseAction(BaseMouseAction action)
        {
            mouseActionHandler.HandleMouseAction(action,appWindow);
        }

        public Port GetImageProducerConnInfo()
        {
            return new Port() { ThePort = 30303 };
            throw new NotImplementedException();
        }

        public override void HandleRequest(BaseRequest message)
        {
            object payload = null;
            if (message is Handshake _handshake)
            {
                payload = Handshake(_handshake.arg1PrimaryKey);
            }
            else if (message.GetType().IsGenericType)
            {
                var type = message.GetType().GetGenericTypeDefinition();
                var gType = typeof(DoMouseAction<BaseMouseAction>).GetGenericTypeDefinition();
                if (type.Equals(typeof(DoMouseAction<BaseMouseAction>).GetGenericTypeDefinition()))
                {
                    //TODO see if there is a non dynamic way to do this
                    dynamic mouseAction = message;
                    DoMouseAction(mouseAction.arg1MouseAction);
                    payload = null;
                }
            }
            else if (message is GetImageProducerConnectionInfo _getImageProducer)
            {
                payload = GetImageProducerConnInfo();
            }
            else
            {
                throw new NotImplementedException();
            }

            var response = GenerateResponseBasedOnRequestAndPayload(message, payload);
            SendResponse(response);
        }


        public Tuple<int,int> Handshake(PrimaryKey pk)
        {
            return WindowUtils.GetApplicationSize(appWindow);
        }
    }
}
