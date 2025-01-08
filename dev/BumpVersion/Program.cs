string repoFolder = Locate.GetRepoRootFolder();
string[] csprojFilePaths = Locate.GetCsprojFilePaths(repoFolder);

foreach (string csprojFilePath in csprojFilePaths)
{
    CsprojEditor.BumpVersion(csprojFilePath);
}