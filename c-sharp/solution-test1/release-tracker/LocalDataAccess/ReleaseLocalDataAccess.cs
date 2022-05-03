using Microsoft.Extensions.Configuration;
using release_tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace release_tracker.LocalDataAccess
{
    public class ReleaseLocalDataAccess
    {
        private IConfiguration config;

        public ReleaseLocalDataAccess(IConfiguration config) {
            this.config = config;
        }

        //public ReleaseFile GetReleaseFile() {
        //    string location = GetReleaseFullFilePath();

        //    ReleaseFile releaseFile = new ReleaseFile();
        //    releaseFile.Location = location;
        //    releaseFile.Name = Path.GetFileName(location);
        //    releaseFile.File = ReadFile(location);

        //    return releaseFile;
        //}

        //public ReleaseFile SaveReleaseFile(ReleaseFile releaseFile) {
        //    string path = config.GetSection("localStore")["path"];
        //    string location = String.Format("{0}{1}", path, releaseFile.Name);

        //    WriteFile(location, releaseFile.File);

        //    ReleaseFile releaseFileReponse = new ReleaseFile();
        //    releaseFileReponse.Location = location;
        //    releaseFileReponse.Name = Path.GetFileName(location);
        //    releaseFileReponse.File = ReadFile(location);

        //    return releaseFile;
        //}

        public ReleaseFile SaveReleaseFile(ReleaseFile releaseFile, string pathLocalStore, string folderName)
        {
            string path = string.Format("{0}{1}", pathLocalStore, folderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string file = string.Format("{0}{1}{2}", path, Path.DirectorySeparatorChar, releaseFile.Name);


            WriteFile(file, releaseFile.File);

            ReleaseFile releaseFileReponse = new ReleaseFile();
            releaseFileReponse.Location = pathLocalStore;
            releaseFileReponse.Name = releaseFile.Name;
            releaseFileReponse.File = ReadFile(file);

            return releaseFile;
        }        

        public ReleaseFile GetReleaseFileBeforeRelease(string storeName)
        {
            ReleaseFile? releaseFile = null;

            Repository[] repositories = config.GetSection("repositories").Get<Repository[]>();
            Repository? repository = repositories.FirstOrDefault(r => r.name.Equals(storeName));
            if (repository != null)
            {
                string file = string.Format("{0}{1}{2}{3}",
                    repository.pathLocalStore,
                    repository.name,
                    Path.DirectorySeparatorChar,
                    Path.GetFileName(repository.fileUrlBeforeRelease));

                releaseFile = GetReleaseFile(file);
            }

            return releaseFile;
        }

        public ReleaseFile GetReleaseFileAfterRelease(string storeName)
        {
            ReleaseFile? releaseFile = null;

            Repository[] repositories = config.GetSection("repositories").Get<Repository[]>();
            Repository? repository = repositories.FirstOrDefault(r => r.name.Equals(storeName));
            if (repository != null)
            {
                string file = string.Format("{0}{1}{2}{3}",
                    repository.pathLocalStore,
                    repository.name,
                    Path.DirectorySeparatorChar,
                    Path.GetFileName(repository.fileUrlAfterRelease));

                releaseFile = GetReleaseFile(file);
            }

            return releaseFile;
        }

        private ReleaseFile GetReleaseFile(string location)
        {
            ReleaseFile releaseFile = new ReleaseFile();
            releaseFile.Location = location;
            releaseFile.Name = Path.GetFileName(location);
            releaseFile.File = ReadFile(location);

            return releaseFile;
        }

        private string ReadFile(String location)
        {
            string data = "";
            FileStream fileStream = new FileStream(location, FileMode.Open, FileAccess.Read);
            using (StreamReader sr = new StreamReader(fileStream))
            {
                data = sr.ReadToEnd();
            }
            return data;
        }

        private string WriteFile(string location, string content) {

            FileStream fileStream = new FileStream(location, FileMode.OpenOrCreate, FileAccess.ReadWrite);            
            using (StreamWriter sw = new StreamWriter(fileStream))
            {
                sw.Write(content);
            }

            return location;
        }

        //private String GetReleaseFullFilePath() {
        //    string path = Directory.GetCurrentDirectory();
        //    DirectoryInfo? directoryInfo = Directory.GetParent(path).Parent.Parent;
        //    string location = String.Format("{0}{1}{2}{3}{4}{5}{6}",
        //        directoryInfo,
        //        Path.DirectorySeparatorChar,
        //        "LocalDataAccess",
        //        Path.DirectorySeparatorChar,
        //        "xml-input-files",
        //        Path.DirectorySeparatorChar,
        //        "toggles.xml");

        //    return location;
        //}
    }
}
