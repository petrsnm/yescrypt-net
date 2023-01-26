using System.Text;

namespace tests
{
    [TestClass]
    public class Yescrypt64EncoderTest
    {
        [TestMethod]
        public void TestEncodeUint32Min()
        {
            Yescrypt64Encoder encoder = new Yescrypt64Encoder();
            Assert.AreEqual(".", encoder.AppendUint32Min(0, 0).ToString());
            encoder.Reset();
            Assert.AreEqual("/", encoder.AppendUint32Min(1, 0).ToString());
            encoder.Reset();
            Assert.AreEqual("0", encoder.AppendUint32Min(2, 0).ToString());
            encoder.Reset();
            Assert.AreEqual("j", encoder.AppendUint32Min(47, 0).ToString());
            encoder.Reset();
            Assert.AreEqual("k.", encoder.AppendUint32Min(48, 0).ToString());
            encoder.Reset();
            Assert.AreEqual(".", encoder.AppendUint32Min(1, 1).ToString());
            encoder.Reset();
            Assert.AreEqual(".", encoder.AppendUint32Min(2, 2).ToString());
            encoder.Reset();
            Assert.AreEqual("zzzzzz", encoder.AppendUint32Min(0x4108422f, 0).ToString());
            encoder.Reset();
            Assert.AreEqual("zzzzzz", encoder.AppendUint32Min(0x41084230, 1).ToString());
            encoder.Reset();
        }

        [TestMethod()]
        [ExpectedException(typeof(Yescrypt64EncoderException))]
        public void TestEncodeUint32MinInvalidMin()
        {
            Yescrypt64Encoder encoder = new Yescrypt64Encoder();
            encoder.AppendUint32Min(1, 2);
        }

        [TestMethod()]
        [ExpectedException(typeof(Yescrypt64EncoderException))]
        public void TestEncodeUint32MinWithInvalidValue()
        {
            Yescrypt64Encoder encoder = new Yescrypt64Encoder();
            encoder.AppendUint32Min(0x41084230, 0);
        }

        [TestMethod]
        public void AppendUint32Bits()
        {
            Yescrypt64Encoder encoder = new Yescrypt64Encoder();
            Assert.AreEqual("......", encoder.AppendUint32Bits(0, 32).ToString());
            encoder.Reset();
            Assert.AreEqual("/.....", encoder.AppendUint32Bits(1, 32).ToString());
            encoder.Reset();
            Assert.AreEqual("z", encoder.AppendUint32Bits(63, 1).ToString());
            encoder.Reset();
            Assert.AreEqual(".", encoder.AppendUint32Bits(64, 1).ToString());
            encoder.Reset();
            Assert.AreEqual("/", encoder.AppendUint32Bits(65, 1).ToString());
            encoder.Reset();
            Assert.AreEqual("z.", encoder.AppendUint32Bits(63, 7).ToString());
            encoder.Reset();
            Assert.AreEqual("./", encoder.AppendUint32Bits(64, 7).ToString());
            encoder.Reset();
            Assert.AreEqual("//", encoder.AppendUint32Bits(65, 7).ToString());
            encoder.Reset();
            Assert.AreEqual("", encoder.AppendUint32Bits(0xffffffff, 0).ToString());
            encoder.Reset();
            Assert.AreEqual("z", encoder.AppendUint32Bits(0xffffffff, 1).ToString());
            encoder.Reset();
            Assert.AreEqual("zzzzz1", encoder.AppendUint32Bits(0xffffffff, 32).ToString());
        }

        [TestMethod]
        public void AppendAppendBytes()
        {
            Yescrypt64Encoder encoder = new Yescrypt64Encoder();
            byte[] bytes;

            bytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Assert.AreEqual("...................", encoder.AppendBytes(bytes).ToString());
            encoder.Reset();

            bytes = new byte [] { 0xf, 0xf, 0xf, 0xf, 0xf, 0xf, 0x0f, 0x0f, 0x0f, 0x0f, 0x0f, 0x0f, 0x0f, 0x0f };
            Assert.AreEqual("Dwk1Dwk1Dwk1Dwk1Dw.", encoder.AppendBytes(bytes).ToString());
            encoder.Reset();

            bytes = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            Assert.AreEqual("zzzzzzzzzzzzzzzzzzD", encoder.AppendBytes(bytes).ToString());
            encoder.Reset();

            bytes = new byte[] { 0xff };
            Assert.AreEqual("z1", encoder.AppendBytes(bytes).ToString());
            encoder.Reset();

            bytes = new byte [] { 0xac, 0xd9, 0xa4, 0x20, 0x1c, 0xf4, 0xa4, 0x76, 0xec, 0xf7, 0xba, 0xa6, 0x11, 0x3d};
            Assert.AreEqual("gaBdUk/xYO5vrffdFo1", encoder.AppendBytes(bytes).ToString());
            encoder.Reset();

        }
    }
    
}