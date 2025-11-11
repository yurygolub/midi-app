param(
    [string] $Project = 'MidiApp',

    [ValidateSet('release', 'debug')]
    [string] $Config = 'release',

    [ValidateSet('win-x64')]
    [string] $Runtime = 'win-x64',
    [bool] $SingleFile = $true,
    [bool] $SelfContained = $true,
    [bool] $ReadyToRun = $false,
    [bool] $Trimmed = $false,
    [bool] $NoRestore = $false,
    [bool] $Aot = $false
)

if (!(Get-Command -ErrorAction Ignore -Type Application dotnet))
{
    Write-Warning 'dotnet was not found'
    return
}

$ErrorActionPreference = 'Stop'

if(Test-Path -Path $Project -PathType Leaf)
{
    $projectName = [System.IO.Directory]::GetParent($Project).Name
}
elseif(Test-Path -Path $Project -PathType Container)
{
    $projectName = (Get-Item $Project).Name
}
else
{
    Write-Warning "specified project '$Project' does not exist"
    return
}

$projects = 'MidiApp'
if ($projects -notcontains $projectName)
{
    Write-Warning "'$Project' is not supported"
    Write-Host "Supported projects: $projects"
    return
}

$argsList = New-Object System.Collections.Generic.List[string]

if ($Config -eq 'release')
{
    $argsList.Add('-p:DebugType=None')
    $argsList.Add('-p:DebugSymbols=false')
}

if ($NoRestore)
{
    $argsList.Add('--no-restore')
}

$baseOutPath = 'publish'

if ($Aot)
{
    $outPath = [System.IO.Path]::Combine($baseOutPath, 'aot', $Config, $projectName)

    $argsList.Add("-p:PublishAot=$Aot")
}
else
{
    $outPath = [System.IO.Path]::Combine($baseOutPath, $Runtime, $Config, $projectName)

    if ($Config -eq 'release')
    {
        $argsList.Add("-p:IncludeNativeLibrariesForSelfExtract=$SingleFile")
        $argsList.Add("-p:IsTransformWebConfigDisabled=$SingleFile")
        $argsList.Add("-p:PublishSingleFile=$SingleFile")
        $argsList.Add("-p:PublishReadyToRun=$ReadyToRun")
        $argsList.Add("-p:PublishTrimmed=$Trimmed")
    }

    $argsList.AddRange([string[]]@('--self-contained', $SelfContained))
    $argsList.AddRange([string[]]@('--runtime', $Runtime))
}

dotnet publish $projectName --configuration $Config --output $outPath $argsList
