#!/bin/bash

project_files=(
    "src/ScottPlot5/ScottPlot5/ScottPlot.csproj"
    "src/ScottPlot5/ScottPlot5 Tests/ScottPlot Tests.csproj"
    "src/ScottPlot5/ScottPlot5 Cookbook/ScottPlot Cookbook.csproj"
    "src/ScottPlot5/ScottPlot5 Demos/ScottPlot5 WinForms Demo/WinForms Demo.csproj"
    "src/ScottPlot4/ScottPlot/ScottPlot.csproj"
)

for project_file in "${project_files[@]}"; do
    echo "Building: $project_file"
    dotnet restore "$project_file"
    dotnet build "$project_file" --configuration Release
done
