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
    public class HtmlReportAllRepository
    {
        public static string FILE_TEMPLATE = "";
        public static string FILE_OUTPUT = "";

        private readonly IConfiguration configuration;

        public HtmlReportAllRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
            FILE_TEMPLATE =(string) this.configuration["htmlFileTemplateAllRepository"];
            FILE_OUTPUT = string.Format("{0}{1}", (string)this.configuration["htmlFileOutput"], "report.all.repositories.html");
        }

        public string GenerateReportAllRepositories(List<ReportCompareRepositories> reportCompareRepositories)
        {
            string html = ReleaseLocalDataAccess.ReadFile(FILE_TEMPLATE);

            string table = BuildHtmlTableAllRepositories(html, reportCompareRepositories);

            var filePath = ReleaseLocalDataAccess.WriteFile(FILE_OUTPUT, table);

            return filePath;

        }

        private string BuildHtmlTableAllRepositories(string html, List<ReportCompareRepositories> reportCompareRepositories)
        {   
            List<string> columnValues = new List<string>();            
            columnValues.Add(GetTableRowColumnNames(html));

            List<string> rowRepositories = new List<string>();
            
            foreach (ReportCompareRepositories reportCompareRepository in reportCompareRepositories)
            {
                KeyValuePair<int, string> data = GetHtmlRows(reportCompareRepository, html, true);
                rowRepositories.Add(data.Value);
            }

            string table = string.Format("{0}{1}{2}{3}", GetTableDefinition(html), GetTableRowColumnNames(html), "{{tableBody}}", "</table>");
            table = table.Replace("{{tableBody}}", string.Join("", rowRepositories));            

            return table;
        }

        public KeyValuePair<int, string> GetHtmlRows(ReportCompareRepositories reportCompareRepository, string html, bool addColumnRepositoryName)
        {
            var htmlRows = new List<string>();

            var numRows = 0;
            foreach(ReportFeatureDistinctStore data in reportCompareRepository.FeatureUpdated)
            {

                var htmlColumnFieldNameTmpl = GetTableColumnFieldName(html);
                var htmlColumnValueTmpl = GetTableColumnFieldValue(html);
                var htmlColumnUpdatedTmpl = GetTableColumnUpdated(html);
                var htmlColumnFeatureIdTmpl = GetTableColumnFeatureId(html);
                var htmlColumnRepositoryName = GetTableColumnRepositoryName(html);

                var rows = new List<string>();
                

                if (data.OffFor.Updated)
                {
                    var columns = new List<string>();
                    if (addColumnRepositoryName)
                    {
                        columns.Add(htmlColumnRepositoryName
                            .Replace("{{repositoryName1}}", reportCompareRepository.RepositoryName1)
                            .Replace("{{repositoryName2}}", reportCompareRepository.RepositoryName2));
                        addColumnRepositoryName = false;
                    }

                    columns.Add(htmlColumnFeatureIdTmpl.Replace("{{featureID}}", data.FeatureId));

                    columns.Add(htmlColumnFieldNameTmpl.Replace("{{fieldName}}", "OffFor"));

                    columns.Add(htmlColumnValueTmpl
                        .Replace("{{valueInRepository1}}", data.OffFor.ValueRepository1)
                        .Replace("{{valueInRepository2}}", data.OffFor.ValueRepository2));

                    columns.Add(htmlColumnUpdatedTmpl.Replace("{{updated}}", "true"));

                    rows.Add(GetFirstRowAddingColumn(string.Join("", columns)));
                }
                if (data.OnFor.Updated)
                {
                    var columns = new List<string>();

                    if (addColumnRepositoryName)
                    {
                        columns.Add(htmlColumnRepositoryName
                            .Replace("{{repositoryName1}}", reportCompareRepository.RepositoryName1)
                            .Replace("{{repositoryName2}}", reportCompareRepository.RepositoryName2));
                        addColumnRepositoryName = false;
                    }

                    columns.Add(htmlColumnFeatureIdTmpl.Replace("{{featureID}}", data.FeatureId));

                    columns.Add(htmlColumnFieldNameTmpl.Replace("{{fieldName}}", "OnFor"));

                    columns.Add(htmlColumnValueTmpl
                        .Replace("{{valueInRepository1}}", data.OnFor.ValueRepository1)
                        .Replace("{{valueInRepository2}}", data.OnFor.ValueRepository2));

                    columns.Add(htmlColumnUpdatedTmpl.Replace("{{updated}}", "true"));

                    rows.Add(GetFirstRowAddingColumn(string.Join("", columns)));
                }
                if (data.Owner.Updated)
                {
                    var columns = new List<string>();

                    if (addColumnRepositoryName)
                    {
                        columns.Add(htmlColumnRepositoryName
                            .Replace("{{repositoryName1}}", reportCompareRepository.RepositoryName1)
                            .Replace("{{repositoryName2}}", reportCompareRepository.RepositoryName2));
                        addColumnRepositoryName = false;
                    }

                    columns.Add(htmlColumnFeatureIdTmpl.Replace("{{featureID}}", data.FeatureId));

                    columns.Add(htmlColumnFieldNameTmpl.Replace("{{fieldName}}", "Owner"));

                    columns.Add(htmlColumnValueTmpl
                        .Replace("{{valueInRepository1}}", data.Owner.ValueRepository1)
                        .Replace("{{valueInRepository2}}", data.Owner.ValueRepository2));

                    columns.Add(htmlColumnUpdatedTmpl.Replace("{{updated}}", "true"));

                    rows.Add(GetFirstRowAddingColumn(string.Join("", columns)));
                }
                
                if (data.Description.Updated)
                {
                    var columns = new List<string>();
                    if (addColumnRepositoryName)
                    {
                        columns.Add(htmlColumnRepositoryName
                            .Replace("{{repositoryName1}}", reportCompareRepository.RepositoryName1)
                            .Replace("{{repositoryName2}}", reportCompareRepository.RepositoryName2));
                        addColumnRepositoryName = false;
                    }

                    columns.Add(htmlColumnFeatureIdTmpl.Replace("{{featureID}}", data.FeatureId));

                    columns.Add(htmlColumnFieldNameTmpl.Replace("{{fieldName}}", "Description"));

                    columns.Add(htmlColumnValueTmpl
                        .Replace("{{valueInRepository1}}", data.Description.ValueRepository1)
                        .Replace("{{valueInRepository2}}", data.Description.ValueRepository2));

                    columns.Add(htmlColumnUpdatedTmpl.Replace("{{updated}}", "true"));

                    rows.Add(GetFirstRowAddingColumn(string.Join("", columns)));
                }

                numRows = numRows + rows.Count;                
                htmlRows.Add(string.Join("", rows));
            }

            var htmlFeatures = string.Join("", htmlRows).Replace("{{rowspanRepositoryName}}", numRows + "");

            return new KeyValuePair<int, string>(htmlRows.Count, string.Join("", htmlFeatures)) ;
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

        private string GetTableColumnFieldValue(string html)
        {
            int startIndex = html.IndexOf("<!-- @FieldValue -->");
            int endIndex = html.IndexOf("<!-- @FieldValue@ -->");
            int count = (endIndex - startIndex) + "<!-- @FieldValue@ -->".Length;

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
