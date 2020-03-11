import os
import glob
import subprocess

PATH_HERE = os.path.abspath(os.path.dirname(__file__))
PATH_SRC = os.path.abspath(PATH_HERE+"../../../../src")
SOLUTION_FILE = PATH_SRC+"/ScottPlotV4.sln"
PROJECT_FILES = [
    PATH_SRC+"/ScottPlot/ScottPlot.csproj",
    PATH_SRC+"/ScottPlot.WinForms/ScottPlot.WinForms.csproj",
    PATH_SRC+"/ScottPlot.WPF/ScottPlot.WPF.csproj",
    PATH_SRC+"/ScottPlot.Demo/ScottPlot.Demo.csproj",
    #PATH_SRC+"/ScottPlot.Demo.WinForms/ScottPlot.Demo.WinForms.csproj",
    PATH_SRC+"/ScottPlot.Demo.WPF/ScottPlot.Demo.WPF.csproj",
]
assert os.path.exists(SOLUTION_FILE)
for filePath in PROJECT_FILES:
    assert os.path.exists(filePath)

def buildAllPackages():
    for projectFile in PROJECT_FILES:
        print(f"building {os.path.basename(projectFile)}...")

        projectFolder = os.path.abspath(os.path.dirname(projectFile))
        releaseFolder = projectFolder+"/bin/Release"
        for oldPackage in glob.glob(releaseFolder+"/*nupkg"):
            os.remove(oldPackage)

        args = ["dotnet", "build", projectFile, "--configuration", "Release"]
        p = subprocess.call(args, stdout=subprocess.DEVNULL)
        if p != 0:
            raise Exception("build failed")


