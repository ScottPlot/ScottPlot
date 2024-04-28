#!/bin/bash

project_files=(
    "src/ScottPlot5/ScottPlot5/ScottPlot.csproj"
    "src/ScottPlot5/ScottPlot5 Tests/Unit Tests/ScottPlot Unit Tests.csproj"
    "src/ScottPlot5/ScottPlot5 Cookbook/ScottPlot Cookbook.csproj"
    "src/ScottPlot5/ScottPlot5 Demos/ScottPlot5 WinForms Demo/WinForms Demo.csproj"
    "src/ScottPlot5/ScottPlot5 Demos/ScottPlot5 WPF Demo/WPF Demo.csproj"
    "src/ScottPlot4/ScottPlot/ScottPlot.csproj"
)

for project_file in "${project_files[@]}"; do
    echo "dotnet format $project_file"
    dotnet format "$project_file" || { echo 'FAILED!' ; exit 1; }
done
