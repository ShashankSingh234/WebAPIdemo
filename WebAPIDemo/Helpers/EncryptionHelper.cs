using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WebAPIDemo.Helpers
{
    public class EncryptionHelper
    {
        const string Key = "sndhgkjdfjkfkjkjfmfvhkjkjkkjgjhy";
        const string IV = "sndhgkjdfjkfjtyh";
        public static string Encrypt(string plainText)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                byte[] plainTextByte = ASCIIEncoding.ASCII.GetBytes(plainText);

                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.Key = ASCIIEncoding.ASCII.GetBytes(Key);
                aes.IV = ASCIIEncoding.ASCII.GetBytes(IV);
                aes.Padding = PaddingMode.Zeros;
                aes.Mode = CipherMode.CBC;
                ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] encrypted = crypto.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);
                crypto.Dispose();
                return Convert.ToBase64String(encrypted);
            }

            //var data = Encoding.Unicode.GetBytes(plainText);
            //byte[] encryptedText = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            //return Convert.ToBase64String(encryptedText);
        }

        public static string Decrypt(string cipher)
        {
            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                byte[] encryptedBytes = Convert.FromBase64String(cipher);

                aes.BlockSize = 128;
                aes.KeySize = 256;
                aes.Key = ASCIIEncoding.ASCII.GetBytes(Key);
                aes.IV = ASCIIEncoding.ASCII.GetBytes(IV);
                aes.Padding = PaddingMode.Zeros;
                aes.Mode = CipherMode.CBC;
                ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] decrypted = crypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                crypto.Dispose();
                return System.Text.ASCIIEncoding.ASCII.GetString(decrypted).Replace("\0", string.Empty);
            }

            //byte[] data = Convert.FromBase64String(cipher);
            //byte[] plainText = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
            //return Encoding.Unicode.GetString(plainText);
        }
    }
}
