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
$BuildScript={npm run build:docker}
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
$BuildScript={npm run build:docker}
Invoke-Command -ScriptBlock $BuildScript
Write-Host 'Build Pad Completed' -ForegroundColor Green

Write-Host 'Compress Pad Starting' -ForegroundColor Yellow
$PadZIPFileName="WMSPad"+$CurDateString+".zip"
$PadZIPFilePath=$PadPath+"\"+$PadZIPFileName
$PadCompressPath=$PadPath+"\dist\*"
Compress-Archive -Path $PadCompressPath -DestinationPath $PadZIPFilePath
Write-Host 'Compress Pad Completed' -ForegroundColor Green

Set-Location -Path $CurPath

Write-Host 'Deploy Starting' -ForegroundColor Yellow
$Session = New-PSSession -HostName 10.76.20.162 -UserName root -KeyFilePath "C:\Users\Administrator\.ssh\id_rsa"
$Session
Write-Host 'Successfully connected to the server' -ForegroundColor Green
Write-Host 'Start copying files to the server' -ForegroundColor Yellow
$RemotePath="/srv/zeqpwms/"
Copy-Item $APIZIPFilePath -Destination $RemotePath -ToSession $Session
Copy-Item $WebZIPFilePath -Destination $RemotePath -ToSession $Session
Copy-Item $PadZIPFilePath -Destination $RemotePath -ToSession $Session
Write-Host 'Copy files completed' -ForegroundColor Green
Write-Host 'Start Expand files on the server' -ForegroundColor Yellow
$APIRemoteDestinationPath=$RemotePath+"WMSAPI"
$APIRemoteZipPath=$RemotePath+$APIZIPFileName
$WebRemoteDestinationPath=$RemotePath+"WMSWeb"
$WebRemoteZipPath=$RemotePath+$WebZIPFileName
$PadRemoteDestinationPath=$RemotePath+"WMSPad"
$PadRemoteZipPath=$RemotePath+$PadZIPFileName
Invoke-Command -Session $Session -ScriptBlock {param($p) Remove-Item -Path $p -Recurse -Force} -ArgumentList $APIRemoteDestinationPath
Invoke-Command -Session $Session -ScriptBlock {param($p) Remove-Item -Path $p -Recurse -Force} -ArgumentList $WebRemoteDestinationPath
Invoke-Command -Session $Session -ScriptBlock {param($p) Remove-Item -Path $p -Recurse -Force} -ArgumentList $PadRemoteDestinationPath
Invoke-Command -Session $Session -ScriptBlock {param($p,$dp) Expand-Archive -Path $p -DestinationPath $dp} -ArgumentList $APIRemoteZipPath,$APIRemoteDestinationPath
Invoke-Command -Session $Session -ScriptBlock {param($p,$dp) Expand-Archive -Path $p -DestinationPath $dp} -ArgumentList $WebRemoteZipPath,$WebRemoteDestinationPath
Invoke-Command -Session $Session -ScriptBlock {param($p,$dp) Expand-Archive -Path $p -DestinationPath $dp} -ArgumentList $PadRemoteZipPath,$PadRemoteDestinationPath
Write-Host 'Expand Completed' -ForegroundColor Green

Write-Host 'Deploy to Docker Starting' -ForegroundColor Yellow
Invoke-Command -Session $Session -ScriptBlock {docker stop wmsapi}
Invoke-Command -Session $Session -ScriptBlock {docker rm wmsapi}
Invoke-Command -Session $Session -ScriptBlock {docker rmi wmsapi}
Invoke-Command -Session $Session -ScriptBlock {param($p) docker build -t wmsapi $p} -ArgumentList $APIRemoteDestinationPath
Invoke-Command -Session $Session -ScriptBlock {docker run -d -p 5000:5000 --name wmsapi wmsapi}

Invoke-Command -Session $Session -ScriptBlock {docker stop wmsweb}
Invoke-Command -Session $Session -ScriptBlock {docker rm wmsweb}
Invoke-Command -Session $Session -ScriptBlock {docker rmi wmsweb}
Invoke-Command -Session $Session -ScriptBlock {param($p) docker build -t wmsweb $p} -ArgumentList $WebRemoteDestinationPath
Invoke-Command -Session $Session -ScriptBlock {docker run -d -p 8080:80 --name wmsweb wmsweb}

Invoke-Command -Session $Session -ScriptBlock {docker stop wmspad}
Invoke-Command -Session $Session -ScriptBlock {docker rm wmspad}
Invoke-Command -Session $Session -ScriptBlock {docker rmi wmspad}
Invoke-Command -Session $Session -ScriptBlock {param($p) docker build -t wmspad $p} -ArgumentList $PadRemoteDestinationPath
Invoke-Command -Session $Session -ScriptBlock {docker run -d -p 8081:80 --name wmspad wmspad}
Write-Host 'Deploy to Docker Completed' -ForegroundColor Green

Remove-Item -Path $APIZIPFilePath
Remove-Item -Path $WebZIPFilePath
Remove-Item -Path $PadZIPFilePath
Write-Host 'Deploy Completed' -ForegroundColor Green