using NUnit.Framework;
using System.IO;

namespace TestSEYR
{
    [TestFixture]
    //[Ignore("UI Tests")]
    public class WIzardTests
    {
        SEYR.Session.Channel Channel = null;

        [OneTimeSetUp]
        public void Setup()
        {
            Channel = new SEYR.Session.Channel($"{Path.GetTempPath()}proj.seyr", $"{Path.GetTempPath()}log.txt");
        }

        [Test]
        public void RunWizard()
        {
            Channel.RunWizard(Properties.Resources.PxEng_A);
            Assert.IsTrue(true);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Channel.Close();
        }
    }
}
