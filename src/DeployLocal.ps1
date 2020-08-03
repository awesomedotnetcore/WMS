$CurPath=(Resolve-Path .).Path
$APIPath=$CurPath+"\Coldairarrow.Api"
$WebPath=$CurPath+"\Coldairarrow.Web"
$PadPath=$CurPath+"\Coldairarrow.Pad"
$CurDateString=Get-Date -Format "yyyyMMddHHmmss"

Set-Location -Path $APIPath
Write-Host 'Build API Starting' -ForegroundColor Yellow
$APIOutputPath=$APIPath+"\bin\publish\"
$APIProjectFilePath=$APIPath+"\05.Coldairarrow.Api.csproj"
Remove-Item -Path $APIOutputPath -Force -Recurse
Invoke-Command -ScriptBlock {param($o,$p) dotnet publish -o $o -c "Release" --no-self-contained -v m --nologo $p} -ArgumentList $APIOutputPath,$APIProjectFilePath
Write-Host 'Build API  Completed' -ForegroundColor Green

Write-Host 'Compress API Starting' -ForegroundColor Yellow
$APIZIPFileName="WMSAPI"+$CurDateString+".zip"
$APIZIPFilePath=$APIPath+"\"+$APIZIPFileName
$APICompressPath=$APIOutputPath+"*"
Compress-Archive -Path $APICompressPath -DestinationPath $APIZIPFilePath
Write-Host 'Compress API Completed' -ForegroundColor Green

Set-Location -Path $WebPath
Write-Host 'Build Web Starting' -ForegroundColor Yellow
$BuildScript={npm run build:live}
Invoke-Command -ScriptBlock $BuildScript
Write-Host 'Build Web Completed' -ForegroundColor Green

Write-Host 'Compress Web Starting' -ForegroundColor Yellow
$WebZIPFileName="WMSWeb"+$CurDateString+".zip"
$WebZIPFilePath=$WebPath+"\"+$WebZIPFileName
$WebCompressPath=$WebPath+"\dist\*"
Compress-Archive -Path $WebCompressPath -DestinationPath $WebZIPFilePath
Write-Host 'Compress Web Completed' -ForegroundColor Green

Set-Location -Path $PadPath
Write-Host 'Build Pad Starting' -ForegroundColor Yellow
$BuildScript={npm run build:live}
Invoke-Command -ScriptBlock $BuildScript
Write-Host 'Build Pad Completed' -ForegroundColor Green

Write-Host 'Compress Pad Starting' -ForegroundColor Yellow
$PadZIPFileName="WMSPad"+$CurDateString+".zip"
$PadZIPFilePath=$PadPath+"\"+$PadZIPFileName
$PadCompressPath=$PadPath+"\dist\*"
Compress-Archive -Path $PadCompressPath -DestinationPath $PadZIPFilePath
Write-Host 'Compress Pad Completed' -ForegroundColor Green

Set-Location -Path $CurPath

$RemotePath="E:\Publish\ZEQPWMS\"
Copy-Item $APIZIPFilePath -Destination $RemotePath
Copy-Item $WebZIPFilePath -Destination $RemotePath
Copy-Item $PadZIPFilePath -Destination $RemotePath
Write-Host 'Copy files completed' -ForegroundColor Green
Write-Host 'Start Expand files on the server' -ForegroundColor Yellow
$APIRemoteDestinationPath=$RemotePath+"WMSAPI\"
$APIRemoteZipPath=$RemotePath+$APIZIPFileName
$WebRemoteDestinationPath=$RemotePath+"WMSWeb\"
$WebRemoteZipPath=$RemotePath+$WebZIPFileName
$PadRemoteDestinationPath=$RemotePath+"WMSPad\"
$PadRemoteZipPath=$RemotePath+$PadZIPFileName
Stop-Service -Name "W3SVC"
Remove-Item -Path $APIRemoteDestinationPath -Recurse -Force
Remove-Item -Path $WebRemoteDestinationPath -Recurse -Force
Remove-Item -Path $PadRemoteDestinationPath -Recurse -Force
Expand-Archive -Path $APIRemoteZipPath -DestinationPath $APIRemoteDestinationPath
Expand-Archive -Path $WebRemoteZipPath -DestinationPath $WebRemoteDestinationPath
Expand-Archive -Path $PadRemoteZipPath -DestinationPath $PadRemoteDestinationPath
Start-Service -Name "W3SVC"
Write-Host 'Expand Completed' -ForegroundColor Green

Remove-Item -Path $APIZIPFilePath
Remove-Item -Path $WebZIPFilePath
Remove-Item -Path $PadZIPFilePath
Write-Host 'Disconnected from server' -ForegroundColor Yellow
Write-Host 'Deploy Completed' -ForegroundColor Green