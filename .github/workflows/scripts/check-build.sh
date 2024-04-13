#!/bin/bash

while IFS=$'\n' read -r line; do
    echo "Building: $line"
    dotnet restore "$line"
    dotnet build "$line" --configuration Release
done <".github/workflows/scripts/check-projects.txt"
