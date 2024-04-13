#!/bin/bash

test_project_files=(
    "src/ScottPlot5/ScottPlot5 Tests/ScottPlot Tests.csproj"
    "src/ScottPlot5/ScottPlot5 Cookbook/ScottPlot Cookbook.csproj"
    "src/ScottPlot4/ScottPlot/ScottPlot.csproj"
)

for test_project_file in "${project_files[@]}"; do
    echo "Testing: $test_project_file"
    dotnet restore "$test_project_file"
    dotnet build "$test_project_file" --configuration Release
done
