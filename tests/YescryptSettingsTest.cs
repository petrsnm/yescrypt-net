using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{


    [TestClass]
    public class YescryptSettingsTest
    {
        [TestMethod]
        public void DecodingTest()
        {
            var settings = new YescryptSettings("$y$j9T$CoUww2aXLQYsu1DSF5GRN.$QMv6evLbghgjmlWPbRbFrUjYMWqrZ.Xpi4ydv6S5eS4");
            Assert.AreEqual(182u, settings.flags);
            Assert.AreEqual(4096u, settings.N);
            Assert.AreEqual(32u, settings.r);
            Assert.AreEqual(1u, settings.p);
            Assert.AreEqual(0u, settings.t);
            Assert.AreEqual(0u, settings.g);
            byte[] expected = new byte[] { 0x0e, 0x0d, 0xf2, 0x3c, 0x61, 0x8e, 0x17, 0x47, 0xe2, 0xfa, 0xf0, 0x78, 0xd1, 0x21, 0x75, 0x19 };
            CollectionAssert.AreEqual(expected, settings.salt);
            expected = new byte[] { 0x1c, 0xb6, 0x23, 0xea, 0x7e, 0x9d, 0x6c, 0xcb, 0xbe, 0x72, 0x2c, 0x6e, 0x67, 0x77, 0x46, 0x37, 0xf8, 0x92, 0x98, 0x68, 0xdf, 0x25, 0x30, 0xd6, 0xae, 0xe1, 0xa7, 0x3b, 0xe2, 0x1d, 0xaa, 0x67 };
            CollectionAssert.AreEqual(expected, settings.key);
        }


        [TestMethod]
        public void EncodingTest()
        {
            YescryptSettings settings = new YescryptSettings();
            settings.flags = 182u;
            settings.N = 4096u;
            settings.r = 32u;
            settings.salt = new byte[] { 0x0e, 0x0d, 0xf2, 0x3c, 0x61, 0x8e, 0x17, 0x47, 0xe2, 0xfa, 0xf0, 0x78, 0xd1, 0x21, 0x75, 0x19 };
            settings.key = new byte[] { 0x1c, 0xb6, 0x23, 0xea, 0x7e, 0x9d, 0x6c, 0xcb, 0xbe, 0x72, 0x2c, 0x6e, 0x67, 0x77, 0x46, 0x37, 0xf8, 0x92, 0x98, 0x68, 0xdf, 0x25, 0x30, 0xd6, 0xae, 0xe1, 0xa7, 0x3b, 0xe2, 0x1d, 0xaa, 0x67 };
            Assert.AreEqual("$y$j9T$CoUww2aXLQYsu1DSF5GRN.$QMv6evLbghgjmlWPbRbFrUjYMWqrZ.Xpi4ydv6S5eS4", settings.ToString());
        }
    }

}
