using System;
using System.Collections.Generic;
using System.Text;
using client_slave_message_communication.custom_requests;
using slave_control_api.ConnectionWrapper;

namespace slave_control_api.controlers
{

    public class KeyboardControlApi
    {
        private PythonWrapper pythonWrapper;

        public KeyboardControlApi(PythonWrapper pythonWrapper)
        {
            this.pythonWrapper = pythonWrapper;
        }

        public void ExecuteKeyboardAction(DoKeyboardAction keyboardAction)
        {
            ExecuteCommand(keyboardAction.Key, keyboardAction.IsKeyDownAction);
        }


        protected void ExecuteCommand(string key, bool isDownAction)
        {

            string command = "key!@#"+key + "#@!" + "isDownAction!@#" + isDownAction.ToString().ToLower();
            
            pythonWrapper.executeApiCommand(command);
        }

    }
}
