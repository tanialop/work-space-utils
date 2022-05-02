using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace release_tracker.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public String ReleaseVersion { get; set; }

        public String Owner { get; set; }

        public String StrategyOffFor { get; set; }

        public String StrategyOnFor { get; set; }
    }
}
