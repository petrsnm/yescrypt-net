using System.Text;

namespace tests
{
    [TestClass]
    public class B64StringWriterTest
    {
        [TestMethod]
        public void TestWriteUint32Min()
        {
            B64StringWriter writer = new B64StringWriter();
            Assert.AreEqual(".", writer.WriteUint32Min(0, 0).ToString());
            writer.Reset();
            Assert.AreEqual("/", writer.WriteUint32Min(1, 0).ToString());
            writer.Reset();
            Assert.AreEqual("0", writer.WriteUint32Min(2, 0).ToString());
            writer.Reset();
            Assert.AreEqual("j", writer.WriteUint32Min(47, 0).ToString());
            writer.Reset();
            Assert.AreEqual("k.", writer.WriteUint32Min(48, 0).ToString());
            writer.Reset();
            Assert.AreEqual(".", writer.WriteUint32Min(1, 1).ToString());
            writer.Reset();
            Assert.AreEqual(".", writer.WriteUint32Min(2, 2).ToString());
            writer.Reset();
            Assert.AreEqual("zzzzzz", writer.WriteUint32Min(0x4108422f, 0).ToString());
            writer.Reset();
            Assert.AreEqual("zzzzzz", writer.WriteUint32Min(0x41084230, 1).ToString());
            writer.Reset();
        }

        [TestMethod()]
        [ExpectedException(typeof(Yescrypt64StringWriterException))]
        public void TestWriteUint32MinInvalidMin()
        {
            B64StringWriter writer = new B64StringWriter();
            writer.WriteUint32Min(1, 2);
        }

        [TestMethod()]
        [ExpectedException(typeof(Yescrypt64StringWriterException))]
        public void TestWriteUint32MinWithInvalidValue()
        {
            B64StringWriter writer = new B64StringWriter();
            writer.WriteUint32Min(0x41084230, 0);
        }

        [TestMethod]
        public void TestWriteUint32Bits()
        {
            B64StringWriter writer = new B64StringWriter();
            Assert.AreEqual("......", writer.WriteUint32Bits(0, 32).ToString());
            writer.Reset();
            Assert.AreEqual("/.....", writer.WriteUint32Bits(1, 32).ToString());
            writer.Reset();
            Assert.AreEqual("z", writer.WriteUint32Bits(63, 1).ToString());
            writer.Reset();
            Assert.AreEqual(".", writer.WriteUint32Bits(64, 1).ToString());
            writer.Reset();
            Assert.AreEqual("/", writer.WriteUint32Bits(65, 1).ToString());
            writer.Reset();
            Assert.AreEqual("z.", writer.WriteUint32Bits(63, 7).ToString());
            writer.Reset();
            Assert.AreEqual("./", writer.WriteUint32Bits(64, 7).ToString());
            writer.Reset();
            Assert.AreEqual("//", writer.WriteUint32Bits(65, 7).ToString());
            writer.Reset();
            Assert.AreEqual("", writer.WriteUint32Bits(0xffffffff, 0).ToString());
            writer.Reset();
            Assert.AreEqual("z", writer.WriteUint32Bits(0xffffffff, 1).ToString());
            writer.Reset();
            Assert.AreEqual("zzzzz1", writer.WriteUint32Bits(0xffffffff, 32).ToString());
        }

        [TestMethod]
        public void TestWriteBytes()
        {
            B64StringWriter writer = new B64StringWriter();
            byte[] bytes;

            bytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            Assert.AreEqual("...................", writer.WriteBytes(bytes).ToString());
            writer.Reset();

            bytes = new byte [] { 0xf, 0xf, 0xf, 0xf, 0xf, 0xf, 0x0f, 0x0f, 0x0f, 0x0f, 0x0f, 0x0f, 0x0f, 0x0f };
            Assert.AreEqual("Dwk1Dwk1Dwk1Dwk1Dw.", writer.WriteBytes(bytes).ToString());
            writer.Reset();

            bytes = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            Assert.AreEqual("zzzzzzzzzzzzzzzzzzD", writer.WriteBytes(bytes).ToString());
            writer.Reset();

            bytes = new byte[] { 0xff };
            Assert.AreEqual("z1", writer.WriteBytes(bytes).ToString());
            writer.Reset();

            bytes = new byte [] { 0xac, 0xd9, 0xa4, 0x20, 0x1c, 0xf4, 0xa4, 0x76, 0xec, 0xf7, 0xba, 0xa6, 0x11, 0x3d};
            Assert.AreEqual("gaBdUk/xYO5vrffdFo1", writer.WriteBytes(bytes).ToString());
            writer.Reset();

            bytes = new byte[] { 0xac, 0xd9 };
            writer.WriteBytes(bytes);
            bytes = new byte[] { 0xac, 0xd9, 0xa4, 0x20, 0x1c, 0xf4, 0xa4, 0x76, 0xec, 0xf7, 0xba, 0xa6, 0x11, 0x3d };
            var x = writer.WriteBytes(bytes).ToString();
        }
    }
    
}