#!/bin/sh

# Install/update the dotnet 5 autoformatter
dotnet tool update -g dotnet-format

# Run the dotnet formatter
dotnet format ScottPlot4.sln
