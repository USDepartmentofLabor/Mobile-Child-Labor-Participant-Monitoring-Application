# create a dotnot project to create ef migrations for the xamarin app

Param([string]$migrationName)

# relative path
Resolve-Path AddMigration.ps1
$projectPath = ".\MigrationsDotNetProj\"
$csprojPath = (Join-Path -Path $projectPath -ChildPath "Migrations.csproj")

# check if folder exsists
$folderExists = Test-Path $projectPath

# delete if exists
if ($folderExists) { Remove-Item $projectPath -Recurse }

# create a new folder
New-Item -Path $projectPath -ItemType directory

# create csproj file
Set-Content $csprojPath '<Project Sdk="Microsoft.NET.Sdk">'
Add-Content $csprojPath ''
Add-Content $csprojPath '  <PropertyGroup>'
Add-Content $csprojPath '    <OutputType>Exe</OutputType>'
Add-Content $csprojPath '    <TargetFramework>netcoreapp2.0</TargetFramework>'
Add-Content $csprojPath '  </PropertyGroup>'
Add-Content $csprojPath ''
Add-Content $csprojPath '  <ItemGroup>'
Add-Content $csprojPath '    <PackageReference Include="NETStandard.Library" Version="2.0.1" />'
Add-Content $csprojPath '    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="2.0.1" />'
Add-Content $csprojPath '    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="2.0.1" />'
Add-Content $csprojPath '    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="2.0.1" />'
Add-Content $csprojPath '    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.1" />'
Add-Content $csprojPath '  </ItemGroup>'
Add-Content $csprojPath ''
Add-Content $csprojPath '</Project>'

# create main for build
$programCsPath = (Join-Path -Path $projectPath -ChildPath "Program.cs")
Set-Content $programCsPath 'using System;'
Add-Content $programCsPath ''
Add-Content $programCsPath 'namespace Migrations'
Add-Content $programCsPath '{'
Add-Content $programCsPath '    class Program'
Add-Content $programCsPath '    {'
Add-Content $programCsPath '        static void Main(string[] args)'
Add-Content $programCsPath '        {'
Add-Content $programCsPath '            Console.WriteLine("Migrations!");'
Add-Content $programCsPath '            Console.ReadLine();'
Add-Content $programCsPath '        }'
Add-Content $programCsPath '    }'
Add-Content $programCsPath '}'
Add-Content $programCsPath ''

# copy files from ef database project
Copy-Item .\MDPMS\MDPMS.EfDatabase\Database\* $projectPath
Copy-Item .\MDPMS\MDPMS.EfDatabase\EfModels\* $projectPath
Copy-Item .\MDPMS\MDPMS.EfDatabase\EfModels\Base\* $projectPath

# copy existing migrations
$tempProjMigrationsPath = (Join-Path -Path $projectPath -ChildPath "Migrations\")
New-Item -Path $tempProjMigrationsPath -ItemType directory
Copy-Item .\MDPMS\MDPMS.EfDatabase\Migrations\* $tempProjMigrationsPath

# build a migration
iex "dotnet restore $csprojPath"
iex "dotnet build $csprojPath"
cd $projectPath
iex "dotnet ef migrations add $migrationName"
cd ..

#copy generated migrations back
Copy-Item (Join-Path -Path $tempProjMigrationsPath -ChildPath "\*") .\MDPMS\MDPMS.EfDatabase\Migrations\

Write-Output "done"
