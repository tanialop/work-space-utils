using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ReleaseFile {
    public String name { get; set; }
    public String location { get; set; }

    public String file { get; set; }

    private List<Feature> features = new List<Feature>(); 

    public List<Feature> GetFeatures() {
        if (this.features.Count > 0) {
            return this.features;
        }
        // List<Feature> features = new List<Feature>();
        var doc = new XmlDocument();
        doc.LoadXml(file);

        var json = JsonConvert.SerializeXmlNode(doc);
        JObject jsonData = JObject.Parse(json);        
        var jsonFeatures = jsonData["features"]["feature"]; // JArray
        
        foreach (JObject item in jsonFeatures) {
            Feature feature = new Feature();
            feature.Description = (string)item["@description"];
            feature.Id = (int)item["@uid"];
            feature.StrategyOffFor = GetStrategyOffFor((JObject)item["flipstrategy"]);
            feature.StrategyOnFor = GetStrategyOnFor((JObject)item["flipstrategy"]);
            feature.ReleaseVersion = (string)item["documentation"]["release"];
            feature.Owner = (string)item["documentation"]["owner"];

            this.features.Add(feature);
        }

        return this.features;
    }

    private String GetStrategyOffFor(JObject json) {
        JArray parameters = (JArray)json["param"];
        var data = parameters.FirstOrDefault(p => ((string)p["@name"]).Equals("off_for"));
        return (string)data["@value"];
    }

    private String GetStrategyOnFor(JObject json) {
        JArray parameters = (JArray)json["param"];
        var data = parameters.FirstOrDefault(p => ((string)p["@name"]).Equals("on_for"));
        return (string)data["@value"];
    }

    public List<String> GetAllFeatureVersions() {
        return GetFeatures().Select(feature => feature.ReleaseVersion).ToList();
    }

    public List<String> getDistinctFeatureVersions() {
        return GetAllFeatureVersions().Distinct().ToList();
    }
}