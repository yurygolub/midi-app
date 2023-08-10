#define AppExec "publish\MidiApp.exe"
#define AppName "MidiApp"
#define AppVersion GetStringFileInfo(AppExec, "Assembly Version")
#define AppPublisher "yurygolub"
#define AppURL "https://github.com/yurygolub/midi-sample"
#define OutputDir "deploy"
#define SourceDir "publish\*"

[Setup]
AppName={#AppName}
AppId=net.example.midi-sample
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
VersionInfoVersion={#AppVersion}
OutputDir={#OutputDir}
OutputBaseFilename={#AppName}.x64
AppPublisher={#AppPublisher}
AppCopyright=Copyright (C) {#AppPublisher} 2023
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
SetupIconFile=
DefaultGroupName={#AppName}
DefaultDirName={autopf}\{#AppName}
AllowNoIcons=yes
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
MinVersion=10
PrivilegesRequired=lowest

[Files]
Source: {#SourceDir}; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs;

[Tasks]
Name: "desktopicon"; Description: "Create a &Desktop Icon"; GroupDescription: "Additional icons:"; Flags: unchecked

[REGISTRY]

[Icons]
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppName}.exe"
Name: "{userdesktop}\{#AppName}"; Filename: "{app}\{#AppName}.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\{#AppName}.exe"; Description: Start Application Now; Flags: postinstall nowait skipifsilent

[InstallDelete]
Type: filesandordirs; Name: "{app}\*";
Type: filesandordirs; Name: "{group}\*";

[UninstallRun]

[UninstallDelete]
Type: dirifempty; Name: "{app}"
