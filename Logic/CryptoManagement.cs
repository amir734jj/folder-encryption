using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Dawn;
using Logic.Interfaces;
using Models;

namespace Logic
{
    public class CryptoManagement : ICryptoManagement
    {
        private readonly IKeyGenerationLogic _keyGenerationLogic;
        private readonly IEncryptLogic _encryptLogic;
        private readonly IDecryptLogic _decryptLogic;

        public CryptoManagement(IKeyGenerationLogic keyGenerationLogic, IEncryptLogic encryptLogic,
            IDecryptLogic decryptLogic)
        {
            _keyGenerationLogic = keyGenerationLogic;
            _encryptLogic = encryptLogic;
            _decryptLogic = decryptLogic;
        }

        private static bool EnsureCompleteAccess(string path)
        {
            var security = new FileSecurity(path,
                AccessControlSections.Owner |
                AccessControlSections.Group |
                AccessControlSections.Access);

            var info = new FileInfo(path);
            info.SetAccessControl(security);

            return info.Attributes.HasFlag(FileAccess.Read) && info.Attributes.HasFlag(FileAccess.Write);
        }

        public async Task Handle(CryptoRequest request)
        {
            Guard.Argument(request.Path).NotEmpty();
            Guard.Argument(request.Password).NotEmpty();
            Guard.Argument(request.UnlockDate).NotNull();
            Guard.Argument(request.Action).NotNull();

            var files = Directory.GetFiles(request.Path);
            var key = _keyGenerationLogic.GenerateKey(request);

            switch (request.Action)
            {
                case CryptoAction.Encryption:
                    await Task.WhenAll(files.Where(x => Path.GetExtension(x) != ".aes")
                        .Select(file => _encryptLogic.AesEncrypt(file, key)));
                    break;
                case CryptoAction.Decryption:
                    await Task.WhenAll(files.Where(x => Path.GetExtension(x) == ".aes")
                        .Select(file => _decryptLogic.AesDecrypt(file, key)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}