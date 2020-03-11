import os
import glob
import subprocess
import shutil
import time

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

def clean():
    for projectFile in PROJECT_FILES:
        print(f"cleaning {os.path.basename(projectFile)}...")
        args = ["dotnet", "clean", projectFile]
        p = subprocess.call(args, stdout=subprocess.DEVNULL)
        if p != 0:
            raise Exception("clean failed")

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

def createDemoZip(newVersion):
    
    for sourceFile in glob.glob("*.zip*"):
        print("deleting", os.path.basename(sourceFile))
        os.remove(sourceFile)
    for sourceFile in glob.glob("../*.zip*"):
        print("deleting", os.path.basename(sourceFile))
        os.remove(sourceFile)
    
    releaseFolder = PATH_SRC + "\\ScottPlot.Demo.WPF\\bin\\Release"
    
    print("copying bundle files...")      
    for sourceFile in glob.glob("bundle/*"):
        print("copying", os.path.basename(sourceFile))
        shutil.copy(sourceFile, releaseFolder)
        
    print("copying source code files...")     
    pthSrcOut = releaseFolder+"/netcoreapp3.0/source/"
    if not os.path.exists(releaseFolder+"/netcoreapp3.0/source"):
        os.mkdir(pthSrcOut)
    for pth in glob.glob(PATH_SRC + "/ScottPlot.Demo/*"):
        if os.path.isdir(pth):
            csFilesInside = glob.glob(pth+"/*.cs")
            if len(csFilesInside):
                if not os.path.exists(pthSrcOut+os.path.basename(pth)):
                    os.mkdir(pthSrcOut+os.path.basename(pth))
                for csFile in csFilesInside:
                    shutil.copy(csFile, pthSrcOut+os.path.basename(pth)+"/"+os.path.basename(csFile))
        elif pth.endswith(".cs"):
            shutil.copy(pth, pthSrcOut+os.path.basename(pth))
    
    print("zipping release folder...")  
    args = ["powershell", "Compress-Archive", releaseFolder+"\\*", f"../ScottPlotDemo-{newVersion}.zip"]
    p = subprocess.call(args, stdout=subprocess.DEVNULL)
    if p != 0:
        raise Exception("zip failed")