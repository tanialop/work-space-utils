using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace release_tracker.Models
{
    public class Repository
    {
        public string name { get; set; }
        public string description { get; set; }
        public string fileUrlBeforeRelease { get; set; }
        public string fileUrlAfterRelease { get; set; }
        public string token { get; set; }
        public string pathLocalStore { get; set; }
    }
}
