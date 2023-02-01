using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Tests")]
namespace Fasterlimit.Yescrypt
{
    internal class Yescrypt64StringWriterException : Exception
    {
        public Yescrypt64StringWriterException(string message) : base(message) { }
    }

     internal class B64StringWriter
    {
        private readonly char[] itoa64 = { '.', '/', '0', '1', '2', '3', 
            '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G',
            'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g',
            'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
            'u', 'v', 'w', 'x', 'y', 'z' };

        private StringBuilder encodedValue;

        public B64StringWriter()
        {
            encodedValue = new StringBuilder(80);
        }

        public B64StringWriter WriteUint32Min(uint val, uint valMin)
        {
            uint start = 0;
            uint end = 47;
            uint chars = 1; 
            int bits = 0;

            if (val < valMin)
            {
                throw new Yescrypt64StringWriterException("src (" + val + ") must not be less than valMin (" + valMin + ")");
            }
            
            val -= valMin;

            while(true)
            {
                uint count = (end + 1 - start) << bits;
                if (val < count)
                    break;
                if (start >= 63)
                {
                    throw new Yescrypt64StringWriterException("Um... I crapped my pants");
                }                    
                start = end + 1;
                end = start + (62 - end) / 2;
                val -= count;
                chars++;
                bits += 6;
            }

            encodedValue.Append(itoa64[start + (val >> bits)]);
            for (int i = 1; i<chars;i++)
            {
                bits -= 6;
                encodedValue.Append(itoa64[(val >> bits) & 0x3f]);                
            }

            return this;
        }

        public B64StringWriter WriteUint32Bits(uint val, int valBits)
        {
            for (uint bits = 0; bits < valBits; bits += 6)
            {
                encodedValue.Append(itoa64[val & 0x3f]);
                val >>= 6;
            }
            return this;
        }

        public B64StringWriter WriteBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length;)
            {
                uint value = 0;
                int bits = 0;
                
                do
                {
                    value |= (uint)bytes[i++] << bits;
                    bits += 8;
                } while (bits < 24 && i < bytes.Length);

                WriteUint32Bits(value, bits);               
            }           

            return this;
        }

        public B64StringWriter Reset()
        {
            encodedValue.Clear();
            return this;
        }

        public override string ToString() 
        { 
            return encodedValue.ToString(); 
        }
    }
}