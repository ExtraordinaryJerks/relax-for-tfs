using Relax.Model.Enums;

namespace Relax.Repository.Interfaces
{
    public interface IConnectionRepository
    {
        ConnectionStatus TestConnection();
    }
}