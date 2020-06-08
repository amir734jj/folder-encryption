using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Logic.Abstracts
{
    public class AbstractCrypto
    {
        private static readonly ILogger Log = NullLogger.Instance;

        private readonly Random _random = new Random();

        protected static byte[] GenerateRandomSalt()
        {
            var data = new byte[32];
            using var rng = new RNGCryptoServiceProvider();

            for (var i = 0; i < 10; i++)
            {
                rng.GetBytes(data);
            }

            return data;
        }

        protected enum LogType
        {
            Info = 0,
            Error,
            Warn,
            Debug,
            Fatal
        };

        protected void ConsoleLog(LogType logType, string message)
        {
            switch (logType)
            {
                case LogType.Info:
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Log.LogInformation(message);
                }
                    break;
                case LogType.Debug:
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Log.LogTrace(message);
                }
                    break;
                case LogType.Error:
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Log.LogError(message);
                }
                    break;
                case LogType.Fatal:
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Log.LogCritical(message);
                }
                    break;
                case LogType.Warn:
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Log.LogWarning(message);
                }
                    break;
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        protected string GenerateKey(int bytes)
        {
            var letters = new char[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u',
                'v', 'w', 'y', 'x', 'z'
            };
            var lettersUpper = new char[]
            {
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
                'V', 'W', 'Y', 'X', 'Z'
            };
            var numbers = new char[] {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
            var symbols = new char[] {'!', '@', '#', '$', '%', '&', '*', '=', ']', '[', '/', '+', '~'};
            var key = string.Empty;

            for (var i = 0; i < bytes; ++i)
            {
                var random = _random.Next(4);
                if (random == 0)
                {
                    var slot = _random.Next(26);
                    key += letters[slot];
                }
                else if (random == 1)
                {
                    var slot = _random.Next(10);
                    key += numbers[slot];
                }
                else if (random == 2)
                {
                    var slot = _random.Next(26);
                    key += lettersUpper[slot];
                }
                else
                {
                    var slot = _random.Next(13);
                    key += symbols[slot];
                }
            }

            return key;
        }
    }
}