using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Relax.Model;
using Relax.Model.Mapper.Interfaces;

namespace Relax.Mappers
{
    public class RelaxTeamProjectMapper : IMapper<RelaxTeamProject, Project>
    {
        public ICollection<RelaxTeamProject> Map(ICollection<Project> entity)
        {
            return entity.Select(teamProject => new RelaxTeamProject() { Name = teamProject.Name }).ToList();
        }

        public RelaxTeamProject Map(Project entity)
        {
            return new RelaxTeamProject() { Name = entity.Name };
        }

        public ICollection<RelaxTeamProject> Map(ProjectCollection projects)
        {
            return (from Project project in projects select new RelaxTeamProject { Name = project.Name, Id = project.Id }).ToList();
        }
    }
}