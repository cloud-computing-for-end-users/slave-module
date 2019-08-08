using System;
using System.Runtime.InteropServices;

namespace slave_module_cs
{
    class Program
    {
        [DllImport(@"Resources/screen-capture-cpp.dll")]
        public static extern void PrintHelloWorld();

        static void Main(string[] args)
        {
            PrintHelloWorld();

            Console.Read();
        }
    }
}