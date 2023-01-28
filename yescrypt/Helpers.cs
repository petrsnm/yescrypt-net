using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fasterlimit.Yescrypt
{
    internal class Helpers
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
    }
}
