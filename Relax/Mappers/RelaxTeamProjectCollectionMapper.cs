using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Framework.Client;
using Relax.Model;
using Relax.Model.Mapper.Interfaces;

namespace Relax.Mappers
{
    public class RelaxTeamProjectCollectionMapper : IMapper<RelaxTeamProjectCollection, CatalogNode>
    {
        public ICollection<RelaxTeamProjectCollection> Map(ICollection<CatalogNode> entity)
        {
            return entity.Select(Map).ToList();
        }

        public RelaxTeamProjectCollection Map(CatalogNode entity)
        {
            return new RelaxTeamProjectCollection { Id = entity.Resource.Identifier, Name = entity.Resource.DisplayName, Description = entity.Resource.Description, ResourceType = entity.Resource.ResourceType.DisplayName};

        }
    }
}