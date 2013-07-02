using System;
using System.Collections.Generic;
using Relax.Model;

namespace Relax.Repository.Interfaces
{
    public interface IRelaxRepository
    {
        ICollection<RelaxTeamProject> GetTeamProjects();
        ICollection<RelaxTeamProjectCollection> GetTeamProjectCollections();
        void GetProjectQueries(string projectName);
        RelaxTeamProject GetTeamProject(int teamProjectId);
        String GetTeamProjectName(int teamProjectId);
    }
}