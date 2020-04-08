using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIDemo.Healpers
{
    public class PasswordHelper
    {
        public static string Encrypt(string password)
        {
            var data = Encoding.Unicode.GetBytes(password);
            byte[] encryptedPassword = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedPassword);
        }

        public static string Decrypt(string encryptedPassword)
        {
            byte[] data = Convert.FromBase64String(encryptedPassword);
            byte[] decryptedPassword = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decryptedPassword);
        }
    }
}
