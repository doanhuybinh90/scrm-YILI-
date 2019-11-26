using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace Pb.Wechat.Web.Resources
{
    public static class AesCryptoService
    {
        public static int KEYSIZE = 128;

        public static string Encrypt(string plainText, string aesKey, string aesIv)
        {
            byte[] encryptBytes = null;

            ICryptoTransform encryptor = AesCryptoService.GetAesEncryptor(aesKey, aesIv);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encryptBytes = msEncrypt.ToArray();
                }
            }

            return Convert.ToBase64String(encryptBytes);
        }
        public static string Decrypt(string encryptText, string aesKey, string aesIv)
        {
            byte[] cipherBytes = Convert.FromBase64String(encryptText);

            ICryptoTransform decryptor = AesCryptoService.GetAesDecryptor(aesKey, aesIv);

            string plainText = string.Empty;

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
            {
                msDecrypt.Position = 0;
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plainText = srDecrypt.ReadToEnd();
                    }
                }
            }

            return plainText;
        }

        private static ICryptoTransform GetAesEncryptor(string aesKey, string aesIv)
        {
            RijndaelManaged aesAlg = new RijndaelManaged { IV = RsaService.Decrypt(Convert.FromBase64String(aesIv)), Key = RsaService.Decrypt(Convert.FromBase64String(aesKey)) };


            return aesAlg.CreateEncryptor();
        }

        private static ICryptoTransform GetAesDecryptor(string aesKey, string aesIv)
        {
            RijndaelManaged aesAlg = new RijndaelManaged { IV= RsaService.Decrypt(Convert.FromBase64String(aesIv)) ,Key= RsaService.Decrypt(Convert.FromBase64String(aesKey))};

            return aesAlg.CreateDecryptor();
        }
       

        /// <summary>
        /// Return 
        /// </summary>
        /// <returns></returns>
        public static string GetAesKeyAndIv(string aesKey, string aesIv)
        {
            RijndaelManaged aesAlg = new RijndaelManaged { IV = RsaService.Decrypt(Convert.FromBase64String(aesIv)), Key = RsaService.Decrypt(Convert.FromBase64String(aesKey)) };
            return Convert.ToBase64String(aesAlg.Key) + "$" + Convert.ToBase64String(aesAlg.IV);
        }

        ///<summary>
        /// AES解密
        ///</summary>
        ///<param name="CryptText">待解密字符串</param>
        ///<param name="Key">加密密钥</param>
        ///<param name="IV">初始化向量</param>
        ///<param name="TextData">输出:已解密的字符串</param>
        ///<returns>0:成功解密 -1:待解密字符串不为能空 -2:加密密钥不能为空 -3:初始化向量字节长度不为KEYSIZE/8 -4:其他错误</returns>
        public static string DecryptText(string CryptText, string Key, string IV)
        {
            if (string.IsNullOrEmpty(CryptText)) throw new Exception("字符不能为空");
            if (string.IsNullOrEmpty(Key)) throw new Exception("密钥不能为空");
            if (string.IsNullOrEmpty(IV)) throw new Exception("向量不正确");

            Key = Encoding.UTF8.GetString(Convert.FromBase64String(Key));
            IV = Encoding.UTF8.GetString(Convert.FromBase64String(IV));

            if (IV.Length != KEYSIZE / 8) throw new Exception("向量不正确");

            RijndaelManaged rijndaelCipher = new RijndaelManaged();

            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = KEYSIZE;
            rijndaelCipher.BlockSize = KEYSIZE;

            byte[] encryptedData = Convert.FromBase64String(CryptText);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(Key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(IV);

            byte[] keyBytes = new byte[KEYSIZE / 8];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length) len = keyBytes.Length;
            System.Array.Copy(pwdBytes, keyBytes, len);

            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = ivBytes;

            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            return Encoding.UTF8.GetString(plainText);
        }
    }
}
