using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fasterlimit.Yescrypt
{
    public class Yescrypt
    {
        public string ToEncoded(string passwd, uint flavor, uint N, uint r)
        {
            return "Hello";
        }


        public static bool CheckPasswd(string passwd, string encoded)
        {

            // Decode string that looks like this: $y$jD5.7$LdJMENpBABJJ3hIHjB1Bi.$HboGM6qPrsK.StKYGt6KErmUYtioHreJd98oIugoNB6
            string[] parts = encoded.Split('$');
            
            // check for correct marker prefix
            if(parts.Length != 5)
            {
                throw new ArgumentException("Invalid encoding. Valid encoding should have 4 values separated by '$'");
            }

            // we only support "y" (not "7")
            if (parts[1] != "y")
            {
                throw new ArgumentException($"Unsupported prefix: ${parts[0]}$");
            }
            
            // Decode flavor
            B64StringReader reader = new B64StringReader(parts[2]);
            uint flavor = reader.ReadUint32Min(0);
            uint flags;
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
                 throw new ArgumentException($"Unsupported flavor"); 
            }

            // Decode N
            int nlog2 = (int) reader.ReadUint32Min(1);
            if(nlog2 > 31)
            {
                throw new ArgumentException($"Unsupported N.  Nlog2 must be < 32");
            }
            uint N =  1u << nlog2;

            // Decode r
            uint r = reader.ReadUint32Min(1);

            // Decode p, t, and g and ROM (if they exist)
            if (reader.HasMore())
            {
                uint have = reader.ReadUint32Min(1);

                if ((have & 1) != 0) {
                    uint p = reader.ReadUint32Min(2);
                    if (p > 1)
                    {
                        throw new NotImplementedException("P > 1 is not supported");
                    }
                }

                if ((have & 2) !=0)
                {
                    uint t = reader.ReadUint32Min(1);
                    if (t > 0)
                    {
                        throw new NotImplementedException("t > 0 is not supported");
                    }
                }

                if ((have & 4) !=0)
                {
                    uint g = reader.ReadUint32Min(1);
                    if (g > 0)
                    {
                        throw new NotImplementedException("g > 0 is not supported");
                    }
                }

                if ((have & 8) !=0)
                {
                    throw new NotImplementedException("ROM is not supported");
                }
            }

            // TODO: encrypt salt?
            reader = new B64StringReader(parts[3]);
            byte[] saltBytes = reader.ReadBytes(parts[3].Length * 4 + 1); 
            reader = new B64StringReader(parts[4]);
            byte[] expectedKey = reader.ReadBytes(parts[4].Length * 4 + 1);
            byte[] passwdBytes = Encoding.Default.GetBytes(passwd);

            YescryptKdf kdf;
            byte[] derivedKey;
            // Prehash
            if ((flags & YescryptFlags.YESCRYPT_RW) !=0 && N >= 0x100 && N * r >= 0x20000)
            {
                kdf = new YescryptKdf(flags, N >> 6, r);
                passwdBytes = kdf.DeriveKey(passwdBytes, saltBytes, true, 32);                
            }

            kdf = new YescryptKdf(flags, N, r);
            derivedKey = kdf.DeriveKey(passwdBytes, saltBytes, false, 32);

            return Enumerable.SequenceEqual(expectedKey,derivedKey);            
        }
    }
}
