using client_slave_message_communication.model.mouse_action;
using custom_message_based_implementation.consts;using message_based_communication.model;
using System;
using System.Threading;
using custom_message_based_implementation.model;
using NLog;

namespace slave_controller
{
    class Program
    {

        private const string SELF_IP = "sip";
        private const string SELF_COMM_PORT = "scp";
        private const string SELF_REG_PORT = "srp";
        private const string ROUTER_IP = "rip";
        private const string ROUTER_COMM_PORT = "rcp";
        private const string ROUTER_REG_PORT = "rrp";

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static bool IsTesting = false;
        private const bool IsLocalhost = true;
        static void Main(string[] args)
        {
            SetupNLog();
            checked
            {
                try
                {
                    Port portToListenForRegistration = new Port(){ThePort = 10143 };
                    var self_conn_info = new ConnectionInformation()
                    {
                        IP = new IP() { TheIP = IsLocalhost ? "127.0.0.1" : "192.168.137.149" },
                        Port = new Port() { ThePort = 10142 }
                    };

                    Port portToRegisterOn = new Port() { ThePort = 10123 };
                    var router_conn_info = new ConnectionInformation()
                    {
                        IP = new IP() { TheIP = IsLocalhost ? "127.0.0.1" : "192.168.137.149" },
                        Port = new Port() { ThePort = 10122 }
                    };



                    foreach (var arg in args) // decoding system arguments
                    {
                        var split = arg.Split(":");
                        if (2 != split.Length)
                        {
                            throw new ArgumentException("Got badly formatted system arguments");
                        }
                        if (split[0].Equals(SELF_IP)) // set self ip
                        {
                            self_conn_info.IP.TheIP = split[1];
                            Console.WriteLine("Overriding self ip with: " + split[1]);
                        }
                        else if (split[0].Equals(SELF_COMM_PORT)) // set self communication port
                        {
                            self_conn_info.Port.ThePort = Convert.ToInt32(split[1]);
                            Console.WriteLine("Overriding self communication port with: " + split[1]);
                        }
                        else if (split[0].Equals(SELF_REG_PORT)) // set self registration port
                        {
                            portToListenForRegistration.ThePort = Convert.ToInt32(split[1]);
                            Console.WriteLine("Overriding register to self port with: " + split[1]);
                        }
                        if (split[0].Equals(ROUTER_IP)) // set self ip
                        {
                            router_conn_info.IP.TheIP = split[1];
                            Console.WriteLine("Overriding router ip with: " + split[1]);
                        }
                        else if (split[0].Equals(ROUTER_COMM_PORT)) // set self communication port
                        {
                            router_conn_info.Port.ThePort = Convert.ToInt32(split[1]);
                            Console.WriteLine("Overriding router communication port with: " + split[1]);
                        }
                        else if (split[0].Equals(ROUTER_REG_PORT)) // set self registration port
                        {
                            portToRegisterOn.ThePort = Convert.ToInt32(split[1]);
                            Console.WriteLine("Overriding register to port with: " + split[1]);
                        }
                    }





                    Logger.Info("Slave Controller is starting...");
                    Console.WriteLine("Slave Controller is starting...");
                    


                    var slaveController = new SlaveController(new Port() { ThePort = 60606 }, portToListenForRegistration, new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE }, new client_slave_message_communication.encoding.CustomEncoding());
                    slaveController.Setup(router_conn_info, portToRegisterOn, self_conn_info, new client_slave_message_communication.encoding.CustomEncoding());

                    Console.WriteLine("Slave Controller has started successfully with IP: " + self_conn_info.IP.TheIP);

                    if (IsTesting)
                    {
                        slaveController.Handshake(new PrimaryKey());

                        Thread.Sleep(5000);

                        /*
                        slaveController.DoMouseAction(new MouseMoveAction()
                        {
                            RelativeScreenLocation = new client_slave_message_communication.model.RelativeScreenLocation()
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
                        */
                        slaveController.DoMouseAction(new LeftMouseDownAction());


                        Thread.Sleep(2000);
                        /*
                        slaveController.DoMouseAction(new MouseMoveAction()
                        {
                            RelativeScreenLocation = new client_slave_message_communication.model.RelativeScreenLocation()
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
                        */
                        slaveController.DoMouseAction(new LeftMouseUpAction());

                        Thread.Sleep(2000);
                    }

                    Console.WriteLine("Putting main thread to sleep in a loop. Will terminate when reciving SaveFilesAndTerminate command from a client.");
                    while (slaveController.IsRunning)
                    {
                        Thread.Sleep(1000);
                    }
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

        static void SetupNLog()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var logFile = "slave-module-log.txt";

            /*
            var rootFolder = System.AppDomain.CurrentDomain.BaseDirectory;
            if (File.Exists(Path.Combine(rootFolder, logFile)))
            {  
                File.Delete(Path.Combine(rootFolder, logFile));
            }
            */

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = logFile };

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;

        }

    }
}
