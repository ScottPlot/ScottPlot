import os
import shutil
import glob

PATH_HERE = os.path.abspath(os.path.dirname(__file__))
PATH_SOURCE = os.path.abspath(PATH_HERE+"/bin/Debug/")
PATH_DEST = os.path.abspath(PATH_HERE+"/../")


def copyImages():
    for imageFilePath in glob.glob(PATH_SOURCE+"/images/*.png"):
        imageFileName = os.path.basename(imageFilePath)

        # skip files that change a lot
        if imageFileName.startswith("30_Signal"):
            continue
        if imageFileName.startswith("31_Signal"):
            continue

        destination = os.path.join(PATH_DEST+"/images/", imageFileName)
        print(f"copying {imageFilePath}")
        shutil.copy(imageFilePath, destination)

def copyReadme():
    source = os.path.join(PATH_SOURCE, "readme.md")
    destination = os.path.join(PATH_DEST, "readme.md")
    shutil.copy(source, destination)
    print(f"copying readme.md")

if __name__ == "__main__":
    copyImages()
    copyReadme()
    print("DONE")
    input()
