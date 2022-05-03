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

        internal List<ReportRelease> CompareReleaseFilesOnAllRepositories(string v)
        {
            throw new NotImplementedException();
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
            //string url = configuration.GetSection("localStore")["fileUrlAfterRelease"];
            //string token = configuration.GetSection("localStore")["repositoryToken"];

            //DownloadReleaseFile downloader = new DownloadReleaseFile(new ReleaseRepository(url, token));
            //List<ReleaseFile> releases = downloader.getReleaseFile();
            //ReleaseFile releaseFile = releases.ElementAt(0); // There is an only file.

            //releaseLocalDataAccess.SaveReleaseFile(releaseFile);

            //return releaseFile;
        }

        /// <summary>
        /// versionBeforeRelease= 22.1
        /// versionAfterRelease = 22.2
        /// 
        /// </summary>
        /// <param name="versionBeforeRelease"></param>
        /// <param name="versionAfterRelease"></param>
        /// <returns></returns>
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
