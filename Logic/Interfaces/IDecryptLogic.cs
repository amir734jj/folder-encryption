namespace Logic.Interfaces
{
    public interface IDecryptLogic
    {
        void AesDecrypt(string inputFile, string password, int blockSize);
    }
}