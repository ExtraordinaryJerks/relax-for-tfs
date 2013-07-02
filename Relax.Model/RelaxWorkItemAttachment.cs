namespace Relax.Model
{
    public class RelaxWorkItemAttachment : BaseEntity
    {
        public int WorkItemId { get; set; }
        public string Filename { get; set; }
        public byte[] Attachment { get; set; }
    }
}