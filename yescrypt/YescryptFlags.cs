using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fasterlimit.Yescrypt
{
    internal class YescryptFlags
    {
        public static readonly uint YESCRYPT_WORM      = 1;
        public static readonly uint YESCRYPT_RW        = 0x002;
        public static readonly uint YESCRYPT_ROUNDS_3  = 0x000;
        public static readonly uint YESCRYPT_ROUNDS_6  = 0x004;
        public static readonly uint YESCRYPT_GATHER_1  = 0x000;
        public static readonly uint YESCRYPT_GATHER_2  = 0x008;
        public static readonly uint YESCRYPT_GATHER_4  = 0x010;
        public static readonly uint YESCRYPT_GATHER_8  = 0x018;
        public static readonly uint YESCRYPT_SIMPLE_1  = 0x000;
        public static readonly uint YESCRYPT_SIMPLE_2  = 0x020;
        public static readonly uint YESCRYPT_SIMPLE_4  = 0x040;
        public static readonly uint YESCRYPT_SIMPLE_8  = 0x060;
        public static readonly uint YESCRYPT_SBOX_6K   = 0x000;
        public static readonly uint YESCRYPT_SBOX_12K  = 0x080;
        public static readonly uint YESCRYPT_SBOX_24K  = 0x100;
        public static readonly uint YESCRYPT_SBOX_48K  = 0x180;
        public static readonly uint YESCRYPT_SBOX_96K  = 0x200;
        public static readonly uint YESCRYPT_SBOX_192K = 0x280;
        public static readonly uint YESCRYPT_SBOX_384K = 0x300;
        public static readonly uint YESCRYPT_SBOX_768K = 0x380;
        public static readonly uint YESCRYPT_RW_DEFAULTS = (YESCRYPT_RW | YESCRYPT_ROUNDS_6 | YESCRYPT_GATHER_4 | YESCRYPT_SIMPLE_2 | YESCRYPT_SBOX_12K);
        public static readonly uint YESCRYPT_RW_FLAVOR_MASK = 0x3fc;
        public static readonly uint YESCRYPT_PREHASH = 0x10000000;
    }
}
