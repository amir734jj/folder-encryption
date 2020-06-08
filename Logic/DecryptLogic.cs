using System;
using System.IO;
using System.Security.Cryptography;
using Logic.Abstracts;
using Logic.Interfaces;

namespace Logic
{
    public class DecryptLogic : AbstractCrypto, IDecryptLogic
    {
        public void AesDecrypt(string inputFile, string password, int blockSize)
        {
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            var salt = new byte[32];

            var fsCrypt = new FileStream(inputFile, FileMode.Open);
            fsCrypt.Read(salt, 0, salt.Length);

            var aes = new RijndaelManaged {KeySize = 256, BlockSize = blockSize};
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CFB;

            var fileExtension = Path.GetExtension(inputFile);
            var fileName = Path.GetFileNameWithoutExtension(inputFile);
            var dirName = Path.GetDirectoryName(inputFile);
            var cs = new CryptoStream(fsCrypt, aes.CreateDecryptor(), CryptoStreamMode.Read);
            var fsOut = new FileStream(dirName + "\\" + fileName, FileMode.Create);

            int read;
            var buffer = new byte[1048576];

            try
            {
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                    fsOut.Write(buffer, 0, read);
            }
            catch (CryptographicException exCryptographicException)
            {
                ConsoleLog(LogType.Fatal, "CryptographicException error: " + exCryptographicException.Message);
            }
            catch (Exception ex)
            {
                ConsoleLog(LogType.Error, "Error: " + ex.Message);
            }

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
                ConsoleLog(LogType.Error, "Error by closing CryptoStream: " + ex.Message);
            }
            finally
            {
                File.Delete(dirName + "\\" + fileName + fileExtension);
                fsOut.Close();
                fsCrypt.Close();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("success");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}