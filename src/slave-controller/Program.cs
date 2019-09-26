using client_slave_message_communication.model.mouse_action;
using custom_message_based_implementation.consts;
using message_based_communication.model;
using System;
using System.Threading;

namespace slave_controller
{
    class Program
    {
        private const bool IsTesting = true;
        private const bool IsLocalhost = true;
        static void Main(string[] args)
        {
            checked
            {
                try
                {
                    Console.WriteLine("Slave Controller is starting...");
                    
                    var slaveCommInfo = new ConnectionInformation()
                    {
                        IP = new IP() { TheIP = IsLocalhost ? "127.0.0.1" : "10.152.212.21" },
                        Port = new Port() { ThePort = 10142 }
                    };

                    var slaveController = new SlaveController(new Port() { ThePort = 60606 }, new Port() { ThePort = 10143 }, new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE }, new client_slave_message_communication.encoding.CustomEncoding());
                    slaveController.Setup(slaveCommInfo, new Port() { ThePort = 10143 }, slaveCommInfo, new client_slave_message_communication.encoding.CustomEncoding());

                    Console.WriteLine("Slave Controller has started successfully with IP: " + slaveCommInfo.IP.TheIP);

                    if (IsTesting)
                    {
                        Thread.Sleep(5000);

                        slaveController.DoMouseAction(new MouseMoveAction()
                        {
                            arg1RelativeScreenLocation = new client_slave_message_communication.model.RelativeScreenLocation()
                            {
                                FromLeft = new client_slave_message_communication.model.Percent()
                                {
                                    ThePercentage = 20
                                },
                                FromTop = new client_slave_message_communication.model.Percent()
                                {
                                    ThePercentage = 25
                                }
                            }
                        });

                        slaveController.DoMouseAction(new LeftMouseDownAction());


                        Thread.Sleep(2000);

                        slaveController.DoMouseAction(new MouseMoveAction()
                        {
                            arg1RelativeScreenLocation = new client_slave_message_communication.model.RelativeScreenLocation()
                            {
                                FromLeft = new client_slave_message_communication.model.Percent()
                                {
                                    ThePercentage = 50
                                },
                                FromTop = new client_slave_message_communication.model.Percent()
                                {
                                    ThePercentage = 30
                                }
                            }
                        });
                        Thread.Sleep(2000);

                        slaveController.DoMouseAction(new LeftMouseUpAction());


                        Thread.Sleep(2000);


                    }

                    do
                    {
                        Console.WriteLine("tye exit to stop the slave controller");
                        var input = Console.ReadLine();
                        if ("exit".Equals(input))
                        {
                            break;
                        }
                    } while (true);


                }
                catch (Exception ex)
                {
                    Console.WriteLine("The slave encountered an exception during startup");
                    Console.WriteLine("Exception message: " + ex.Message);
                    Console.WriteLine("Exception stack trace: " + ex.StackTrace);
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                }


            }

        }
    }
}
