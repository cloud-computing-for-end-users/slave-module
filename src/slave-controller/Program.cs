using client_slave_message_communication.model.mouse_action;
using custom_message_based_implementation.consts;
using message_based_communication.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using client_slave_message_communication.custom_requests;
using custom_message_based_implementation.encoding;
using custom_message_based_implementation.model;
using message_based_communication.connection;
using NLog;
using File = custom_message_based_implementation.model.File;

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
        private const string IS_LOCALHOST = "isLocal";


        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public const bool IsTesting = false;
        private static bool IsLocalhost = true;

        static void Main(string[] args)
        {
            SetupNLog();
            checked
            {
                try
                {
                    Port portToListenForRegistration = new Port() { ThePort = 10143 };
                    var self_conn_info = new ConnectionInformation()
                    {
                        IP = new IP() { TheIP = string.Empty },
                        Port = new Port() { ThePort = 10142 }
                    };

                    Port portToRegisterOn = new Port() { ThePort = 5523 };
                    var router_conn_info = new ConnectionInformation()
                    {
                        IP = new IP() { TheIP = string.Empty },
                        Port = new Port() { ThePort = 5522 }
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
                        else if (split[0].Equals(IS_LOCALHOST)) // set is localhost
                        {
                            IsLocalhost = Convert.ToBoolean(split[1]);
                            Console.WriteLine("Overriding is localhost with: " + split[1]);
                        }
                    }

                    Console.WriteLine("Using locaholhost:" + IsLocalhost);

                    if (self_conn_info.IP.TheIP == string.Empty)
                    {
                        self_conn_info.IP.TheIP = IsLocalhost ? "127.0.0.1" : "10.152.212.42";
                    }

                    if (router_conn_info.IP.TheIP == string.Empty)
                    {
                        router_conn_info.IP.TheIP = IsLocalhost ? "127.0.0.1" : "127.0.0.1";
                    }


                    Logger.Info("Slave Controller is starting...");
                    Console.WriteLine("Slave Controller is starting...");


                    var slaveController = new SlaveController(portToListenForRegistration,
                        new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE },
                        new client_slave_message_communication.encoding.CustomEncoding());
                    Console.WriteLine("Finished slave controller constructor");
                    slaveController.Setup(router_conn_info, portToRegisterOn, self_conn_info,
                        new client_slave_message_communication.encoding.CustomEncoding());

                    //  var routerProxyHelper = new ProxyHelper();
                    //routerProxyHelper.Setup(router_conn_info, portToRegisterOn, new ModuleType(){TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE}, self_conn_info,slaveController, new CustomEncoder());
                    //slaveController.ServerModuleProxyHelper = routerProxyHelper;

                    Console.WriteLine("Slave Controller has started successfully with IP: " + self_conn_info.IP.TheIP);

                    if (IsTesting)
                    {
                        Console.WriteLine(
                            "NOTICE: IS_TESTING IS SET TO TRUE!, THE SLAVE CONTROLLER WILL NOT BE ABLE TO SEND RESPONSES TO EXTERNAL PROCESSES");

                        Thread.Sleep(2000);

                        Console.WriteLine("sending: key a down");
                        slaveController.HandleRequest(
                                new DoKeyboardAction()
                                {
                                    CallID = new CallID() { ID = "-10" }
                                        ,
                                    SenderModuleID = new ModuleID() { ID = ModuleTypeConst.MODULE_TYPE_CLIENT }
                                        ,
                                    TargetModuleType = new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE }
                                    ,
                                    Key = "a"
                                    ,
                                    IsKeyDownAction = true
                                }
                            );
                        Console.WriteLine("sending: key a up");

                        slaveController.HandleRequest(
                            new DoKeyboardAction()
                            {
                                CallID = new CallID() { ID = "-10" }
                                ,
                                SenderModuleID = new ModuleID() { ID = ModuleTypeConst.MODULE_TYPE_CLIENT }
                                ,
                                TargetModuleType = new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE }
                                ,
                                Key = "a"
                                ,
                                IsKeyDownAction = false
                            }
                        );

                        Console.WriteLine("sending: key <ctrl> down");

                        Thread.Sleep(1000);
                        slaveController.HandleRequest(
                            new DoKeyboardAction()
                            {
                                CallID = new CallID() { ID = "-10" }
                                ,
                                SenderModuleID = new ModuleID() { ID = ModuleTypeConst.MODULE_TYPE_CLIENT }
                                ,
                                TargetModuleType = new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE }
                                ,
                                Key = "ctrl"
                                ,
                                IsKeyDownAction = true
                            }
                        );
                        Console.WriteLine("sending: key <tab> down");

                        slaveController.HandleRequest(
                            new DoKeyboardAction()
                            {
                                CallID = new CallID() { ID = "-10" }
                                ,
                                SenderModuleID = new ModuleID() { ID = ModuleTypeConst.MODULE_TYPE_CLIENT }
                                ,
                                TargetModuleType = new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE }
                                ,
                                Key = "tab"
                                ,
                                IsKeyDownAction = true
                            }
                        );
                        Console.WriteLine("sending: key <ctrl> up");

                        slaveController.HandleRequest(
                            new DoKeyboardAction()
                            {
                                CallID = new CallID() { ID = "-10" }
                                ,
                                SenderModuleID = new ModuleID() { ID = ModuleTypeConst.MODULE_TYPE_CLIENT }
                                ,
                                TargetModuleType = new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE }
                                ,
                                Key = "ctrl"
                                ,
                                IsKeyDownAction = false
                            }
                        );
                        Console.WriteLine("sending: key <tab> up");

                        slaveController.HandleRequest(
                            new DoKeyboardAction()
                            {
                                CallID = new CallID() { ID = "-10" }
                                ,
                                SenderModuleID = new ModuleID() { ID = ModuleTypeConst.MODULE_TYPE_CLIENT }
                                ,
                                TargetModuleType = new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE }
                                ,
                                Key = "tab"
                                ,
                                IsKeyDownAction = false
                            }
                        );

                        //slaveController.HandleRequest(new Handshake()
                        //{
                        //    arg1PrimaryKey = new PrimaryKey() { TheKey = -10 }
                        //    ,
                        //    SenderModuleID = new ModuleID() { ID = ModuleTypeConst.MODULE_TYPE_CLIENT }
                        //    ,
                        //    TargetModuleType = new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE }
                        //    ,
                        //    CallID = new CallID() { ID = "-1qdqwdqasdwd0" },

                        //});

                        //slaveController.HandleRequest(
                        //    new FetchRemoteFile()
                        //    {
                        //        CallID = new CallID() { ID = "-1qdqwdqwd0" },
                        //        FileName = "kenneth-test.txt"
                        //        ,
                        //        SenderModuleID = new ModuleID() { ID = ModuleTypeConst.MODULE_TYPE_CLIENT }
                        //        ,
                        //        TargetModuleType = new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE }
                        //    });
                        //slaveController.HandleRequest(new SaveFilesAndTerminate()
                        //{
                        //    CallID = new CallID() { ID = "-10" }
                        //        ,
                        //    SenderModuleID = new ModuleID() { ID = ModuleTypeConst.MODULE_TYPE_CLIENT }
                        //        ,
                        //    TargetModuleType = new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE }

                        //}
                        //    );

                        //slaveController.HandleRequest(new FetchRemoteFile() { }});
                        ////slaveController.FileProxy.UploadFile(
                        ////    new File()
                        ////    {
                        ////        FileName = new FileName() { FileNameProp = "kenneth-test.txt"}
                        ////        ,FileData = System.IO.File.ReadAllBytes(@"C:\Users\MSI\Desktop\test-kenneth.txt")
                        ////    }
                        ////    , new PrimaryKey() { TheKey = -10}
                        ////    , false
                        ////    , () => { }
                        ////    );
                        ////Console.WriteLine("File sent to file server");


                        ////List<FileName> list = null;
                        ////slaveController.FileProxy.GetListOfFiles(
                        ////    new PrimaryKey() { TheKey = -10 }
                        ////    , (obj) => { list = obj;}
                        ////);
                        ////while (null == list)
                        ////{
                        ////    Thread.Sleep(20);
                        ////}

                        ////Console.WriteLine("Printing files that are available on server");

                        ////foreach (var fileName in list)
                        ////{

                        ////    Console.WriteLine("File: " + fileName.FileNameProp);
                        ////    //retrive files
                        ////    File file = null;
                        ////    slaveController.FileProxy.DownloadFile(fileName, new PrimaryKey() { TheKey = -10},
                        ////        (f) => { file = f;});
                        ////    while (null == file)
                        ////    {
                        ////        Thread.Sleep(10);
                        ////    }
                        ////    //save the file
                        ////    using(var fs = new FileStream(@"C:\Users\MSI\Desktop\" + "file-from-server.txt" , FileMode.Create,FileAccess.Write))
                        ////    {
                        ////        fs.Write(file.FileData);
                        ////        fs.Flush();
                        ////    }
                        ////}


                        //Thread.Sleep(5000);

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
                        //slaveController.DoMouseAction(new LeftMouseDownAction());


                        //Thread.Sleep(2000);
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
                        //slaveController.DoMouseAction(new LeftMouseUpAction());

                        //Thread.Sleep(2000);
                        return;
                    }

                    Console.WriteLine(
                        "Putting main thread to sleep in a loop. Will terminate when reciving SaveFilesAndTerminate command from a client.");
                    while (slaveController.IsRunning)
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception ex)
                {
                    PythonStarter.KillAllStartedProcesses();
                    Console.WriteLine("The slave encountered an exception during startup");
                    Console.WriteLine("Exception message: " + ex.Message);
                    Console.WriteLine("Exception stack trace: " + ex.StackTrace);
                    Console.WriteLine("Press any key to exit");
                    Logger.Debug("Terminated due to exception in main");
                    Logger.Debug(ex);
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
