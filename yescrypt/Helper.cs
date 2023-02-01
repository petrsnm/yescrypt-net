using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fasterlimit.Yescrypt
{
    internal class Helper
    {
       
        public static void WordsToBytes( uint[] src, uint srcIndex, byte[] dst, uint dstIndex, int wordCount)
        {
            while (wordCount > 0) 
            {
                Endian.le32enc(dst, dstIndex, src[srcIndex]);
                dstIndex += 4;
                srcIndex++;
                wordCount--;
            }
        }

        public static void WordsFromBytes(  byte[] src, uint srcIndex, uint[] dst, uint dstIndex, int wordCount)
        {
            while (wordCount > 0)
            {
                dst[dstIndex] = Endian.le32dec(src, srcIndex);
                srcIndex += 4;
                dstIndex++;
                wordCount--;
            }
        }

        public static void BlockCopy(uint[] dst, uint dstIndex, uint[] src, uint srcIndex, uint count)
        {
            Array.Copy(src, srcIndex, dst, dstIndex, count);
        }

        public static void BlockXor(uint[] dst, uint dstIndex, uint[] src, uint srcIndex, uint count)
        {
            for (uint i = 0; i < count; i++)
            {
                dst[i+dstIndex] ^= src[i + srcIndex];
            }
        }

        /**
         * integerify(B, r):
         * Return the result of parsing B_{2r-1} as a little-endian integer.
         */
        public static ulong Integerify(uint[] B, uint bIndex, uint r)
        {
            uint X = bIndex + (2 * r - 1) * 16;
            ulong rval = B[X + 13];
            rval <<= 32;
            rval += B[X];
            return rval;
        }
         
        
        /**
         * p2floor(x) :
         * Largest power of 2 not greater than argument.
         */
        public static uint P2floor(uint x)
        {
            uint y;
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
        public static uint wrap(uint x, uint i)
        {
            uint n = P2floor(i);
            return (x & (n - 1)) + (i - n);
        }
    }
}
