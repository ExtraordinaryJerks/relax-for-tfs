using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Relax.Mappers;
using Relax.Model;
using Relax.Model.Mapper;
using Relax.Repository.Interfaces;

namespace Relax.Repository
{
    public class WorkItemRepository : RepositoryBase, IWorkItemRepository
    {
        private String _tempDir;

        public WorkItemRepository(BaseEntity baseEntity)
            : this(baseEntity.Username, baseEntity.Password, baseEntity.Domain, baseEntity.TfsProjectCollection, baseEntity.TfsServerUrl)
        {
        }

        internal WorkItemRepository(string username, string password, string domain, string tfsProjectCollection, string tfsServerUrl)
            : base(username, password, domain, tfsProjectCollection, tfsServerUrl)
        {
        }
        public void Update(RelaxWorkItemAttachment entity)
        {
            var workItemStore = _tfs.GetService<WorkItemStore>();
            var item = workItemStore.GetWorkItem(entity.WorkItemId);
            item.PartialOpen();
            var fullPath = String.Concat(_tempDir, "\\", entity.Filename);
            File.WriteAllBytes(fullPath, entity.Attachment);
            item.Attachments.Add(new Attachment(fullPath));
            item.Save();
            foreach (var file in Directory.EnumerateFiles(_tempDir))
            {
                File.Delete(file);
            }
        }

        public ICollection<RelaxWorkItemType> GetWorkItemTypes(string projectName)
        {
            var workItemStore = _tfs.GetService<WorkItemStore>();
            return new RelaxWorkItemTypeMapper().Map(workItemStore.Projects[projectName].WorkItemTypes.Cast<WorkItemType>().ToList());
        }

        public ICollection<RelaxWorkItem> GetWorkItems(string projectName, string workItemType, int page, int pageSize)
        {
            var workItemStore = _tfs.GetService<WorkItemStore>();

            var query = Queries.WorkItemByProjectByType(projectName, workItemType);
            var workItems = workItemStore.Projects[projectName].Store.Query(query).Cast<WorkItem>().Skip((page - 1) * pageSize).Take(pageSize);

            return new RelaxWorkItemMapper(workItemType).Map(workItems.Cast<WorkItem>().ToList());
        }

        public ICollection<RelaxWorkItem> GetWorkItems(string projectName, string workItemType)
        {
            var workItemStore = _tfs.GetService<WorkItemStore>();

            var query = Queries.WorkItemByProjectByType(projectName, workItemType);
            var workItems = workItemStore.Projects[projectName].Store.Query(query).Cast<WorkItem>();

            return new RelaxWorkItemMapper().Map(workItems.Cast<WorkItem>().ToList());
        }

    }
}