using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fasterlimit.Yescrypt
{
    internal class Salsa
    {
        private static uint R(uint a, int b)
        {
            return (((a) << (b)) | ((a) >> (32 - (b))));
        }

        /**
         * Apply the Salsa20 core to the provided block.
        */
        public static void Salsa20(uint[] B, uint rounds)
        {
            uint[] x = new uint[16];

            /// SIMD unshuffle
            for (int i = 0; i < 16; i++)
            {
                x[i * 5 % 16] = B[i];
            }

            for (int i = 0; i < rounds; i += 2)
            {
                /* Operate on columns */
                x[4] ^= R(x[0] + x[12], 7); x[8] ^= R(x[4] + x[0], 9);
                x[12] ^= R(x[8] + x[4], 13); x[0] ^= R(x[12] + x[8], 18);

                x[9] ^= R(x[5] + x[1], 7); x[13] ^= R(x[9] + x[5], 9);
                x[1] ^= R(x[13] + x[9], 13); x[5] ^= R(x[1] + x[13], 18);

                x[14] ^= R(x[10] + x[6], 7); x[2] ^= R(x[14] + x[10], 9);
                x[6] ^= R(x[2] + x[14], 13); x[10] ^= R(x[6] + x[2], 18);

                x[3] ^= R(x[15] + x[11], 7); x[7] ^= R(x[3] + x[15], 9);
                x[11] ^= R(x[7] + x[3], 13); x[15] ^= R(x[11] + x[7], 18);

                /* Operate on rows */
                x[1] ^= R(x[0] + x[3], 7); x[2] ^= R(x[1] + x[0], 9);
                x[3] ^= R(x[2] + x[1], 13); x[0] ^= R(x[3] + x[2], 18);

                x[6] ^= R(x[5] + x[4], 7); x[7] ^= R(x[6] + x[5], 9);
                x[4] ^= R(x[7] + x[6], 13); x[5] ^= R(x[4] + x[7], 18);

                x[11] ^= R(x[10] + x[9], 7); x[8] ^= R(x[11] + x[10], 9);
                x[9] ^= R(x[8] + x[11], 13); x[10] ^= R(x[9] + x[8], 18);

                x[12] ^= R(x[15] + x[14], 7); x[13] ^= R(x[12] + x[15], 9);
                x[14] ^= R(x[13] + x[12], 13); x[15] ^= R(x[14] + x[13], 18);
            }

            // SIMD shuffle 
            for (int i = 0; i < 16; i++)
            {
                B[i] += x[i * 5 % 16];
            }
        }        

        public static void BlockmixSalsa8(uint[] B, uint[] Y, int r)
        {
            uint[] X = new uint[16];

            /* 1: X <-- B_{2r - 1} */
            Helpers.BlockCopy(X, 0, B, (2 * r - 1) * 16, 16);

            /* 2: for i = 0 to 2r - 1 do */
            for (int i = 0; i < 2 * r; i++)
            {
                /* 3: X <-- H(X xor B_i) */
                Helpers.BlockXor(X, B, i * 16, 16);
                Salsa20(X, 8);

                /* 4: Y_i <-- X */
                Helpers.BlockCopy(Y, i * 16, X, 0, 16);
            }

            /* 6: B' <-- (Y_0, Y_2 ... Y_{2r-2}, Y_1, Y_3 ... Y_{2r-1}) */
            for (int i = 0; i < r; i++)
            {
                Helpers.BlockCopy(B, i * 16, Y, (i * 2) * 16, 16);
            }

            for (int i = 0; i < r; i++)
            {
                Helpers.BlockCopy(B, (i + r) * 16, Y, (i * 2 + 1) * 16, 16);
            }
        }
    }
}
