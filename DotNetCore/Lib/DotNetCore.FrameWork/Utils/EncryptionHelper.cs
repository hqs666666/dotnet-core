 


/*****************************************************************************
 * 
 * Created On: 2018-05-23
 * Purpose:    加密
 * 
 ****************************************************************************/

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotNetCore.FrameWork.Utils
{
    public class EncryptionHelper
    {
        public static string AesEncrypt(string input, string key)
        {
            var lEncryptKey = Encoding.UTF8.GetBytes(key);

            using (var lAesAlg = Aes.Create())
            {
                using (var lEncryptor = lAesAlg.CreateEncryptor(lEncryptKey, lAesAlg.IV))
                {
                    using (var lMsEncrypt = new MemoryStream())
                    {
                        using (var lCsEncrypt = new CryptoStream(lMsEncrypt, lEncryptor,
                            CryptoStreamMode.Write))

                        using (var lSwEncrypt = new StreamWriter(lCsEncrypt))
                        {
                            lSwEncrypt.Write(input);
                        }

                        var lIv = lAesAlg.IV;

                        var lDecryptedContent = lMsEncrypt.ToArray();

                        var lResult = new byte[lIv.Length + lDecryptedContent.Length];

                        Buffer.BlockCopy(lIv, 0, lResult, 0, lIv.Length);
                        Buffer.BlockCopy(lDecryptedContent, 0, lResult,
                            lIv.Length, lDecryptedContent.Length);

                        return Convert.ToBase64String(lResult);
                    }
                }
            }
        }

        public static string AesDecrypt(string input, string key)
        {
            var lFullCipher = Convert.FromBase64String(input);

            var lIv = new byte[16];
            var lCipher = new byte[16];

            Buffer.BlockCopy(lFullCipher, 0, lIv, 0, lIv.Length);
            Buffer.BlockCopy(lFullCipher, lIv.Length, lCipher, 0, lIv.Length);
            var lDecryptKey = Encoding.UTF8.GetBytes(key);

            using (var lAesAlg = Aes.Create())
            {
                using (var lDecryptor = lAesAlg.CreateDecryptor(lDecryptKey, lIv))
                {
                    string lResult;
                    using (var lMsDecrypt = new MemoryStream(lCipher))
                    {
                        using (var lCsDecrypt = new CryptoStream(lMsDecrypt,
                            lDecryptor, CryptoStreamMode.Read))
                        {
                            using (var lSrDecrypt = new StreamReader(lCsDecrypt))
                            {
                                lResult = lSrDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return lResult;
                }
            }
        }

        public static string Md5Encrypt(string input)
        {
            var lMd5 = new MD5CryptoServiceProvider();
            var lBytes = lMd5.ComputeHash(Encoding.GetEncoding(65001).GetBytes(input));
            var lSb = new StringBuilder(32);
            for (int i = 0; i < lBytes.Length; i++)
            {
                lSb.Append(lBytes[i].ToString("x").PadLeft(2, '0'));
            }
            return lSb.ToString();
        }
    }
}
