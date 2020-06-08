using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Logic
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            Console.ForegroundColor = ConsoleColor.White;
            var key = string.Empty;
            var blockSize = 0;

            Console.WriteLine();
            Console.WriteLine("==================== AES Encryption ====================");
            Console.WriteLine("               AES File Encryptor v1.0.0");
            Console.WriteLine("               BlockSizes: 128, 192, 256");
            Console.WriteLine("                 Developed by xSmoking");
            Console.WriteLine("       BitCoin: 17s6D2prtrB4iT8TCSFZZVgfBQZNcB7PVV");
            Console.WriteLine("========================================================\n");
            Console.WriteLine("Enumeration:");
            Console.WriteLine("    These options can be used to change the back-end settings before encrypting your files.");
            Console.WriteLine("");
            Console.WriteLine("    --set\t\tSet the key or block size for encryption");
            Console.WriteLine("    --key128\t\tGenerate a secure hash of 16 bytes (128 Bit)");
            Console.WriteLine("    --key256\t\tGenerate a secure hash of 32 bytes (256 Bit)");
            Console.WriteLine("    --key512\t\tGenerate a secure hash of 64 bytes (512 Bit)");
            Console.WriteLine("    --block\t\tSet the block size (128, 192, 256)");
            Console.WriteLine("    -k, --key\t\tUse your own key");
            Console.WriteLine("    -e, --encrypt\tEncrypt a folder/file");
            Console.WriteLine("    -d, --decrypt\tDecrypt a folder/file");
            Console.WriteLine("    --exit\t\tExit the program\n");
            Console.WriteLine("Examples:");
            Console.WriteLine("    --set --key128");
            Console.WriteLine("    --set --block 256");
            Console.WriteLine("    --set --autodelete true");
            Console.WriteLine("    --encrypt C:\\Users\\PDU\\Documents\\");
            Console.WriteLine("    -d C:\\Users\\PDU\\Documents\\\n");

            for (;;)
            {
                Console.Write(username + ">");
                var input = Console.ReadLine();

                if (input.Length > 0)
                {
                    var commandList = new List<string>(input.Split(' '));

                    if (commandList[0] == "--set")
                    {
                        if (commandList.Count > 1)
                        {
                            var valid = true;
                            if (commandList[1] == "--key128")
                                key = GenerateKey(16);
                            else if (commandList[1] == "--key256")
                                key = GenerateKey(32);
                            else if (commandList[1] == "--key512")
                                key = GenerateKey(64);
                            else if (commandList[1] == "-k" || commandList[1] == "--key")
                            {
                                if (commandList.Count > 2)
                                {
                                    if (commandList[2].Length > 0)
                                        key = commandList[2];
                                    else
                                    {
                                        valid = false;
                                        ConsoleLog(LogType.Error, "Key cannot be empty\n");
                                    }
                                }
                                else
                                {
                                    valid = false;
                                    ConsoleLog(LogType.Error, "Missing commands or parameters\n");
                                }
                            }
                            else if (commandList[1] == "--block")
                            {
                                valid = false;
                                if (commandList.Count > 2)
                                {
                                    if (commandList[2] == "128")
                                        blockSize = 128;
                                    else if (commandList[2] == "192")
                                        blockSize = 192;
                                    else if (commandList[2] == "256")
                                        blockSize = 256;
                                    else
                                        ConsoleLog(LogType.Error, "'" + commandList[2] + "' is not a valid block size\n");
                                }
                                else
                                    ConsoleLog(LogType.Error, "Missing commands or parameters\n");
                            }
                            else
                            {
                                valid = false;
                                ConsoleLog(LogType.Error, "'" + commandList[1] + "' is not a command\n");
                            }

                            if (valid)
                            {
                                ConsoleLog(LogType.Warn, "Save the key below to decrypt your files.");
                                ConsoleLog(LogType.Info, "Key: " + key + "\n");
                            }
                        }
                        else
                            ConsoleLog(LogType.Error, "Missing commands or parameters\n");
                    }
                    else if (commandList[0] == "-e" || commandList[0] == "--encrypt")
                    {
                        if (key.Length > 0)
                        {
                            if (commandList.Count > 1)
                            {
                                var keepGoing = true;
                                if (blockSize == 0)
                                {
                                    Console.Write("BlockSize not set. Wanna proceed with the default (128 Bit)? [y/n]>");
                                    var option = Console.ReadLine();
                                    if (option == "Y" || option == "y")
                                        blockSize = 128;
                                    else
                                    {
                                        ConsoleLog(LogType.Info, "Operation aborted by user\n");
                                        keepGoing = false;
                                    }
                                }

                                if (keepGoing)
                                {
                                    // Detect whether its a directory or a file
                                    var attr = new FileAttributes();
                                    if (File.Exists(commandList[1]) || Directory.Exists(commandList[1]))
                                        attr = File.GetAttributes(commandList[1]);

                                    // if its a directory
                                    if (attr.HasFlag(FileAttributes.Directory))
                                    {
                                        if (Directory.Exists(commandList[1]))
                                        {
                                            foreach (var file in Directory.EnumerateFiles(commandList[1]))
                                            {
                                                Console.Write("Encrypting " + file + " - result: ");
                                                AES_Encrypt(file, key, blockSize);
                                            }
                                            Console.WriteLine();
                                        }
                                        else
                                            ConsoleLog(LogType.Error, "'" + commandList[1] + "' is not a valid path\n");
                                    }
                                    // if its a file
                                    else
                                    {
                                        if (File.Exists(commandList[1]))
                                        {
                                            Console.Write("Encrypting " + commandList[1] + " - result: ");
                                            AES_Encrypt(commandList[1], key, blockSize);
                                            Console.WriteLine();
                                        }
                                        else
                                            ConsoleLog(LogType.Error, "'" + commandList[1] + "' is not a valid file\n");
                                    }
                                }
                            }
                            else
                                ConsoleLog(LogType.Error, "Missing commands or parameters\n");
                        }
                        else
                            ConsoleLog(LogType.Error, "You do not have any key assigned, set the key first\n");
                    }
                    else if (commandList[0] == "-d" || commandList[0] == "--decrypt")
                    {
                        if (key.Length > 0)
                        {
                            if (commandList.Count > 1)
                            {
                                var keepGoing = true;
                                if (blockSize == 0)
                                {
                                    Console.Write("BlockSize not set. Wanna proceed with the default (128 Bit)? [y/n]>");
                                    var option = Console.ReadLine();
                                    if (option == "Y" || option == "y")
                                        blockSize = 128;
                                    else
                                    {
                                        ConsoleLog(LogType.Info, "Operation aborted by user\n");
                                        keepGoing = false;
                                    }
                                }

                                if (keepGoing)
                                {
                                    // Detect whether its a directory or a file
                                    var attr = new FileAttributes();
                                    if (File.Exists(commandList[1]) || Directory.Exists(commandList[1]))
                                        attr = File.GetAttributes(commandList[1]);

                                    // if its a directory
                                    if (attr.HasFlag(FileAttributes.Directory))
                                    {
                                        if (Directory.Exists(commandList[1]))
                                        {
                                            foreach (var file in Directory.EnumerateFiles(commandList[1], "*.aes"))
                                            {
                                                Console.Write("Decrypting " + file + " - result: ");
                                                AES_Decrypt(file, key, blockSize);
                                            }
                                            Console.WriteLine();
                                        }
                                        else
                                            ConsoleLog(LogType.Error, "'" + commandList[1] + "' is not a valid path\n");
                                    }
                                    // if its a file
                                    else
                                    {
                                        if (File.Exists(commandList[1]))
                                        {
                                            Console.Write("Decrypting " + commandList[1] + " - result: ");
                                            AES_Decrypt(commandList[1], key, blockSize);
                                            Console.WriteLine();
                                        }
                                        else
                                            ConsoleLog(LogType.Error, "'" + commandList[1] + "' is not a valid file\n");
                                    }
                                }
                            }
                            else
                                ConsoleLog(LogType.Error, "Missing commands or parameters\n");
                        }
                        else
                            ConsoleLog(LogType.Error, "You do not have any key assigned, set the key first\n");
                    }
                    else if (commandList[0] == "--exit")
                        break;
                    else
                        ConsoleLog(LogType.Error, "'" + commandList[0] + "' is not a command\n");
                }
                else
                    ConsoleLog(LogType.Error, "No commands received\n");
            }
        }
    }
}