#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=ReportGenerator&version=4.8.11"
#tool "nuget:?package=OpenCoverToCoberturaConverter&version=0.3.4"
#addin "nuget:?package=Cake.Coverlet&version=4.0.1"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

var solution = "Richter.WhoAmIApi";
var applicationName = "teste";

var appVersion = Argument("appVersion", "");
var sonarHost = Argument("sonarHost", "");
var sonarToken = Argument("sonarToken", "");
var branchName = Argument("branchName", "");
var idPullRequest = Argument("idPullRequest", "");
var branchDestination = Argument("branchDestination", "");

Task("Clean")
    .Does(() =>
	{
		var settings = new DotNetCleanSettings
        {
            NoLogo = true,
            Verbosity = DotNetVerbosity.Quiet
        };

		DotNetClean(".", settings);
	});

Task("BuildSolution")
	.IsDependentOn("Clean")
    .Does(() =>
    {
	    var settings = new DotNetBuildSettings
        {
            Configuration = "Debug",
            ArgumentCustomization = args => args.Append(" --configfile .nuget/NuGet.config /p:DebugType=Full")
        };

        DotNetBuild(".", settings);
    });

Task("BuildTest")
    .Does(() =>
    {
	    var settings = new DotNetBuildSettings
        {
            Configuration = "Debug",
            ArgumentCustomization = args => args.Append(" --configfile .nuget/NuGet.config /p:DebugType=Full")
        };

        DotNetBuild(".", settings);
    });

Task("TestRun")
.IsDependentOn("BuildTest")
    .Does(() =>
    {
		var testSettings = new DotNetTestSettings
		{
			Configuration = "Debug",
            NoBuild = true,
			ArgumentCustomization = args => args.Append("--logger trx;LogFileName=result.trx --results-directory TestReports")
		};

		var coverletSettings = new CoverletSettings {
			CollectCoverage = true,
			CoverletOutputFormat = CoverletOutputFormat.opencover,
			CoverletOutputDirectory = Directory(@".\TestReports\"),
			CoverletOutputName = $"coverage",
			ExcludeByAttribute = new List<string> { "*.ExcludeFromCodeCoverage*" },
			Exclude = new List<string> {$"[{solution}.Domain*]*Commands*",
										$"[{solution}.Domain*]*Filters*",
										$"[{solution}.Domain*]*Queries*",
										$"[{solution}.Domain*]*Exceptions*"}

		};

    	DotNetTest($"./tests/{solution}.Domain.Testes/{solution}.Domain.Testes.csproj", testSettings, coverletSettings);
    });

Task("TestReport")
    .IsDependentOn("BuildSolution")
    .IsDependentOn("TestRun")
    .Does(() =>
    {
		if (Jenkins.Environment.Build.BuildNumber != 0)
        {
            StartProcess("./tools/OpenCoverToCoberturaConverter.0.3.4/tools/OpenCoverToCoberturaConverter.exe", new ProcessSettings {
				Arguments = new ProcessArgumentBuilder()
					.Append("-input:TestReports/coverage.opencover.xml")
					.Append("-output:TestReports/cobertura.xml")
					.Append("-sources:" + Jenkins.Environment.Build.Workspace)
				}
			);
        }
		else
		{
            var reportGeneratorSettings = new ReportGeneratorSettings()
            {
                HistoryDirectory = new DirectoryPath("./TestReports/ReportsHistory")
            };

            ReportGenerator(new FilePath("./TestReports/coverage.opencover.xml"), new DirectoryPath("./TestReports/ReportGeneratorOutput"), reportGeneratorSettings);
			var reportFilePath = ".\\TestReports\\ReportGeneratorOutput\\index.htm";

            StartProcess("explorer", reportFilePath);
		}
    });

Task("Sonar")
  .IsDependentOn("SonarBegin")
  .IsDependentOn("BuildSolution")
  .IsDependentOn("TestRun")
  .IsDependentOn("SonarEnd");

Task("SonarBegin")
    .Does(() =>
    {
        var diretorioDoProjeto = MakeAbsolute(Directory("./"));

        var argumentos = new ProcessArgumentBuilder();
        argumentos.Append("begin ");
        argumentos.Append("/d:sonar.verbose=false ");
        argumentos.Append("/k:\"" + applicationName + "\" ");
        argumentos.Append("/n:\"" + applicationName + "\" ");
        argumentos.Append("/v:\"" + appVersion + "\" ");
        argumentos.Append("/d:sonar.host.url=\"" + sonarHost + "\" ");
        argumentos.Append("/d:sonar.login=\"" + sonarToken + "\" ");
        argumentos.Append("/d:sonar.cs.opencover.reportsPaths=\"" + diretorioDoProjeto +"\\TestReports\\coverage.opencover.xml\" ");
        argumentos.Append("/d:sonar.cs.vstest.reportsPaths=\"" + diretorioDoProjeto + "\\TestReports\\result.trx\" ");

        if(idPullRequest != "" && (branchDestination == "desenvolvimento" || branchDestination.Contains("release/")))
        {
            argumentos.Append("/d:sonar.pullrequest.key=\"" + idPullRequest + "\" ");
            argumentos.Append("/d:sonar.pullrequest.branch=\"" + branchName + "\" ");
            argumentos.Append("/d:sonar.pullrequest.base=\"" + branchDestination + "\" ");
        }

        if(branchName.Contains("release/"))
        {
            argumentos.Append("/d:sonar.branch.name=\"" + branchName + "\" ");
        }

        DotNetTool(".", "sonarscanner", argumentos);
    });

Task("SonarEnd")
    .Does(() =>
    {
        DotNetTool(".", "sonarscanner", "end /d:sonar.login=\"" + sonarToken + "\"");
    });

Task("Default")
    .Does(() =>
    {
		Information("Tem alguma ideia do que poderiamos colocar aqui?");
        Information(branchName);
    });

RunTarget(target);