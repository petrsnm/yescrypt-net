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
            Assert.IsTrue(Yescrypt.CheckPasswd("Test12345", "$y$j9T$hMDx1X7.Y3T.Q9EAwbRjz/$qZtAjQMr0plVTDQL/cL128rWowQjJmhicme07W7Onb/"));

        }
           
    }
}
