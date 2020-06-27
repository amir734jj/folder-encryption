using System.Threading.Tasks;
using Models;

namespace Logic.Interfaces
{
    public interface ICryptoManagement
    {
        Task Handle(CryptoRequest request);
    }
}