using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace release_tracker.Models
{
    public class DiffData
    {
        [JsonPropertyName("oldValue")]
        public string OldValue { get; set; }

        [JsonPropertyName("newValue")]
        public string NewValue { get; set; }

        [JsonPropertyName("updated")]
        public bool Updated { get; set; }
    }
}
