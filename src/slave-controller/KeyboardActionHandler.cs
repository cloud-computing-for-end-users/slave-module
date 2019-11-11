using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using client_slave_message_communication.custom_requests;
using client_slave_message_communication.model.mouse_action;
using slave_control_api.ConnectionWrapper;
using slave_control_api.controlers;

namespace slave_controller
{
    public class KeyboardActionHandler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private KeyboardControlApi keyboardControlApi;

        private Queue<DoKeyboardAction> keyboardActionQueue = new Queue<DoKeyboardAction>();
        public void QueueKeyboardCommand(DoKeyboardAction keyboardAction)
        {
            keyboardActionQueue.Enqueue(keyboardAction);
        }


        public KeyboardActionHandler(PythonWrapper pythonWrapper)
        {
            keyboardControlApi = new KeyboardControlApi(pythonWrapper);

            var t = new Thread(() =>
            {

                while (true)
                {
                    if (0 == keyboardActionQueue.Count)
                    {
                        Thread.Sleep(10);
                    }
                    else
                    {
                        try
                        {
                            HandleKeyboardAction(keyboardActionQueue.Dequeue());
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


        protected void HandleKeyboardAction(DoKeyboardAction keyboardAction)
        {
            keyboardControlApi.ExecuteKeyboardAction(keyboardAction);
        }
    }
}
