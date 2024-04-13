#!/bin/bash

while IFS=$'\n' read -r line; do
    time1=$(date +%s)
    echo "Building: $line"
    dotnet restore "$line"
    dotnet build "$line" --configuration Release /p:TargetFrameworks="net8.0"
    time2=$(date +%s)
    elapsed=$((time2 - time1))
    echo "$line built in $elapsed seconds"
done <".github/workflows/scripts/check-projects.txt"
