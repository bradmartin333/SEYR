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
        public void Setup()
        {
            Channel = new SEYR.Session.Channel($"{Path.GetTempPath()}proj.seyr", $"{Path.GetTempPath()}log.txt", 5.2e-3);
        }

        [Test]
        public async Task LoadImage()
        {
            Channel.Tasks.Clear();
            for (int i = 0; i < 5; i++)
                Channel.NewImage(Properties.Resources.PxEng_A);
            int len = await Task.Factory.ContinueWhenAll(Channel.Tasks.ToArray(), x => x.Length);
            Assert.AreEqual(5, len);
            Assert.IsTrue(!string.IsNullOrEmpty(Channel.GetLastData()));
        }

        [Test]
        public async Task LoadBadImage()
        {
            Channel.Tasks.Clear();
            for (int i = 0; i < 5; i++)
                Channel.NewImage(new System.Drawing.Bitmap(1, 1));
            int len = await Task.Factory.ContinueWhenAll(Channel.Tasks.ToArray(), x => x.Length);
            Assert.AreEqual(5, len);
            Assert.AreEqual("Failed to process image", Channel.GetLastData());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Channel.Close();
        }
    }
}
