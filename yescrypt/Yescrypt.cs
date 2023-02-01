using System.Security.Cryptography;

namespace Fasterlimit.Yescrypt
{


    public class Yescrypt
    {
        private static byte[] DeriveKey (byte[] passwd, YescryptSettings settings)
        {
            if (settings.p > 1)
            {
                throw new NotImplementedException("P > 1 is not supported");
            }
            if (settings.t > 0)
            {
                throw new NotImplementedException("t > 0 is not supported");
            }
            if (settings.g > 0)
            {
                throw new NotImplementedException("g > 0 is not supported");
            }            

            YescryptKdf kdf;                         
            if ((settings.flags & YescryptFlags.YESCRYPT_RW) != 0 && settings.N >= 0x100 && settings.N * settings.r >= 0x20000)
            {
                kdf = new YescryptKdf(settings.flags, settings.N >> 6, settings.r);
                passwd = kdf.DeriveKey(passwd, settings.salt, true, 32);
            }
            kdf = new YescryptKdf(settings.flags, settings.N, settings.r);

            return kdf.DeriveKey(passwd, settings.salt, false, 32);
        }

        public static string NewPasswd(byte[] newPasswd, YescryptSettings settings)
        {
            settings.salt = RandomNumberGenerator.GetBytes(16);
            return ChangePasswd(newPasswd, settings.ToString());
        }


        public static string ChangePasswd(byte[] newPasswd, string encoded)
        {
            YescryptSettings settings = new YescryptSettings(encoded);            
            settings.salt = RandomNumberGenerator.GetBytes(16);
            byte[] derivedKey = DeriveKey(newPasswd, settings);
            settings.key = derivedKey;
            return settings.ToString();
        }

        public static bool CheckPasswd( byte[] passwd, string encoded)
        {
            YescryptSettings settings = new YescryptSettings(encoded);
            if (settings.key == null || settings.salt == null)
            {
                return false;
            }
            byte [] derivedKey = DeriveKey(passwd, settings);
            return Enumerable.SequenceEqual(settings.key, derivedKey);            
        }        
    }
}
