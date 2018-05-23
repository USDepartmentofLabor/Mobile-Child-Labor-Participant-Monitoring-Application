#!/bin/bash
# Add a migration to MDPMS.Database.Localization

# MDPMS SLN DIR
MDPMS_SLN_DIR=$1
MIGRATION_NAME=$2

# get the path this is running from
SCRIPT_DIR="$( cd "$( dirname "$0" )" && pwd )"

# get a relative path to create the migration temp project in
REL_PATH=$SCRIPT_DIR/MigrationsDotNetProj

# check if the directory exists already
if [ -d $REL_PATH ]; then
  # delete recurse if it exists
  rm -rf $REL_PATH
fi

# create a new directory
mkdir $REL_PATH

# create a csproj file
CSPROJ_PATH=${REL_PATH}/Migrations.csproj
touch $CSPROJ_PATH
echo '<Project Sdk="Microsoft.NET.Sdk">' >> $CSPROJ_PATH
echo '' >> $CSPROJ_PATH
echo '  <PropertyGroup>' >> $CSPROJ_PATH
echo '    <OutputType>Exe</OutputType>' >> $CSPROJ_PATH
echo '    <TargetFramework>netcoreapp2.0</TargetFramework>' >> $CSPROJ_PATH
echo '  </PropertyGroup>' >> $CSPROJ_PATH
echo '' >> $CSPROJ_PATH
echo '  <ItemGroup>' >> $CSPROJ_PATH
echo '    <PackageReference Include="NETStandard.Library" Version="2.0.1" />' >> $CSPROJ_PATH
echo '    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="2.0.1" />' >> $CSPROJ_PATH
echo '    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="2.0.1" />' >> $CSPROJ_PATH
echo '    <PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="2.0.1" />' >> $CSPROJ_PATH
echo '    <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.1" />' >> $CSPROJ_PATH
echo '  </ItemGroup>' >> $CSPROJ_PATH
echo '' >> $CSPROJ_PATH
echo '</Project>' >> $CSPROJ_PATH

# create main for build
CSMAIN_PATH=${REL_PATH}/Program.cs
echo 'using System;' >> $CSMAIN_PATH
echo '' >> $CSMAIN_PATH
echo 'namespace Migrations' >> $CSMAIN_PATH
echo  '{' >> $CSMAIN_PATH
echo  '    class Program' >> $CSMAIN_PATH
echo  '    {' >> $CSMAIN_PATH
echo  '        static void Main(string[] args)' >> $CSMAIN_PATH
echo  '        {' >> $CSMAIN_PATH
echo  '            Console.WriteLine("Migrations!");' >> $CSMAIN_PATH
echo  '            Console.ReadLine();' >> $CSMAIN_PATH
echo  '        }' >> $CSMAIN_PATH
echo  '    }' >> $CSMAIN_PATH
echo  '}' >> $CSMAIN_PATH
echo '' >> $CSMAIN_PATH

# copy files from ef database project
DIRS_TO_COPY=('MDPMS/MDPMS.Database.Localization/Database' 'MDPMS/MDPMS.Database.Localization/Models' 'MDPMS/MDPMS.Database.Localization/Translations')
for i in "${DIRS_TO_COPY[@]}"; do
  # $i
  DB_DIR=$MDPMS_SLN_DIR$i
  if [ -d $DB_DIR ]; then
    # safe to copy
    cp $DB_DIR/* $REL_PATH
  else
    # error
    echo "### ERROR FINDING " $i " ###"
    exit
  fi
done

# make migrations folder
MIGRATIONS_PATH=$REL_PATH/Migrations
mkdir $MIGRATIONS_PATH

# copy existing migrations
EXISTING_MIGRATIONS_PATH=$MDPMS_SLN_DIR/MDPMS/MDPMS.Database.Localization/Migrations
if [ -d $EXISTING_MIGRATIONS_PATH ]; then
  # safe to copy
  cp $EXISTING_MIGRATIONS_PATH/. $MIGRATIONS_PATH
fi

# add needed refs here
cd $REL_PATH

# build a migration
dotnet restore
dotnet build
dotnet ef migrations add "$MIGRATION_NAME"
cd ..

#copy generated migrations back
cp $MIGRATIONS_PATH/* $EXISTING_MIGRATIONS_PATH

# clean up temp files
rm -rf $REL_PATH

echo "DONE"
