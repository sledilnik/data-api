#addin nuget:?package=Cake.Docker&version=0.11.0
using System.Xml.Linq;

var target = Argument("target", "Default");
var pull = Argument<bool>("pull", true);
var push = Argument<bool>("push", false);
var dokreg = Argument<string>("dokreg", "mihamarkic");
var setVersion = Argument<string>("buildVersion", null);

var rootDirectory = Directory("./sources/SloCovidServer");
var solution = rootDirectory + File("SloCovidServer.sln");
var appDirectory = rootDirectory + Directory("SloCovidServer"); 
var appProject = appDirectory + File("SloCovidServer.csproj");
 
var versionFile = Directory(".") + File("version.xml");
const string versionsRoot = "version";

Task("BuildImage")
	.Does(() =>{
		string version = GetVersion();
		BuildAndPush("slo-covid19-server", new []{ "latest", version });
	});

void BuildAndPush(string tag, string[] tagSuffixes)
{
	string[] tags = new string[tagSuffixes.Length];
	for (int i=0; i< tags.Length; i++)
	{
		tags[i] = $"{dokreg}/{tag}:{tagSuffixes[i]}";
	}
    var settings = new DockerImageBuildSettings { Tag = tags, Pull=pull, NoCache=true };
    DockerBuild(settings, rootDirectory);
	if (push)
	{
		Information($"Pushing {(string.Join(",", tags))}");
		for (int i=0; i<tags.Length; i++)
		{
			DockerPush(tags[i]);
		}
	}
	else
	{
		Information($"Won't push {(string.Join(",", tags))}. Set --docker-push=true when push is required.");
	}
}

// version has to be in Major.Minor.Build format
bool CheckVersion(string text)
{
	if (string.IsNullOrEmpty(text))
	{
		Error("Version is null");
		throw new Exception("Version is null");
	}
	string[] parts = text.Split('.');
	if (parts.Length != 3) {
		Error($"Version {text} is not in major.minor.build format");
		throw new Exception($"Version {text} is not in major.minor.build format");
	}
	foreach (string part in parts)
	{
		if (int.TryParse(part, out int v))
		{
			if (v < 0) {
				Error($"Version {text} part {part} is less than zero");
				throw new Exception($"Version {text} part {part} is less than zero");
			}
		}
		else {
			Error($"Version {text} part {part} is not an integer");
			throw new Exception($"Version {text} part {part} is not an integer");
		}
	}
	return true;
}
XDocument LoadVersions()
{
	if (System.IO.File.Exists(versionFile))
	{
		return XDocument.Load(versionFile);
	}
	var doc = new XDocument();
	doc.Add(new XElement(versionsRoot, "0.0.0"));
	return doc;
}
void SaveVersions(XDocument doc) => doc.Save(versionFile);
string GetVersion()
{
	var doc = LoadVersions();
	return doc.Element(versionsRoot).Value;
}
Task("GetVersion")
	.Does(() =>{
		var root = GetVersion();
		Information($"Version: {root}");
});
Task("SetVersion")
	.Does(() => {
		var doc = LoadVersions();
		var element = doc.Element(versionsRoot);
		Information($"Current version is {element.Value}");
		if (!string.IsNullOrEmpty(setVersion))
		{
			CheckVersion(setVersion);
			element.Value = setVersion.ToString();
			Information($"After setting version is {element.Value}");
			SaveVersions(doc);
		}
		else
		{
			Warning("No version set. -setVersion is required");
		}
});
Task("IncreaseVersion")
	.Does(() =>{
		var doc = LoadVersions();
		var element = doc.Element(versionsRoot);
		Information($"Current version is {element.Value}");
		string[] versionParts = element.Value.Split('.');
		versionParts[2] = (int.Parse(versionParts[2])+1).ToString();
		element.Value = string.Join(".", versionParts);
		SaveVersions(doc);
		Information($"After changing version is {element.Value}");
});

Task("Default")
    .Does(() => { 
		Information("KzsParser build process targets");
		foreach (string target in new string[]{"Default", "BuildImage", "GetVersion", "SetVersion", "IncreaseVersion"})
		{
			Information($"\t{target}");
		}
	});

RunTarget(target);