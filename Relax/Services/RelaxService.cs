using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using Relax.Model;
using Relax.Model.Enums;
using Relax.Model.Services;
using Relax.Repository;
using Relax.Services.Interfaces;

namespace Relax.Services
{
    public class RelaxService : IRelaxService
    {
        private static EventLog _eventLog;
        private static INotificationService _notificationService;


        public RelaxService()
        {
            _eventLog = new EventLog { Source = "Relax for TFS", Log = "Application" };

            if (!EventLog.SourceExists(_eventLog.Source))
                EventLog.CreateEventSource(_eventLog.Source, _eventLog.Log);

            _notificationService = new NotificationService();
        }

        public void PostAttachment(Stream fileContents)
        {
            try
            {
                var multipartParser = new MultipartParser(fileContents);
                var jsonContent = multipartParser.Contents.First(m => m.PropertyName == "jsonData");
                if (jsonContent == null) throw new Exception("jsonData parameter must be populated.");
                var json = jsonContent.StringData
                    .Replace("/r", string.Empty)
                    .Replace("/n", string.Empty)
                    .Trim();
                var javaScriptSerializer = new JavaScriptSerializer();
                var relaxWorkItemAttachment = javaScriptSerializer.Deserialize<RelaxWorkItemAttachment>(json);
                relaxWorkItemAttachment.Attachment = multipartParser.FileContents;

                var repository = new WorkItemRepository(relaxWorkItemAttachment);
                repository.Update(relaxWorkItemAttachment);
            }
            catch (Exception e)
            {
                _eventLog.WriteEntry(e.Message, EventLogEntryType.Error);
            }
        }

        public ICollection<RelaxWorkItemType> GetWorkItemTypes(RelaxTeamProject baseEntity)
        {
            var repository = new WorkItemRepository(baseEntity);
            return repository.GetWorkItemTypes(baseEntity.TfsTeamProject);
        }

        public ICollection<RelaxWorkItem> GetWorkItemsByPage(RelaxWorkItemType baseEntity, string page, string pageSize)
        {
            ICollection<RelaxWorkItem> workItems = new List<RelaxWorkItem>();

            try
            {
                int pg;
                int pgSize;

                int.TryParse(page, out pg);
                int.TryParse(pageSize, out pgSize);

                var repository = new WorkItemRepository(baseEntity);
                if (pg != 0 && pgSize != 0)
                    workItems = repository.GetWorkItems(baseEntity.TfsTeamProject, baseEntity.Name, pg, pgSize);
                else
                    workItems = repository.GetWorkItems(baseEntity.TfsTeamProject, baseEntity.Name, 1, 6);
            }
            catch (Exception e)
            {
                _eventLog.WriteEntry(e.Message, EventLogEntryType.Error);
                return null;
            }
            return workItems;
        }

        public ICollection<RelaxWorkItem> GetWorkItems(RelaxWorkItemType baseEntity)
        {
            ICollection<RelaxWorkItem> workItems = new List<RelaxWorkItem>();

            try
            {

                var repository = new WorkItemRepository(baseEntity);

                workItems = repository.GetWorkItems(baseEntity.TfsTeamProject, baseEntity.Name);
            }
            catch (Exception e)
            {
                _eventLog.WriteEntry(e.Message, EventLogEntryType.Error);
                return null;
            }
            return workItems;
        }

        public bool QueueBuild(string id, RelaxBuildDefinition entity)
        {
            try
            {
                var repository = new BuildServiceRepository(entity);

                var queueBuild = new AsyncDelegates.AsyncQueueBuild(repository.QueueBuild);

                var callback = new AsyncCallback(Notification);

                IAsyncResult result = queueBuild.BeginInvoke(entity, callback, queueBuild);

            }
            catch (Exception e)
            {
                _eventLog.WriteEntry(e.Message, EventLogEntryType.Error);
            }

            return true;
        }

        public ICollection<RelaxBuildDefinition> GetBuilds(string teamProjectId, BaseEntity entity)
        {
            try
            {
                var repository = new BuildServiceRepository(entity);
                var relaxrepository= new RelaxRepository(entity);

                return repository.GetBuilds(relaxrepository.GetTeamProject(int.Parse(teamProjectId)).Name);
            }
            catch (Exception e)
            {
                _eventLog.WriteEntry(e.Message, EventLogEntryType.Error);
                return null;
            }
        }

        public RelaxBuildDefinition GetBuild(string teamProjectId, string id, BaseEntity entity)
        {
            try
            {
                var repository = new BuildServiceRepository(entity);
                var relaxrepository = new RelaxRepository(entity);

                return repository.GetBuild(relaxrepository.GetTeamProject(int.Parse(teamProjectId)).Name, id);
            }
            catch (Exception e)
            {
                _eventLog.WriteEntry(e.Message, EventLogEntryType.Error);
                return null;
            }
        }

        public ConnectionStatus TestConnection(BaseEntity entity)
        {
            var result = ConnectionStatus.ConnectedButUnableToAuthenticate;
            try
            {
                var repository = new ConnectionRepository(entity);
                result = repository.TestConnection();
            }
            catch (Exception e)
            {
                _eventLog.WriteEntry(e.Message, EventLogEntryType.Error);
            }
            return result;
        }

        public ICollection<RelaxTeamProject> GetTeamProjects(BaseEntity entity)
        {
            try
            {
                var repository = new RelaxRepository(entity);
                return repository.GetTeamProjects();
            }
            catch (Exception e)
            {
                _eventLog.WriteEntry(e.Message, EventLogEntryType.Error);
                return null;
            }
        }

        public ICollection<RelaxTeamProjectCollection> GetTeamProjectCollections(BaseEntity entity)
        {
            try
            {
                var repository = new RelaxRepository(entity);
                return repository.GetTeamProjectCollections();
            }
            catch (Exception e)
            {
                _eventLog.WriteEntry(e.Message, EventLogEntryType.Error);
                return null;
            }
        }

        public string CheckVersion()
        {
            try
            {
                return ConfigurationManager.AppSettings["RelaxForTfs.Version"];
            }
            catch (Exception e)
            {
                _eventLog.WriteEntry(e.Message, EventLogEntryType.Error);
                return null;
            }
        }

        //public string GetProcessTemplateName(BaseEntity entity)
        //{
        //    try
        //    {
        //        var repository = new RelaxRepository(entity);
        //        return repository.GeProcessTemplateName(entity.TfsTeamProject);
        //    }
        //    catch (Exception e)
        //    {
        //        _eventLog.WriteEntry(e.Message, EventLogEntryType.Error);
        //        return null;
        //    }
        //}

        private void Notification(IAsyncResult result)
        {
            var buildDelegate = result.AsyncState as AsyncDelegates.AsyncQueueBuild;
            if (buildDelegate != null)
            {
                var returnValue = buildDelegate.EndInvoke(result);

                _notificationService.Notify(new RelaxNotificationMessage{CollapseKey = "Build Status", 
                                                                              RegistrationId = returnValue.RegistrationIds.FirstOrDefault(), 
                                                                              Payload = new RelaxNotificationMessagePayload{Message = returnValue.BuildStatus, 
                                                                                                                            Title = returnValue.BuildName,
                                                                                                                            NotificationType = NotificationType.Notify}
                                                                              });
            }
        }
    }
}