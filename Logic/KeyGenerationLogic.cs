using System.Security.Cryptography;
using System.Text;
using Logic.Interfaces;
using Models;

namespace Logic
{
    public class KeyGenerationLogic : IKeyGenerationLogic
    {
        public string GenerateKey(CryptoRequest request)
        {
            using var sha256Hash = SHA256.Create();

            // Convert the input string to a byte array and compute the hash.
            var data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(
                $"{request.Password}{request.Path}{request.UnlockDate.GetValueOrDefault().Ticks}"
            ));

            // Create a new StringBuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}