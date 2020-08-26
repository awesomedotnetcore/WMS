Write-Host 'Build Starting' -ForegroundColor Yellow
$CurPath=(Resolve-Path .).Path
$OutputPath=$CurPath+"\bin\publish\"
Remove-Item -Path $OutputPath -Force -Recurse
Invoke-Command -ScriptBlock {param($o) dotnet publish -o $o -c "Release" --no-self-contained -r linux-arm64 -v m --nologo "05.Coldairarrow.Api.csproj"} -ArgumentList $OutputPath
Write-Host 'Build Completed' -ForegroundColor Green

Write-Host 'Compress Starting' -ForegroundColor Yellow
$CurDateString=Get-Date -Format "yyyyMMddHHmmss"
$ZIPFileName="Deploy"+$CurDateString+".zip"
$ZIPFilePath=$CurPath+"\"+$ZIPFileName
$CompressPath=$OutputPath+"*"
Compress-Archive -Path $CompressPath -DestinationPath $ZIPFilePath
Write-Host 'Compress Completed' -ForegroundColor Green

Write-Host 'Deploy Starting' -ForegroundColor Yellow
$Session = New-PSSession -HostName 10.76.20.162 -UserName root -KeyFilePath "C:\Users\Administrator\.ssh\id_rsa"
$Session
Write-Host 'Successfully connected to the server' -ForegroundColor Green
Write-Host 'Start copying files to the server' -ForegroundColor Yellow
$RemotePath="/srv/Deploy/"
Copy-Item $ZIPFilePath -Destination $RemotePath -ToSession $Session
Write-Host 'Copy files completed' -ForegroundColor Green
Write-Host 'Start Expand files on the server' -ForegroundColor Yellow
$RemoteDestinationPath=$RemotePath+"API/"
$RemoteZipPath=$RemotePath+$ZIPFileName
Invoke-Command -Session $Session -ScriptBlock {param($p) Remove-Item -Path $p -Recurse -Force} -ArgumentList $RemoteDestinationPath
Invoke-Command -Session $Session -ScriptBlock {param($p,$dp) Expand-Archive -Path $p -DestinationPath $dp} -ArgumentList $RemoteZipPath,$RemoteDestinationPath

$ConfigProductionFile=$RemoteDestinationPath+"appsettings.Production.json"
Invoke-Command -Session $Session -ScriptBlock {param($p) Remove-Item -Path $p -Force} -ArgumentList $ConfigProductionFile
Write-Host 'Expand Completed' -ForegroundColor Green

Write-Host 'Deploy to Docker Starting' -ForegroundColor Yellow
Invoke-Command -Session $Session -ScriptBlock {docker stop api}
Invoke-Command -Session $Session -ScriptBlock {docker rm api}
Invoke-Command -Session $Session -ScriptBlock {docker rmi api}
Invoke-Command -Session $Session -ScriptBlock {docker build -t api /srv/Deploy/API}
Invoke-Command -Session $Session -ScriptBlock {docker run -d -p 5000:5000 --name api api}
Write-Host 'Deploy to Docker Completed' -ForegroundColor Green

Remove-Item -Path $ZIPFilePath
Write-Host 'Deploy Completed' -ForegroundColor Green

