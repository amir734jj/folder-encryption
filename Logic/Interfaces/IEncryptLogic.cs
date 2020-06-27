using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IEncryptLogic
    {
        Task AesEncrypt(string inputFile, string key);
    }
}