﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace release_tracker.Models
{
    public class ReportReleaseDistinctStore
    {
        [JsonPropertyName("repository1")]
        public string RepositoryName1 { get; set; }

        [JsonPropertyName("repository2")]
        public string RepositoryName2 { get; set; }

        [JsonPropertyName("featureId")]
        public string FeatureId { get; set; }

        [JsonPropertyName("description")]
        public DiffDistinctStore Description { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("owner")]
        public DiffDistinctStore Owner { get; set; }

        [JsonPropertyName("offFor")]
        public DiffDistinctStore OffFor { get; set; }

        [JsonPropertyName("onFor")]
        public DiffDistinctStore OnFor { get; set; }

    }
}
