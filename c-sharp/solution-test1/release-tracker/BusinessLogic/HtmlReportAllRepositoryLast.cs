using release_tracker.LocalDataAccess;
using release_tracker.Models;
using release_tracker.Models.html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace release_tracker.BusinessLogic
{
    public class HtmlReportAllRepositoryLast
    {        
        public string FILE_OUTPUT = "";

        private Configuration configuration;

        List<HtmlRowReport> htmlRows = new List<HtmlRowReport>();

        public HtmlReportAllRepositoryLast(Configuration configuration)
        {
            this.configuration = configuration;            
            FILE_OUTPUT = string.Format("{0}{1}",
                configuration.HtmlReportPathOutput,
                "report.all.repositories.last.html");
        }

        private List<string> GetFeatureUIDs(List<ReportCompareRepositories> reportCompareRepositories)
        {
            List<string> featureUIDs = new List<string>();
            foreach (ReportCompareRepositories reportCompareRepository in reportCompareRepositories)
            {
                List<ReportFeatureDistinctStore> reportFeatures = reportCompareRepository.FeatureUpdated;
                foreach (ReportFeatureDistinctStore reportFeature in reportFeatures)
                {
                    featureUIDs.Add(reportFeature.FeatureId);
                }
            }

            featureUIDs = featureUIDs.Distinct().ToList();
            featureUIDs.Sort();
            return featureUIDs;
        }

        public void GenerateReport(List<ReportCompareRepositories> reportCompareRepositories)
        {
            BuildHtmlTables(reportCompareRepositories);
        }

        private string BuildHtmlTables(List<ReportCompareRepositories> reportCompareRepositories)
        {
            //var json = JsonSerializer.Serialize(reportCompareRepositories);

            List<string> featureIds = GetFeatureUIDs(reportCompareRepositories);
            foreach(string featureId in featureIds)
            {
                List<ReportCompareRepositories> withFeatureId = GetByFeatureId(featureId, reportCompareRepositories);
                foreach(ReportCompareRepositories reportCompareRepository in withFeatureId)
                {
                    ReportFeatureDistinctStore reportFeatureDistinctStore = reportCompareRepository.GetByFeatureId(featureId);
                    
                    HtmlRowReport? htmlRowReport = htmlRows.Find(x => x.GetHtmlFeatureRowReportByName(reportCompareRepository.RepositoryName1) != null);
                    if(htmlRowReport == null)
                    {
                        htmlRowReport = new HtmlRowReport();

                    } else
                    {

                    }
                    
                }
            }

            List<string> repositoryNames = new List<string>();
            foreach (ReportCompareRepositories reportCompareRepository in reportCompareRepositories)
            {
                repositoryNames.Add(reportCompareRepository.RepositoryName1);
                repositoryNames.Add(reportCompareRepository.RepositoryName2);                
            }
            repositoryNames = repositoryNames.Distinct().ToList();
            repositoryNames.Sort();

            string htmlRowsRepositoryNames = GetHtmlRepositoryName(repositoryNames);
            htmlRowsRepositoryNames = htmlRowsRepositoryNames + GetHtmlFieldColumnNames(repositoryNames);

            var table = string.Format("{0}{1}{2}",
                "<table border=\"1\" cellspacing=\"0\" cellpadding=\"1px\" style=\"border: 1px solid rgb(146, 140, 140); border-collapse: collapse;\">",
                htmlRowsRepositoryNames,
                "</table>");
            
            var filePath = ReleaseLocalDataAccess.WriteFile(FILE_OUTPUT, table);

            return filePath;
        }

        

        private List<ReportCompareRepositories> GetByFeatureId(string featureId, List<ReportCompareRepositories> reportCompareRepositories)
        {
            List <ReportCompareRepositories> result = new List<ReportCompareRepositories> ();
            foreach (ReportCompareRepositories reportCompareRepository in reportCompareRepositories)
            {
                List<ReportFeatureDistinctStore> featureUpdated = reportCompareRepository.FeatureUpdated;
                foreach (ReportFeatureDistinctStore featureDistinctStore in featureUpdated)
                {
                    if (featureDistinctStore.FeatureId.Equals(featureId))
                    {
                        result.Add(reportCompareRepository);
                    }
                }
            }
            return result;
        }

        public string GetHtmlFieldColumnNames(List<string> repositoryNames)
        {
            List<string> columnNames = new List<string>();
            columnNames.Add("Feature UID");
            columnNames.Add("Description");
            columnNames.Add("OffFor");
            columnNames.Add("OnFor");
            columnNames.Add("Version");
            columnNames.Add("Owner");

            List<string> htmlColumns = new List<string>();
            foreach (string repositoryName in repositoryNames)
            {
                List<string> columns = new List<string>();
                foreach (string name in columnNames)
                {
                    string htmlColumn = string.Format("<td>{0}</td>", name);
                    columns.Add(htmlColumn);
                }

                htmlColumns.Add(string.Join("\n", columns));
            }            
            
            return string.Format("{0}{1}{2}",
                "<tr style=\"color: white; background-color: #7aacbb; text-align: center; font-weight: bold;\">",
                string.Join("\n", htmlColumns),
                "</tr>");
        }
        public string GetHtmlRepositoryName(List<string> repositoryNames)
        {
            List<string> columns = new List<string>();
            foreach (var repositoryName in repositoryNames)
            {
                string htmlColumn = string.Format("<td colspan =\"{0}\">{1}</td>", "6", repositoryName);
                columns.Add(htmlColumn);
            }

            return string.Format("{0}{1}{2}",
                "<tr style=\"color: white; background-color: #00bfff; text-align: center; font-weight: bold;\">",
                string.Join("\n", columns),
                "</tr>");
        }
    }
}
