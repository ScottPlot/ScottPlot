#!/bin/bash

while IFS=$'\n' read -r line; do
    echo "Testing: $line"
    dotnet test "$line" --configuration Release --no-build --verbosity minimal
done <".github/workflows/scripts/check-projects.txt"
