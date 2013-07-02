using Relax.Model.Enums;

namespace Relax.Model.Interfaces
{
    public interface ICloudMessage
    {
        NotificationType NotificationType { get; set; }
        string Title { get; set; }
        string Message { get; set; }
    }
}