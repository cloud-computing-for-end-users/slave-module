using client_slave_message_communication.model.mouse_action;
using slave_control_api.controlers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using message_based_communication.model;
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


        public MouseActionHandler(slave_control_api.controlers.MouseControlApi mouseControlApi, IntPtr windowHandle)
        {
            this.mouseControlApi = mouseControlApi;
            this.windowHandle = windowHandle;

            //var threadReciver = new Thread(() =>
            //{
            //    ReciveMouseActions(new Port(){ThePort = 6969 /*should not be hardcoded forever*/});
            //});
            //threadReciver.IsBackground = true;
            //threadReciver.Start();



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

        //private void ReciveMouseActions(Port portToListenOn)
        //{
        //    var boundSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        //    boundSocket.Bind(new IPEndPoint(IPAddress.Any, portToListenOn.ThePort));
        //    boundSocket.Listen(1);
        //    var inSocket = boundSocket.Accept();
        //    byte[] receiveBuffer;
        //    int bytesRead;
        //    while (true)
        //    {
        //        //recive int for the recive size
        //        receiveBuffer = new byte[4];
        //        bytesRead = inSocket.Receive(receiveBuffer);
        //        if (4 != bytesRead)
        //        {
        //            throw new Exception("Exception in MouseActionHandler");
        //        }

        //        int bytesToRead = BitConverter.ToInt32(receiveBuffer);
        //        receiveBuffer = new byte[bytesToRead];
        //        bytesRead = inSocket.Receive(receiveBuffer);
        //        if (bytesToRead != bytesRead)
        //        {
        //            throw new Exception("Exception in MouseActionHandler");
        //        }
        //        //convert back to string
        //        var receivedString = Encoding.UTF8.GetString(receiveBuffer);
        //        Logger.Debug("Received string: " + receivedString + " in slave controller MouseActionHandler");

        //        ForwardToMouseControllApi(receivedString);
        //    }

        //}

        //private void ForwardToMouseControllApi(string toSplit)
        //{
        //    var splits = toSplit.Split(" ");

        //    string fullCommand = String.Empty;

        //    if(3 == splits.Length)
        //    {
        //        //adjust left and right screen location from relative to actual, if they are set
        //        var left = Convert.ToDouble(splits[1], new System.Globalization.CultureInfo("en-US"));
        //        var right = Convert.ToDouble(splits[2], new System.Globalization.CultureInfo("en-US"));

        //        var winPosition = WindowUtils.GetWindowPosition(windowHandle);
        //        //var winPosition2 = WindowUtils.GetApplicationSize2(windowHandle);

        //        var arg1 = Convert.ToInt32(winPosition.Left + left / 100 * winPosition.Width);
        //        var arg2 = Convert.ToInt32(winPosition.Top + right / 100 * winPosition.Height);

        //        fullCommand = splits[0] + " " + arg1 + " " + arg2;
        //    }
        //    else
        //    {
        //        //just forward
        //        fullCommand = toSplit;
        //    }

        //    mouseControlApi.RunCommand(fullCommand);
        //}


        protected void HandleMouseAction(BaseMouseAction mouseAction, IntPtr windowHandle)
        {
            var Type = mouseAction.GetType();
            /*
            if (BaseMouseAction.MouseAction.ClickLeft.ToString().Equals(mouseAction.Action)
                && mouseAction is ClickLeftAction _clickLeft)
            {
                //this.mouseControlApi.ClickLeft(null); // not really used
                throw new NotImplementedException("Click left not implemented");
            }
            else if (BaseMouseAction.MouseAction.MouseMove.ToString().Equals(mouseAction.Action)
                && mouseAction is MouseMoveAction _mouseMove)
            {
                this.mouseControlApi.MoveMouse(_mouseMove.RelativeScreenLocation, windowHandle);
                if (_mouseMove.RelativeScreenLocation != null)
                {
                    //only do the last in a stream of mouse moves
                    this.mouseControlApi.MoveMouse(_mouseMove.RelativeScreenLocation, windowHandle);



                    //if (0 == mouseMoveCounter)
                    //{
                    //    //dont do the mouse move
                    //}
                    //else
                    //{
                    //}
                    //mouseMoveCounter = ++mouseMoveCounter % (1 + THROW_AWAY_MOUSE_MOVE_EVERY); 
                }
            }
            
            else */if (BaseMouseAction.MouseAction.LeftDown.ToString().Equals(mouseAction.Action)
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
                this.mouseControlApi.RightUp(_rightUp.RelativeScreenLocation,windowHandle);
            }
            else if (BaseMouseAction.MouseAction.RightDown.ToString().Equals(mouseAction.Action)
                     && mouseAction is RightMouseDownAction _rightDown)
            {
                this.mouseControlApi.RightDown(_rightDown.RelativeScreenLocation, windowHandle);
            }
            else
            {
                throw new NotImplementedException("Not implemented in the ");
            }
        }
    }
}
