using Fasterlimit.Yescrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fasterlimit.Yescrypt
{
    internal class Smix
    {
        /**
         * smix1(B, r, N, flags, V, NROM, VROM, XY, ctx):
         * Compute first loop of B = SMix_r(B, N).  The input B must be 128r bytes in
         * length; the temporary storage V must be 128rN bytes in length; the temporary
         * storage XY must be 256r bytes in length.
         */
        public static void smix1(uint[] B, int r, int N, uint flags, uint []V, uint []XY, Pwxform ctx)
        {
            int s = 32 * r;
            uint[]X = XY;
            
	        /* 1: X <-- B */
	        for (int k = 0; k< 2 * r; k++)
		        for (int i = 0; i< 16; i++)
			        X[k * 16 + i] = Endian.le32dec(B, k * 16 + (i * 5 % 16));

            /* 2: for i = 0 to N - 1 do */
            for (int i = 0; i < N; i++)
            {
                /* 3: V_i <-- X */
                Helper.BlockCopy(V, i * s, X, 0, s);

                if ((flags & Flags.YESCRYPT_RW) != 0 && i > 1)
                {
                    /* j <-- Wrap(Integerify(X), i) */
                    uint j = (uint)Helper.wrap(Helper.Integerify(X, 0, r), (ulong)i);
                    /* X <-- X xor V_j */
                    Helper.BlockXor(X, 0, V, (int)(j * s), s);
                }
                /* 4: X <-- H(X) */
                ctx.Blockmix(X, r);                
            }

            /* B' <-- X */
            for (int k = 0; k < 2 * r; k++)
            {
                for (int i = 0; i < 16; i++)
                {
                    Endian.le32enc(B, k * 16 + (i * 5 % 16), X[k * 16 + i]);
                }
            }
        }

        /**
         * smix2(B, r, N, Nloop, flags, V, NROM, VROM, XY, ctx):
         * Compute second loop of B = SMix_r(B, N).  The input B must be 128r bytes in
         * length; the temporary storage V must be 128rN bytes in length; the temporary
         * storage XY must be 256r bytes in length.  The value N must be a power of 2
         * greater than 1.
         */
        public static void smix2(uint[] B, int r, int N, int Nloop, uint flags, uint[] V, uint[] XY, Pwxform ctx)
        {
            int s = 32 * r;
            uint[] X = XY;

            /* X <-- B */
            for (int k = 0; k < 2 * r; k++)
                for (int i = 0; i < 16; i++)
                    X[k * 16 + i] = Endian.le32dec(B, k * 16 + (i * 5 % 16));

            /* 6: for i = 0 to N - 1 do */
            for (int i = 0; i < Nloop; i++)
            {
                /* 7: j <-- Integerify(X) mod N */
                uint j = (uint) (Helper.Integerify(X, 0, r) & (ulong)(N - 1));

                /* 8.1: X <-- X xor V_j */
                Helper.BlockXor(X, 0, V, (int)j * s, s);

                /* V_j <-- X */
                Helper.BlockCopy(V, (int)j * s, X, 0, s);

                ctx.Blockmix(X, r);
            }

            /* 10: B' <-- X */
            for (int k = 0; k < 2 * r; k++)
            {
                for (int i = 0; i < 16; i++)
                {
                    Endian.le32enc(B, k * 16 + (i * 5 % 16), X[k * 16 + i]);
                }
            }
        }
    }
}
