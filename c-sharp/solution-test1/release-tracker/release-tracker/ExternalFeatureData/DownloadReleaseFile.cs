using release_tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace release_tracker.ExternalFeatureData
{
    public class DownloadReleaseFile
    {
        private List<ReleaseRepository> releaseRepositories = new List<ReleaseRepository>();

        List<ReleaseFile> releases = new List<ReleaseFile>();

        public DownloadReleaseFile(string urlRepository, string tokenRepository) {
            this.releaseRepositories.Add(new ReleaseRepository(urlRepository, tokenRepository));
        }

        public DownloadReleaseFile(ReleaseRepository releaseRepository) {
            this.releaseRepositories.Add(releaseRepository);
        }

        public DownloadReleaseFile(List<ReleaseRepository> releaseRepository)
        {
            this.releaseRepositories = releaseRepository;
        }

        public List<ReleaseFile> getFileRelease() {
            return DownloadFilesFromRepositories();
        }

        private List<ReleaseFile> DownloadFilesFromRepositories()
        {
            List < ReleaseFile > releaseFiles = new List<ReleaseFile>();

            foreach (ReleaseRepository repository in this.releaseRepositories) {
                HttpClient client = new HttpClient();
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", repository.Token);
                string content = client.GetStringAsync(repository.Url).Result;

                ReleaseFile releaseFile = new ReleaseFile();
                releaseFile.File = content;
                releaseFile.Name = Path.GetFileName(repository.Url);
                releaseFile.Location = repository.Url;

                client.Dispose();

                releaseFiles.Add(releaseFile);
            }

            return releaseFiles;
        }


        //public List<ReleaseFile> getFileReleases()
        //{
        //    string path = Directory.GetCurrentDirectory();                        
        //    DirectoryInfo? directoryInfo = Directory.GetParent(path).Parent.Parent;
        //    string location = String.Format("{0}{1}{2}{3}",
        //        directoryInfo,
        //        Path.DirectorySeparatorChar,
        //        "xml-input-files",
        //        Path.DirectorySeparatorChar);

        //    ReleaseFile file1 = new ReleaseFile();
        //    file1.Name = "toggles.xml";
        //    file1.Location = location + file1.Name;
        //    file1.File = readFile(file1.Location);

        //    ReleaseFile file2 = new ReleaseFile();
        //    file2.Name = "newToggles.xml";
        //    file2.Location = location + file2.Name;
        //    file2.File = readFile(file2.Location);

        //    releases.Add(file1);
        //    releases.Add(file2);

        //    return releases;
        //}

        //private String readFile(String location)
        //{
        //    String data = "";
        //    FileStream fileStream = new FileStream(location, FileMode.Open, FileAccess.Read);
        //    using (StreamReader sr = new StreamReader(fileStream))
        //    {
        //        data = sr.ReadToEnd();
        //    }
        //    return data;
        //}
    }
}
