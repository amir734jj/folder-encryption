using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IDecryptLogic
    {
        Task AesDecrypt(string inputFile, string key);
    }
}