using Relax.Model;
using Relax.Model.Enums;

namespace Relax.Repository
{
    public class ConnectionRepository : RepositoryBase
    {
        public ConnectionRepository(BaseEntity baseEntity)
            : this(baseEntity.Username, baseEntity.Password, baseEntity.Domain, baseEntity.TfsProjectCollection, baseEntity.TfsServerUrl)
        {
        }

        internal ConnectionRepository(string username, string password, string domain, string tfsProjectCollection, string tfsServerUrl)
            : base(username, password, domain, tfsProjectCollection, tfsServerUrl)
        {
        }

        public ConnectionStatus TestConnection()
        {
            return ConnectionStatus.ConnectedAndAuthenticated;
        }
    }
}
