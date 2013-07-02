namespace Relax.Model
{
    public class BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }

        public string TfsProjectCollection { get; set; }
        public string TfsServerUrl { get; set; }
        public string TfsTeamProject { get; set; }
        public string RegistrationId { get; set; }
    }
}