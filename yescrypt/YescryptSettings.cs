using System;
using System.Text;

namespace Fasterlimit.Yescrypt
{
    public class YescryptSettings
    {
        public uint flags { get; set; }
        public uint N { get; set; }
        public uint r { get; set; }
        public uint p { get; set; }
        public uint t { get; set; }
        public uint g { get; set; }
        public byte[] salt { get; set; }
        public byte[] key { get; set; }

        public YescryptSettings()
        {
            flags = YescryptFlags.YESCRYPT_RW_DEFAULTS;
            N = 4096;
            r = 32;
            p = 1;
            t = 0;
            g = 0;
            salt = new byte[16];
            key = new byte[32];
        }

        public YescryptSettings(string encoded) : this()
        {
            // Decode string that looks like this: $y$jD5.7$LdJMENpBABJJ3hIHjB1Bi.$HboGM6qPrsK.StKYGt6KErmUYtioHreJd98oIugoNB6
            string[] parts = encoded.Split('$');            

            // check for correct marker prefix
            if (parts.Length != 5)
            {
                throw new ArgumentException("Invalid encoding. Valid yescrypt encoding should have 4 values separated by '$'");
            }

            // we only support "y" (not "7")
            if (parts[1] != "y")
            {
                throw new ArgumentException($"Unsupported prefix: ${parts[0]}$");
            }

            // Decode flavor
            B64StringReader reader = new B64StringReader(parts[2]);
            uint flavor = reader.ReadUint32Min(0);

            if (flavor < YescryptFlags.YESCRYPT_RW)
            {
                flags = flavor;
            }
            else if (flavor <= YescryptFlags.YESCRYPT_RW + (YescryptFlags.YESCRYPT_RW_FLAVOR_MASK) >> 2)
            {
                flags = YescryptFlags.YESCRYPT_RW + ((flavor - YescryptFlags.YESCRYPT_RW) << 2);
            }
            else
            {
                throw new ArgumentException($"Invalid flavor");
            }

            // Decode N
            int nlog2 = (int)reader.ReadUint32Min(1);
            if (nlog2 > 31)
            {
                throw new ArgumentException($"Invalid N.  Nlog2 must be < 32");
            }
            N = 1u << nlog2;

            // Decode r
            r = reader.ReadUint32Min(1);

            // Decode p, t, and g and ROM (if they exist)
            p = 1;
            if (reader.HasMore())
            {
                uint have = reader.ReadUint32Min(1);

                if ((have & 1) != 0)
                {
                    p = reader.ReadUint32Min(2);

                }
                if ((have & 2) != 0)
                {
                    uint t = reader.ReadUint32Min(1);

                }
                if ((have & 4) != 0)
                {
                    uint g = reader.ReadUint32Min(1);

                }
                if ((have & 8) != 0)
                {
                    throw new NotImplementedException("ROM is not supported");
                }
            }

            // Decode Salt
            reader = new B64StringReader(parts[3]);
            salt = reader.ReadBytes(16);

            // Decode Key
            reader = new B64StringReader(parts[4]);
            key = reader.ReadBytes(32);
        }

        private uint N2log2(uint N)
        {
            int nlog2;
            if (N < 2) return 0;
            nlog2 = 2;
            while (N >> nlog2 != 0) nlog2++;
            nlog2--;
            if (N >> nlog2 != 1) return 0;
            return (uint)nlog2;
        }

        public override string ToString()
        {
            StringBuilder rval = new StringBuilder("$y$");
            B64StringWriter writer = new B64StringWriter();

            uint flavor;
            if (flags < YescryptFlags.YESCRYPT_RW)
            {
                flavor = flags;
            }
            else if ((flags & YescryptFlags.YESCRYPT_MODE_MASK) == YescryptFlags.YESCRYPT_RW &&
                flags <= (YescryptFlags.YESCRYPT_RW | YescryptFlags.YESCRYPT_RW_FLAVOR_MASK))
            {
                flavor = YescryptFlags.YESCRYPT_RW + (flags >> 2);
            }
            else
            {
                throw new ArgumentException($"Invalid flavor");
            }

            uint nlog2 = N2log2(N);
            if (nlog2 == 0)
            {
                throw new ArgumentException("N must be power of 2");
            }

            if (r * p >= (1U << 30))
            {
                throw new ArgumentException("Invalid r");
            }

            if (salt == null)
            {
                throw new ArgumentException("Invalid Salt");
            }

            if (key == null)
            {
                throw new ArgumentException("Invalid key");
            }

            writer.WriteUint32Min(flavor, 0);
            writer.WriteUint32Min(nlog2, 1);
            writer.WriteUint32Min(r, 1);
            // for now, we don't write p,t,g or ROM (obviously)            
            rval.Append(writer.ToString());

            writer = new B64StringWriter();
            writer.WriteBytes(salt);
            rval.Append("$").Append(writer.ToString());

            writer = new B64StringWriter();
            writer.WriteBytes(key);
            rval.Append("$").Append(writer.ToString());

            return rval.ToString();
        }
    }
}
