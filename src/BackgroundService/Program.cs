using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka;
using Akka.Actor;
using Akka.Configuration;

using BackgroundService.Messages;

namespace BackgroundService
{
	public static class Program
	{
#if DEBUG
		public static string EnvConst => "DEBUG";

		public static string[] EnvExcludes => new string[] { "RELEASE", "INVALID" };
#elif RELEASE
		public static string EnvConst => "RELEASE";

		public static string[] EnvExcludes => new string[] { "DEBUG", "INVALID" };
#else
		public static string EnvConst => "INVALID";

		public static string[] EnvExcludes => new string[] {"RELEASE", "DEBUG" };
#endif

		public static ActorSystem MainSystem { get; private set; }

		private static IActorRef _MainActor;
		private static volatile bool _Shutdown = false;

		public static void Main(string[] args)
		{
			Console.CancelKeyPress += Console_CancelKeyPress;
			var baseDirectory = args.ElementAtOrDefault(1) ?? Environment.CurrentDirectory;

			Console.WriteLine("Starting up ActorSystem ...");

			using (MainSystem = ActorSystem.Create("daily-cmd", GetConfiguration(baseDirectory)))
			{
				_ = Task.Delay(100);

				_MainActor = MainSystemActor.Props(MainSystem);

				_MainActor.Tell(new SystemPreStartup());
				_MainActor.Tell(new SystemStartup());

				while (!_Shutdown)
				{
					//Console.Write(".");
					_ = Task.Delay(100);
				}
			}

			Console.WriteLine("Application Ended!");
		}

		private static void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
		{
			e.Cancel = true;
			_MainActor.Tell(new SystemEnd());
			MainSystem.Terminate();
			_Shutdown = true;
		}

		private static Config GetConfiguration(string baseDirectory)
		{
			var config = Config.Empty;

			// process any DEBUG, RELEASE, etc. hocon before all the others
			IEnumerable<string> envFiles = Directory.EnumerateFiles(baseDirectory, $"*.{EnvConst}.hocon");
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

		private static bool ContainsExcludes(string f)
		{
			foreach (var excl in EnvExcludes)
			{
				if (f.Contains(excl))
				{
					return true;
				}
			}

			return false;
		}
	}
}
