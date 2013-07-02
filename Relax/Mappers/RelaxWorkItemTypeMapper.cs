using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Relax.Model;
using Relax.Model.Mapper.Interfaces;

namespace Relax.Mappers
{
    public class RelaxWorkItemTypeMapper : IMapper<RelaxWorkItemType,WorkItemType>
    {
        public ICollection<RelaxWorkItemType> Map(ICollection<WorkItemType> entity)
        {
            return entity.Select(Map).ToList();
        }

        public RelaxWorkItemType Map(WorkItemType entity)
        {
            return new RelaxWorkItemType {Name = entity.Name, Description = entity.Description, ProjectId = entity.Project.Id, IsDefault = entity.Name.Contains("Task")};
        }
    }
}