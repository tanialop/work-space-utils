using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace release_tracker.Models
{
    public class ReportRelease
    {
        [JsonPropertyName("repositoryName")]
        public string RepositoryName { get; set; }

        [JsonPropertyName("repositoryDescription")]
        public string RepositoryDescription { get; set; }
        
        [JsonPropertyName("deletedFeatures")]
        public List<Feature>? DeletedFeatures { get; set; }

        [JsonPropertyName("updatedFeatures")]
        public List<FeatureUpdated>? UpdatedFeatures { get; set; }

        public ReportRelease()
        {
            DeletedFeatures = new List<Feature>();
            UpdatedFeatures = new List<FeatureUpdated>();
        }

        public void AddDeletedFeature(Feature feature) {
            if (DeletedFeatures == null)
            {
                DeletedFeatures = new List<Feature>();
            }

            DeletedFeatures.Add(feature);
        }

        public string GetReportAsHtml() {
            return "";
        }

        public string GetReportAsJson() {

            foreach (FeatureUpdated featureUpdated in UpdatedFeatures)
            {
                featureUpdated.BuildDiff();
            }

            foreach (Feature deletedFeature in DeletedFeatures) { 

            }

            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string strJson = JsonSerializer.Serialize<ReportRelease>(this, opt);

            return strJson;
        }

        public void AddUpdatedFeature(FeatureUpdated featureUpdated)
        {
            if (UpdatedFeatures == null) { 
                UpdatedFeatures = new List<FeatureUpdated>();
            }

            UpdatedFeatures.Add(featureUpdated);
        }
        

        public void PrintOnConsole() {
            
            Console.WriteLine("============================ Feature Reports============================");
            Console.WriteLine("{0}\n", GetReportAsJson());
        }

        public static void PrintOnConsole(List<ReportRelease> reportReleases) {
            foreach (ReportRelease report in reportReleases)
            {
                report.GetReportAsJson();
            }

            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string strJson = JsonSerializer.Serialize(reportReleases, opt);
            Console.WriteLine("============================ Feature Reports============================");
            Console.WriteLine("{0}\n", strJson);
        }
    }
}
