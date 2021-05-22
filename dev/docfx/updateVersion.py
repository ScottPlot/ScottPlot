from os import path
import pathlib

thisFolder = pathlib.Path(__file__).parent
sourceFile = thisFolder.joinpath("api/index.md.txt")
destFile = thisFolder.joinpath("api/index.md")
destFile2 = thisFolder.joinpath("index.md")
csprojFile = thisFolder.joinpath("../../src/ScottPlot/ScottPlot.csproj")
jsonFile = thisFolder.joinpath("docfx.json")

with csprojFile.open() as f:
    lines = f.readlines()
    for line in lines:
        line = line.strip()
        if not line.startswith("<Version>"):
            continue
        version = line.split(">")[1].split("<")[0]

with open(sourceFile) as f:
    text = f.read()
with open(destFile, 'w') as f:
    f.write(text.replace("{{VERSION}}", version))
with open(destFile2, 'w') as f:
    f.write(text.replace("{{VERSION}}", version))

with open(jsonFile, 'r') as f:
    jsonLines = f.readlines()
    for i, line in enumerate(jsonLines):
        if "_appTitle" in line:
            jsonLines[i] = f'            "_appTitle": "ScottPlot {version} API",\n'
with open(jsonFile, 'w') as f:
    f.writelines(jsonLines)
