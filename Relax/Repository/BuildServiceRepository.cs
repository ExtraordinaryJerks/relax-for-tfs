using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.TeamFoundation.Build.Client;
using Relax.Mappers;
using Relax.Model;
using Relax.Model.Mapper;
using Relax.Repository.Interfaces;

namespace Relax.Repository
{
    public class BuildServiceRepository : RepositoryBase, IBuildRepository
    {
        public BuildServiceRepository(BaseEntity baseEntity)
            : this(baseEntity.Username, baseEntity.Password, baseEntity.Domain, baseEntity.TfsProjectCollection, baseEntity.TfsServerUrl)
        {
        }

        internal BuildServiceRepository(string username, string password, string domain, string tfsProjectCollection, string tfsServerUrl)
            : base(username, password, domain, tfsProjectCollection, tfsServerUrl)
        {
        }

        public ICollection<RelaxBuildDefinition> GetBuilds(string teamProject)
        {
            var build = _tfs.GetService<IBuildServer>();

            var definitions = build.QueryBuildDefinitions(teamProject);

            return new RelaxBuildDefinitionMapper().Map(definitions);

        }

        public RelaxBuildDefinition GetBuild(string teamProject, string id)
        {
            var build = _tfs.GetService<IBuildServer>();

            var definitions = build.QueryBuildDefinitions(teamProject).SingleOrDefault(p => p.Id == id);

            return new RelaxBuildDefinitionMapper().Map(definitions);

        }

        public RelaxBuildResults QueueBuild(RelaxBuildDefinition entity)
        {
            var build = _tfs.GetService<IBuildServer>();

            var definition = build.GetBuildDefinition(entity.Uri);
            var queuedBuild = build.QueueBuild(definition);

            while (queuedBuild.Status == QueueStatus.InProgress || queuedBuild.Status == QueueStatus.Queued)
            {
                Thread.Sleep(1000);
                queuedBuild.Refresh(QueryOptions.All);
            }

            return new RelaxBuildResults { BuildName = queuedBuild.BuildDefinition.Name, BuildStatus = String.Format("Build {0}", queuedBuild.Build.Status.ToString()), RegistrationIds = new List<string> { entity.RegistrationId } };
        }
    }
}