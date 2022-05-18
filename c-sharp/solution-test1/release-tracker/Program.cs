// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using release_tracker.BusinessLogic;
using release_tracker.ExternalFeatureData;
using release_tracker.LocalDataAccess;
using release_tracker.Models;
using System.Text.Json;
using System.Xml;

IConfiguration configuration = GetConfig();


ReleaseFileFacade releaseFileFacade = Factory.GetReleaseFileFacade(configuration);
releaseFileFacade.DownloadReleaseFileBeforeRelease();
releaseFileFacade.DownloadReleaseFileAfterRelease();

List<ReportRelease> reportReleases = releaseFileFacade.CompareReleaseFilesOnAllRepositories("22.1", "22.2");
HtmlReportSameRepository htmlReport = new HtmlReportSameRepository(configuration);
htmlReport.GenerateReportSameRepository(reportReleases);

//ReportRelease.PrintOnConsole(reportReleases);

List<ReportCompareRepositories> reportCompareRepositories = releaseFileFacade.CompareReleaseBetweenAllRepositories("22.2");
HtmlReportAllRepository htmlReportAllRepository = new HtmlReportAllRepository(configuration);
htmlReportAllRepository.GenerateReportAllRepositories(reportCompareRepositories);
//Console.WriteLine(releaseFileFacade.GetReportAsJson(reportCompareRepositories));

HtmlReportAllRepositoryLast htmlReportAllRepositoryLast = Factory.GetHtmlReportAllRepositoryLast(configuration);
htmlReportAllRepositoryLast.GenerateReport(reportCompareRepositories);

//EmailFacade emailFacade = Factory.GetEmailFacade(configuration);
//emailFacade.SendHtmlReportSameRepositoryByEmail();  // It is throwing an Exception. 'The SMTP server requires a secure connection or the client was not authenticated. The server response was: 5.7.0 Authentication Required. Learn more a



















static IConfiguration GetConfig()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(System.AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json",
        optional: true,
        reloadOnChange: true);

    return builder.Build();
}
