using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace release_tracker.Models.html
{
    public class HtmlFeatureRowReport
    {
        private string repositoryName;
        private string featureUID;
        private string description;
        private string offFor;
        private string onFor;
        private string version;
        private string owner;

        private string style = "color: rgb(17, 16, 16); background-color: yellow; text-align: center; font-weight: bold;";
        private string html = "";

        public HtmlFeatureRowReport(string featureUID, string description, string offFor, string onFor, string version, string owner)
        {
            // <td style="{{style}}">{0}</td>
            html = string.Format("<td>{0}</td>\n", featureUID);
            html = html + string.Format("<td style=\"{{styleDescription}}\">{0}</td>\n", description);
            html = html + string.Format("<td style=\"{{styleOffFor}}\">{0}</td>\n", offFor);
            html = html + string.Format("<td style=\"{{styleOnFor}}\">{0}</td>\n", onFor);
            html = html + string.Format("<td style=\"{{styleVersion}}\">{0}</td>\n", version);
            html = html + string.Format("<td style=\"{{styleOwner}}\">{0}</td>\n", owner);
        }

        public void SetRepositoryName(string name)
        {
            this.repositoryName = name;
        }

        public string GetRepositoryName()
        {
            return this.repositoryName;
        }

        public void SetStyleDescription()
        {
            this.html.Replace("{{styleDescription}}", style);
        }

        public void SetStyleOffFor()
        {
            this.html.Replace("{{styleOffFor}}", style);
        }

        public void SetStyleOnFor()
        {
            this.html.Replace("{{styleOnFor}}", style);
        }

        public void SetStyleVersion()
        {
            this.html.Replace("{{styleVersion}}", style);
        }

        public void SetStyleOwner()
        {
            this.html.Replace("{{styleOwner}}", style);
        }

        public string GetHtml()
        {
            return html;
        }
    }
}
