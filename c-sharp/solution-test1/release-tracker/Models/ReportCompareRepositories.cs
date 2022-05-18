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

        public ReportFeatureDistinctStore GetByFeatureId(string featureId)
        {
            
            if (FeatureUpdated == null)
            {
                return null;
            }

            ReportFeatureDistinctStore result = null;
            foreach (var store in FeatureUpdated)
            {
                if(store.FeatureId.Equals(featureId))
                {
                    result = store;
                    break;
                }
            }
            return result;
        }
    }
}
