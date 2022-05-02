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

        public ReleaseFile GetReleaseFile() {
            string location = GetReleaseFullFilePath();

            ReleaseFile releaseFile = new ReleaseFile();
            releaseFile.Location = location;
            releaseFile.Name = Path.GetFileName(location);
            releaseFile.File = ReadFile(location);

            return releaseFile;
        }

        public ReleaseFile SaveReleaseFile(ReleaseFile releaseFile) {
            string location = GetReleaseFullFilePath();

            WriteFile(location, releaseFile.File);

            ReleaseFile releaseFileReponse = new ReleaseFile();
            releaseFileReponse.Location = location;
            releaseFileReponse.Name = Path.GetFileName(location);
            releaseFileReponse.File = ReadFile(location);

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

        private String GetReleaseFullFilePath() {
            string path = Directory.GetCurrentDirectory();
            DirectoryInfo? directoryInfo = Directory.GetParent(path).Parent.Parent;
            string location = String.Format("{0}{1}{2}{3}{4}{5}{6}",
                directoryInfo,
                Path.DirectorySeparatorChar,
                "LocalDataAccess",
                Path.DirectorySeparatorChar,
                "xml-input-files",
                Path.DirectorySeparatorChar,
                "toggles.xml");

            return location;
        }
    }
}
