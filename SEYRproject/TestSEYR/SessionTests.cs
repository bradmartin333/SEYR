using NUnit.Framework;
using System.IO;

namespace TestSEYR
{
    [TestFixture]
    public class SessionTests
    {
        [Test, Order(1)]
        public void VerifyProjectCreation()
        {
            SEYR.Session.Channel ch = new SEYR.Session.Channel($"{Path.GetTempPath()}proj.seyr", $"{Path.GetTempPath()}log.txt", 5.2e-3);
            Assert.AreEqual("0.0052", ch.GetProjectInfo());
            ch.Close();
        }

        [Test, Order(2)]
        public void LoadProject()
        {
            SEYR.Session.Channel ch = new SEYR.Session.Channel($"{Path.GetTempPath()}proj.seyr", $"{Path.GetTempPath()}log.txt");
            Assert.AreEqual("0.0052", ch.GetProjectInfo());
            ch.Close();
        }
    }
}
