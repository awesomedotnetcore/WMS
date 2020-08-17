#安装IIS
# Install-WindowsFeature -Name Web-Server -IncludeAllSubFeature -IncludeManagementTools -ComputerName Server1 -Credential contoso.com\johnj99

#安装.Net Core
#https://dotnet.microsoft.com/download/dotnet-core/scripts/v1/dotnet-install.ps1


$ListSiteName="WMSAPI","WMSWeb","WMSPad"
$DomainName="giantliu.cn"
$RootPhysicalPath="C:\WorkSpace\Test\"
foreach($SiteName in $ListSiteName)
{
    #创建应用程序池
    New-WebAppPool $SiteName
    $PropPath="IIS:\AppPools\"+$SiteName
    Set-ItemProperty -Path $PropPath -Name "managedRuntimeVersion" -Value ''
    #创建站点
    $PhysicalPath=$RootPhysicalPath+$SiteName
    New-Item -Path $PhysicalPath -ItemType Directory
    $HostHeader=$SiteName.ToLower()+"."+$DomainName
    New-Website -Name $SiteName -Port 80 -IPAddress * -HostHeader $HostHeader -PhysicalPath $PhysicalPath -ApplicationPool $SiteName
    #New-WebBinding -Name $SiteName -Protocol "http" -Port 80 -IPAddress * -HostHeader $HostHeader
    New-WebBinding -Name $SiteName -Protocol "https" -Port 443 -IPAddress * -HostHeader $HostHeader -SslFlags 0
}