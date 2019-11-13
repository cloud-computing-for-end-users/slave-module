using client_slave_message_communication.model.mouse_action;
using slave_control_api.controlers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using message_based_communication.model;
using slave_control_api.ConnectionWrapper;
using window_utility;

namespace slave_controller
{
    public class MouseActionHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private MouseControlApi mouseControlApi;
        private IntPtr windowHandle;

        private Queue<BaseMouseAction> mouseActionQueue = new Queue<BaseMouseAction>();
        public void QueueMouseCommand(BaseMouseAction mouseAction)
        {
            mouseActionQueue.Enqueue(mouseAction);
        }


        public MouseActionHandler(PythonWrapper pythonWrapper, IntPtr windowHandle)
        {
            this.mouseControlApi = new MouseControlApi(pythonWrapper);
            this.windowHandle = windowHandle;

            var t = new Thread(() =>
            {

                while (true)
                {
                    if (0 == mouseActionQueue.Count)
                    {
                        Thread.Sleep(10);
                    }
                    else
                    {
                        try
                        {
                            HandleMouseAction(mouseActionQueue.Dequeue(), this.windowHandle);
                        }
                        catch (NullReferenceException e)
                        {
                            Logger.Debug(e);
                        }
                    }
                }
            });
            t.IsBackground = true;
            t.Start();
        }


        protected void HandleMouseAction(BaseMouseAction mouseAction, IntPtr windowHandle)
        {
            var Type = mouseAction.GetType();
            if (BaseMouseAction.MouseAction.LeftDown.ToString().Equals(mouseAction.Action)
                    && mouseAction is LeftMouseDownAction _leftDown)
            {
                this.mouseControlApi.LeftDown(_leftDown.RelativeScreenLocation, windowHandle);
            }

            else if (BaseMouseAction.MouseAction.LeftUp.ToString().Equals(mouseAction.Action)
                && mouseAction is LeftMouseUpAction _leftUp)
            {
                this.mouseControlApi.LeftUp(_leftUp.RelativeScreenLocation, windowHandle);
            }
            else if (BaseMouseAction.MouseAction.RightUp.ToString().Equals(mouseAction.Action)
                     && mouseAction is RightMouseUpAction _rightUp)
            {
                this.mouseControlApi.RightUp(_rightUp.RelativeScreenLocation, windowHandle);
            }
            else if (BaseMouseAction.MouseAction.RightDown.ToString().Equals(mouseAction.Action)
                     && mouseAction is RightMouseDownAction _rightDown)
            {
                this.mouseControlApi.RightDown(_rightDown.RelativeScreenLocation, windowHandle);
            }
            else if (BaseMouseAction.MouseAction.MouseMove.ToString().Equals(mouseAction.Action)
                     && mouseAction is MouseMoveAction _mouseMove)
            {
                this.mouseControlApi.MoveMouse(_mouseMove.RelativeScreenLocation, windowHandle);
            }
            else
            {
                throw new NotImplementedException("Not implemented in the ");
            }
        }
    }
}
