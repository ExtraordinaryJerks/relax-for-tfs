using System.Collections.Generic;
using Relax.Model;

namespace Relax.Repository.Interfaces
{
    public interface IWorkItemRepository
    {
        void Update(RelaxWorkItemAttachment entity);
        ICollection<RelaxWorkItem> GetWorkItems(string projectName, string workItemType, int page, int pageSize);
        ICollection<RelaxWorkItem> GetWorkItems(string projectName, string workItemType);
        ICollection<RelaxWorkItemType> GetWorkItemTypes(string projectName);

    }
}