using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace TestSEYR
{
    [TestFixture]
    public class ImageTests
    {
        SEYR.Session.Channel Channel = null;

        [OneTimeSetUp]
        [Ignore("Don't wantcha")]
        public void Setup()
        {
            Channel = new SEYR.Session.Channel($"{Path.GetTempPath()}proj.seyr", $"{Path.GetTempPath()}log.txt", 5.2e-3);
        }
    }
}
