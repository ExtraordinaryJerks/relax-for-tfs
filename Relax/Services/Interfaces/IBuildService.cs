using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Relax.Model;

namespace Relax.Services.Interfaces
{
    [ServiceContract(Namespace = "http://localhost")]
    public interface IBuildService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Build/Queue/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        bool QueueBuild(string id, RelaxBuildDefinition entity);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Build/{teamProjectId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        ICollection<RelaxBuildDefinition> GetBuilds(string teamProjectId, BaseEntity entity);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/Build/{teamProjectId}/{id}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        RelaxBuildDefinition GetBuild(string teamProjectId, string id, BaseEntity entity);


    }
}