using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace release_tracker.Models
{
    public class ReportCompareRepositories
    {
        [JsonPropertyName("repository1")]
        public string RepositoryName1 { get; set; }

        [JsonPropertyName("repository2")]
        public string RepositoryName2 { get; set; }

        [JsonPropertyName("featureUpdated")]
        public List<ReportFeatureDistinctStore> FeatureUpdated { get; set; }
    }
}
