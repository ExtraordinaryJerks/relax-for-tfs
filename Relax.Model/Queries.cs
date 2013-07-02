namespace Relax.Model
{
    public static class Queries
    {
        public static string WorkItemByProjectByType(string projectName, string workItemType)
        {
            return "SELECT [System.Id], [System.WorkItemType], [System.Title], [System.AssignedTo], [System.State] FROM WorkItems WHERE [System.TeamProject] = '" + projectName + "' AND [System.WorkItemType] = '" + workItemType + "' ";
        }
    }
}