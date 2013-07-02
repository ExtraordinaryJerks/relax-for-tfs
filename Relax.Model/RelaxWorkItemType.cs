namespace Relax.Model
{
    public class RelaxWorkItemType : BaseEntity
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
    }
}