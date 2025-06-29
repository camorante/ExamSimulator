using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExamSimulator.Utils
{
    public class EncryptionUtils
    {
        public static string EncryptText(string text, string password)
        {
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(text);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            // Ensure the password length is 32 bytes for AES-256
            Array.Resize(ref passwordBytes, 32);
            byte[] encryptedBytes = AESEncrypt(bytesToBeEncrypted, passwordBytes);
            string encryptedText = Convert.ToBase64String(encryptedBytes);
            return encryptedText;
        }
        public static string DecryptText(string encryptedText, string password)
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            // Ensure the password length is 32 bytes for AES-256
            Array.Resize(ref passwordBytes, 32);
            byte[] decryptedBytes = AESDecrypt(bytesToBeDecrypted, passwordBytes);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public static byte[] AESEncrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] lbEncryptedBytes = null;

            // Set your salt here, change it to meet your flavor:  
            // The salt bytes must be at least 8 bytes.  
            byte[] lbSaltBytes = new byte[] { 6, 9, 1, 4, 5, 2, 1, 9 };

            using (MemoryStream loMemoryStream = new MemoryStream())
            {
                using (Aes loAES = Aes.Create()) // Replace RijndaelManaged with Aes  
                {
                    loAES.KeySize = 256;
                    loAES.BlockSize = 128;

                    // Updated constructor to specify hash algorithm and iteration count
                    var loKey = new Rfc2898DeriveBytes(passwordBytes, lbSaltBytes, 1000, HashAlgorithmName.SHA256);
                    loAES.Key = loKey.GetBytes(loAES.KeySize / 8);
                    loAES.IV = loKey.GetBytes(loAES.BlockSize / 8);

                    loAES.Mode = CipherMode.CBC;

                    using (var loCryptoStream = new CryptoStream(loMemoryStream, loAES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        loCryptoStream.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        loCryptoStream.Close();
                    }
                    lbEncryptedBytes = loMemoryStream.ToArray();
                }
            }

            return lbEncryptedBytes;
        }

        public static byte[] AESDecrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] loDecryptedBytes = null;

            // Set your salt here, change it to meet your flavor:  
            // The salt bytes must be at least 8 bytes.  
            byte[] lbSaltBytes = new byte[] { 6, 9, 1, 4, 5, 2, 1, 9 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (Aes loAES = Aes.Create()) // Replace RijndaelManaged with Aes  
                {
                    loAES.KeySize = 256;
                    loAES.BlockSize = 128;

                    // Updated constructor to specify hash algorithm and iteration count
                    var loKey = new Rfc2898DeriveBytes(passwordBytes, lbSaltBytes, 1000, HashAlgorithmName.SHA256);
                    loAES.Key = loKey.GetBytes(loAES.KeySize / 8);
                    loAES.IV = loKey.GetBytes(loAES.BlockSize / 8);

                    loAES.Mode = CipherMode.CBC;

                    using (var loCryptoStream = new CryptoStream(ms, loAES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        loCryptoStream.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        loCryptoStream.Close();
                    }
                    loDecryptedBytes = ms.ToArray();
                }
            }

            return loDecryptedBytes;
        }
    }
}
