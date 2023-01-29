using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fasterlimit.Yescrypt
{
    internal class Endian
    {
        static private uint SwapBytes(uint x)
        {
            x = (x >> 16) | (x << 16);
            return ((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8);
        }

        static private ulong SwapBytes(ulong x)
        {
            x = (x >> 32) | (x << 32);
            x = ((x & 0xFFFF0000FFFF0000) >> 16) | ((x & 0x0000FFFF0000FFFF) << 16);
            return ((x & 0xFF00FF00FF00FF00) >> 8) | ((x & 0x00FF00FF00FF00FF) << 8);
        }

        public static uint le32dec(uint[] B, int bIndex)
        {
            if (!BitConverter.IsLittleEndian)
            {
                return SwapBytes(B[bIndex]); 
            }
            return B[bIndex];
        }

        public static void le32enc(uint[] B, int bIndex, uint x)
        {
            if (!BitConverter.IsLittleEndian)
            {
                x = SwapBytes(x);                
            }
            B[bIndex] = x;
        }
    }
}
