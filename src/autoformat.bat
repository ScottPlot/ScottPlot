:: Install and update the dotnet 4 autoformatter
::dotnet tool install --global dotnet-format

:: Install and update the dotnet 5 autoformatter
dotnet tool update -g dotnet-format

:: Run the dotnet formatter
dotnet-format
pause