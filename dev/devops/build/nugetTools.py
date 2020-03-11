import os
import glob
import subprocess

def updateNuget():
    p = subprocess.call(["nuget", "update", "-self"])
    if p != 0:
        raise Exception("nuget update failed")
        
def uploadToNuGet(PROJECT_FILES):
    # requires you to have run "nuget.exe setApiKey [secretKey]" previously
    updateNuget()
    for projectFile in PROJECT_FILES:
        releaseFolder = os.path.dirname(projectFile)+"/bin/Release"
        releaseFolder = os.path.abspath(releaseFolder)
        packages = glob.glob(releaseFolder+"/*.nupkg")
        assert len(packages) == 1
        nupkgPath = packages[0]
        print(f"uploading {os.path.basename(nupkgPath)}...")
        args = [
            "nuget", "push", nupkgPath,
            "-Source", "https://api.nuget.org/v3/index.json"
        ]
        p = subprocess.call(args)
        if p != 0:
            raise Exception("nuget update failed")
