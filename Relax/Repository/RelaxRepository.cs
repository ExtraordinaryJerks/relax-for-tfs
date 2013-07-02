using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Relax.Mappers;
using Relax.Model;
using Relax.Model.Mapper;
using Relax.Repository.Interfaces;

namespace Relax.Repository
{
    public class RelaxRepository : RepositoryBase, IRelaxRepository
    {
        public RelaxRepository(BaseEntity baseEntity)
            : this(
                baseEntity.Username, baseEntity.Password, baseEntity.Domain, baseEntity.TfsProjectCollection,
                baseEntity.TfsServerUrl)
        {
        }

        internal RelaxRepository(string username, string password, string domain, string tfsProjectCollection,
                                 string tfsServerUrl)
            : base(username, password, domain, tfsProjectCollection, tfsServerUrl)
        {
        }

        public ICollection<RelaxTeamProject> GetTeamProjects()
        {
            var workItemStore = _tfs.GetService<WorkItemStore>();
            return new RelaxTeamProjectMapper().Map(workItemStore.Projects);
        }

        public ICollection<RelaxTeamProjectCollection> GetTeamProjectCollections()
        {
            CatalogNode catalog = _configuration.CatalogNode;
            ICollection<RelaxTeamProjectCollection> returnValue = new List<RelaxTeamProjectCollection>();

            var nodes = catalog.QueryChildren(new Guid[] { CatalogResourceTypes.ProjectCollection }, false, CatalogQueryOptions.None);

            new RelaxTeamProjectCollectionMapper().Map(nodes).ToList().ForEach(returnValue.Add);

            return returnValue;
        }

        public void GetProjectQueries(string projectName)
        {
            var workItemStore = _tfs.GetService<WorkItemStore>();
            var hierarchy = workItemStore.Projects[projectName].QueryHierarchy;

            foreach (QueryFolder item in hierarchy)
            {
                if (typeof(QueryFolder) == item.GetType())
                {

                }
                else
                {

                }
            }
        }

        public RelaxTeamProject GetTeamProject(int teamProjectId)
        {
            var workItemStore = _tfs.GetService<WorkItemStore>();
            return new RelaxTeamProjectMapper().Map(workItemStore.Projects.GetById(teamProjectId));
        }

        public String GetTeamProjectName(int teamProjectId)
        {
            return _tfs.GetService<WorkItemStore>().Projects.GetById(teamProjectId).Name;
        }


        public String GeProcessTemplateName(string projectName)
        {
            //THIS IS NOT FAIL PROOF, TEMPLATES MAY OR MAY NOT HAVE THIS PROPERTY DEFINED.
            var vcs = _tfs.GetService<VersionControlServer>();
            var ics = _tfs.GetService<ICommonStructureService>();
            ProjectProperty[] ProjectProperties = null;

            var p = vcs.GetTeamProject(projectName);
            string ProjectName = string.Empty;
            string ProjectState = String.Empty;
            int templateId = 0;
            ProjectProperties = null;

            ics.GetProjectProperties(p.ArtifactUri.AbsoluteUri, out ProjectName, out ProjectState, out templateId, out ProjectProperties);

            var templateName = ProjectProperties.Where(t => t.Name == "Process Template").Select(t => t.Value).FirstOrDefault();

            return templateName;
        }
    }
}