using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Akka.Configuration;

using Ardalis.GuardClauses;

using Serilog;

namespace BackgroundService;

internal static class DailCmdrHelpers
{
#if DEBUG
	public static string[] EnvExcludes => new string[] { "RELEASE", "INVALID" };
#elif RELEASE
	public static string[] EnvExcludes => new string[] { "DEBUG", "INVALID" };
#else
	public static string EnvConst => "INVALID";

	public static string[] EnvExcludes => new string[] { "RELEASE", "DEBUG" };
#endif

	public static void ConfigureSerilog()
	{
		var logger = new LoggerConfiguration()
									.WriteTo.Console()
									.MinimumLevel.Information()
									.CreateLogger();

		Serilog.Log.Logger = logger;
	}

	public static bool ContainsExcludes(string file)
	{
		foreach (var excl in EnvExcludes)
		{
			if (file.Contains(excl))
			{
				return true;
			}
		}

		return false;
	}

	public static Config GetConfiguration(string baseDirectory)
	{
		baseDirectory = Guard.Against.NullOrWhiteSpace(baseDirectory, nameof(baseDirectory));
		var config = Config.Empty;

		// process any DEBUG, RELEASE, etc. hocon before all the others
		IEnumerable<string> envFiles = Directory.EnumerateFiles(baseDirectory, $"*.{DailCmdr.EnvConst}.hocon");
		foreach (var hFile in envFiles)
		{
			config = config.WithFallback(ConfigurationFactory.ParseString(File.ReadAllText(hFile)));
		}

		// process any default *.hocon files not processed before
		IEnumerable<string> hFiles = Directory.EnumerateFiles(baseDirectory, "*.hocon").Where((f) => !envFiles.Contains(f) && !ContainsExcludes(f));
		foreach (var hFile in hFiles)
		{
			config = config.WithFallback(ConfigurationFactory.ParseString(File.ReadAllText(hFile)));
		}

		config = config.WithFallback(ConfigurationFactory.Default());

		Console.WriteLine("Akka Loglevel: " + config.GetValue("akka.loglevel"));

		return config;
	}
}
