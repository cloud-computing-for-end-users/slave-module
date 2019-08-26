using System;
using NUnit.Framework;
using screen_capture_api;

namespace Tests
{
    public class Tests
    {
        ScreenCapture sc;

        [SetUp]
        public void Setup()
        {
            sc = new ScreenCapture();
        }

        [Test]
        public void Test2()
        {
            var path = sc.CaptureScreen("Notepad++");
            Console.WriteLine(path);
        }
    }
}