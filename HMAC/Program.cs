using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TripleDESCrypto
{
    class Program
    {
        const string key = "AaBbCcDdEeFfGgHhJjKkLlMm";//24 karakter olmalı
        static void Main(string[] args)
        {
            string guid = Guid.NewGuid().ToString();
            Console.WriteLine(guid);
            string Enc = Encrypt(guid);
            Console.WriteLine("Encrypt : " + Enc);
            Console.WriteLine("Decrypt : " + Decrypt(Enc));
            Console.ReadLine();
        }
        public static string Encrypt(string toEncrypt)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            byte[] keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            
            hashmd5.Clear();
            
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            tdes.Clear();
            
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string cipherString)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
         
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            byte[] keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            
            hashmd5.Clear();
            
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateDecryptor();

            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);              
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
