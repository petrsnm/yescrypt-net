using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fasterlimit.Yescrypt
{

    internal class Pwxform
    {
        const int PWXsimple = 2;
        const int PWXgather = 4;
        const int PWXrounds = 6;
        const int Swidth = 8;

        public uint Smask
        {
            get
            {
                return ((1 << Swidth) - 1) * PWXsimple * 8;
            }
        }

        public int PWXbytes
        {
            get
            {
                return PWXgather * PWXsimple * 8;
            }
        }

        public int PWXwords
        {
            get
            {
                return PWXbytes / 4;
            }
        }

        public uint[] S0
        {
            get; set;
        }

        public uint[] S1
        {
            get; set;
        }

        public uint[] S2
        {
            get; set;
        }

        public uint W
        {
            get; set;
        }

        public Pwxform()
        {
            int sboxUints = (1 << Swidth) * PWXsimple * 2;
            S0 = new uint[sboxUints];
            S1 = new uint[sboxUints];
            S2 = new uint[sboxUints];
            W = 0;
        }

        public void Transform(uint[] B)
        {
            uint[] S0 = this.S0;
            uint[] S1 = this.S1;
            uint[] S2 = this.S2;
            uint w = W;

            /* 1: for i = 0 to PWXrounds - 1 do */
            for (int i = 0; i < PWXrounds; i++)
            {
                /* 2: for j = 0 to PWXgather - 1 do */
                for (int j = 0; j < PWXgather; j++)
                {
                    uint xl = B[j * PWXsimple * 2];
                    uint xh = B[j * PWXsimple * 2 + 1];

                    /* 3: p0 <-- (lo(B_{j,0}) & Smask) / (PWXsimple * 8) */
                    uint p0 = (xl & Smask) / 4;
                    /* 4: p1 <-- (hi(B_{j,0}) & Smask) / (PWXsimple * 8) */
                    uint p1 = (xh & Smask) / 4;

                    /* 5: for k = 0 to PWXsimple - 1 do */
                    for (int k = 0; k < PWXsimple; k++)
                    {
                        ulong x, s0, s1;
                        
                        /* 6: B_{j,k} <-- (hi(B_{j,k}) * lo(B_{j,k}) + S0_{p0,k}) xor S1_{p1,k} */                     
                        s0 = ((ulong)(S0[p0 + k * 2 + 1]) << 32) + S0[p0 + k * 2];
                        s1 = ((ulong)(S1[p1 + k * 2 + 1]) << 32) + S1[p1 + k * 2];

                        xl = B[j * PWXsimple * 2 + k * 2];
                        xh = B[j * PWXsimple * 2 + k * 2 + 1];

                        x = (ulong)xh * xl;
                        x += s0;
                        x ^= s1;

                        B[j * PWXsimple * 2 + k * 2] = (uint)x;
                        B[j * PWXsimple * 2 + k * 2 + 1] = (uint)(x >> 32);

                        /* 8: if (i != 0) and (i != PWXrounds - 1) */
                        if (i != 0 && i != PWXrounds - 1)
                        {
                            /* 9: S2_w <-- B_j */
                            S2[w * 2] = (uint)x;
                            S2[w * 2 + 1] = (uint)(x >> 32);
                            /* 10: w <-- w + 1 */
                            w++;
                        }
                    }
                }
            }

            /* 14: (S0, S1, S2) <-- (S2, S0, S1) */
            this.S0 = S2;
            this.S1 = S0;
            this.S2 = S1;

            /* 15: w <-- w mod 2^Swidth */
            W = w & ((1 << Swidth) * PWXsimple - 1);
        }



        public void Blockmix(uint[] B, int r)
        {
            uint[] X = new uint[PWXwords];
            int r1, i;

            /* Convert 128-byte blocks to PWXbytes blocks */
            /* 1: r_1 <-- 128r / PWXbytes */
            r1 = 128 * r / PWXbytes;

            /* 2: X <-- B'_{r_1 - 1} */
            Helper.BlockCopy(X, 0, B, (r1 - 1) * PWXwords, PWXwords);

            /* 3: for i = 0 to r_1 - 1 do */
            for (i = 0; i < r1; i++)
            {
                /* 4: if r_1 > 1 */
                if (r1 > 1)
                {
                    /* 5: X <-- X xor B'_i */
                    Helper.BlockXor(X, 0, B, i * PWXwords, PWXwords);
                }

                /* 7: X <-- pwxform(X) */
                Transform(X);

                /* 8: B'_i <-- X */
                Helper.BlockCopy(B, i * PWXwords, X, 0, PWXwords);
            }

            /* 10: i <-- floor((r_1 - 1) * PWXbytes / 64) */
            i = (r1 - 1) * PWXbytes / 64;

            /* 11: B_i <-- H(B_i) */
            Salsa.Salsa20(B, i * 16, 2);

	        /* 12: for i = i + 1 to 2r - 1 do */
	        for (i++; i < 2 * r; i++) 
            {
                /* 13: B_i <-- H(B_i xor B_{i-1}) */
                Helper.BlockXor(B, i * 16, B, (i - 1) * 16, 16);
                Salsa.Salsa20(B, i * 16, 2);
	        }
        }
    }
}
