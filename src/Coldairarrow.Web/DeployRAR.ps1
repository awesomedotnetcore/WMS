$WinRARPath="C:\Program Files\WinRAR\WinRAR.exe"
Write-Host 'Build Starting' -ForegroundColor Yellow
$BuildScript={npm run build:live}
Invoke-Command -ScriptBlock $BuildScript
Write-Host 'Build Completed' -ForegroundColor Green
Write-Host 'RAR Starting' -ForegroundColor Yellow
$CurDateString=Get-Date -Format "yyyyMMddHHmmss"
$RARFileName="WMSWeb"+$CurDateString+".rar"
$CurPath=(Resolve-Path .).Path
Start-Process -FilePath $WinRARPath -ArgumentList "a","-r","-ep1","-cfg-","-ibck",$RARFileName,".\dist\*.*" -WorkingDirectory $CurPath -NoNewWindow -Wait
Write-Host 'RAR Completed' -ForegroundColor Green
$RARFilePath=$CurPath+"\"+$RARFileName
Write-Host 'Deploy Starting' -ForegroundColor Yellow
$User = "WDeployAdmin"
$Password = Read-Host -Prompt "Please enter the server password" -AsSecureString
Write-Host 'Start connecting to the server' -ForegroundColor Yellow
$Credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $User, $Password
$Session = New-PSSession -ComputerName 114.115.162.100 -Credential $Credential
Write-Host 'Successfully connected to the server' -ForegroundColor Green
Write-Host 'Start deploying files to the server' -ForegroundColor Yellow
$RemotePath="D:\ZEQPWMS\"
Copy-Item $RARFilePath -Destination $RemotePath -ToSession $Session
$RemoteRemovePath=$RemotePath+"WMSWeb\*"
Invoke-Command -Session $Session -ScriptBlock {param($p) Remove-Item -Path $p -Recurse -Force} -ArgumentList $RemoteRemovePath
Invoke-Command -Session $Session -ScriptBlock {param($p,$n,$rp) Start-Process -FilePath $p -ArgumentList "x",$n,"*",".\WMSWeb\","-ibck" -WorkingDirectory $rp -NoNewWindow -Wait} -ArgumentList $WinRARPath,$RARFileName,$RemotePath
Disconnect-PSSession -Session $Session
Remove-Item -Path $RARFilePath
Write-Host 'Disconnected from server' -ForegroundColor Yellow
Write-Host 'Deploy Completed' -ForegroundColor Green