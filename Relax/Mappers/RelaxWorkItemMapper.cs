using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Relax.Model;
using Relax.Model.Mapper.Interfaces;

namespace Relax.Mappers
{
    public class RelaxWorkItemMapper : IMapper<RelaxWorkItem, WorkItem>
    {
        private readonly string _workItemType;

        public RelaxWorkItemMapper() : this(string.Empty)
        {
        }

        public RelaxWorkItemMapper(string workItemType)
        {
            _workItemType = workItemType;
        }

        

        public ICollection<RelaxWorkItem> Map(ICollection<WorkItem> entity)
        {
            return entity.Select(Map).ToList();
        }

        public RelaxWorkItem Map(WorkItem entity)
        {
            return new RelaxWorkItem { Id = entity.Id, Title = entity.Title, Description = entity.Fields[GetDescriptionFieldName()].Value.ToString(), Type = entity.Type.Name, State = entity.State, AssignedTo = entity.Fields["Assigned To"].Value.ToString() };
        }

        private string GetDescriptionFieldName()
        {
            if (_workItemType == "Task")
            {
                return "Description HTML";
            }
            return "Description";
        }
    }
}