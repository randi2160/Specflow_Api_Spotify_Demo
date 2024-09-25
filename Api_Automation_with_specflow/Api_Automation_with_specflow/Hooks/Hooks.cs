using System.Net.Http;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using TechTalk.SpecFlow;
using SpecFlowProject.utils;
using Api_Automation_with_specflow.utils;
using System.Configuration;

[Binding]
public class Hooks
{
    private static HttpClient _httpClient;

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        // Initialize HttpClient with HttpClientHandler, which is a concrete class
        var handler = new HttpClientHandler();
        _httpClient = new HttpClient(handler);
    }

    [BeforeScenario]
    public void RegisterDependencies(ScenarioContext scenarioContext)
    {
        var configService = new ConfigurationService();
        var tokenManager = new TokenManager(_httpClient, configService);

        // Register the services in the ScenarioContext for dependency injection
        scenarioContext.ScenarioContainer.RegisterInstanceAs(configService);
        scenarioContext.ScenarioContainer.RegisterInstanceAs(tokenManager);
        scenarioContext.ScenarioContainer.RegisterInstanceAs(_httpClient);
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        _httpClient?.Dispose();

        // Generate LivingDoc Report
        GenerateLivingDocReport();
    }

    private static void GenerateLivingDocReport()
    {
        // Get the current directory of the executing assembly
        var assemblyDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        // Traverse to the root of the project (assuming the folder structure has /bin/Debug or /bin/Release)
        var projectRootDirectory = Directory.GetParent(assemblyDirectory).Parent.Parent.FullName;

        // Define the report folder at the project root level
        var reportDirectory = Path.Combine(projectRootDirectory, "report");

        // Ensure the 'report' directory exists
        if (!Directory.Exists(reportDirectory))
        {
            Directory.CreateDirectory(reportDirectory);
        }

        // Paths for the SpecFlow project assembly and the test execution JSON file
        var assemblyPath = Path.Combine(assemblyDirectory, "SpecFlowProject.dll");
        var jsonPath = Path.Combine(assemblyDirectory, "TestExecution.json");

        // Update the command to output the report to the dynamically located 'report' folder
        var command = $"livingdoc test-assembly {assemblyPath} -t {jsonPath} --output {reportDirectory}";

        // Start the process to generate the LivingDoc report
        var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var process = new Process())
        {
            process.StartInfo = processInfo;
            process.OutputDataReceived += (sender, args) => Debug.WriteLine(args.Data);
            process.ErrorDataReceived += (sender, args) => Debug.WriteLine(args.Data);

            process.Start();

            // Begin asynchronously reading the output and error streams
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Wait for the process to exit
            process.WaitForExit();

            // Ensure that both the output and error streams are fully consumed
            process.WaitForExit(); // Wait for the process to complete
        }
    }

    public static string GetSpotifyClientId()
    {
        return ConfigurationManager.AppSettings["SpotifyClientId"];
    }
}
