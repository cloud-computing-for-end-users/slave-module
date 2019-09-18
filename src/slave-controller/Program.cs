using custom_message_based_implementation.consts;
using message_based_communication.model;
using mouse_control_api.ConnectionWrapper;
using System;

namespace slave_controller
{
    class Program
    {
        static void Main(string[] args)
        {
            checked
            {
                try
                {
                    Console.WriteLine("Starting the slave controller");

                    var slaveCommInfo = new ConnectionInformation()
                    {
                        IP = new IP() { TheIP = "127.0.0.1" },
                        Port = new Port() { ThePort = 10142 }
                    };



                    var slaveController = new SlaveController(new Port() { ThePort = 60606 }, new Port() { ThePort = 10143 }, new ModuleType() { TypeID = ModuleTypeConst.MODULE_TYPE_SLAVE }, new client_slave_message_communication.encoding.CustomEncoding());
                    slaveController.Setup(slaveCommInfo, new Port() { ThePort = 10143 }, slaveCommInfo, new client_slave_message_communication.encoding.CustomEncoding());

                    Console.WriteLine("The slave controller started without experiencing any exceptions");

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
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine("Press any key to exit");
                    Console.ReadKey();
                }


            }

        }
    }
}
