using System;

namespace Relax.Model
{
    public class RelaxBuildDefinition : BaseEntity
    {
        public string Id { get; set; }
        public Uri Uri { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
    }
}