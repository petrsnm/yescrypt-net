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
        public void Smix1WithPwxFormBlockmixerTest()
        {
            uint N = 2048;
            uint r = 2;

            uint[] B = new uint[r * 32];
            for (uint i = 0; i < r * 32; i++)
            {
                B[i] = i;
            }

            uint[] V = new uint[32 * r * N];
            uint[] XY = new uint[64 * r];

            uint[] expected;

            uint[] s0 = new uint[PwxFormBlockmixer.SboxWords];
            uint[] s1 = new uint[PwxFormBlockmixer.SboxWords];
            uint[] s2 = new uint[PwxFormBlockmixer.SboxWords];
            for (int i = 0; i < PwxFormBlockmixer.SboxWords; i++)
            {
                s0[i] = s1[i] = s2[i] = (uint)(i << 16 + i);
            }
            Blockmixer blockmixer = new PwxFormBlockmixer(s0, s1, s2);
            Smix.smix1(B, 0, r, N, YescryptFlags.YESCRYPT_RW_DEFAULTS, V, 0, XY, blockmixer);
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
        public void Smix1WithSalsa8BlockmixerTest()
        {
            uint N = 2048;
            uint r = 2;

            uint[] B = new uint[r * 32];
            for (uint i = 0; i < r * 32; i++)
            {
                B[i] = i;
            }

            uint[] V = new uint[32 * r * N];
            uint[] XY = new uint[64 * r];

            uint[] expected;

            Blockmixer blockmixer = new Salsa8Blockmixer();
            Smix.smix1(B, 0, r, N, 0, V, 0, XY, blockmixer);
            expected = new uint[] {
                0x67cfb092, 0xe0622eec, 0x625e5e75, 0x5fc07141, 0xfd1698b2, 0x13a9dadb, 0x114af52a, 0x1b88ddf3,
                0x32d7546f, 0xb0163a5a, 0x66008d49, 0x19f0e6ca, 0x5af98165, 0x6b9b95bd, 0x29179175, 0x94bd1680,
                0x21f390c2, 0xc40a143e, 0x12039a9d, 0x35a759f7, 0xb9ffe722, 0x00d62487, 0x3f442ac9, 0x682a79c1,
                0xa17d8c94, 0xd4ec7da9, 0x3cabcdf6, 0x33db981f, 0x4250c73e, 0xbb28dc78, 0xc465c9ab, 0xdbbbe27f,
                0x3385f642, 0xe46155b6, 0xfcd443fa, 0x2016ac41, 0xaf53af80, 0xee640e12, 0x5b6f002f, 0xcc6639ae,
                0x40d1d5d7, 0xfa8357fb, 0x8c4f36b9, 0x9aff0032, 0xbb28d7e5, 0x9d072e79, 0x04f1b53b, 0x3010638a,
                0xeb96673b, 0xc7ea3a83, 0x4debe51d, 0x88c86911, 0xbdbe4c86, 0x4586990d, 0x36bac4de, 0x09a65cf3,
                0x796048a1, 0x4ba3b6b7, 0x2e301450, 0x3e1df439, 0xdf0fe834, 0xdecde771, 0x7884b78a, 0xee334746
            };
            CollectionAssert.AreEqual(expected, B);
        }


        [TestMethod]
        public void Smix2WithPwxFormBlockmixerTest()
        {
            uint N = 2048;
            uint r = 2;

            uint[] B = new uint[r * 32];
            for (uint i = 0; i < r * 32; i++)
            {
                B[i] = i;
            }

            uint[] V = new uint[32 * r * N];
            uint[] XY = new uint[64 * r];
            uint[] expected;


            uint[] s0 = new uint[PwxFormBlockmixer.SboxWords];
            uint[] s1 = new uint[PwxFormBlockmixer.SboxWords];
            uint[] s2 = new uint[PwxFormBlockmixer.SboxWords];
            for (int i = 0; i < PwxFormBlockmixer.SboxWords; i++)
            {
                s0[i] = s1[i] = s2[i] = (uint)(i << 16 + i);
            }
            Blockmixer blockmixer = new PwxFormBlockmixer(s0, s1, s2);
            Smix.smix2(B, 0, r, N, N - 1, YescryptFlags.YESCRYPT_RW_DEFAULTS, V, 0, XY, blockmixer);
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

        [TestMethod]
        public void Smix2WithSalsa8BlockmixerTest()
        {
            uint N = 2048;
            uint r = 2;

            uint[] B = new uint[r * 32];
            for (uint i = 0; i < r * 32; i++)
            {
                B[i] = i;
            }

            uint[] V = new uint[32 * r * N];
            uint[] XY = new uint[64 * r];
            uint[] expected;

            Blockmixer blockmixer = new Salsa8Blockmixer();
            Smix.smix2(B, 0, r, N, N - 1, 0, V, 0, XY, blockmixer);
            expected = new uint[] {
                0x89827fad, 0x984428c0, 0x178c0337, 0x1d8eeb28, 0x4e1a2494, 0xe9ba1715, 0x582ca074, 0x0cc68726,
                0xaa9dbee2, 0x9249b7fb, 0x9fefb8c1, 0x0a72ab78, 0x53474ad1, 0x189f1dd9, 0xebe511c1, 0xe837b83a,
                0xf66ccb34, 0xcc96a2ec, 0x9e9750f5, 0x24355fe5, 0x58c3fe1b, 0x1d31c63b, 0xb70c3ac5, 0x2076e0eb,
                0xbaf14fc1, 0x8b04a291, 0xe8a6dc16, 0xbe2893fd, 0x57390ddc, 0xe1799e63, 0x3f15c8fa, 0x80a62d02,
                0xcd91fe5b, 0x97c6d527, 0x91215494, 0xe30634cf, 0x8621fe70, 0x8b327b68, 0x39941d69, 0x0f8e8937,
                0xdfb5076f, 0xfac3efba, 0x04d187cb, 0x7b459e5c, 0xb619cbfa, 0x2aa5a4ba, 0xaad273fd, 0x28ece31e,
                0x7d2c3a09, 0x9f842c47, 0xf69ac42a, 0x9d5c9ee3, 0x65f7ec71, 0xa22ac28b, 0xc89bd57e, 0xc7bff804,
                0xf7841422, 0xb3ab8ace, 0xd4364a68, 0x11b64036, 0x880b957b, 0xb9b4eb7c, 0x3ba153b1, 0xf1234815
            };
            CollectionAssert.AreEqual(expected, B);
        }

        [TestMethod]
        public void TheSmixTest()
        {        
            uint N = 2048;
            uint r = 2;

            uint[] B = new uint[r * 32];
            for (uint i = 0; i < r * 32; i++)
            {
                B[i] = i;
            }

            uint[] V = new uint[32 * r * N];           
            uint[] expected;
            byte[] expectedPasswd;


            byte[] passwd = new byte[32]; 
            Smix.Mix(B, 0, r, N, YescryptFlags.YESCRYPT_RW, V,ref passwd);
            expected = new uint[] { 
                0x26c60121, 0x3f14a162, 0x5e0602b9, 0x6e16f2c6, 0x395ff920, 0x920006d3, 0xba4ecd91, 0x82f215ee, 
                0xf3652648, 0x424bd230, 0xcff4eabf, 0xaa71508d, 0x7060a382, 0x71ae14e2, 0x05ccbdd5, 0x67eeae47, 
                0xf383e126, 0x479b0151, 0x281800f0, 0xc3bf08a0, 0xbb7ad036, 0xf49346b8, 0xfd25513d, 0x1ce00d22, 
                0x4d43a467, 0x4ad33f2b, 0x5e46ab5b, 0x1b4899d3, 0x9365758d, 0xa7534438, 0xf7455cb7, 0xbae33e1b, 
                0x19802a53, 0x66c8b5e7, 0x1d654c88, 0x3e824472, 0xa00dec4b, 0xb55de59d, 0x0813ad75, 0xf0d63560, 
                0x7a3ae851, 0x1d5cd1c6, 0x15be99b3, 0x0b8fa163, 0x78df9ab1, 0x6e97dcb2, 0x2091da6a, 0xdf3f30b2, 
                0x550b26fd, 0x6d293860, 0x0b2dc4ae, 0xe66083a6, 0xa56cc3a0, 0x094a10cc, 0xeb9d837f, 0xf8861754,
                0x734859b5, 0x6fcf7365, 0xdd91349b, 0xc3f82c98, 0xc5082540, 0xceaefc0b, 0xf1bd0d70, 0x7c6d703e
            };
            CollectionAssert.AreEqual(expected, B);

            expectedPasswd = new byte[] { 
                0x7d, 0x84, 0x5c, 0xbd, 0x93, 0x31, 0x6c, 0xa3, 0xef, 0xf7, 0x0a, 0xfb, 0x77, 0xac, 0xe9, 0xae, 
                0x02, 0x80, 0x1e, 0xa4, 0xa5, 0x56, 0xcc, 0x9e, 0xbe, 0x01, 0xf9, 0x19, 0xb5, 0x13, 0x6a, 0xec
            };
            CollectionAssert.AreEqual(expectedPasswd, passwd);
        }
    }
}
