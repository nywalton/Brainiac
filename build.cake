#addin "Cake.Powershell"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var binPath = "./bin";
var publishPath = "./publish";
var solutionFile = "./Brainiac.sln";

var binDir = Directory(binPath);
var objDir = Directory("./obj");
var publishDir = Directory(publishPath);
var projectFile = "./src/Brainiac/Brainiac.csproj";

var testProjectfile = "./test/Brainiac.Test/Brainiac.Test.csproj";

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var framework = Argument("framework", "netcoreapp2.1");
var runtime = Argument("runtime", "win10-arm");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean").Does(() => 
{
    CleanDirectory(binDir);
    CleanDirectory(Directory("./src/Brainiac/bin"));
    CleanDirectory(Directory("./test/Brainiac.Test/bin"));

    CleanDirectory(objDir);
    CleanDirectory(Directory("./src/Brainiac/obj"));
    CleanDirectory(Directory("./test/Brainiac.Test/obj"));

    CleanDirectory(publishDir);

    if (FileExists(solutionFile)) 
    {
        DeleteFile(solutionFile);
    }
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() => 
{
    DotNetCoreRestore(projectFile);
    DotNetCoreRestore(testProjectfile);
});

Task("GenSln")
    .IsDependentOn("Restore")
    .Does(() => 
{
    Information("Creating the solution...");
    StartPowershellScript("dotnet new sln");
    StartPowershellScript("dotnet sln add ./src/Brainiac");
    StartPowershellScript("dotnet sln add ./test/Brainiac.Test");
});

Task("BuildSln")
    .IsDependentOn("GenSln")
    .Does(() => 
{
    Information("Generating Cake project...");
    DotNetCoreBuild(solutionFile, new DotNetCoreBuildSettings
    {

        Framework = framework,
        Configuration = configuration,
        OutputDirectory = binPath
    });
}).OnError(exception => {
    Error("Build Failed, throwing exception...");
    throw exception;
});

Task("Build")
    .Does(() => 
{
    Information("Building project...");
    DotNetCoreBuild(projectFile, new DotNetCoreBuildSettings
    {
        Framework = framework,
        Configuration = configuration,
        OutputDirectory = binPath
    });
}).OnError(exception => {
    Error("Build Failed, throwing exception...");
    throw exception;
});

Task("Publish")
    .IsDependentOn("Build")
    .Does(() => 
{
    var buildSettings = new DotNetCorePublishSettings
    {
        Framework = framework,
        Configuration = configuration,
        OutputDirectory = publishPath,
        Runtime = runtime
    };

    Information("Publishing project...");
    DotNetCorePublish(projectFile, buildSettings);
}).OnError(exception => {
    Error("Build Failed, throwing exception...");
    throw exception;
});

Task("Run")
	.IsDependentOn("Build")
	.Does(() => 
{
    Information("Running Brainiac.csproj...");
    DotNetCoreRun(projectFile);
}).OnError(exception => {
    Error("Build Failed, throwing exception...");
    throw exception;
});

Task("Execute")
	.IsDependentOn("Build")
	.Does(() => 
{
    Information("Running Brainiac.dll...");
    DotNetCoreExecute(binPath + "/Brainiac.dll");
}).OnError(exception => {
    Error("Build Failed, throwing exception...");
    throw exception;
});

Task("RunUnitTests")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest(testProjectfile, new DotNetCoreTestSettings {
        Configuration = configuration,
        NoBuild = true
    });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run");
	
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);