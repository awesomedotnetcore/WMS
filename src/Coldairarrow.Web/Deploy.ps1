Write-Host 'Deploy Starting' -ForegroundColor Yellow
$User = "WDeployAdmin"
$Password = Read-Host -Prompt "Please enter the server password" -AsSecureString
# $PWord = ConvertTo-SecureString -String $Password -AsPlainText -Force
Write-Host 'Start connecting to the server' -ForegroundColor Yellow
$Credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList $User, $Password
$Session = New-PSSession -ComputerName 114.115.162.100 -Credential $Credential
Write-Host 'Successfully connected to the server' -ForegroundColor Green
Write-Host 'Start deploying files to the server' -ForegroundColor Yellow
Copy-Item ".\dist\*" -Destination "D:\ZEQPWMS\WMSWeb\" -ToSession $Session -Recurse -Force
Write-Host 'Deploy Completed' -ForegroundColor Green
Disconnect-PSSession -Session $Session
Write-Host 'Disconnected from server' -ForegroundColor Yellow