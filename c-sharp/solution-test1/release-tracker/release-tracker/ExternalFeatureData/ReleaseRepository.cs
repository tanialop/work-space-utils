using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace release_tracker.ExternalFeatureData
{
    public class ReleaseRepository
    {
        public ReleaseRepository() { }

        public ReleaseRepository(string url, string token) {
            Url = url;
            Token = token;
        }

        public String Url { get; set; }
        public String Token { get; set; }
    }
}
