import buildTools
import versionTools
import os

def buildOnly():
    pass

def checkBeforeProceeding(message = "upload a NuGet package?"):
    print(message, "(type yes)")
    if input() == "yes":
        return True
    else:
        return False

if __name__ == "__main__":
    if os.path.abspath("./") != os.path.abspath(os.path.dirname(__file__)):
        raise Exception("this script must be run from the same folder.")
    newVersion = versionTools.increasePackageVersions(buildTools.PROJECT_FILES)
    buildTools.clean()
    buildTools.buildAllPackages()
    buildTools.createDemoZip(newVersion)
    
    #buildOnly()
    #if checkBeforeProceeding():
        #print("uploading")
