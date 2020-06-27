using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Logic.Interfaces;
using Microsoft.Extensions.Logging;

namespace Logic
{
    public class DecryptLogic : IDecryptLogic
    {
        private readonly ILogger<DecryptLogic> _logger;

        public DecryptLogic(ILogger<DecryptLogic> logger)
        {
            _logger = logger;
        }

        public async Task AesDecrypt(string inputFile, string password)
        {
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            var salt = new byte[32];

            var fsCrypt = new FileStream(inputFile, FileMode.Open);
            fsCrypt.Read(salt, 0, salt.Length);

            var aes = new RijndaelManaged {KeySize = 256, BlockSize = 128};
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            var fileName = Path.GetFileNameWithoutExtension(inputFile);
            var dirName = Path.GetDirectoryName(inputFile);
            var cs = new CryptoStream(fsCrypt, aes.CreateDecryptor(), CryptoStreamMode.Read);
            var fsOut = new FileStream(Path.Join(dirName, fileName), FileMode.Create);

            var buffer = new byte[1024 * 1024];

            try
            {
                int read;
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    await fsOut.WriteAsync(buffer, 0, read);
                }
                
                _logger.LogInformation($"Successfully decrypted: {fileName}");
            }
            catch (CryptographicException exCryptographicException)
            {
                _logger.Log(LogLevel.Critical, "CryptographicException error: " + exCryptographicException.Message);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Error: " + ex.Message);
            }

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Error by closing CryptoStream: " + ex.Message);
            }
            finally
            {
                File.Delete(inputFile);
                fsOut.Close();
                fsCrypt.Close();
            }
        }
    }
}