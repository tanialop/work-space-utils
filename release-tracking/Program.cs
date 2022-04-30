using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

List<ReleaseFile> releaseFiles = new DownloadReleaseFile().getFileReleases();

Console.WriteLine("Total ReleaseFile " + releaseFiles.Count);

string xml = @"<?xml version='1.0' standalone='no'?>
<root>
  <person id='1'>
    <name>Alan</name>
    <url>http://www.google.com</url>
  </person>
  <person id='2'>
    <name>Louis</name>
    <url>http://www.yahoo.com</url>
  </person>
</root>";

XmlDocument doc = new XmlDocument();
doc.LoadXml(xml);

string json = JsonConvert.SerializeXmlNode(doc);
JObject o = JObject.Parse(json);
Console.WriteLine(json);

JObject root = (JObject)o.GetValue("root");

Console.WriteLine(root.GetValue("person"));

JArray persons = (JArray)root.GetValue("person");
foreach (JObject person in persons) {    
    Console.WriteLine(">>>> {0} | {1} | {2}", person.GetValue("@id"), person.GetValue("name"), person.GetValue("url"));    
}

DownloadReleaseFile downloader = new DownloadReleaseFile();
List<ReleaseFile> releases =downloader.getFileReleases();
List<Feature> features = releases.ElementAt(0).GetFeatures();
List<string> allVersions = releases.ElementAt(0).GetAllFeatureVersions();
List<string> distinctVersions = releases.ElementAt(0).getDistinctFeatureVersions();
Console.WriteLine(distinctVersions);

