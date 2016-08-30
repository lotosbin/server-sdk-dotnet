dotnet restore
dotnet build .\RongCloudServerSDK
dotnet pack .\RongCloudServerSDK

$project = Get-Content .\RongCloudServerSDK\project.json | ConvertFrom-Json
$version = $project.version.Trim("-*")
nuget push .\RongCloudServerSDK\bin\Debug\RongCloudServerSDK.$version.nupkg -source nuget -apikey $env:NUGET_API_KEY