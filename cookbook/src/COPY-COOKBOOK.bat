:: clear the old cookbook
del /q ..\..\doc\cookbook\images\*.*
del /q ..\..\doc\cookbook\README.md

:: copy the new cookbook
copy .\bin\Debug\images\*.* ..\..\doc\cookbook\images\*.*
copy .\bin\Debug\README.md ..\..\doc\cookbook\README.md

pause