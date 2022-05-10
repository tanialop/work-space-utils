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


ReleaseFileFacade releaseFileFacade = Factory.GetReleaseFileFacade(GetConfig());
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



















static IConfiguration GetConfig()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(System.AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json",
        optional: true,
        reloadOnChange: true);

    return builder.Build();
}
