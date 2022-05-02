using release_tracker.ExternalFeatureData;
using release_tracker.LocalDataAccess;
using release_tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace release_tracker.BusinessLogic
{
    public class ComparatorFacade
    {
        private ReleaseLocalDataAccess releaseLocalDataAccess;
        public ComparatorFacade() {
            releaseLocalDataAccess = new ReleaseLocalDataAccess();
        }
        public string CompareReleaseWithSameRepository(ReleaseRepository releaseRepository, string version)
        {
            DownloadReleaseFile downloader = new DownloadReleaseFile(releaseRepository);
            List<ReleaseFile> releases = downloader.getFileRelease();
            

            List<Feature> features = releases.ElementAt(0).GetFeatures();
            List<string> allVersions = releases.ElementAt(0).GetAllFeatureVersions();
            List<string> distinctVersions = releases.ElementAt(0).getDistinctFeatureVersions();
            return "";
        }

        
    }
}
