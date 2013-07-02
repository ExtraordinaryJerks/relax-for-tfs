using System;

namespace Relax.Model
{
    public class RelaxTeamProjectCollection
    {
        public String Name { get; set; }
        public String Description { get; set; }

        public Guid Id { get; set; }
        public String ResourceType { get; set; }
    }
}