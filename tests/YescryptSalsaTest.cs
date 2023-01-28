using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    [TestClass]
    public class YescryptSalsaTest
    {
        [TestMethod]
        public void BlockmixSalsa8Test()
        {
            int r = 1;
            uint [] B = new uint[r * 32];
            uint [] Y = new uint[r * 32];
            
            for (int i = 0; i < r * 32; i++)
            {
                B[i] = (uint)i;
                Y[i] = 0;
            }

            YescryptSalsa.BlockmixSalsa8(B, Y, r);

            uint[] expected = new uint[] {
                0xdb987384,
                0xdb987384,
                0xdb987384,
                0xdb987384,
                0xa98124f2,
                0xa98124f2,
                0xa98124f2,
                0xa98124f2,
                0xe835b110,
                0xe835b110,
                0xe835b110,
                0xe835b110,
                0x6e6df192,
                0x6e6df192,
                0x6e6df192,
                0x6e6df192,
                0xeb59357c,
                0x3141a7bc,
                0x2947f33b,
                0x2ec2b541,
                0x7b2543ee,
                0x84737c8f,
                0x565696ab,
                0xe0dc84dd,
                0xc7b6564a,
                0x91759613,
                0xd195d1e1,
                0x9e0b9a0d,
                0xcd16cc92,
                0x77814811,
                0xe585fe9e,
                0x7b62222e };

            CollectionAssert.AreEqual(expected, B);
        }
    }
}
