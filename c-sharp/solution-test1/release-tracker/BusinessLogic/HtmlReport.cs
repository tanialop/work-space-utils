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
        public string FILE_TEMPLATE = "";

        private readonly IConfiguration configuration;

        public HtmlReport(IConfiguration configuration)
        {
            this.configuration = configuration;
            FILE_TEMPLATE =(string) this.configuration["htmlFileTemplate"];
        }

        public string generateReportSameRepository(List<ReportRelease> reportReleases)
        {
            string filePath = "";

            string html = ReleaseLocalDataAccess.ReadFile(FILE_TEMPLATE);
            GetHtmlPage(html);
            GetTableColumnNames(html);
            GetTableColumnRepositoryName(html);
            GetTableColumnFeatureId(html);
            GetTableColumnFieldName(html);
            GetTableColumnUpdated(html);
            GetTableColumnDeleted(html);
            

            return filePath;

        }

        private string BuildHtmlTable(string html, List<ReportRelease> reportReleases)
        {
            string table = GetTableDefinition(html);
            table = table + GetTableColumnNames(html);

            string htmlFieldNameTemplate = GetTableColumnFieldName(html);
            string htmlFeatureUpdatedTemplate = GetTableColumnUpdated(html);
            string htmlFeatureDeletedTemplate = GetTableColumnFeatureDeleted(html);
            string htmlWasDeletedTemplate = GetTableColumnDeleted(html);
            foreach(ReportRelease release in reportReleases)
            {
                List<string> columnValues = new List<string>();
                foreach(FeatureUpdated featureUpdated in release.UpdatedFeatures)
                {
                    if (featureUpdated.Version.Updated)
                    {
                        var htmlFieldNameValue = htmlFieldNameTemplate
                            .Replace("{{fieldName}}", "Version");

                        var htmlUpdatedValue = htmlFeatureUpdatedTemplate
                            .Replace("{{oldValue}}", featureUpdated.Version.OldValue)
                            .Replace("{{newValue}}", featureUpdated.Version.NewValue);

                        var htmlDeletedValue = htmlWasDeletedTemplate.Replace("{{deleted}}", "false");

                        columnValues.Add(htmlFieldNameValue);
                        columnValues.Add(htmlUpdatedValue);
                        columnValues.Add(htmlDeletedValue);
                    }

                    if (featureUpdated.Description.Updated)
                    {
                        var htmlFieldNameValue = htmlFieldNameTemplate
                            .Replace("{{fieldName}}", "Description");

                        var htmlUpdatedValue = htmlFeatureUpdatedTemplate
                            .Replace("{{oldValue}}", featureUpdated.Description.OldValue)
                            .Replace("{{newValue}}", featureUpdated.Description.NewValue);

                        var htmlDeletedValue = htmlWasDeletedTemplate.Replace("{{deleted}}", "false");

                        columnValues.Add(htmlFieldNameValue);
                        columnValues.Add(htmlUpdatedValue);
                        columnValues.Add(htmlDeletedValue);
                    }

                    if (featureUpdated.Owner.Updated)
                    {
                        var htmlFieldNameValue = htmlFieldNameTemplate
                            .Replace("{{fieldName}}", "Owner");

                        var htmlUpdatedValue = htmlFeatureUpdatedTemplate
                            .Replace("{{oldValue}}", featureUpdated.Owner.OldValue)
                            .Replace("{{newValue}}", featureUpdated.Owner.NewValue);

                        var htmlDeletedValue = htmlWasDeletedTemplate.Replace("{{deleted}}", "false");

                        columnValues.Add(htmlFieldNameValue);
                        columnValues.Add(htmlUpdatedValue);
                        columnValues.Add(htmlDeletedValue);
                    }

                    if (featureUpdated.OffFor.Updated)
                    {
                        var htmlFieldNameValue = htmlFieldNameTemplate
                            .Replace("{{fieldName}}", "OffFor");

                        var htmlUpdatedValue = htmlFeatureUpdatedTemplate
                            .Replace("{{oldValue}}", featureUpdated.OffFor.OldValue)
                            .Replace("{{newValue}}", featureUpdated.OffFor.NewValue);

                        var htmlDeletedValue = htmlWasDeletedTemplate.Replace("{{deleted}}", "false");

                        columnValues.Add(htmlFieldNameValue);
                        columnValues.Add(htmlUpdatedValue);
                        columnValues.Add(htmlDeletedValue);
                    }

                    if (featureUpdated.OnFor.Updated)
                    {
                        var htmlFieldNameValue = htmlFieldNameTemplate
                            .Replace("{{fieldName}}", "OnFor");

                        var htmlUpdatedValue = htmlFeatureUpdatedTemplate
                            .Replace("{{oldValue}}", featureUpdated.OnFor.OldValue)
                            .Replace("{{newValue}}", featureUpdated.OnFor.NewValue);

                        var htmlDeletedValue = htmlWasDeletedTemplate.Replace("{{deleted}}", "false");

                        columnValues.Add(htmlFieldNameValue);
                        columnValues.Add(htmlUpdatedValue);
                        columnValues.Add(htmlDeletedValue);
                    }                  
                }

                foreach(Feature feature in release.DeletedFeatures)
                {
                    // we need a template for this, becuase we will need to display all field of old feature.
                }
            }

            table = table + "</table>";
            return table;
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

        private string GetTableColumnNames(string html) {            

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
            int startIndex = html.IndexOf("<!-- @Updated -->");
            int endIndex = html.IndexOf("<!-- @Updated@ -->");
            int count = (endIndex - startIndex) + "<!-- @Updated@ -->".Length;

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
    }
}
