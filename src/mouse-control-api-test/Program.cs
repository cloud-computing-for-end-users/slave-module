using mouse_control_api;
using System;
/// <summary>
/// is intended for manual testing and therefore is not names "Test" with a lower case "t".
/// </summary>
namespace MouseControlApiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var mouseControlApi = new MouseControlApi();
            mouseControlApi.moveMouse(200, 200);
            mouseControlApi.moveMouse(400, 200);
            mouseControlApi.moveMouse(200, 400);
            mouseControlApi.moveMouse(800, 200);
            mouseControlApi.moveMouse(200, 800);
            mouseControlApi.moveMouse(1600, 600);

            Console.ReadKey();
        }
    }
}
