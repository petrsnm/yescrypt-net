using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    [TestClass]
    public class SmixTest
    {
        [TestMethod]
        public void Smix1Test()
        {
            var pwxform = new Pwxform();

            for (int i = 0; i < pwxform.S0.Length; i++)
            {
                pwxform.S2[i] = pwxform.S1[i] = pwxform.S0[i] = (uint)(i << 16 + i);
            }

            int N = 2048;
            int r = 2;

            uint[] B = new uint[r * 32];
            for (uint i = 0; i < r * 32; i++)
            {
                B[i] = i;
            }

            uint[] V  = new uint[32 * r * N];
            uint[] XY = new uint[64 * r];

            
            uint[] expected;
            
            Smix.smix1(B, r, N, Flags.YESCRYPT_RW_DEFAULTS, V, XY, pwxform);
            expected = new uint[] { 
                0x01a573d3, 0xdb6a5b98, 0x9b2a8e2c, 0x97f833ac, 0xe03b2026, 0xb24a3c83, 0x780b11fc, 0x78f931e5,
                0x1691c32c, 0x8e79f513, 0x839dad28, 0x8520f143, 0xcb42035d, 0xd8a52649, 0x724f4714, 0x6b662c7f,
                0x07851ccc, 0xab552585, 0x840434b0, 0x6e63264b, 0x4e2bd040, 0xf40c8c37, 0xed483347, 0x8268dd74,
                0x7389838f, 0x4cf9e550, 0x8ac9cc6c, 0x3d337c49, 0x626dd950, 0x54dd8eac, 0xe7797191, 0x32cf92f7,
                0x75d3eb31, 0xd2a4f29d, 0xe3ee77b7, 0xcb4fb5ff, 0x63efc4a0, 0x3b1b6eb1, 0x96725d49, 0xc3b24d36,
                0x3bc7a174, 0x4f722438, 0x61752d71, 0xc61fce1c, 0x3c024816, 0x85f8d8e1, 0x6c67737b, 0x2605504d,
                0x9c7f3544, 0xf892289b, 0xb4a925e7, 0x2293172d, 0x5fb25d58, 0x3f8e0ec1, 0x7071e00a, 0x2eaad99d,
                0x2eeb9670, 0x187f008e, 0x2edb5806, 0xc7eaf03d, 0x80d8b62e, 0x753c847e, 0xffbc8cb5, 0x6f12d569 
            };

            CollectionAssert.AreEqual(expected, B);
        }

        [TestMethod]
        public void Smix2Test()
        {
            var pwxform = new Pwxform();

            for (int i = 0; i < pwxform.S0.Length; i++)
            {
                pwxform.S2[i] = pwxform.S1[i] = pwxform.S0[i] = (uint)(i << 16 + i);
            }

            int N = 2048;
            int r = 2;

            uint[] B = new uint[r * 32];
            for (uint i = 0; i < r * 32; i++)
            {
                B[i] = i;
            }

            uint[] V = new uint[32 * r * N];
            uint[] XY = new uint[64 * r];


            uint[] expected;

            Smix.smix2(B, r, N, N - 1, Flags.YESCRYPT_RW_DEFAULTS, V, XY, pwxform);

            expected = new uint[] {
                0x884300b3, 0xc7770f43, 0xce3a1444, 0x7d14b7e5, 0xb376bc7e, 0xfff475b9, 0xcdfb00dd, 0x424d350f,
                0xe980814d, 0xc05ebd9f, 0x0fb19fe6, 0x313d5db4, 0x41f4b42e, 0x8e2139a1, 0x6ed5cca6, 0x38753c78,
                0x3a3b7fa3, 0x1eb41893, 0xd83cf953, 0xef133ac7, 0x3c8d8475, 0x83d6d79c, 0x4ca4e165, 0xa41cc88f,
                0xed9276b4, 0xbdba61d5, 0x9f328d3b, 0x16c1a4ac, 0xd458cb2c, 0x12d99f70, 0x2ba202a4, 0xcf213108,
                0xd8bb9be8, 0x0a766574, 0x84a07ae6, 0x977765fb, 0x5a832e4a, 0x1cc9d50a, 0x9129a074, 0x27f231cb,
                0x8680e51e, 0xe6fb3490, 0xdce6c86b, 0x1ab20449, 0xd79279f4, 0x20af113e, 0xb9b495da, 0x9fc5c858,
                0x4e70e5bd, 0x095260fe, 0xa0934ac3, 0x7379c392, 0xe8b9352e, 0xfe44c18a, 0x2b4e1296, 0xaf6167e5,
                0x28fbc8c5, 0xbcb6aa05, 0x60dd9841, 0xb4bf47e3, 0x7200b836, 0xae64f996, 0xc2c28f34, 0x6c64a366
            };

            CollectionAssert.AreEqual(expected, B);
        }
    }
}
