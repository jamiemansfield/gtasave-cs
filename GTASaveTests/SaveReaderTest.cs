using GTASave;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace GTASaveTests
{
    [TestClass]
    public class SaveReaderTest
    {
        [TestMethod]
        public void TestReadGTAString()
        {
            byte[] real = Encoding.ASCII.GetBytes("Example");
            byte[] raw = new byte[real.Length + 1];
            Array.Copy(real, raw, real.Length);
            raw[real.Length] = 0;

            Stream stream = new MemoryStream(raw);
            SaveReader reader = new SaveReader(stream);
            string read = reader.ReadGTAString(raw.Length);

            Assert.AreEqual("Example", read);
        }
    }
}
