using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;
using Relax.Model;
using Relax.Model.Mapper.Interfaces;

namespace Relax.Mappers
{
    public class RelaxBuildDefinitionMapper : IMapper<RelaxBuildDefinition, IBuildDefinition>
    {
        public ICollection<RelaxBuildDefinition> Map(ICollection<IBuildDefinition> entity)
        {
            return entity.Select(buildDefinition => new RelaxBuildDefinition() { Uri = buildDefinition.Uri, Id = buildDefinition.Id, Name = buildDefinition.Name, Enabled = buildDefinition.Enabled }).ToList();
        }

        public RelaxBuildDefinition Map(IBuildDefinition entity)
        {
            return new RelaxBuildDefinition() { Uri = entity.Uri, Id = entity.Id, Name = entity.Name, Enabled = entity.Enabled };
        }
    }
}