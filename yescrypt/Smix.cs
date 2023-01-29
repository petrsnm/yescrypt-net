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
            // uint32_t* Y = &XY[s];

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
                    ulong jj = Helper.Integerify(X, 0, r);
                    ulong jjj = (uint)Helper.wrap(jj, (ulong)i);

                    uint j = (uint)jjj;
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

    }
}
