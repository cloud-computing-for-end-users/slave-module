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
            //TODO
            throw new NotImplementedException();
        }



    }
}
