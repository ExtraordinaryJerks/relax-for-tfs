using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using Relax.Model;

namespace Relax.Services.Interfaces
{
    [ServiceContract(Namespace = "http://localhost")]
    public interface IWorkItemService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "WorkItem/Attachment")]
        void PostAttachment(Stream fileContents);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "WorkItem/Types", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ICollection<RelaxWorkItemType> GetWorkItemTypes(RelaxTeamProject baseEntity);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "WorkItems/{page}/{pageSize}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ICollection<RelaxWorkItem> GetWorkItemsByPage(RelaxWorkItemType baseEntity, string page, string pageSize);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "WorkItems", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ICollection<RelaxWorkItem> GetWorkItems(RelaxWorkItemType baseEntity);
    
    }
}