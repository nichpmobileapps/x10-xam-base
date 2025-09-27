using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace X10Card.Models
{
    public class CommonClass
    {
        protected static string SecretKey = "%&2022$X10%Registration$$1309cardKey%";

        //Encryption Algorithm
        public static string Encrypt(string plainText)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);

            try
            {
                return Convert.ToBase64String(Encrypt(plainBytes, GetRijndaelManaged(SecretKey)));
            }
            catch
            {
                return " ";
            }
        }
        public static string Decrypt(string encryptedText)
        {
            try
            {
                var encryptedBytes = Convert.FromBase64String(encryptedText);
                return Encoding.UTF8.GetString(Decrypt(encryptedBytes, GetRijndaelManaged(SecretKey)));
            }
            catch
            {
                return "";
            }
        }
        public static byte[] Encrypt(byte[] plainBytes, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }
        public static byte[] Decrypt(byte[] encryptedData, RijndaelManaged rijndaelManaged)
        {
            return rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        }
        public static RijndaelManaged GetRijndaelManaged(string secretKey)
        {
            var keyBytes = new byte[16];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            Array.Copy(secretKeyBytes, keyBytes, Math.Min(keyBytes.Length, secretKeyBytes.Length));
            return new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128,
                Key = keyBytes,
                IV = keyBytes
            };
        }


        public static string GetSha256FromString(string strData)
        {
            try
            {
                var message = Encoding.ASCII.GetBytes(strData);
                SHA256Managed hashString = new SHA256Managed();
                string hex = "";
                var hashValue = hashString.ComputeHash(message);
                foreach (byte x in hashValue)
                {
                    hex += string.Format("{0:x2}", x);
                }
                return hex;
            }
            catch { return ""; }
        }
    }
}
