using System;
using System.Security.Cryptography;
using System.Text;

namespace Fasterlimit.Yescrypt
{
    public class YescryptKdf
    {
        uint Flags;
        uint N;
        uint r; 

        public YescryptKdf(uint flags, uint N, uint r )
        {
            Flags = flags;
            this.N = N;
            this.r = r;             
        }

        public YescryptKdf() : this(YescryptFlags.YESCRYPT_RW_DEFAULTS, 4096, 8)
        {
        }
       
        public byte[] DeriveKey(byte[] passwd, byte[] salt, bool isPrehash, int keyLength)
        {
            uint[] V = new uint[32 * r * N];
            uint[] B = new uint[32 * r];
            byte[] buf = new byte[32];

            if (Flags == 1)
            {
                throw new NotImplementedException("YESCRYPT_WORM not supported");
            }

            byte[] key = Encoding.ASCII.GetBytes(isPrehash ? "yescrypt-prehash" : "yescrypt");
            byte[] passwdHash; 
            using (var hmacsha256 = new HMACSHA256(key))
            {
                passwdHash = hmacsha256.ComputeHash(passwd);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(passwdHash, salt, 1, HashAlgorithmName.SHA256))
            {
                var bytes = pbkdf2.GetBytes(B.Length * 4);
                Helper.WordsFromBytes( bytes,0, B, 0, B.Length);
                Array.Copy(bytes, buf, buf.Length);
            }

            if ((Flags & YescryptFlags.YESCRYPT_RW) !=0)
            {
                Smix.Mix(B, 0, r, N, Flags, V, ref buf);
            }
            else
            {
                /* 3: B_i <-- MF(B_i, N) */
                Smix.Mix( B, 4 * r, r, N, Flags, V, ref buf);                
            }

            /* 5: DK <-- PBKDF2(P, B, 1, dkLen) */
            byte[] eSalt = new byte[B.Length * 4];
            Helper.WordsToBytes(B,0,eSalt, 0, B.Length);
            byte[] dk;
            using (var pbkdf2 = new Rfc2898DeriveBytes(buf, eSalt, 1, HashAlgorithmName.SHA256))
            {
                dk = pbkdf2.GetBytes(keyLength < 32 ? 32 : keyLength);
            }

            if (!isPrehash)
            {
                /*
                 * Except when computing classic scrypt, allow all computation so far
                 * to be performed on the client.  The final steps below match those of
                 * SCRAM (RFC 5802), so that an extension of SCRAM (with the steps so
                 * far in place of SCRAM's use of PBKDF2 and with SHA-256 in place of
                 * SCRAM's use of SHA-1) would be usable with yescrypt hashes.
                 */

                /* Compute ClientKey */
                byte[] dk32 = new byte[32];
                Array.Copy(dk, dk32, dk32.Length);
                using (var hmacsha256 = new HMACSHA256(dk32))
                {
                    buf = hmacsha256.ComputeHash(Encoding.ASCII.GetBytes("Client Key"));
                    using (var sha256 = SHA256.Create())
                    {
                        buf = sha256.ComputeHash(buf);
                        if (keyLength > buf.Length)
                        {
                            Array.Copy(buf, dk, buf.Length);
                        }
                        else
                        {
                            dk = new byte[keyLength];
                            Array.Copy(buf, dk, dk.Length);
                        }
                    }
                }
            }

            return dk;
        }
    }
}
