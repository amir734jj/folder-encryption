using Models;

namespace Logic.Interfaces
{
    public interface IKeyGenerationLogic
    {
        string GenerateKey(CryptoRequest request);
    }
}