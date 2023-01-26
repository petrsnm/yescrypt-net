using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    [TestClass]
    public class Yescrypt64StringReaderTest
    {

        [TestMethod]
        public void TestReadUint32Min()
        {
            Yescrypt64StringReader reader;

            reader = new Yescrypt64StringReader(".");
            Assert.AreEqual(0u, reader.ReadUint32Min(0));
            
            reader = new Yescrypt64StringReader("/");
            Assert.AreEqual(1u, reader.ReadUint32Min(0));

            reader = new Yescrypt64StringReader("0");
            Assert.AreEqual(2u, reader.ReadUint32Min(0));

            reader = new Yescrypt64StringReader("j");
            Assert.AreEqual(47u, reader.ReadUint32Min(0));

            reader = new Yescrypt64StringReader("k.");
            Assert.AreEqual(48u, reader.ReadUint32Min(0));

            reader = new Yescrypt64StringReader(".");
            Assert.AreEqual(1u, reader.ReadUint32Min(1));

            reader = new Yescrypt64StringReader(".");
            Assert.AreEqual(2u, reader.ReadUint32Min(2));

            reader = new Yescrypt64StringReader("zzzzzz");
            Assert.AreEqual(0x4108422fu, reader.ReadUint32Min(0));

            reader = new Yescrypt64StringReader("zzzzzz");
            Assert.AreEqual(0x41084230u, reader.ReadUint32Min(1));
        }

        [TestMethod()]
        [ExpectedException(typeof(Yescrypt64StringReaderException))]
        public void TestReadUint32MinInvalidEncoding()
        {
            Yescrypt64StringReader reader = new Yescrypt64StringReader("$");
            reader.ReadUint32Min(0);
        }

        [TestMethod]
        public void TestReadUint32Bits()
        {
            Yescrypt64StringReader reader;

            reader = new Yescrypt64StringReader("......");
            Assert.AreEqual(0u, reader.ReadUint32Bits(32));

            reader = new Yescrypt64StringReader("/.....");
            Assert.AreEqual(1u, reader.ReadUint32Bits(32));

            reader = new Yescrypt64StringReader("z");
            Assert.AreEqual(63u, reader.ReadUint32Bits(1));

            reader = new Yescrypt64StringReader(".");
            Assert.AreEqual(0u, reader.ReadUint32Bits(1));

            reader = new Yescrypt64StringReader("/");
            Assert.AreEqual(1u, reader.ReadUint32Bits(1));

            reader = new Yescrypt64StringReader("z.");
            Assert.AreEqual(63u, reader.ReadUint32Bits(7));

            reader = new Yescrypt64StringReader("./");
            Assert.AreEqual(64u, reader.ReadUint32Bits(7));

            reader = new Yescrypt64StringReader("//");
            Assert.AreEqual(65u, reader.ReadUint32Bits(7));

            reader = new Yescrypt64StringReader("z");
            Assert.AreEqual(0u, reader.ReadUint32Bits(0));

            reader = new Yescrypt64StringReader("z");
            Assert.AreEqual(63u, reader.ReadUint32Bits(1));

            reader = new Yescrypt64StringReader("zzzzz1");
            Assert.AreEqual(0xffffffffu, reader.ReadUint32Bits(32));
        }

        [TestMethod()]
        [ExpectedException(typeof(Yescrypt64StringReaderException))]
        public void TestReadUint32BitsInvalidEncoding()
        {
            Yescrypt64StringReader reader = new Yescrypt64StringReader("$");
            reader.ReadUint32Bits(32);
        }


        [TestMethod]
        public void TestReadBytes()
        {
            Yescrypt64StringReader reader;
            byte[] bytes;

            reader = new Yescrypt64StringReader("...................");
            bytes = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            CollectionAssert.AreEqual(bytes, reader.ReadBytes(14));
                       
            reader = new Yescrypt64StringReader("Dwk1Dwk1Dwk1Dwk1Dw.");
            bytes = new byte[] { 0xf, 0xf, 0xf, 0xf, 0xf, 0xf, 0x0f, 0x0f, 0x0f, 0x0f, 0x0f, 0x0f, 0x0f, 0x0f };
            CollectionAssert.AreEqual(bytes, reader.ReadBytes(14));

            reader = new Yescrypt64StringReader("zzzzzzzzzzzzzzzzzzD");
            bytes = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
            CollectionAssert.AreEqual(bytes, reader.ReadBytes(14));

            reader = new Yescrypt64StringReader("z1");
            bytes = new byte[] { 0xff };
            CollectionAssert.AreEqual(bytes, reader.ReadBytes(1));

            reader = new Yescrypt64StringReader("gaBdUk/xYO5vrffdFo1");
            bytes = new byte[] { 0xac, 0xd9, 0xa4, 0x20, 0x1c, 0xf4, 0xa4, 0x76, 0xec, 0xf7, 0xba, 0xa6, 0x11, 0x3d };
            CollectionAssert.AreEqual(bytes, reader.ReadBytes(14));

            reader = new Yescrypt64StringReader("gaBgaBdUk/xYO5vrffdFo1");
            bytes = new byte[] { 0xac, 0xd9 };
            CollectionAssert.AreEqual(bytes, reader.ReadBytes(2));
            bytes = new byte[] { 0xac, 0xd9, 0xa4, 0x20, 0x1c, 0xf4, 0xa4, 0x76, 0xec, 0xf7, 0xba, 0xa6, 0x11, 0x3d };
            CollectionAssert.AreEqual(bytes, reader.ReadBytes(14));
            
        }
    }
}
