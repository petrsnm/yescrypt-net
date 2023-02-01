namespace Fasterlimit.Yescrypt
{
    internal interface Blockmixer
    {
        public void Blockmix(uint[] B, uint r);
    }

    internal class Salsa8Blockmixer : Blockmixer
    {
        public Salsa8Blockmixer()
        {
        }

        public void Blockmix(uint[] B, uint r)
        {
            uint[] X = new uint[16];
            uint[] Y = new uint[32 * r];

            /* 1: X <-- B_{2r - 1} */
            Helper.BlockCopy(X, 0, B, (2 * r - 1) * 16, 16);

            /* 2: for i = 0 to 2r - 1 do */
            for (uint i = 0; i < 2 * r; i++)
            {
                /* 3: X <-- H(X xor B_i) */
                Helper.BlockXor(X, 0, B, i * 16, 16);
                Salsa.Salsa20(X, 0, 8);

                /* 4: Y_i <-- X */
                Helper.BlockCopy(Y, i * 16, X, 0, 16);
            }

            /* 6: B' <-- (Y_0, Y_2 ... Y_{2r-2}, Y_1, Y_3 ... Y_{2r-1}) */
            for (uint i = 0; i < r; i++)
            {
                Helper.BlockCopy(B, i * 16, Y, (i * 2) * 16, 16);
            }

            for (uint i = 0; i < r; i++)
            {
                Helper.BlockCopy(B, (i + r) * 16, Y, (i * 2 + 1) * 16, 16);
            }
        }
    }

    internal class PwxFormBlockmixer : Blockmixer
    {
        const int PWXsimple = 2;
        const int PWXgather = 4;
        const int PWXrounds = 6;
        const int Swidth = 8;

        public static uint Smask
        {
            get
            {
                return ((1 << Swidth) - 1) * PWXsimple * 8;
            }
        }

        public static uint PWXbytes
        {
            get
            {
                return PWXgather * PWXsimple * 8;
            }
        }

        public static uint PWXwords
        {
            get
            {
                return PWXbytes / 4;
            }
        }

        public static uint SboxWords
        {
            get
            {
                return (1 << Swidth) * PWXsimple * 2;
            }
        }

        uint[] S0;
        uint[] S1;
        uint[] S2;
        uint W;
        

        public PwxFormBlockmixer()
        {
            S0 = new uint[SboxWords];
            S1 = new uint[SboxWords];
            S2 = new uint[SboxWords];
            W = 0;
        }
        public PwxFormBlockmixer(uint[] s0, uint[] s1, uint[] s2)
        {
           if(s0.Length != SboxWords ||
              s1.Length != SboxWords ||
              s2.Length != SboxWords )
            {
                throw new ArgumentException("Individual sbox arrays must have length: " + SboxWords);
            }

            S0 = s0;
            S1 = s1;
            S2 = s2;
            W = 0;
        }

        public PwxFormBlockmixer(uint[] s) : this() 
        {
            if (s.Length != SboxWords * 3)
            {
                throw new ArgumentException("Sbox array have length: " + SboxWords);
            }

            uint i = 0;
            Array.Copy(s, i, S2, 0, SboxWords);
            i += SboxWords;
            Array.Copy(s, i, S1, 0, SboxWords);
            i += SboxWords;
            Array.Copy(s, i, S0, 0, SboxWords);
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

        public void Blockmix(uint[] B, uint r)
        {
            uint[] X = new uint[PWXwords];
            uint r1, i;

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
            Salsa.Salsa20(B, i * 16u, 2);

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
