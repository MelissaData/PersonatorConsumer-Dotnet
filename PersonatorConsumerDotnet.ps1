<#
.SYNOPSIS
    Builds and runs the Melissa Personator Consumer Cloud API .NET sample.

.DESCRIPTION
    This script builds PersonatorConsumerDotnet with dotnet publish, then runs the
    resulting executable, passing along the license and (if supplied) the address fields.

    Overall flow:
      1. Resolve the license (parameter, prompt, or MD_LICENSE environment variable).
      2. Publish PersonatorConsumerDotnet in Release configuration to
         .\PersonatorConsumerDotnet\Build.
      3. Run the built executable: one-shot mode if any address field was supplied,
         otherwise interactive mode (the .NET program prompts for each field).

.PARAMETER addressline1
    Street address to verify in one-shot mode.

.PARAMETER city
    City to verify in one-shot mode.

.PARAMETER state
    State to verify in one-shot mode.

.PARAMETER postal
    Postal code to verify in one-shot mode.

.PARAMETER country
    Country to verify in one-shot mode.

.PARAMETER license
    License string. Resolved in this order:
      1. This parameter.
      2. An interactive prompt, if the parameter was not supplied.
      3. The MD_LICENSE environment variable, if the prompt was left blank.
    Note that the environment variable is the last resort, not the first: running
    without -license always prompts, even when MD_LICENSE is set.

.PARAMETER quiet
    Accepted for parity with other sample scripts; not currently used to suppress output.

.EXAMPLE
    .\PersonatorConsumerDotnet.ps1 -license "your-license"

.EXAMPLE
    .\PersonatorConsumerDotnet.ps1 -addressline1 "22382 Avenida Empresa" -city "Rancho Santa Margarita" -state "CA" -postal "92688" -country "United States" -license "your-license"
#>

######################### Parameters ##########################
param(
    $addressline1 = '',
    $city = '',
    $state = '',
    $postal = '',
    $country = '',
    $license = '',
    [switch]$quiet = $false
    )

# Uses the location of the .ps1 file
$CurrentPath = $PSScriptRoot
Set-Location $CurrentPath
$ProjectPath = "$CurrentPath\PersonatorConsumerDotnet"
$BuildPath = "$ProjectPath\Build"

If (!(Test-Path $BuildPath)) {
  New-Item -Path $ProjectPath -Name 'Build' -ItemType "directory"
}

########################## Main ############################
Write-Host "`n==================== Melissa Personator Consumer Cloud API =====================`n"

# Get license (either from parameters or user input)
if ([string]::IsNullOrEmpty($license) ) {
  $license = Read-Host "Please enter your license string"
}

# Check for License from Environment Variables 
if ([string]::IsNullOrEmpty($license) ) {
  $license = $env:MD_LICENSE 
}

if ([string]::IsNullOrEmpty($license)) {
  Write-Host "`nLicense String is invalid!"
  Exit
}

# Start program
# Build project
Write-Host "`n================================= BUILD PROJECT ================================"

dotnet publish -f="net8.0" -c Release -o $BuildPath PersonatorConsumerDotnet\PersonatorConsumerDotnet.csproj

# Run project
# No address fields supplied -> run interactively; otherwise pass the supplied ones through for one-shot mode.
if ([string]::IsNullOrEmpty($addressline1) -and [string]::IsNullOrEmpty($city) -and [string]::IsNullOrEmpty($state) -and [string]::IsNullOrEmpty($postal) -and [string]::IsNullOrEmpty($country)) {
  dotnet $BuildPath\PersonatorConsumerDotnet.dll --license $license
}
else {
  # Only pass flags that have a value. Windows PowerShell drops empty-string arguments to
  # native programs, which would shift the next flag name into this flag's value.
  # Any field left out here is prompted for by the program.
  $runArgs = @('--license', $license)
  if (-not [string]::IsNullOrEmpty($addressline1)) { $runArgs += '--addressline1', $addressline1 }
  if (-not [string]::IsNullOrEmpty($city))         { $runArgs += '--city', $city }
  if (-not [string]::IsNullOrEmpty($state))        { $runArgs += '--state', $state }
  if (-not [string]::IsNullOrEmpty($postal))       { $runArgs += '--postal', $postal }
  if (-not [string]::IsNullOrEmpty($country))      { $runArgs += '--country', $country }
  dotnet $BuildPath\PersonatorConsumerDotnet.dll @runArgs
}
