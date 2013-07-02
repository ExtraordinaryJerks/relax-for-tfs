namespace Relax.Model
{
    public class RelaxTeamProject : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WorkItemQuery { get; set; }
    }
}