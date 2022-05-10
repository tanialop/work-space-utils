using Microsoft.Extensions.Configuration;
using release_tracker.LocalDataAccess;
using release_tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace release_tracker.BusinessLogic
{
    public class HtmlReport
    {
        public static string FILE_TEMPLATE = "";
        public static string FILE_OUTPUT_SAME_SERVER = "";

        private readonly IConfiguration configuration;

        public HtmlReport(IConfiguration configuration)
        {
            this.configuration = configuration;
            FILE_TEMPLATE =(string) this.configuration["htmlFileTemplate"];
            FILE_OUTPUT_SAME_SERVER = string.Format("{0}{1}", (string)this.configuration["htmlFileOutput"], "report.same.server.html");
        }

        public string generateReportSameRepository(List<ReportRelease> reportReleases)
        {
            string filePath = "";

            string html = ReleaseLocalDataAccess.ReadFile(FILE_TEMPLATE);
            BuildHtmlTable(html, reportReleases);
            //GetHtmlPage(html);
            //GetTableRowColumnNames(html);
            //GetTableColumnRepositoryName(html);
            //GetTableColumnFeatureId(html);
            //GetTableColumnFieldName(html);
            //GetTableColumnUpdated(html);
            //GetTableColumnDeleted(html);
            //GetTableColumnFeatureDeleted(html);


            return filePath;

        }

        private string BuildHtmlTable(string html, List<ReportRelease> reportReleases)
        {
            string htmlFieldNameTemplate = GetTableColumnFieldName(html);
            string htmlFeatureUpdatedTemplate = GetTableColumnUpdated(html);            
            string htmlWasDeletedTemplate = GetTableColumnDeleted(html);
            
            string htmlColumnFeatureId = GetTableColumnFeatureId(html);
            
            
            List<string> columnValues = new List<string>();            
            columnValues.Add(GetTableRowColumnNames(html));

            List<string> rowRepositories = new List<string>();
            
            //ReportRelease release = reportReleases[0];
            foreach (ReportRelease release in reportReleases)
            {
                var htmlReportRepository = GetHtmlRowRepositoryChangedFeatures(release, html);

                rowRepositories.Add(htmlReportRepository);
            }
            string table = string.Format("{0}{1}{2}{3}", GetTableDefinition(html), GetTableRowColumnNames(html), "{{tableBody}}", "</table>");
            table = table.Replace("{{tableBody}}", string.Join("", rowRepositories));

            ReleaseLocalDataAccess.WriteFile(FILE_OUTPUT_SAME_SERVER, table);

            return table;

            //columnValues = new List<string>();

            //firstColumnRepositoryName = GetTableColumnRepositoryName(html)
            //    // .Replace("{{rowspanrepositoryName}}", rowValues.Count + "") // it will be replace as last step.
            //    .Replace("{{repositoryName}}", release.RepositoryName);
            //columnValues.Add(firstColumnRepositoryName);

            //string htmlFeatureDeletedTemplate = GetTableColumnFeatureDeleted(html);
            //foreach (Feature feature in release.DeletedFeatures)
            //{                    
            //    var htmlColumnFeatureIdValue =  htmlColumnFeatureId
            //        //.Replace("{{rowspanFeatureID}}", "0")
            //        .Replace("{{featureID}}", feature.Id + "");

            //    if (columnValues.Count == 0)
            //    {
            //        columnValues.Add(htmlColumnFeatureIdValue);
            //    }

            //    var htmlColumnFieldNameValue = htmlFeatureDeletedTemplate
            //            .Replace("{{description}}", feature.Description)
            //            .Replace("{{owner}}", feature.Owner)
            //            .Replace("{{version}}", feature.ReleaseVersion)
            //            .Replace("{{offFor}}", feature.StrategyOffFor)
            //            .Replace("{{onFor}}", feature.StrategyOnFor);

            //    var htmlColumnWasDeletedValue = htmlWasDeletedTemplate
            //        .Replace("{{deleted}}", "true");



            //    columnValues.Add(htmlColumnFieldNameValue);
            //    columnValues.Add(htmlColumnWasDeletedValue);

            //    string rows = GetFirstRowAddingColumn(string.Join("", columnValues));
            //    rowRepository.Add(rows);
            //}

            //var htmlReportRepository = string.Join("", rowRepository).Replace("{{rowspanrepositoryName}}", rowRepository.Count + "");
            //rowRepositories.Add(htmlReportRepository);
            //}



            //string table = string.Format("{0}{1}{2}", GetTableDefinition(html), "{{tableBody}}", "</table>");
            //table = table.Replace("{{tableBody}}", string.Join("", rowRepositories));

            //ReleaseLocalDataAccess.WriteFile(FILE_OUTPUT_SAME_SERVER, table);

            //return table;
        }

        private string GetHtmlRowRepositoryChangedFeatures(ReportRelease release, string html)
        {
            var rowRepository = new List<string>();

            int numRows = 0;
            bool addRepositoryColumnName = true;
            foreach (FeatureUpdated featureUpdated in release.UpdatedFeatures)
            {
                var keyValue = GetPartialHtmlRowFeatureChanged(featureUpdated, html, addRepositoryColumnName);
                numRows = numRows + keyValue.Key;
                addRepositoryColumnName = false;

                rowRepository.Add(keyValue.Value);
            }

            var htmlReportRepository = string.Join("", rowRepository)
                .Replace("{{rowspanRepositoryName}}", numRows + "")
                .Replace("{{repositoryName}}", release.RepositoryName);
            return htmlReportRepository;
        }

        private KeyValuePair<int, string> GetPartialHtmlRowFeatureChanged(FeatureUpdated featureUpdated, string htmlTemplate, bool addRepositoryColumnName) {
            string htmlFieldNameTemplate = GetTableColumnFieldName(htmlTemplate);
            string htmlFeatureUpdatedTemplate = GetTableColumnUpdated(htmlTemplate);
            string htmlWasDeletedTemplate = GetTableColumnDeleted(htmlTemplate);

            string htmlColumnFeatureId = GetTableColumnFeatureId(htmlTemplate);

            bool featureIdWasAdded = false;
            bool repositoryColumnWasAdded = false;
            
            var htmlRowFeatures = new List<string>();
            // column Version
            if (featureUpdated.Version.Updated)
            {
                List<string> columns = new List<string>();
                var htmlFieldNameValue = htmlFieldNameTemplate
                    .Replace("{{fieldName}}", "Version");

                var htmlUpdatedValue = htmlFeatureUpdatedTemplate
                    .Replace("{{oldValue}}", featureUpdated.Version.OldValue)
                    .Replace("{{newValue}}", featureUpdated.Version.NewValue);

                var htmlDeletedValue = htmlWasDeletedTemplate.Replace("{{deleted}}", "false");

                if (!featureIdWasAdded)
                {
                    var featureColumn = htmlColumnFeatureId
                        //.Replace("{{rowspanFeatureID}}", columnValues.Count() + "")
                        .Replace("{{featureID}}", featureUpdated.Id + "");
                    columns.Add(featureColumn);
                    featureIdWasAdded = true;
                }

                if (!repositoryColumnWasAdded && addRepositoryColumnName)
                {
                    var htmlRepositoryName = GetTableColumnRepositoryName(htmlTemplate);
                    columns.Insert(0, htmlRepositoryName);
                    repositoryColumnWasAdded = true;
                }

                columns.Add(htmlFieldNameValue);
                columns.Add(htmlUpdatedValue);
                columns.Add(htmlDeletedValue);

                string row = GetFirstRowAddingColumn(String.Join("", columns));
                htmlRowFeatures.Add(row);
            }
            // column Description
            if (featureUpdated.Description.Updated)
            {
                var htmlFieldNameValue = htmlFieldNameTemplate
                    .Replace("{{fieldName}}", "Description");

                var htmlUpdatedValue = htmlFeatureUpdatedTemplate
                    .Replace("{{oldValue}}", featureUpdated.Description.OldValue)
                    .Replace("{{newValue}}", featureUpdated.Description.NewValue);

                var htmlDeletedValue = htmlWasDeletedTemplate.Replace("{{deleted}}", "false");

                List<string> columns = new List<string>();
                if (!featureIdWasAdded)
                {
                    var featureColumn = htmlColumnFeatureId
                        //.Replace("{{rowspanFeatureID}}", columnValues.Count() + "")
                        .Replace("{{featureID}}", featureUpdated.Id + "");
                    columns.Add(featureColumn);
                    featureIdWasAdded = true;
                }

                if (!repositoryColumnWasAdded && addRepositoryColumnName)
                {
                    var htmlRepositoryName = GetTableColumnRepositoryName(htmlTemplate);
                    columns.Insert(0, htmlRepositoryName);
                    repositoryColumnWasAdded = true;
                }

                columns.Add(htmlFieldNameValue);
                columns.Add(htmlUpdatedValue);
                columns.Add(htmlDeletedValue);

                string row = GetFirstRowAddingColumn(String.Join("", columns));
                htmlRowFeatures.Add(row);
            }
            // column Owner
            if (featureUpdated.Owner.Updated)
            {
                var htmlFieldNameValue = htmlFieldNameTemplate
                    .Replace("{{fieldName}}", "Owner");

                var htmlUpdatedValue = htmlFeatureUpdatedTemplate
                    .Replace("{{oldValue}}", featureUpdated.Owner.OldValue)
                    .Replace("{{newValue}}", featureUpdated.Owner.NewValue);

                var htmlDeletedValue = htmlWasDeletedTemplate.Replace("{{deleted}}", "false");

                var columns = new List<string>();
                if (!featureIdWasAdded)
                {
                    var featureColumn = htmlColumnFeatureId
                        //.Replace("{{rowspanFeatureID}}", columnValues.Count() + "")
                        .Replace("{{featureID}}", featureUpdated.Id + "");
                    columns.Add(featureColumn);
                    featureIdWasAdded = true;
                }

                if (!repositoryColumnWasAdded && addRepositoryColumnName)
                {
                    var htmlRepositoryName = GetTableColumnRepositoryName(htmlTemplate);
                    columns.Insert(0, htmlRepositoryName);
                    repositoryColumnWasAdded = true;
                }

                columns.Add(htmlFieldNameValue);
                columns.Add(htmlUpdatedValue);
                columns.Add(htmlDeletedValue);

                string row = GetFirstRowAddingColumn(String.Join("", columns));
                htmlRowFeatures.Add(row);
            }
            // column OffFor
            if (featureUpdated.OffFor.Updated)
            {
                var htmlFieldNameValue = htmlFieldNameTemplate
                    .Replace("{{fieldName}}", "OffFor");

                var htmlUpdatedValue = htmlFeatureUpdatedTemplate
                    .Replace("{{oldValue}}", featureUpdated.OffFor.OldValue)
                    .Replace("{{newValue}}", featureUpdated.OffFor.NewValue);

                var htmlDeletedValue = htmlWasDeletedTemplate.Replace("{{deleted}}", "false");

                var columns = new List<string>();
                if (!featureIdWasAdded)
                {
                    var featureColumn = htmlColumnFeatureId
                        //.Replace("{{rowspanFeatureID}}", columnValues.Count() + "")
                        .Replace("{{featureID}}", featureUpdated.Id + "");
                    columns.Add(featureColumn);
                    featureIdWasAdded = true;
                }

                if (!repositoryColumnWasAdded && addRepositoryColumnName)
                {
                    var htmlRepositoryName = GetTableColumnRepositoryName(htmlTemplate);
                    columns.Insert(0, htmlRepositoryName);
                    repositoryColumnWasAdded = true;
                }

                columns.Add(htmlFieldNameValue);
                columns.Add(htmlUpdatedValue);
                columns.Add(htmlDeletedValue);

                string row = GetFirstRowAddingColumn(String.Join("", columns));
                htmlRowFeatures.Add(row);
            }
            // column OnFor
            if (featureUpdated.OnFor.Updated)
            {
                var htmlFieldNameValue = htmlFieldNameTemplate
                    .Replace("{{fieldName}}", "OnFor");

                var htmlUpdatedValue = htmlFeatureUpdatedTemplate
                    .Replace("{{oldValue}}", featureUpdated.OnFor.OldValue)
                    .Replace("{{newValue}}", featureUpdated.OnFor.NewValue);

                var htmlDeletedValue = htmlWasDeletedTemplate.Replace("{{deleted}}", "false");

                var columns = new List<string>();
                if (!featureIdWasAdded)
                {
                    var featureColumn = htmlColumnFeatureId
                        //.Replace("{{rowspanFeatureID}}", columnValues.Count() + "")
                        .Replace("{{featureID}}", featureUpdated.Id + "");
                    columns.Add(featureColumn);
                    featureIdWasAdded = true;
                }

                if (!repositoryColumnWasAdded && addRepositoryColumnName)
                {
                    var htmlRepositoryName = GetTableColumnRepositoryName(htmlTemplate);
                    columns.Insert(0, htmlRepositoryName);
                    repositoryColumnWasAdded = true;
                }

                columns.Add(htmlFieldNameValue);
                columns.Add(htmlUpdatedValue);
                columns.Add(htmlDeletedValue);

                string row = GetFirstRowAddingColumn(String.Join("", columns));
                htmlRowFeatures.Add(row);
            }

            

            var htmlRowsFeature = string.Join("", htmlRowFeatures).Replace("{{rowspanFeatureID}}", htmlRowFeatures.Count() + "");            
            return new KeyValuePair<int, string>(htmlRowFeatures.Count, htmlRowsFeature);
        }

        private string GetHtmlPage(string html)
        {            

            int starTable = html.IndexOf("<!-- @table -->");
            int endTable = html.IndexOf("<!-- @table@ -->");
            int count = (endTable - starTable) + "<!-- @table@ -->".Length;

            var htmlTmpl = html.Remove(starTable, count);
            Console.WriteLine(htmlTmpl);
            return htmlTmpl;
        }

        private string GetTableRowColumnNames(string html) {            

            int startIndex = html.IndexOf("<!-- @ColumnNames -->");
            int endIndex = html.IndexOf("<!-- @ColumnNames@ -->");
            int count = (endIndex - startIndex) + "<!-- @ColumnNames@ -->".Length;

            var htmlTmpl = html.Substring(startIndex, count);
            Console.WriteLine(htmlTmpl);
            return htmlTmpl;
        }

        private string GetTableColumnRepositoryName(string html)
        {
            int startIndex = html.IndexOf("<!-- @RepositoryName -->");
            int endIndex = html.IndexOf("<!-- @RepositoryName@ -->");
            int count = (endIndex - startIndex) + "<!-- @RepositoryName@ -->".Length;

            var htmlTmpl = html.Substring(startIndex, count);
            Console.WriteLine(htmlTmpl);
            return htmlTmpl;
        }

        private string GetTableColumnFeatureId(string html)
        {
            int startIndex = html.IndexOf("<!-- @FeatureID -->");
            int endIndex = html.IndexOf("<!-- @FeatureID@ -->");
            int count = (endIndex - startIndex) + "<!-- @FeatureID@ -->".Length;

            var htmlTmpl = html.Substring(startIndex, count);
            Console.WriteLine(htmlTmpl);
            return htmlTmpl;
        }

        private string GetTableColumnFieldName(string html)
        {
            int startIndex = html.IndexOf("<!-- @FieldName -->");
            int endIndex = html.IndexOf("<!-- @FieldName@ -->");
            int count = (endIndex - startIndex) + "<!-- @FieldName@ -->".Length;

            var htmlTmpl = html.Substring(startIndex, count);
            Console.WriteLine(htmlTmpl);
            return htmlTmpl;
        }

        private string GetTableColumnUpdated(string html)
        {
            int startIndex = html.IndexOf("<!-- @Updated -->");
            int endIndex = html.IndexOf("<!-- @Updated@ -->");
            int count = (endIndex - startIndex) + "<!-- @Updated@ -->".Length;

            var htmlTmpl = html.Substring(startIndex, count);
            Console.WriteLine(htmlTmpl);
            return htmlTmpl;
        }

        private string GetTableColumnFeatureDeleted(string html)
        {
            int startIndex = html.IndexOf("<!-- @FeatureDeletedData  -->");
            int endIndex = html.IndexOf("<!-- @FeatureDeletedData@  -->");
            int count = (endIndex - startIndex) + "<!-- @FeatureDeletedData@  -->".Length;

            var htmlTmpl = html.Substring(startIndex, count);
            Console.WriteLine(htmlTmpl);
            return htmlTmpl;
        }

        private string GetTableColumnDeleted(string html)
        {
            int startIndex = html.IndexOf("<!-- @Deleted -->");
            int endIndex = html.IndexOf("<!-- @Deleted@ -->");
            int count = (endIndex - startIndex) + "<!-- @Deleted@ -->".Length;

            var htmlTmpl = html.Substring(startIndex, count);
            Console.WriteLine(htmlTmpl);
            return htmlTmpl;
        }

        private string GetTableDefinition(string html)
        {
            int startIndex = html.IndexOf("<!-- @tableDefinition -->");
            int endIndex = html.IndexOf("<!-- @tableDefinition@ -->");
            int count = (endIndex - startIndex) + "<!-- @tableDefinition@ -->".Length;

            var htmlTmpl = html.Substring(startIndex, count);
            Console.WriteLine(htmlTmpl);
            return htmlTmpl;
        }

        private string GetFirstRowAddingColumn(string htmlColumns)
        {
            return string.Format("<tr style=\"background - color: #8fd4eb;\">{0}</tr>", htmlColumns);
        }

        private string GetSecondRowAddingColumn(string htmlColumns)
        {
            return string.Format("<tr style=\"background - color: #b6e0ee;\">{0}</tr>", htmlColumns);
        }

        private string GetFirstRow() {
            return string.Format("<tr style=\"background - color: #8fd4eb;\">{{htmlColumns}}</tr>");
        }
        private string GetSecondRow()
        {
            return string.Format("<tr style=\"background - color: #b6e0ee;\">{{htmlColumns}}</tr>");
        }
    }
}
