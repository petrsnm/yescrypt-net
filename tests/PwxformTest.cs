using Fasterlimit.Yescrypt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    [TestClass]
    public class PwxformTest
    {
        [TestMethod]
        public void TestTransform()
        {
            var pwxform = new Pwxform();

            for (int i = 0; i < pwxform.S0.Length; i++)
            {
                pwxform.S2[i] = pwxform.S1[i] = pwxform.S0[i] = (uint)(i << 16 + i);
            }

            int r = 1;
            uint[] B = new uint[r * 32];
            for (uint i = 0; i < r * 32; i++)
            {
                B[i] = i;
            }

            uint[] expected;

            pwxform.Transform(B);
            expected = new uint[] {
                0x00000000,0x00000000,0x00000000,0x00000000,0x02000000,0x04000000,0x08000000,
                0x10000000,0x3e400000,0x3a266794,0x69197000,0xe7650a11,0x02000000,0x04000000,
                0x08000000,0x10000000,0x00000010,0x00000011,0x00000012,0x00000013,0x00000014,
                0x00000015,0x00000016,0x00000017,0x00000018,0x00000019,0x0000001a,0x0000001b,
                0x0000001c,0x0000001d,0x0000001e,0x0000001f
            };
            CollectionAssert.AreEqual(expected, B);

            pwxform.Transform(B);
            expected = new uint[] {
                0x00000000,0x00020000,0xbfb00000,0x001a751a,0x00000000,0x00020000,0x40900000,
                0x0017538f,0xf2f022c0,0x8afd4af8,0x36b5bcc0,0x792925d7,0x00000000,0x00020000,
                0x40900000,0x0017538f,0x00000010,0x00000011,0x00000012,0x00000013,0x00000014,
                0x00000015,0x00000016,0x00000017,0x00000018,0x00000019,0x0000001a,0x0000001b,
                0x0000001c,0x0000001d,0x0000001e,0x0000001f
            };
            CollectionAssert.AreEqual(expected, B);

            pwxform.Transform(B);
            expected = new uint[] {
                0x00000000,0x00020000,0x79e00000,0x0034c3e0,0x00000000,0x00020000,0xa7a80000,
                0x00292842,0x677f85e0,0x3fc47fed,0xa735af40,0xae17057d,0x00000000,0x00020000,
                0xa7a80000,0x00292842,0x00000010,0x00000011,0x00000012,0x00000013,0x00000014,
                0x00000015,0x00000016,0x00000017,0x00000018,0x00000019,0x0000001a,0x0000001b,
                0x0000001c,0x0000001d,0x0000001e,0x0000001f
            };
            CollectionAssert.AreEqual(expected, B);
        }

        [TestMethod]
        public void TestBlockmix()
        {
            var pwxform = new Pwxform();

            for (int i = 0; i < pwxform.S0.Length; i++)
            {
                pwxform.S2[i] = pwxform.S1[i] = pwxform.S0[i] = (uint)(i << 16 + i);
            }

            int r = 2;
            uint[] B = new uint[r * 32];
            for (uint i = 0; i < r * 32; i++)
            {
                B[i] = i;
            }

            uint[] expected;

            pwxform.Blockmix(B, r);
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

            pwxform.Blockmix(B, r);
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

            pwxform.Blockmix(B, r);
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
