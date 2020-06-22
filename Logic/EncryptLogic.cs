using System;
using System.IO;
using System.Security.Cryptography;
using Logic.Abstracts;
using Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace Logic
{
    public class EncryptLogic : AbstractCrypto, IEncryptLogic
    {
        public void AesEncrypt(string inputFile, string password, int blockSize)
        {
            var salt = GenerateRandomSalt();
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            var fsCrypt = new FileStream(inputFile + ".aes", FileMode.Create);

            var aes = new RijndaelManaged
            {
                KeySize = 256, BlockSize = blockSize, Padding = PaddingMode.PKCS7
            };

            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            
            fsCrypt.Write(salt, 0, salt.Length);

            var cs = new CryptoStream(fsCrypt, aes.CreateEncryptor(), CryptoStreamMode.Write);
            var fsIn = new FileStream(inputFile, FileMode.Open);
            
            var buffer = new byte[1048576];

            try
            {
                int read;
                while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cs.Write(buffer, 0, read);
                }

                fsIn.Close();
            }
            catch (Exception ex)
            {
                Log(LogLevel.Error, "Error: " + ex.Message);
            }
            finally
            {
                File.Delete(inputFile);
                cs.Close();
                fsCrypt.Close();
            }
        }
    }
}