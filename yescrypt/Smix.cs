using Fasterlimit.Yescrypt;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

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
        public static void smix1(uint[] B, uint bIndex, uint r, uint N, uint flags, uint []V, uint vIndex, uint []XY, Blockmixer blockmixer)
        {
            uint s = 32 * r;
            uint[]X = XY;

            /* 1: X <-- B */
            for (int k = 0; k < 2 * r; k++)
            {
                for (int i = 0; i < 16; i++)
                {
                    X[k * 16 + i] = B[k * 16 + (i * 5 % 16) + bIndex];
                }
            }                    

            /* 2: for i = 0 to N - 1 do */
            for (uint i = 0; i < N; i++)
            {
                /* 3: V_i <-- X */
                Helper.BlockCopy(V, (i * s) + vIndex, X, 0, s);

                if ((flags & YescryptFlags.YESCRYPT_RW) != 0 && i > 1)
                {
                    /* j <-- Wrap(Integerify(X), i) */
                    uint j = Helper.wrap((uint)Helper.Integerify(X, 0, r), i);
                    /* X <-- X xor V_j */
                    Helper.BlockXor(X, 0, V, (j * s) + vIndex, s);
                }

                /* 4: X <-- H(X) */
                blockmixer.Blockmix(X, r);                
            }

            /* B' <-- X */
            for (int k = 0; k < 2 * r; k++)
            {
                for (int i = 0; i < 16; i++)
                {
                    B[k * 16 + (i * 5 % 16) + bIndex] = X[k * 16 + i];
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
        public static void smix2(uint[] B, uint bIndex, uint r, uint N, uint Nloop, uint flags, uint[] V, uint vIndex, uint[] XY, Blockmixer blockmixer)
        {
            uint s = 32 * r;
            uint[] X = XY;

            /* X <-- B */
            for (int k = 0; k < 2 * r; k++)
            {
                for (int i = 0; i < 16; i++)
                {
                    X[k * 16 + i] = B[k * 16 + (i * 5 % 16) + bIndex];
                }
            }

            /* 6: for i = 0 to N - 1 do */
            for (int i = 0; i < Nloop; i++)
            {
                /* 7: j <-- Integerify(X) mod N */
                uint j = (uint) (Helper.Integerify(X, 0, r) & (ulong)(N - 1));

                /* 8.1: X <-- X xor V_j */
                Helper.BlockXor(X, 0, V, (j * s) + vIndex, s);

                /* V_j <-- X */
                if ((flags & YescryptFlags.YESCRYPT_RW) != 0)
                {
                    Helper.BlockCopy(V, (j * s) + vIndex, X, 0, s);
                }

                blockmixer.Blockmix(X, r);
            }

            /* 10: B' <-- X */
            for (int k = 0; k < 2 * r; k++)
            {
                for (int i = 0; i < 16; i++)
                {
                    B[k * 16 + (i * 5 % 16) + bIndex] = X[k * 16 + i];
                }
            }
        }

        /**
         * smix(B, r, N, p, t, flags, V, NROM, VROM, XY, ctx, passwd):
         * Compute B = SMix_r(B, N).  The input B must be 128rp bytes in length; the
         * temporary storage V must be 128rN bytes in length; the temporary storage
         * XY must be 256r bytes in length.  The value N must be a power of 2 greater
         * than 1.
         */
        public static void Mix(uint[] B, uint bIndex, uint r, uint N, uint flags, uint[] V, ref byte[] passwd)
        {
            // We only support p=1 and t=0;
            uint p = 1;
            uint t = 0;            

            uint[] XY = new uint[64 * r];

            /* 1: n <-- N / p */
            uint Nchunk = N / p;

            /* 2: Nloop_all <-- fNloop(n, t, flags) */
            uint Nloop_all = Nchunk;
            if (t <= 1)
            {
                if (t != 0)
                {
                    Nloop_all *= 2; /* 2/3 */
                }
                Nloop_all = (Nloop_all + 2) / 3; /* 1/3, round up */
            }
            else
            {
                Nloop_all *= t - 1;
            }

            /* 4: Nloop_rw <-- Nloop_all / p */
            uint Nloop_rw = Nloop_all / p;

            /* 8: n <-- n - (n mod 2) */
            Nchunk &= ~1u; /* round down to even */
            /* 9: Nloop_all <-- Nloop_all + (Nloop_all mod 2) */
            Nloop_all++; Nloop_all &= ~1u; /* round up to even */
            /* 10: Nloop_rw <-- Nloop_rw + (Nloop_rw mod 2) */
            Nloop_rw++; Nloop_rw &= ~1u; /* round up to even */

            /* 11: for i = 0 to p - 1 do */
            /* 12: u <-- in */
            uint s = 32u * r;
            Blockmixer[] mixers = new Blockmixer[p];
            for (uint i = 0, Vchunk = 0; i < p; i++, Vchunk += Nchunk)
            {
                /* 13: if i = p - 1 */
                /* 14:   n <-- N - u */
                /* 15: end if */
                /* 16: v <-- u + n - 1 */
                uint Np = (i < p - 1) ? Nchunk : (N - Vchunk);
                uint Bp = (i * s) + bIndex;
                uint Vp = Vchunk * s;

                /* 17: if YESCRYPT_RW flag is set */
                if ((flags & YescryptFlags.YESCRYPT_RW) != 0)
                {
                    uint[] S = new uint[PwxFormBlockmixer.SboxWords * 3];
                    /* 18: SMix1_1(B_i, Sbytes / 128, S_i, no flags) */
                    smix1(B, Bp, 1, (PwxFormBlockmixer.SboxWords * 3)/32, 0, S, 0, XY, new Salsa8Blockmixer());
                    mixers[i] = new PwxFormBlockmixer(S);
                    if (i == 0)
                    {

                        /* 24: passwd <-- HMAC-SHA256(B_{0,2r-1}, passwd) */
                        byte[] key = new byte[64];
                        Helper.WordsToBytes( B, Bp + (s - 16), key, 0,  key.Length / 4);

                        var hmacsha256 = new HMACSHA256(key);
                        var passwdHash = hmacsha256.ComputeHash(passwd, 0, 32);
                        Array.Copy(passwdHash, passwd, 32);
                    }

                    /* 27: SMix1_r(B_i, n, V_{u..v}, flags) */
                    smix1(B, Bp, r, Np, flags, V, Vp, XY, mixers[i]);
                    /* 28: SMix2_r(B_i, p2floor(n), Nloop_rw, V_{u..v}, flags) */
                    smix2(B, Bp, r, Helper.P2floor(Np), Nloop_rw, flags, V, Vp, XY, mixers[i]);

                }
                else
                {
                    smix1(B, Bp, r, Np, flags, V, Vp, XY, new Salsa8Blockmixer());
                    smix2(B, Bp, r, Helper.P2floor(Np), Nloop_rw, flags, V, Vp, XY, new Salsa8Blockmixer());
                }
                
            }

            /* 30: for i = 0 to p - 1 do */
            for (uint i = 0; i < p; i++)
            {
                uint Bp = (i * s) + bIndex;
                /* 31: SMix2_r(B_i, N, Nloop_all - Nloop_rw, V, flags excluding YESCRYPT_RW) */
                smix2(B, Bp, r, N, Nloop_all - Nloop_rw, flags & ~YescryptFlags.YESCRYPT_RW, V, 0, XY, (flags & YescryptFlags.YESCRYPT_RW) !=0 ? mixers[i] : new Salsa8Blockmixer());
            }
        }  

    }
}
