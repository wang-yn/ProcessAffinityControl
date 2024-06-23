using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace ProcessAffinityControl;

public static class Program
{
    static void Main(string[] args)
    {
        $"Process Affinity Control version: {Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version}"
            .Log();

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        var config = GetConfig(configuration);

        while (true)
        {
            ScanAndManage(config);

            Thread.Sleep(3000);
        }
    }

    // private static HashSet<int> _protectedProcesses = new HashSet<int>();

    private static void ScanAndManage(Config config)
    {
        if (config == null || config.HighPerformanceProcesses == null || config.HighPerformanceProcesses.Count == 0)
        {
            "config is empty".Log();
            return;
        }

        foreach (var process in Process.GetProcesses())
        {
            if (config.HighPerformanceProcesses.Contains(process.ProcessName))
            {
                if (process.ProcessorAffinity == config.Mask)
                    continue;

                try
                {
                    $"[{process.Id}] process: {process.ProcessName} mask: [{process.ProcessorAffinity}]->[{config.Mask}]"
                        .Log();
                    process.ProcessorAffinity = config.Mask;
                }
                catch (Exception ex)
                {
                    $"set process [{process.Id}]{process.ProcessName} error: {ex.Message}".Log();
                }
            }
        }
    }

    private static Config GetConfig(IConfigurationRoot configuration)
    {
        var config = new Config();
        configuration.GetSection("Config").Bind(config);
        return config;
    }
}

public record Config
{
    public CaseInsensitiveHashSet HighPerformanceProcesses { get; set; }
    public int Mask { get; set; }
}

public class CaseInsensitiveHashSet : HashSet<string>
{
    public CaseInsensitiveHashSet() : base(StringComparer.OrdinalIgnoreCase)
    {
    }
}