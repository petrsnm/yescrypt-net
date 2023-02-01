using System.Security.Cryptography;
using System.Text;

namespace tests
{
    [TestClass]
    public class YescryptTests
    {
        [TestMethod]
        public void TestCheckPasswd()
        {
            Assert.IsTrue(Yescrypt.CheckPasswd(Encoding.ASCII.GetBytes("foo"), "$y$j9T$IYOtk1P7X7XerR2MxSBt41$ylOTRMdaL7amUytGWDGmMeCvzk3yPxMwliVqAMmeuUB"));
            Assert.IsTrue(Yescrypt.CheckPasswd(Encoding.UTF8.GetBytes("klâwen"), "$y$j9T$gdl4VsceJ0dcK65iQQZzc0$7b08F6h5QwLdzQVhJlbT1LakWThuW7MLEGrRV5S.X0C"));
        }

        [TestMethod]
        public void TestChangePasswd()
        {
            Assert.IsTrue(Yescrypt.CheckPasswd(Encoding.ASCII.GetBytes("Test12345"), "$y$j9T$CoUww2aXLQYsu1DSF5GRN.$QMv6evLbghgjmlWPbRbFrUjYMWqrZ.Xpi4ydv6S5eS4"));
            string newVal = Yescrypt.ChangePasswd(Encoding.ASCII.GetBytes("Test123456"), "$y$j9T$CoUww2aXLQYsu1DSF5GRN.$QMv6evLbghgjmlWPbRbFrUjYMWqrZ.Xpi4ydv6S5eS4");
            Assert.IsTrue(Yescrypt.CheckPasswd(Encoding.ASCII.GetBytes("Test123456"), newVal));
            Assert.IsFalse(Yescrypt.CheckPasswd(Encoding.ASCII.GetBytes("Test12345"), newVal));
        }

        [TestMethod]
        public void TestNewPasswd()
        {
            YescryptSettings settings = new YescryptSettings();            
            string newVal = Yescrypt.NewPasswd(Encoding.ASCII.GetBytes("foobar"), settings);
            Assert.IsTrue(Yescrypt.CheckPasswd(Encoding.ASCII.GetBytes("foobar"), newVal));
            Assert.IsFalse(Yescrypt.CheckPasswd(Encoding.ASCII.GetBytes("foobaz"), newVal));
        }
    }
}
