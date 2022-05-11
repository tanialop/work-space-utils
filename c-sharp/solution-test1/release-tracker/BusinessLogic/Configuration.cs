using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace release_tracker.BusinessLogic
{
    public class Configuration
    {
        private string[] emailDestination = new string[] { };
        private string senderEmail = "";
        private string senderPassword = "";
        private string htmlFileOutputSameRepository = "";
        private string htmlFileOutputAllRepositories = "";
        public Configuration(IConfiguration configuration)
        {
            senderEmail = configuration.GetSection("emails").GetSection("sender").GetValue<string>("username");
            senderPassword = configuration.GetSection("emails").GetSection("sender").GetValue<string>("password");
            emailDestination = configuration.GetSection("emails").GetSection("destinations").Get<string[]>();
            htmlFileOutputSameRepository = string.Format("{0}{1}",
                configuration.GetValue<string>("htmlFileOutput"), "report.same.repository.html");
            htmlFileOutputAllRepositories = string.Format("{0}{1}",
                configuration.GetValue<string>("htmlFileOutput"), "report.all.repositories.html");
        }

        public string[] EmailDestination { get { return emailDestination; } }
        public string SenderEmail { get { return senderEmail; } }

        public int SenderPassword { get { return SenderPassword; } }

        public string HtmlReportFileSameRepository { get { return htmlFileOutputSameRepository; } }

        public string HtmlReportFileAllRepositories { get { return htmlFileOutputAllRepositories; } }
    }
}
