using client_slave_message_communication.model.mouse_action;
using slave_control_api.controlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace slave_controller
{
    public class MouseActionHandler
    {
        private MouseControlApi mouseControlApi;

        public MouseActionHandler(slave_control_api.controlers.MouseControlApi mouseControlApi)
        {
            this.mouseControlApi = mouseControlApi;
        }


        public void HandleMouseAction(BaseMouseAction mouseAction, IntPtr windowHandle)
        {
            var Type = mouseAction.GetType();
            if(BaseMouseAction.MouseAction.ClickLeft == mouseAction.Action
                && mouseAction is ClickLeftAction _clickLeft)
            {
                this.mouseControlApi.ClickLeft();
            }
            else if(BaseMouseAction.MouseAction.MouseMove == mouseAction.Action
                && mouseAction is MouseMoveAction _mouseMove)
            {
                this.mouseControlApi.MoveMouse(_mouseMove.arg1RelativeScreenLocation, windowHandle);
            }
            else if(BaseMouseAction.MouseAction.LeftDown == mouseAction.Action
                && mouseAction is LeftMouseDownAction _leftDown)
            {
                this.mouseControlApi.LeftDown();
            }
            else if (BaseMouseAction.MouseAction.LeftUp == mouseAction.Action
                && mouseAction is LeftMouseUpAction _leftUp)
            {
                this.mouseControlApi.LeftUp();
            }
            else
            {
                throw new NotImplementedException("Not implemented in the ");
            }
        }
    }
}
