using System.Collections.Generic;
using Relax.Model;

namespace Relax.Repository.Interfaces
{
    public interface IBuildRepository
    {
        ICollection<RelaxBuildDefinition> GetBuilds(string teamProject);
        RelaxBuildDefinition GetBuild(string teamProject, string id);
        RelaxBuildResults QueueBuild(RelaxBuildDefinition entity);
    }
}