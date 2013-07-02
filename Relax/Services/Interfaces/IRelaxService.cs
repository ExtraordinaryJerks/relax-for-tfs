using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Relax.Model;
using Relax.Model.Enums;

namespace Relax.Services.Interfaces
{
    [ServiceContract(Namespace = "http://localhost")]
    public interface IRelaxService : IBuildService, IWorkItemService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/TestConnection", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ConnectionStatus TestConnection(BaseEntity entity);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Projects",  RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ICollection<RelaxTeamProject> GetTeamProjects(BaseEntity entity);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/ProjectCollections", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ICollection<RelaxTeamProjectCollection> GetTeamProjectCollections(BaseEntity entity);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Version", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        string CheckVersion();

        //[OperationContract]
        //[WebInvoke(Method = "GET", UriTemplate = "/ProcessTemplate", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //string GetProcessTemplateName(BaseEntity entity);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/Register", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //String RegisterDevice(string userName, string password, string domain, string deviceId);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/Queries", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        //ICollection<RelaxTeamProject> GetProjectQueries(BaseEntity entity);
    }
}