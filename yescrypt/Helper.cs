using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fasterlimit.Yescrypt
{
    internal class Helper
    {

       

        public static void BlockCopy(uint[] dst, int dstIndex, uint[] src, int srcIndex, int count)
        {
            Array.Copy(src, srcIndex, dst, dstIndex, count);
        }

        public static void BlockXor(uint[] dst, int dstIndex, uint[] src, int srcIndex, int count)
        {
            for (int i = 0; i < count; i++)
            {
                dst[i+dstIndex] ^= src[i + srcIndex];
            }
        }

        /**
         * integerify(B, r):
         * Return the result of parsing B_{2r-1} as a little-endian integer.
         */
        public static ulong Integerify(uint[] B, int bIndex, int r)
        {
            int X = bIndex + (2 * r - 1) * 16;
            ulong rval = B[X + 13];
            rval <<= 32;
            rval += B[X];
            return rval;
        }
         
        
        /**
         * p2floor(x) :
         * Largest power of 2 not greater than argument.
         */
        public static ulong P2floor(ulong x)
        {
            ulong y;
            while (true)
            {
                y = x & (x - 1);
                if (y == 0) break;
                x = y;
            }
            return x;
        }

        /**
         * wrap(x, i):
         * Wrap x to the range 0 to i-1.
         */
        public static ulong wrap(ulong x, ulong i)
        {
            ulong n = P2floor(i);
            return (x & (n - 1)) + (i - n);
        }
    }
}
