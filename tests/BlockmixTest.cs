using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    [TestClass]
    public class BlockmixTest
    {
        [TestMethod]
        public void Salsa8BlockmixerTest()
        {
            uint r = 1;
            
            uint [] B = new uint[r * 32];
            for (int i = 0; i < r * 32; i++)
            {
                B[i] = (uint)i;                
            }

            Blockmixer blockmixer = new Salsa8Blockmixer();
            blockmixer.Blockmix(B, r);
            uint[] expected = new uint[] {
                0xdb987384, 0xdb987384, 0xdb987384, 0xdb987384, 0xa98124f2, 0xa98124f2, 0xa98124f2, 0xa98124f2,
                0xe835b110, 0xe835b110, 0xe835b110, 0xe835b110, 0x6e6df192, 0x6e6df192, 0x6e6df192, 0x6e6df192,
                0xeb59357c, 0x3141a7bc, 0x2947f33b, 0x2ec2b541, 0x7b2543ee, 0x84737c8f, 0x565696ab, 0xe0dc84dd,
                0xc7b6564a, 0x91759613, 0xd195d1e1, 0x9e0b9a0d, 0xcd16cc92, 0x77814811, 0xe585fe9e, 0x7b62222e 
            };
            CollectionAssert.AreEqual(expected, B);
        }

        [TestMethod]
        public void PwxFormBlockmixerTest()
        {

            uint[] s0 = new uint[PwxFormBlockmixer.SboxWords];
            uint[] s1 = new uint[PwxFormBlockmixer.SboxWords];
            uint[] s2 = new uint[PwxFormBlockmixer.SboxWords];

            for (int i = 0; i < PwxFormBlockmixer.SboxWords; i++)
            {
                s0[i] = s1[i] = s2[i] = (uint)(i << 16 + i);
            }
            Blockmixer blockmixer = new PwxFormBlockmixer(s0, s1, s2);

            uint r = 2;
            uint[] B = new uint[r * 32];
            for (uint i = 0; i < r * 32; i++)
            {
                B[i] = i;
            }

            uint[] expected;

            blockmixer.Blockmix(B, r);
            expected = new uint[] {
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000,
                0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000,
                0x89000000, 0x13211550, 0x3e600000, 0x17b790fd, 0x07801b40, 0x58d86ef1, 0x79006d80, 0x4968ef4f,
                0xc59d0b60, 0x4f6db7a3, 0xfe879600, 0xc680468d, 0x17eaba90, 0x150b3529, 0x612a18c8, 0x39cd038f,
                0x0ac19b80, 0x06a0d504, 0x221df800, 0x64bcb438, 0x397c7000, 0x3c46b012, 0xe6724cc0, 0x10ba1638,
                0xcc32e880, 0xceeb948d, 0x09b03188, 0x4c9b5e0c, 0x221a0000, 0x3947b4cc, 0xccf1d800, 0x08895f75,
                0x0e804cee, 0x5966e76a, 0x8130cc96, 0xef34e3a0, 0x94f94856, 0x554db773, 0x4f556775, 0x59c20bc5,
                0x29521e95, 0x6415389a, 0xe1d435e9, 0xcc5ba45f, 0xffef9c93, 0xcd07d356, 0x86fcd6e9, 0x6f4ae9ea
            };
            CollectionAssert.AreEqual(expected, B);

            blockmixer.Blockmix(B, r);
            expected = new uint[] {
                0x417a3878, 0xa7d33726, 0xcaab3280, 0xebd87335, 0x6841b800, 0xd60e8a5f, 0xf7dddf3c, 0x410e0bc4,
                0x85ecefb0, 0xe150d03f, 0x38f1363d, 0xc6039c6a, 0x407df440, 0x0de9d40d, 0xb91a2840, 0x00caad09,
                0x7c0aa020, 0x26cf6ced, 0xb94055c0, 0x0dfa127a, 0x86be8140, 0x0aabc6b7, 0xa0cf7400, 0x2be2d285,
                0xe4007170, 0x3b3930e8, 0xd0d21ac0, 0xedaa693c, 0xed8cac00, 0x03f326ae, 0x7c8313c0, 0x0c0b9c8c,
                0x7327cb40, 0x40156831, 0xd5e06410, 0x768edc1c, 0x1ad3acd0, 0x1fe8d9cf, 0x7356c9f0, 0x97f78943,
                0xc4444420, 0x12576230, 0xfa74b2e0, 0x38633898, 0xa7f785f0, 0xbdc5c430, 0x95ce32ed, 0xd59548af,
                0x9f6fc4ca, 0x9f87bf2c, 0x37fc8084, 0x340afcbe, 0x858f4f80, 0x0a374c0f, 0x116875e4, 0x922ad721,
                0x0f6c4bfd, 0xe88ab428, 0x2a95317f, 0xf26de4c5, 0xa3fe2dc4, 0xeaa7414f, 0x076e9996, 0xb2cb41c4
            };
            CollectionAssert.AreEqual(expected, B);

            blockmixer.Blockmix(B, r);
            expected = new uint[] {
                0x2266e840, 0xac308a60, 0xd3ba65e0, 0x147f5bcc, 0xfb4c7380, 0x35ee7f76, 0xd404dcfc, 0xacbce52c,
                0xebb5aa56, 0xb68ab445, 0x65e6677c, 0xbb1d9f0b, 0x81d55900, 0xd9681ffc, 0x9b856cd0, 0x1a84d4b2,
                0xbd9241c2, 0x24bd722f, 0x3382bbb8, 0x87927005, 0xb3df7310, 0x06d14904, 0x27eea588, 0x1762900e,
                0x29861910, 0xeae4ca81, 0xa463af00, 0xb101f887, 0x99884ed8, 0x471c2f33, 0xa15e8240, 0x04b97a49,
                0xf698b690, 0x108982ba, 0x2ff13c48, 0x54b2b010, 0x79dc8976, 0xb8bad264, 0x0a2414f6, 0x433d5c49,
                0x81f3e390, 0x0b3345a2, 0xfd1afd7a, 0x2f542729, 0xc4e18ed0, 0x0cad07c2, 0x35de8820, 0x205f0a6a,
                0x4f583634, 0x1972d62f, 0x2a11e13b, 0x6321c29c, 0x07ba431d, 0xf4938d6d, 0xd2eff13f, 0x395a1fff,
                0xb4ba3bd1, 0xb7b3b358, 0xb81d9530, 0xe3824592, 0xc9e22f38, 0xa0d1582e, 0xd1a0bd30, 0xc05d36df
            };
            CollectionAssert.AreEqual(expected, B);

        }
    }
}
