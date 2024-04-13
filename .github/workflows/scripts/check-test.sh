#!/bin/bash

.github/workflows/scripts/check-files.sh

for filepath in "${project_files[@]}"; do
    dotnet test "$filepath" --configuration Release --no-build --verbosity minimal
done
