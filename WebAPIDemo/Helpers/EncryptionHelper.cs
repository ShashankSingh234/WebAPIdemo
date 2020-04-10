using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIDemo.Helpers
{
    public class EncryptionHelper
    {
        public static string Encrypt(string plainText)
        {
            //using (AesManaged aes = new AesManaged())
            //{
            //    aes.Padding = PaddingMode.Zeros;
            //    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            //        {
            //            using (StreamWriter sw = new StreamWriter(cs))
            //                sw.Write(plainText);
            //            return System.Text.Encoding.Unicode.GetString(ms.ToArray());
            //        }
            //    }
            //}

            var data = Encoding.Unicode.GetBytes(plainText);
            byte[] encryptedText = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedText);
        }

        public static string Decrypt(string cipher)
        {
            //using (AesManaged aes = new AesManaged())
            //{
            //    aes.Padding = PaddingMode.Zeros;
            //    // Create a decryptor    
            //    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            //    // Create the streams used for decryption.    
            //    using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(cipher)))
            //    {
            //        // Create crypto stream    
            //        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            //        {
            //            // Read crypto stream    
            //            using (StreamReader reader = new StreamReader(cs))
            //                return reader.ReadToEnd();
            //        }
            //    }
            //}

            byte[] data = Convert.FromBase64String(cipher);
            byte[] plainText = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(plainText);
        }
    }
}
