using client_slave_message_communication.model.mouse_action;
using System;
using System.Collections.Generic;
using System.Text;

namespace slave_controller
{
    public class MouseActionHandler
    {
        private slave_control_api.controlers.MouseControlApi mouseControlApi;

        public MouseActionHandler(slave_control_api.controlers.MouseControlApi mouseControlApi)
        {
            this.mouseControlApi = mouseControlApi;
        }


        public void HandleMouseAction(BaseMouseAction mouseAction)
        {
            var Type = mouseAction.GetType();
            if(BaseMouseAction.MouseAction.ClickLeft == mouseAction.Action
                && mouseAction is ClickLeftAction _clickLeft)
            {
                this.mouseControlApi.ClickLeft(_clickLeft.arg1ScreenLocation);
            }
            else if(BaseMouseAction.MouseAction.MouseMove == mouseAction.Action
                && mouseAction is MouseMoveAction _mouseMove)
            {
                this.mouseControlApi.MoveMouse(_mouseMove.arg1RelativeScreenLocation);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
