@echo off

where /q dotnet
if %ERRORLEVEL% NEQ 0 goto missingDotnet

dotnet publish MidiApp --configuration Release --output publish/MidiApp -p:DebugType=None -p:DebugSymbols=false -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true --no-self-contained --os win

goto exit

:missingDotnet
echo dotnet was not found.
echo You have to install .NET Runtime 7.0 and .NET Desktop Runtime 7.0 to run this application.
echo You can download it here https://dotnet.microsoft.com/en-us/download/dotnet/7.0
goto exit

:exit
