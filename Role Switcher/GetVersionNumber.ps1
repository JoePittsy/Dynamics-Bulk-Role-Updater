# GetAssemblyVersion.ps1

param (
    [Parameter(Mandatory=$true)]
    [string]$dllPath
)

# Convert the provided path to an absolute path
$absolutePath = Resolve-Path $dllPath

# Load the DLL into the current AppDomain
$assembly = [System.Reflection.Assembly]::LoadFile($absolutePath)

# Extract the version
$versionObj = $assembly.GetName().Version

# Convert the version object to a string
$versionStr = "{0}.{1}.{2}.{3}" -f $versionObj.Major, $versionObj.Minor, $versionObj.Build, $versionObj.Revision

return $versionStr
