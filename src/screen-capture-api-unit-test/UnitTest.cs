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
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            Assert.IsTrue(sc.Test());
        }

    }
}