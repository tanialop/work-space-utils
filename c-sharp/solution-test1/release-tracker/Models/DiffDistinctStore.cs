using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace release_tracker.Models
{
    public class DiffDistinctStore
    {
        public DiffDistinctStore(string value1, string value2)
        {
            ValueRepository1 = value1;
            ValueRepository2 = value2;
        }

        [JsonPropertyName("valueInRepository1")]
        public string ValueRepository1 { get; set; }

        [JsonPropertyName("valueInRepository2")]
        public string ValueRepository2 { get; set; }
    }
}
