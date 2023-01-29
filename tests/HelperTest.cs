using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    [TestClass]
    public class HelpertTest
    {
        [TestMethod]
        public void TestP2Floor()
        {
            Assert.AreEqual(4UL, Helper.P2floor(4));
            Assert.AreEqual(4UL, Helper.P2floor(5));
            Assert.AreEqual(67108864UL, Helper.P2floor(123456789));
        }
    }
}
