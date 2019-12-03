var target = Argument("target", "Default");

Setup(context =>
{
    Information("Validators cake script start!");
});

Teardown(context =>
{
    Information("Validators cake script finish!");
});

Task("Default")
  .Does(() =>
{
  Information("Hello World!");
});

Task("Restore")
  .Does(() => 
{
    DotNetCoreRestore();
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => {
        var settings = new DotNetCoreBuildSettings
        {
            Framework = "netstandard2.0",
            Configuration = "Debug",
            OutputDirectory = "./artifacts/"
        };
        DotNetCoreBuild("./Validators/Validators.csproj", settings);
    });

RunTarget("Build");