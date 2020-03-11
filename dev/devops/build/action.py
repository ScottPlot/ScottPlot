import buildTools

def buildOnly():
    pass

def checkBeforeProceeding(message = "upload a NuGet package?"):
    print(message, "(type yes)")
    if input() == "yes":
        return True
    else:
        return False


if __name__ == "__main__":
    buildTools.buildAllPackages()
    #buildOnly()
    #if checkBeforeProceeding():
        #print("uploading")
