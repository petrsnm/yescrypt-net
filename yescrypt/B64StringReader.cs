using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Tests")]
namespace Fasterlimit.Yescrypt
{
    internal class Yescrypt64StringReaderException : Exception
    {
        public Yescrypt64StringReaderException(string message) : base(message) { }
    }

    internal class B64StringReader
    {
        private readonly byte[] atoi64_partial = new byte[77] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11,
            64, 64, 64, 64, 64, 64, 64,
            12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24,
            25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37,
            64, 64, 64, 64, 64, 64,
            38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
            51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63
        };

        private byte[] encodedValue;
        private int currentIndex;

        private uint atoi64(byte val)
        {
            return (val >= '.' && val <= 'z') ? atoi64_partial[val - '.'] : 64u;
        }

        public B64StringReader(string encodedString)
        {
            this.encodedValue = Encoding.ASCII.GetBytes(encodedString);
            this.currentIndex = 0;
        }

        public uint ReadUint32Min(uint min)
        {
            uint rval;
            uint start = 0;
            uint end = 47;
            uint chars = 1;
            int bits = 0;

            uint c;

            c = atoi64(encodedValue[currentIndex++]);
            if (c > 63)
            {
                throw new Yescrypt64StringReaderException("Invalid encoding at index: " + currentIndex);
            }

            rval = min;
            while (c > end)
            {
                rval += (end + 1 - start) << bits;
                start = end + 1;
                end = start + (62 - end) / 2;
                chars++;
                bits += 6;
            }

            rval += (c - start) << bits;

            for (int i = 1; i < chars; i++)
            {
                c = atoi64(encodedValue[currentIndex++]);
                if (c > 63)
                {
                    throw new Yescrypt64StringReaderException("Invalid encoding at index: " + currentIndex);
                }
                bits -= 6;
                rval += c << bits;
            }
            return rval;
        }

        public uint ReadUint32Bits(int valBits)
        {
            uint rval = 0;

            for (int bits = 0; bits < valBits; bits += 6)
            {               
                uint c = atoi64(encodedValue[currentIndex++]);
                if (c > 63)
                {
                    throw new Yescrypt64StringReaderException("Invalid encoding at index: " + currentIndex);
                }
                rval |= c << bits;
            }

            return rval;
        }

        public byte[] ReadBytes(int length)
        {
            byte[] rval = new byte[length];
            int rvalIndex = 0;

            int bitsAvailable = (encodedValue.Length - currentIndex) * 6;
            int bitsRequested = length * 8;

            int bits = bitsAvailable > bitsRequested ? bitsRequested : bitsAvailable;
            for (; bits > 23; bits -= 24)
            {
                uint val = ReadUint32Bits(24);
                for (int i = 0; i < 3; i++)
                {
                    rval[rvalIndex++] = (byte)(val & 0xff);
                    val >>= 8;
                }
            }
            if (bits > 0 )
            {
                uint val = ReadUint32Bits(bits);
                for (int i = 0; i < bits / 6; i++)
                {
                    rval[rvalIndex++] = (byte)(val & 0xff);
                    val >>= 8;
                }
            }   
            
            if(rvalIndex < rval.Length)
            {
                Array.Resize(ref rval, rvalIndex - 1);
            }

            return rval;
        }

        public bool HasMore()
        {
            return currentIndex < encodedValue.Length;            
        }
        
    }
}
