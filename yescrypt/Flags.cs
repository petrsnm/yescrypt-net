using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fasterlimit.Yescrypt
{
    internal class Flags
    {
        public const uint YESCRYPT_WORM      = 1;
        public const uint YESCRYPT_RW        = 0x002;
        public const uint YESCRYPT_ROUNDS_3  = 0x000;
        public const uint YESCRYPT_ROUNDS_6  = 0x004;
        public const uint YESCRYPT_GATHER_1  = 0x000;
        public const uint YESCRYPT_GATHER_2  = 0x008;
        public const uint YESCRYPT_GATHER_4  = 0x010;
        public const uint YESCRYPT_GATHER_8  = 0x018;
        public const uint YESCRYPT_SIMPLE_1  = 0x000;
        public const uint YESCRYPT_SIMPLE_2  = 0x020;
        public const uint YESCRYPT_SIMPLE_4  = 0x040;
        public const uint YESCRYPT_SIMPLE_8  = 0x060;
        public const uint YESCRYPT_SBOX_6K   = 0x000;
        public const uint YESCRYPT_SBOX_12K  = 0x080;
        public const uint YESCRYPT_SBOX_24K  = 0x100;
        public const uint YESCRYPT_SBOX_48K  = 0x180;
        public const uint YESCRYPT_SBOX_96K  = 0x200;
        public const uint YESCRYPT_SBOX_192K = 0x280;
        public const uint YESCRYPT_SBOX_384K = 0x300;
        public const uint YESCRYPT_SBOX_768K = 0x380;

        public const uint YESCRYPT_RW_DEFAULTS = (YESCRYPT_RW | YESCRYPT_ROUNDS_6 | YESCRYPT_GATHER_4 | YESCRYPT_SIMPLE_2 | YESCRYPT_SBOX_12K);
    }
}
