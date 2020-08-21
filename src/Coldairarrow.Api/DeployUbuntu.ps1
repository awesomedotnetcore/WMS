Write-Host 'Build Starting' -ForegroundColor Yellow
$CurPath=(Resolve-Path .).Path
$OutputPath=$CurPath+"\bin\publish\"
Remove-Item -Path $OutputPath -Force -Recurse
Invoke-Command -ScriptBlock {param($o) dotnet publish -o $o -c "Release" --no-self-contained -r linux-arm64 -v m --nologo "05.Coldairarrow.Api.csproj"} -ArgumentList $OutputPath
Write-Host 'Build Completed' -ForegroundColor Green

Write-Host 'Compress Starting' -ForegroundColor Yellow
$CurDateString=Get-Date -Format "yyyyMMddHHmmss"
$ZIPFileName="WMSAPI"+$CurDateString+".zip"
$ZIPFilePath=$CurPath+"\"+$ZIPFileName
$CompressPath=$OutputPath+"*"
Compress-Archive -Path $CompressPath -DestinationPath $ZIPFilePath
Write-Host 'Compress Completed' -ForegroundColor Green

Write-Host 'Deploy Starting' -ForegroundColor Yellow
# $User = "WDeployAdmin"
# $Password = Read-Host -Prompt "Please enter the server password" -AsSecureString
# Write-Host 'Start connecting to the server' -ForegroundColor Yellow
# $Credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $User, $Password
$Session = New-PSSession -HostName 10.76.20.51 -UserName giant -KeyFilePath "C:\Users\Administrator\id_rsa"
$Session
Write-Host 'Successfully connected to the server' -ForegroundColor Green
Write-Host 'Start copying files to the server' -ForegroundColor Yellow
$RemotePath="/home/giant/ZEQPWMS/"
Copy-Item $ZIPFilePath -Destination $RemotePath -ToSession $Session
Write-Host 'Copy files completed' -ForegroundColor Green
Write-Host 'Start Expand files on the server' -ForegroundColor Yellow
$RemoteDestinationPath=$RemotePath+"WMSAPI/"
$RemoteZipPath=$RemotePath+$ZIPFileName
# Invoke-Command -Session $Session -ScriptBlock {Stop-Service -Name "W3SVC"}
#Invoke-Command -Session $Session -ScriptBlock {iisreset}
Invoke-Command -Session $Session -ScriptBlock {param($p) chmod 777 $p -R} -ArgumentList $RemotePath

Invoke-Command -Session $Session -ScriptBlock {param($p) Remove-Item -Path $p -Recurse -Force} -ArgumentList $RemoteDestinationPath
Invoke-Command -Session $Session -ScriptBlock {param($p,$dp) Expand-Archive -Path $p -DestinationPath $dp} -ArgumentList $RemoteZipPath,$RemoteDestinationPath

Invoke-Command -Session $Session -ScriptBlock {param($p) chmod 777 $p -R} -ArgumentList $RemotePath
$ConfigProductionFile=$RemoteDestinationPath+"appsettings.Production.json"
Invoke-Command -Session $Session -ScriptBlock {param($p) Remove-Item -Path $p -Force} -ArgumentList $ConfigProductionFile
# Invoke-Command -Session $Session -ScriptBlock {Start-Service -Name "W3SVC"}
Write-Host 'Expand Completed' -ForegroundColor Green
Remove-Item -Path $ZIPFilePath
Write-Host 'Disconnected from server' -ForegroundColor Yellow
Write-Host 'Deploy Completed' -ForegroundColor Green