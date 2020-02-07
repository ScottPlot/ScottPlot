set cbSource="..\tests\bin\Debug\netcoreapp2.0\cookbook"
set cbDest="..\cookbook"

:: clear contents of the current cookbook
del /s /q %cbDest%\*
rmdir /s /q %cbDest%

:: copy new cookbook
xcopy  %cbSource% %cbDest%\ /E

pause
