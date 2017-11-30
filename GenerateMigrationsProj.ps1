# create a dotnot project to create ef migrations for the xamarin app

# relative path
Resolve-Path GenerateMigrationsProj.ps1
$projectPath = ".\MigrationsDotNetProj\"

# check if folder exsists
$folderExists = Test-Path $projectPath

# delete if exists
if ($folderExists) { Remove-Item $projectPath -Recurse }

# create a new folder
New-Item -Path $projectPath -ItemType directory

# create csproj file
Set-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") '<Project Sdk="Microsoft.NET.Sdk">'
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") ''
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") '  <PropertyGroup>'
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") '    <OutputType>Exe</OutputType>'
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") '    <TargetFramework>netcoreapp2.0</TargetFramework>'
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") '  </PropertyGroup>'
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") ''
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") '  <ItemGroup>'
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") '    <PackageReference Include="NETStandard.Library" Version="2.0.1" />'
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") '    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="2.0.1" />'
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") '    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="2.0.1" />'
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") '    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="2.0.1" />'
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") '    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.1" />'
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") '  </ItemGroup>'
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") ''
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") '</Project>'
Add-Content (Join-Path -Path $projectPath -ChildPath "Migrations.csproj") ''

# create main for build
Set-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") 'using System;'
Add-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") ''
Add-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") 'namespace Migrations'
Add-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") '{'
Add-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") '    class Program'
Add-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") '    {'
Add-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") '        static void Main(string[] args)'
Add-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") '        {'
Add-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") '            Console.WriteLine("Migrations!");'
Add-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") '            Console.ReadLine();'
Add-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") '        }'
Add-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") '    }'
Add-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") '}'
Add-Content (Join-Path -Path $projectPath -ChildPath "Program.cs") ''

Write-Output "done"
