# yescrypt-net

.NET implementation of the yescrypt a password-based key derivation function (KDF) and password hashing scheme.

## Implementation Notes

This implementation is suitable for simple validation of passwords against the yescrypt hash that can be found in the /etc/shadow file on modern linux distos:

    $ sudo cat /etc/shadow
    ...
    testuser:$y$j9T$IZUrEbc9oo9gZ28EqoVjI.$HVWJnkX89URubQkrksozeEoBwresP91xRowRD4ynRE9:19389:0:99999:7:::


Using the yescrypt value from the line above, you can write code like this to validata testuser's password:

``` c#
using fasterlimit.yescrypt

public static void main(string args[])
{
    byte [] passwd = Encoding.UTF8.GetBytes("passw0rd")
    if(Yescrypt.CheckPasswd(passwd, "$y$j9T$IZUrEbc9oo9gZ28EqoVjI.$HVWJnkX89URubQkrksozeEoBwresP91xRowRD4ynRE9"))
    {
        Console.Writeln("Correct");
    }
    else
    {
        Console.Writeln("Incorrect");
    }
}
```

The Yescrypt class has two other useful methods:

* `Yescrypt.ChangePasswd(byte[] newpasswd, string encoded)` -
    * Input: new password and yescrypt encoded string.  
    * Output: yescrypt string with new salt and new password hash (using settings from the encoded input string)
* `Yescrypt.NewPasswd(byte[] newPasswd,YescryptSettings settings)` 
    * Input: new password and yescrypt settings.  
    * Output output: yescrypt string with new salt and password hash

You can also use the raw `YescryptKdf` class if you want complete control of the KDF. 

## Limitations    

This implementation does not support p>1 (no parallelism ). It does not support ROMs either. Therefore, it is best suited casual checking of passwords and password hash maintenance.  It is (obviously) not suitable for proof of work or similar use cases where large numbers of hashes need be calculated with maximum efficiency.

This implementation has been tested with `YESCRYPT_RW` flavor.  This is the flavor you should use anyway.  If this code encounters `YESCRYPT_WORM` hashes... things might break. 

## Credits
This is a straight port of the "ref" implementation from the [original Openwall C sources](https://github.com/openwall/yescrypt).  I  left in comments so that it is easy to compare the c# port with the original C soruces. 