using Microsoft.Extensions.Configuration;
using release_tracker.LocalDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace release_tracker.BusinessLogic
{
    public class EmailFacade
    {
        private Configuration configuration;
        public EmailFacade(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public void SendHtmlReportSameRepositoryByEmail() {

            try
            {
                string html = ReleaseLocalDataAccess.ReadFile(configuration.HtmlReportFileSameRepository);


                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(configuration.SenderEmail);
                mail.To.Add(string.Join(",", configuration.EmailDestination));
                mail.Subject = "Report Release between Same Repository.";


                mail.IsBodyHtml = true;
                mail.Body = html;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("username", "passowrd");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
    }
}
