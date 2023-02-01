namespace Fasterlimit.Yescrypt
{
    internal class Endian
    {
        public static void le32enc(byte[] b, uint bIndex, uint x)
        {
            b[bIndex] = (byte)(x & 0xff);
            b[bIndex + 1] = (byte)((x >> 8) & 0xff);
            b[bIndex + 2] = (byte)((x >> 16) & 0xff);
            b[bIndex + 3] = (byte)((x >> 24) & 0xff);
        }

        public static uint le32dec(byte[] b, uint bIndex)           
        {
            return ((uint)(b[bIndex]) + ((uint)(b[bIndex+1]) << 8) + ((uint)(b[bIndex+2]) << 16) + ((uint)(b[bIndex+3]) << 24));            
        }
    }
}
