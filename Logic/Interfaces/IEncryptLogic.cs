namespace Logic.Interfaces
{
    public interface IEncryptLogic
    {
        void AesEncrypt(string inputFile, string password, int blockSize);
    }
}