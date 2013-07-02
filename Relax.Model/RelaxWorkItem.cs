namespace Relax.Model
{
    public class RelaxWorkItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string State { get; set; }
        public string AssignedTo { get; set; }
    }
}