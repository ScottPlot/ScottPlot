import urllib
import json
import os

class Version:
    def __init__(self, versionString):
        """semantic version (major.minor.patch)"""
        self.setString(versionString)

    def __str__(self):
        return f"{self.major}.{self.minor}.{self.patch}"

    def setNumbers(self, major=None, minor=None, patch=None):
        self.major = int(major) if major else self.major
        self.minor = int(minor) if minor else self.minor
        self.patch = int(patch) if patch else self.patch

    def setString(self, versionString):
        assert isinstance(versionString, str)
        parts = versionString.split(".")
        assert len(parts) == 3
        self.setNumbers(*parts)

    def increase(self, major=False, minor=False, patch=True):
        self.major = self.major + 1 if major else self.major
        self.minor = self.minor + 1 if minor else self.minor
        self.patch = self.patch + 1 if patch else self.patch


def GetOnlineVersion(package="scottplot"):
    """
    Returns the version of the NuGet package online
    """
    print(f"checking the web for the latest {package} version...")
    url = f"https://api.nuget.org/v3/registration4/{package}/index.json"
    response = urllib.request.urlopen(url)
    data = response.read()
    jsonText = data.decode('utf-8')
    parsed = json.loads(jsonText)
    #print(json.dumps(parsed, indent=4, sort_keys=True))
    version = Version(parsed["items"][0]["upper"])
    print(f"latest version of {package} is: {version}")
    return version


def SetProjectVersion(csprojPath, newVersion):
    projectName = os.path.basename(csprojPath)
    print(f"upgrading {projectName} to version {newVersion}...")
    with open(csprojPath) as f:
        lines = f.read().split("\n")
    for i, line in enumerate(lines):
        if "Version" in line and "</" in line and not "LangVersion" in line:
            a, b = line.split(">", 1)
            b, c = b.split("<", 1)
            line = a + ">" + newVersion + "<" + c
            lines[i] = line
    with open(csprojPath, 'w') as f:
        f.write("\n".join(lines))


def increasePackageVersions(PROJECT_FILES):
    version = GetOnlineVersion()
    version.increase()
    newVersion = str(version)
    for projectFile in PROJECT_FILES:
        SetProjectVersion(projectFile, newVersion)
