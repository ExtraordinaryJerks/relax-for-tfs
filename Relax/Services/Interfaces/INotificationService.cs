using Relax.Model;

namespace Relax.Services.Interfaces
{
    public interface INotificationService
    {
        void Notify(RelaxNotificationMessage relaxNotificationMessage);
    }
}