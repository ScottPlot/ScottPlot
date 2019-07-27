"""
The purpose of this script is to find all binaries in the demos src folder and
copy them into the bin folder.
"""

import os
import glob
import shutil

PATH_HERE = os.path.abspath(os.path.dirname(__file__))


def cleanFolder():
    for path in glob.glob(f"{PATH_HERE}/bin/*"):
        print(f"deleting {os.path.basename(path)}")
        os.remove(path)


def copyExes():
    for pathIn in glob.glob(f"{PATH_HERE}/src/*/bin/Debug/*.exe"):
        fileName = os.path.basename(pathIn)
        pathOut = os.path.join(f"{PATH_HERE}/bin/{fileName}")
        print(f"copying {fileName}")
        shutil.copy(pathIn, pathOut)


def copyScottPlotDll():
    pathIn = f"{PATH_HERE}/../src/ScottPlot/bin/Debug/"
    pathIn = os.path.join(os.path.abspath(pathIn), "ScottPlot.dll")
    pathOut = os.path.abspath(os.path.join(f"{PATH_HERE}/bin/ScottPlot.dll"))
    print(f"copying ScottPlot.dll")
    shutil.copy(pathIn, pathOut)


def copyNAudioDll():
    pathIn = f"{PATH_HERE}/../src/packages/NAudio.1.9.0/lib/net35"
    pathIn = os.path.join(os.path.abspath(pathIn), "NAudio.dll")
    pathOut = os.path.abspath(os.path.join(f"{PATH_HERE}/bin/NAudio.dll"))
    print(f"copying NAudio.dll")
    shutil.copy(pathIn, pathOut)


def createZip():
    zipFilePath = os.path.join(PATH_HERE, "ScottPlot-Demos")
    binFolderPath = os.path.join(PATH_HERE, "bin")
    shutil.make_archive(zipFilePath, 'zip', binFolderPath)
    print("creating zip file")


if __name__ == "__main__":
    cleanFolder()
    copyExes()
    copyScottPlotDll()
    copyNAudioDll()
    createZip()
    print("DONE")
