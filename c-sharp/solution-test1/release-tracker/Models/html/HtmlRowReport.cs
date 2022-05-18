using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace release_tracker.Models.html
{
    public class HtmlRowReport
    {
        private List<HtmlFeatureRowReport> htmlFeatureRows = new List<HtmlFeatureRowReport>();

        public void AddHtmlFeatureRowReport(HtmlFeatureRowReport htmlFeatureRow)
        {
            htmlFeatureRows.Add(htmlFeatureRow);
        }

        public HtmlFeatureRowReport? GetHtmlFeatureRowReportByName(string name)
        {
            return htmlFeatureRows.Find(x => x.GetRepositoryName().Equals(name));
        }

        public string GetHtml()
        {
            string html = "";
            foreach(HtmlFeatureRowReport htmlFeatureRow in htmlFeatureRows)
            {
                html = html + htmlFeatureRow.GetHtml() + "\n";
            }
            return html;
        }
    }
}
