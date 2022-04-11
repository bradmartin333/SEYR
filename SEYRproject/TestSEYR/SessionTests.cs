using NUnit.Framework;

namespace TestSEYR
{
    [TestFixture]
    public class SessionTests
    {
        private readonly string DesktopStr = @"C:\Users\brad.martin\Desktop\";
        private SEYR.Session.Channel Channel;

        [SetUp]
        public void Setup()
        {
            Channel = new SEYR.Session.Channel($"{DesktopStr}proj.seyr", $"{DesktopStr}log.txt", 5.2e-3);
        }

        [Test]
        public void Test1()
        {
            Assert.AreEqual("0.0052", Channel.GetProjectInfo());
        }

        [TearDown]
        public void Finish()
        {
            Channel.Close();
        }
    }
}
