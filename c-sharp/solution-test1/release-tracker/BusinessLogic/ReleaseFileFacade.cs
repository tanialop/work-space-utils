using Microsoft.Extensions.Configuration;
using release_tracker.ExternalFeatureData;
using release_tracker.LocalDataAccess;
using release_tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace release_tracker.BusinessLogic
{
    public class ReleaseFileFacade
    {
        private readonly IConfiguration configuration;
        private ReleaseLocalDataAccess releaseLocalDataAccess;
        public ReleaseFileFacade(IConfiguration config, ReleaseLocalDataAccess releaseLocalDataAccess)
        {
            this.configuration = config;
            this.releaseLocalDataAccess = releaseLocalDataAccess;
        }

        public List<ReportCompareRepositories> CompareReleaseBetweenAllRepositories(string version)
        {
            List<ReportCompareRepositories> reportCompareRepositories = new List<ReportCompareRepositories>();

            DownloadReleaseFileToCompareBetweenAllRepositories();

            Repository[] repositories = configuration.GetSection("betweenRepositories").Get<Repository[]>();
            Repository[] targetRepositories = configuration.GetSection("betweenRepositories").Get<Repository[]>();
            for(int index = 0; index < repositories.Length; index++)
            {
                Repository repository = repositories[index];
                ReleaseFile releaseFile1 = releaseLocalDataAccess.GetReleaseFileBeforeRelease(repository.name);

                for (int nextIndex = (index + 1); nextIndex < targetRepositories.Length; nextIndex++)
                {
                    Repository targetRepository = targetRepositories[nextIndex];
                    if (!repository.name.Equals(version, StringComparison.OrdinalIgnoreCase))
                    {
                        ReleaseFile releaseFile2 = releaseLocalDataAccess.GetReleaseFileBeforeRelease(targetRepository.name);
                        List<ReportFeatureDistinctStore> reportReleaseDistinctStores = CompareReleaseFilesDistinctRepositories(
                            releaseFile1, releaseFile2, version, repository, targetRepository);

                        ReportCompareRepositories report = new ReportCompareRepositories();
                        report.RepositoryName1 = repository.name;
                        report.RepositoryName2 = targetRepository.name;
                        report.FeatureUpdated = reportReleaseDistinctStores;

                        reportCompareRepositories.Add(report);
                    }
                }
            }

            //foreach(Repository repository in repositories)
            //{
            //    ReleaseFile releaseFile1 = releaseLocalDataAccess.GetReleaseFileBeforeRelease(repository.name);

            //    for(int nextIndex = 0; nextIndex < targetRepositories.Length; nextIndex++)
            //    {
            //        Repository targetRepository = targetRepositories[nextIndex];
            //        if (!repository.name.Equals(version, StringComparison.OrdinalIgnoreCase))
            //        {
            //            ReleaseFile releaseFile2 = releaseLocalDataAccess.GetReleaseFileBeforeRelease(targetRepository.name);
            //            List<ReportFeatureDistinctStore> reportReleaseDistinctStores = CompareReleaseFilesDistinctRepositories(
            //                releaseFile1, releaseFile2, version, repository, targetRepository);

            //            ReportCompareRepositories report = new ReportCompareRepositories();
            //            report.RepositoryName1 = repository.name;
            //            report.RepositoryName2 = targetRepository.name;
            //            report.FeatureUpdated = reportReleaseDistinctStores;

            //            reportCompareRepositories.Add(report);
            //        }
            //    }

            //    //foreach (Repository targetRepository in targetRepositories)
            //    //{
            //    //    if(!repository.name.Equals(version, StringComparison.OrdinalIgnoreCase))
            //    //    {
            //    //        ReleaseFile releaseFile2 = releaseLocalDataAccess.GetReleaseFileBeforeRelease(targetRepository.name);
            //    //        List<ReportFeatureDistinctStore> reportReleaseDistinctStores = CompareReleaseFilesDistinctRepositories(
            //    //            releaseFile1, releaseFile2, version, repository, targetRepository);

            //    //        ReportCompareRepositories report = new ReportCompareRepositories();
            //    //        report.RepositoryName1 = repository.name;
            //    //        report.RepositoryName2 = targetRepository.name;
            //    //        report.FeatureUpdated = reportReleaseDistinctStores;

            //    //        reportCompareRepositories.Add(report);

            //    //    }                    
            //    //}
            //}

            return reportCompareRepositories;
        }

        public string GetReportAsJson(List<ReportCompareRepositories> reportCompareRepositories)
        {
            var opt = new JsonSerializerOptions() { WriteIndented = true };
            string strJson = JsonSerializer.Serialize(reportCompareRepositories, opt);

            return strJson;
        }

        public List<ReleaseFile> DownloadReleaseFileToCompareBetweenAllRepositories()
        {
            List<ReleaseFile> result = new List<ReleaseFile>();

            Repository[] repositories = configuration.GetSection("betweenRepositories").Get<Repository[]>();
            foreach (Repository repository in repositories)
            {
                string url = repository.fileUrlRepository;
                string token = repository.token;
                DownloadReleaseFile downloader = new DownloadReleaseFile(new ReleaseRepository(url, token));
                List<ReleaseFile> releaseFiles = downloader.getReleaseFile();
                ReleaseFile releaseFile = releaseFiles.ElementAt(0); // There is an only file.

                releaseFile = releaseLocalDataAccess.SaveReleaseFile(releaseFile, repository.pathLocalStore, repository.name);

                result.Add(releaseFile);
            }

            return result;

        }

        public List<ReleaseFile> DownloadReleaseFileBeforeRelease()
        {
            List<ReleaseFile> result = new List<ReleaseFile>();

            Repository[] repositories = configuration.GetSection("repositories").Get<Repository[]>();
            foreach (Repository repository in repositories)
            {
                string url = repository.fileUrlBeforeRelease;
                string token = repository.token;
                DownloadReleaseFile downloader = new DownloadReleaseFile(new ReleaseRepository(url, token));
                List<ReleaseFile> releaseFiles = downloader.getReleaseFile();
                ReleaseFile releaseFile = releaseFiles.ElementAt(0); // There is an only file.

                releaseFile = releaseLocalDataAccess.SaveReleaseFile(releaseFile, repository.pathLocalStore, repository.name);

                result.Add(releaseFile);
            }

            return result;

        }

        public List<ReleaseFile> DownloadReleaseFileAfterRelease()
        {
            List<ReleaseFile> result = new List<ReleaseFile>();

            Repository[] repositories = configuration.GetSection("repositories").Get<Repository[]>();
            foreach (Repository repository in repositories)
            {
                string url = repository.fileUrlAfterRelease;
                string token = repository.token;
                DownloadReleaseFile downloader = new DownloadReleaseFile(new ReleaseRepository(url, token));
                List<ReleaseFile> releaseFiles = downloader.getReleaseFile();
                ReleaseFile releaseFile = releaseFiles.ElementAt(0); // There is an only file.

                releaseFile = releaseLocalDataAccess.SaveReleaseFile(releaseFile, repository.pathLocalStore, repository.name);

                result.Add(releaseFile);
            }

            return result;        
        }

        public ReportRelease CompareReleaseFiles(string versionBeforeRelease, string versionAfterRelease, Repository repository)
        {
            ReportRelease report = new ReportRelease();
            report.RepositoryName = repository.name;
            report.RepositoryDescription = repository.description;

            ReleaseFile releaseFileBeforeRelease = releaseLocalDataAccess.GetReleaseFileBeforeRelease(repository.name);
            ReleaseFile releaseFileAfterRelease = releaseLocalDataAccess.GetReleaseFileAfterRelease(repository.name);

            List<Feature> featuresBeforeRelease = releaseFileBeforeRelease.GetFeatureByVersion(versionBeforeRelease); // ID = 2, 5
            List<Feature> featuresAfterRelease = releaseFileAfterRelease.GetFeatureByVersion(versionAfterRelease); // ID = 3, 4, 5, 6

            // Check if feature was updated.
            foreach (Feature featureBeforeRelease in featuresBeforeRelease)
            {
                Feature? featureAfterRelease = featuresAfterRelease.Find(f => f.Id == featureBeforeRelease.Id);
                if (featureAfterRelease != null)
                {
                    if (!featureAfterRelease.Equals(featureBeforeRelease))
                    {
                        report.AddUpdatedFeature(new FeatureUpdated(featureBeforeRelease, featureAfterRelease));
                    }
                }
            }

            // Check if feature was deleted.
            foreach (Feature featureBeforeRelease in featuresBeforeRelease)
            {
                if (!featuresAfterRelease.Exists(f => f.Id == featureBeforeRelease.Id))
                {
                    // the release with id was removed in new release.
                    report.AddDeletedFeature(featureBeforeRelease);
                }
            }

            return report;
        }

        public List<ReportFeatureDistinctStore> CompareReleaseFilesDistinctRepositories(
            ReleaseFile releaseFile1, ReleaseFile releaseFile2, string version, Repository repository1, Repository repository2)
        {
            var reports = new List<ReportFeatureDistinctStore>();            

            List<Feature> featuresRepository1 = releaseFile1.GetFeatureByVersion(version);
            List<Feature> featuresRepository2 = releaseFile2.GetFeatureByVersion(version);

            // Check if feature was updated.
            foreach (Feature featureRep1 in featuresRepository1)
            {
                Feature? featureRep2 = featuresRepository2.Find(f => f.Id == featureRep1.Id);
                if (featureRep2 != null)
                {
                    if (!featureRep2.Equals(featureRep1))
                    {
                        var diff = new ReportFeatureDistinctStore();
                        diff.FeatureId = featureRep1.Id;
                        diff.Version = version;
                        diff.Owner = new DiffDistinctStore(featureRep1.Owner, featureRep2.Owner);
                        diff.Description = new DiffDistinctStore(featureRep1.Description, featureRep2.Description);
                        diff.OffFor = new DiffDistinctStore(featureRep1.StrategyOffFor, featureRep2.StrategyOffFor);
                        diff.OnFor = new DiffDistinctStore(featureRep1.StrategyOnFor, featureRep2.StrategyOnFor);

                        reports.Add(diff);
                    }
                }
            }

            //// Check if feature was deleted.
            //foreach (Feature featureBeforeRelease in featuresRepository1)
            //{
            //    if (!featuresRepository2.Exists(f => f.Id == featureBeforeRelease.Id))
            //    {
            //        // the release with id was removed in new release.
            //        report.AddDeletedFeature(featureBeforeRelease);
            //    }
            //}

            return reports;
        }

        public List<ReportRelease> CompareReleaseFilesOnAllRepositories(string versionBeforeRelease, string versionAfterRelease)
        {
            List<ReportRelease> result = new List<ReportRelease>();
            
            Repository[] repositories = configuration.GetSection("repositories").Get<Repository[]>();
            foreach (Repository repository in repositories)
            {
                ReportRelease reportRelease = CompareReleaseFiles(versionBeforeRelease, versionAfterRelease, repository);
                result.Add(reportRelease);
            }

            return result;
        }
    }
}
