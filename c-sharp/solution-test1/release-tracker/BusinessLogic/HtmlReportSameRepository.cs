﻿using Microsoft.Extensions.Configuration;
using release_tracker.LocalDataAccess;
using release_tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace release_tracker.BusinessLogic
{
    public class HtmlReportSameRepository
    {
        public static string FILE_TEMPLATE = "";
        public static string FILE_OUTPUT = "";

        private readonly IConfiguration configuration;

        public HtmlReportSameRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            FILE_TEMPLATE =(string) this.configuration["htmlFileTemplateSameRepository"];
            FILE_OUTPUT = string.Format("{0}{1}", (string)this.configuration["htmlFileOutput"], "report.same.repository.html");
        }

        public string GenerateReportSameRepository(List<ReportRelease> reportReleases)
        {
            string html = ReleaseLocalDataAccess.ReadFile(FILE_TEMPLATE);

            string table = BuildHtmlTableSameRepository(html, reportReleases);

            var filePath = ReleaseLocalDataAccess.WriteFile(FILE_OUTPUT, table);

            return filePath;

        }

        private string BuildHtmlTableSameRepository(string html, List<ReportRelease> reportReleases)
        {   
            List<string> columnValues = new List<string>();            
            columnValues.Add(GetTableRowColumnNames(html));

            List<string> rowRepositories = new List<string>();            
            
            foreach (ReportRelease release in reportReleases)
            {
                // Rows for updated features.
                KeyValuePair<int, string> keyValueUpdatedFeature = GetHtmlRowRepositoryChangedFeatures(release, html);
                var htmlRowsUpdatedFeatures = keyValueUpdatedFeature.Value;
                var numRowsByRepository = keyValueUpdatedFeature.Key;

                // rowRepositories.Add(htmlReportRepository);

                // Rows for deleted features.
                var addRepositoryColumnName = false;
                if (numRowsByRepository == 0)
                {
                    addRepositoryColumnName = true;
                }
                KeyValuePair<int, string> keyValueDeletedFeature = GetHtmlRowsRepositoryDeletedFeatures(release, html, addRepositoryColumnName);
                var htmlRowsDeletedFeatures = keyValueDeletedFeature.Value;
                numRowsByRepository = numRowsByRepository + keyValueDeletedFeature.Key;                                

                if(keyValueUpdatedFeature.Key > 0)
                {
                    htmlRowsUpdatedFeatures = string.Join("", htmlRowsUpdatedFeatures)
                    .Replace("{{rowspanRepositoryName}}", numRowsByRepository + "")
                    .Replace("{{repositoryName}}", release.RepositoryName);
                    rowRepositories.Add(htmlRowsUpdatedFeatures);
                } else
                {
                    htmlRowsDeletedFeatures = string.Join("", htmlRowsDeletedFeatures)
                    .Replace("{{rowspanRepositoryName}}", numRowsByRepository + "")
                    .Replace("{{repositoryName}}", release.RepositoryName);
                }
                
                if(keyValueDeletedFeature.Key > 0)
                {                   

                    rowRepositories.Add(htmlRowsDeletedFeatures);
                }                                
            }

            string table = string.Format("{0}{1}{2}{3}", GetTableDefinition(html), GetTableRowColumnNames(html), "{{tableBody}}", "</table>");
            table = table.Replace("{{tableBody}}", string.Join("", rowRepositories));            

            return table;
        }

        public KeyValuePair<int, string> GetHtmlRowsRepositoryDeletedFeatures(ReportRelease release, string html, bool addRepositoryColumnName)
        {
            var rowRepository = new List<string>();

            string htmlColumnFeatureId = GetTableColumnFeatureId(html);
            string htmlFeatureDeletedTemplate = GetTableColumnFeatureDeleted(html);
            string htmlWasDeletedTemplate = GetTableColumnDeleted(html);

            
            foreach (Feature feature in release.DeletedFeatures)
            {
                var columns = new List<string>();                

                if (addRepositoryColumnName)
                {
                    var htmlRepositoryName = GetTableColumnRepositoryName(html)
                        .Replace("{{repositoryName}}", release.RepositoryName);
                    columns.Add(htmlRepositoryName);
                }

                var htmlColumnFeatureIdValue = htmlColumnFeatureId
                    //.Replace("{{rowspanFeatureID}}", "0")
                    .Replace("{{featureID}}", feature.Id + "");
                columns.Add(htmlColumnFeatureIdValue);

                var htmlColumnFieldNameValue = htmlFeatureDeletedTemplate
                        .Replace("{{description}}", feature.Description)
                        .Replace("{{owner}}", feature.Owner)
                        .Replace("{{version}}", feature.ReleaseVersion)
                        .Replace("{{offFor}}", feature.StrategyOffFor)
                        .Replace("{{onFor}}", feature.StrategyOnFor);

                var htmlColumnWasDeletedValue = htmlWasDeletedTemplate
                    .Replace("{{deleted}}", "true");



                columns.Add(htmlColumnFieldNameValue);
                columns.Add(htmlColumnWasDeletedValue);

                string rows = GetFirstRowAddingColumn(string.Join("", columns));
                rowRepository.Add(rows);
            }

            return new KeyValuePair<int, string>(rowRepository.Count, string.Join("", rowRepository));            
        }

        private KeyValuePair<int, string> GetHtmlRowRepositoryChangedFeatures(ReportRelease release, string html)
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

            //var htmlReportRepository = string.Join("", rowRepository)
            //    .Replace("{{rowspanRepositoryName}}", numRows + "")
            //    .Replace("{{repositoryName}}", release.RepositoryName);
            return new KeyValuePair<int, string>(numRows, string.Join("", rowRepository));
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
    }
}
