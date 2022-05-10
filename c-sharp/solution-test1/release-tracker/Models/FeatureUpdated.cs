using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace release_tracker.Models
{
    public class FeatureUpdated
    {
        [JsonIgnore]
        private Feature oldFeature;
        [JsonIgnore]
        private Feature newFeature;

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("description")]
        public DiffData Description { get; set; }

        [JsonPropertyName("version")]
        public DiffData Version { get; set; }

        [JsonPropertyName("owner")]
        public DiffData Owner { get; set; }

        [JsonPropertyName("offFor")]
        public DiffData OffFor { get; set; }

        [JsonPropertyName("onFor")]
        public DiffData OnFor { get; set; }

        public FeatureUpdated(Feature oldFeature, Feature newFeature) { 
            this.oldFeature = oldFeature;
            this.newFeature = newFeature;
            BuildDiff();
        }

        public FeatureUpdated BuildDiff() {
            Id = oldFeature.Id;

            Description = GetDescriptionDiff();
            Version = GetVersionDiff();
            Owner = GetOwnerDiff();
            OffFor = GetOffForDiff();
            OnFor = GetOnForDiff();

            return this;
        }

        private DiffData GetDescriptionDiff() {
            return new DiffData
            {
                OldValue = oldFeature.Description,
                NewValue = newFeature.Description,
                Updated = !(oldFeature.Description.Equals(newFeature.Description))
            };
        }

        private DiffData GetVersionDiff()
        {
            return new DiffData
            {
                OldValue = oldFeature.ReleaseVersion,
                NewValue = newFeature.ReleaseVersion,
                Updated = !(oldFeature.ReleaseVersion.Equals(newFeature.ReleaseVersion))
            };
        }

        private DiffData GetOwnerDiff()
        {
            return new DiffData
            {
                OldValue = oldFeature.Owner,
                NewValue = newFeature.Owner,
                Updated = !(oldFeature.Owner.Equals(newFeature.Owner))
            };
        }

        private DiffData GetOffForDiff()
        {
            return new DiffData
            {
                OldValue = oldFeature.StrategyOffFor,
                NewValue = newFeature.StrategyOffFor,
                Updated = !(oldFeature.StrategyOffFor.Equals(newFeature.StrategyOffFor))
            };
        }

        private DiffData GetOnForDiff()
        {
            return new DiffData
            {
                OldValue = oldFeature.StrategyOnFor,
                NewValue = newFeature.StrategyOnFor,
                Updated = !(oldFeature.StrategyOnFor.Equals(newFeature.StrategyOnFor))
            };
        }
    }
}
