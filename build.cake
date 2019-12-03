#addin Cake.Git
#addin nuget:?package=Nuget.Core

using NuGet;

var artifactsDir = "./artifacts/";
var solutionPath = "./Validators.sln";
var project = "./src/BizValidators/BizValidators.csproj";
var testFolder = "./tests/BizValidators.Tests";
var testProject = testFolder + "BizValidators.Tests.csproj";
var nugetSource = "https://api.nuget.org/v3/index.json";

var packageOutputPath = Argument<DirectoryPath>("packageOutputPath", "packages");
var configuration = Argument("configuration", "Debug");

var propsFile = "./src/Directory.build.props";
var readedVersion = XmlPeek(propsFile, "//Version");
var currentVersion = new Version(readedVersion);

var nugetApiKey = EnvironmentVariable("nugetApiKey") ?? "";
var target = EnvironmentVariable("buildTarget") ?? "DevelopBuild";
var isCiBuild = (EnvironmentVariable("isCIBuild") ?? "").Length > 1;
var isPatchVersion = (EnvironmentVariable("isPatchVersion") ?? "").Length > 1;

Task("Info")
  .Does(() =>
  {
    Information("Env variables list: ");
    Information("target: " + target);
    Information("isCiBuild: " + isCiBuild);
    Information("isPatchVersion: " + isPatchVersion);
    Information("nugetApiKey: " + (nugetApiKey.Length > 1 ? "***" : "not found"));
    Information("Env variables list END");
  });

Task("Clean")
  .IsDependentOn("Info")
  .Does(() => 
  {
    if (DirectoryExists(artifactsDir))
    {
        DeleteDirectory(
            artifactsDir, 
            new DeleteDirectorySettings {
                Recursive = true,
                Force = true
            }
        );
    }
    CreateDirectory(artifactsDir);
    DotNetCoreClean(solutionPath);
  });

Task("Restore")
  .Does(() => 
  {
    DotNetCoreRestore();
  });

Task("UpdateVersion")
  .Does(() =>
  {
    Information("Old version: " + currentVersion);
    if(isCiBuild)
    {
      if(isPatchVersion)
      {
        var semVersion = new Version(currentVersion.Major, currentVersion.Minor, currentVersion.Build + 1);
        var version = semVersion.ToString();
        XmlPoke(propsFile, "//Version", version);
        Information("New version: " + version);
      }
      else
      {
        var semVersion = new Version(currentVersion.Major, currentVersion.Minor + 1, 0);
        var version = semVersion.ToString();
        XmlPoke(propsFile, "//Version", version);
        Information("New version: " + version);
      }
    }
});

Task("Build")
  .IsDependentOn("Clean")
  .IsDependentOn("Restore")
  .IsDependentOn("UpdateVersion")
  .Does(() => 
  {
    var settings = new DotNetCoreBuildSettings
    {
        Framework = "netstandard2.0",
        Configuration = "Debug",
    };

    DotNetCoreBuild(project, settings);
  });

Task("Test")
  .IsDependentOn("Restore")
  .Does(() => 
  {
    DotNetCoreTest(testFolder);
  });

Task("Pack")
  .IsDependentOn("Build")
  .Does(() =>   
  {
    var settings = new DotNetCorePackSettings
    {
      OutputDirectory = artifactsDir,
      NoBuild = true,
    };
    DotNetCorePack(project, settings);
  });

Task("Publish")
  .IsDependentOn("Pack")
  .Does(() => {
      var pushSettings = new DotNetCoreNuGetPushSettings 
      {
          Source = nugetSource,
          ApiKey = nugetApiKey
      };

      var pkgs = GetFiles(artifactsDir + "*.nupkg");
      foreach(var pkg in pkgs) 
      {
        Information($"Publishing \"{pkg}\".");
        DotNetCoreNuGetPush(pkg.FullPath, pushSettings);
      }
  });

Task("DevelopBuild")
  .IsDependentOn("Clean")
  .IsDependentOn("Restore")
  .IsDependentOn("Test")
  .Does(() => 
  {
    var settings = new DotNetCoreBuildSettings
    {
        Framework = "netstandard2.0",
        Configuration = "Debug",
    };

    DotNetCoreBuild(project, settings);
  });

RunTarget(target);