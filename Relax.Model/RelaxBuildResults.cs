using System;
using System.Collections.Generic;

namespace Relax.Model
{
    public class RelaxBuildResults
    {
        public String BuildName { get; set; }
        public String BuildStatus { get; set; }
        public ICollection<String> RegistrationIds { get; set; }
    }
}
