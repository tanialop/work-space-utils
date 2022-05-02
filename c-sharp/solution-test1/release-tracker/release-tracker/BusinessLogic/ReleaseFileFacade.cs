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
    public class ReleaseFileFacade
    {
        private ReleaseLocalDataAccess releaseLocalDataAccess;
        public ReleaseFileFacade()
        {
            releaseLocalDataAccess = new ReleaseLocalDataAccess();
        }

        public ReleaseFile DownloadSaveReleaseFile(ReleaseRepository releaseRepository)
        {

            DownloadReleaseFile downloader = new DownloadReleaseFile(releaseRepository);
            List<ReleaseFile> releases = downloader.getFileRelease();
            ReleaseFile releaseFile = releases.ElementAt(0);

            releaseLocalDataAccess.SaveReleaseFile(releaseFile);

            return releaseFile;
        }
    }
}
