using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace Logic
{
    public class EncryptLogic : IEncryptLogic
    {
        private readonly ILogger<EncryptLogic> _logger;

        public EncryptLogic(ILogger<EncryptLogic> logger)
        {
            _logger = logger;
        }

        private static byte[] GenerateRandomSalt()
        {
            var data = new byte[32];
            using var rng = new RNGCryptoServiceProvider();

            for (var i = 0; i < 10; i++)
            {
                rng.GetBytes(data);
            }

            return data;
        }
        
        public async Task AesEncrypt(string inputFile, string password)
        {
            var salt = GenerateRandomSalt();
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            var fsCrypt = new FileStream(inputFile + ".aes", FileMode.Create);

            var aes = new RijndaelManaged
            {
                KeySize = 256, BlockSize = 128, Padding = PaddingMode.PKCS7
            };

            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            
            await fsCrypt.WriteAsync(salt, 0, salt.Length);

            var fileName = Path.GetFileNameWithoutExtension(inputFile);
            var cs = new CryptoStream(fsCrypt, aes.CreateEncryptor(), CryptoStreamMode.Write);
            var fsIn = new FileStream(inputFile, FileMode.Open);
            
            var buffer = new byte[1024*1024];

            try
            {
                int read;
                while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                {
                    await cs.WriteAsync(buffer, 0, read);
                }

                fsIn.Close();
                
                _logger.LogInformation($"Successfully encrypted: {fileName}");
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Error: " + ex.Message);
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