using NUnit.Framework;
using release_tracker.BusinessLogic;
using release_tracker.ExternalFeatureData;

namespace release_tracker_tests
{
    public class ReleaseFileFacadeTest
    {
        string toggleFileLocation = "https://raw.githubusercontent.com/ronaldespinozato/work-space-utils/main/release-tracking/xml-input-files/toggles.xml";
        string newToggleFileLocation = "https://raw.githubusercontent.com/ronaldespinozato/work-space-utils/main/release-tracking/xml-input-files/newToggles.xml";
        string accessToken = "some access token for github.";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //ReleaseRepository releaseRepository = new ReleaseRepository(toggleFileLocation, accessToken);
            //ReleaseFileFacade releaseFileFacade = new ReleaseFileFacade();
            //release_tracker.Models.ReleaseFile releaseFile = releaseFileFacade.DownloadReleaseFile(releaseRepository);
            //Assert.IsNotNull(releaseFile);
            //Assert.Equals("toggles.xml", releaseFile.Name);            
        }
    }
}