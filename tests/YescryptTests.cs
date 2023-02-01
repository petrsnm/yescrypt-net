using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    [TestClass]
    public class YescryptTests
    {
        [TestMethod]
        public void TestCheckPasswd()
        {
            Assert.IsTrue(Yescrypt.CheckPasswd(Encoding.ASCII.GetBytes("Test12345"), "$y$j9T$CoUww2aXLQYsu1DSF5GRN.$QMv6evLbghgjmlWPbRbFrUjYMWqrZ.Xpi4ydv6S5eS4"));
        }

        [TestMethod]
        public void TestChangePasswd()
        {
            string newVal = Yescrypt.ChangePasswd(Encoding.ASCII.GetBytes("Test123456"), "$y$j9T$CoUww2aXLQYsu1DSF5GRN.$QMv6evLbghgjmlWPbRbFrUjYMWqrZ.Xpi4ydv6S5eS4");
            Assert.IsTrue(Yescrypt.CheckPasswd(Encoding.ASCII.GetBytes("Test123456"), newVal));
        }       
    }
}
