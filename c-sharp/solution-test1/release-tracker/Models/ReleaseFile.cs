using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace release_tracker.Models
{
    public class ReleaseFile
    {
        public String Name { get; set; }
        public String Location { get; set; }

        public String File { get; set; }

        private List<Feature> features = new List<Feature>();

        public List<Feature> GetFeatures()
        {
            if (this.features.Count > 0)
            {
                return this.features;
            }
            // List<Feature> features = new List<Feature>();
            var doc = new XmlDocument();
            doc.LoadXml(File);

            var json = JsonConvert.SerializeXmlNode(doc);
            JObject jsonData = JObject.Parse(json);
            var jsonFeatures = jsonData["features"]["feature"]; // JArray

            foreach (JObject item in jsonFeatures)
            {
                Feature feature = new Feature();
                feature.Description = (string)item["@description"];
                feature.Id = (string)item["@uid"];
                feature.StrategyOffFor = GetStrategyOffFor((JObject)item["flipstrategy"]);
                feature.StrategyOnFor = GetStrategyOnFor((JObject)item["flipstrategy"]);
                feature.ReleaseVersion = (string)item["documentation"]["release"];
                feature.Owner = (string)item["documentation"]["owner"];

                this.features.Add(feature);
            }

            return this.features;
        }

        private String GetStrategyOffFor(JObject json)
        {
            JArray parameters = (JArray)json["param"];
            var data = parameters.FirstOrDefault(p => ((string)p["@name"]).Equals("off_for"));
            return (string)data["@value"];
        }

        private String GetStrategyOnFor(JObject json)
        {
            JArray parameters = (JArray)json["param"];
            var data = parameters.FirstOrDefault(p => ((string)p["@name"]).Equals("on_for"));
            return (string)data["@value"];
        }

        public List<String> GetAllFeatureVersions()
        {
            return GetFeatures().Select(feature => feature.ReleaseVersion).ToList();
        }

        public List<String> getDistinctFeatureVersions()
        {
            return GetAllFeatureVersions().Distinct().ToList();
        }

        public List<Feature> GetFeatureByVersion(string version) { 
            return GetFeatures().FindAll(feature => feature.ReleaseVersion == version).ToList();
        }
    }
}
