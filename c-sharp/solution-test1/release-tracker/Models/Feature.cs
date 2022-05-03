using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace release_tracker.Models
{
    public class Feature
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("version")]
        public string ReleaseVersion { get; set; }

        [JsonPropertyName("owner")]
        public string Owner { get; set; }

        [JsonPropertyName("offFor")]
        public string StrategyOffFor { get; set; }

        [JsonPropertyName("onFor")]
        public string StrategyOnFor { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            var feature = (Feature)obj;

            return this.Description.Equals(feature.Description)
                && this.ReleaseVersion.Equals(feature.ReleaseVersion)
                && this.Owner.Equals(feature.Owner)
                && this.StrategyOffFor.Equals(feature.StrategyOffFor)
                && this.StrategyOnFor.Equals(feature.StrategyOnFor);
            
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
