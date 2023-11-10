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
            Assert.IsFalse(Yescrypt.CheckPasswd(Encoding.ASCII.GetBytes("foobar"), "$y$j9T$IYOtk1P7X7XerR2MxSBt41$ylOTRMdaL7amUytGWDGmMeCvzk3yPxMwliVqAMmeuUB"));
            Assert.IsTrue(Yescrypt.CheckPasswd(Encoding.UTF8.GetBytes("klâwen"), "$y$j9T$gdl4VsceJ0dcK65iQQZzc0$7b08F6h5QwLdzQVhJlbT1LakWThuW7MLEGrRV5S.X0C"));
            
            // Test different salt lengths (23-25 characters before encoding)
            Assert.IsTrue(Yescrypt.CheckPasswd(Encoding.UTF8.GetBytes("foobar"), "$y$j9T$iemju3R6WTIkO45Q5cNJB1$OMiCs4T6oPqi8sUjuhtLnlMXPbbfDwUEpK8KlifZQO9"));
            Assert.IsTrue(Yescrypt.CheckPasswd(Encoding.UTF8.GetBytes("test123"), "$y$j9T$Uld6DeBZ9Yn3FhvyAdwEGiam$/SgUfzYuKXiMVUDyvVJS7kRiwfCHcF6juRglqHR00Y7"));
            Assert.IsTrue(Yescrypt.CheckPasswd(Encoding.UTF8.GetBytes("foobar"), "$y$j9T$pWg/Dy73M0YFPg7hllxIE.$Z.heZ9FROBoqVhzRYBkbQ/hhDwEvIK4hByhpHwpjeM5"));
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void TestThrowOnWORM()
        {
            Yescrypt.CheckPasswd(Encoding.UTF8.GetBytes("pleaseletmein"), "$y$/A2$LdJMENpBABJJ3hIHj/$5IEld1eWdmh5lylrqHLF5dvA3ISpimEM9J1Dd05n/.3");
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
