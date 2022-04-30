public class DownloadReleaseFile {
    List<ReleaseFile> releases = new List<ReleaseFile>(); 

    public List<ReleaseFile> getFileReleases() {
        ReleaseFile file1 = new ReleaseFile();
        file1.name = "toggles.xml";
        file1.location = "/home/ronald/personal/another-projects/release-tracking/xml-input-files/" + file1.name;
        file1.file = readFile(file1.location);

        ReleaseFile file2 = new ReleaseFile();
        file2.name = "newToggles.xml";
        file2.location = "/home/ronald/personal/another-projects/release-tracking/xml-input-files/" + file2.name;
        file2.file = readFile(file2.location);

        releases.Add(file1);
        releases.Add(file2);

        return releases;
    }

    private String readFile(String location) {
        String data = "";
        FileStream fileStream = new FileStream(location, FileMode.Open, FileAccess.Read);
        using (StreamReader sr = new StreamReader(fileStream))
        {
            data = sr.ReadToEnd();
        }
        return data;
    }
}