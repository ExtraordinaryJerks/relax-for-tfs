namespace Relax.Model
{
    public class RelaxNotificationMessage
    {
        public string CollapseKey { get; set; }
        public RelaxNotificationMessagePayload Payload { get; set; }
        public string RegistrationId { get; set; }
    }
}