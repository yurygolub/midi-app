@echo off

where /q dotnet
if %ERRORLEVEL% NEQ 0 goto missingDotnet

where /q iscc
if %ERRORLEVEL% NEQ 0 goto missingIscc

set outputPath=publish

if exist "%outputPath%" rmdir /s /q "%outputPath%"

dotnet publish MidiApp --configuration Release --output "%outputPath%" -p:DebugType=None -p:DebugSymbols=false -p:SatelliteResourceLanguages=en --self-contained --os win

echo:

iscc create-installer.iss

goto exit

:missingDotnet
echo dotnet was not found.
echo You have to install .NET Runtime 7.0 and .NET Desktop Runtime 7.0 to run this application.
echo You can download it here https://dotnet.microsoft.com/en-us/download/dotnet/7.0
goto exit

:missingIscc
echo iscc was not found
echo You can download it here https://jrsoftware.org/isdl.php
goto exit

:exit
