using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;
using Akka.Configuration;

using Ardalis.GuardClauses;

using DailyCmdrLib.Messages;

using Serilog;

namespace BackgroundService;

public static class DailCmdr
{
	private static volatile bool _Shutdown = false;

#if DEBUG
	public static string EnvConst => "DEBUG";
#elif RELEASE
	public static string EnvConst => "RELEASE";
#endif

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public static ActorSystem MainSystem { get; private set; }

	private static IActorRef _MainActor;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

	public static void Init(string baseDirectory)
	{
		Console.CancelKeyPress += Console_CancelKeyPress;
		DailCmdrHelpers.ConfigureSerilog();
		using (MainSystem = ActorSystem.Create("daily-cmd", DailCmdrHelpers.GetConfiguration(baseDirectory)))
		{
			_ = Task.Delay(100);

			// Add SystemActors
			Actors.MainSystem.AddSystemActor<TestSystem>();

			_MainActor = Actors.MainSystem.Props<Actors.MainSystem>(DailCmdr.MainSystem);

			while (!_Shutdown)
			{
				_ = Task.Delay(100);
			}
		}
	}

	private static void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
	{
		e.Cancel = true;
		_MainActor.Tell(new SystemEnd());
		_ = MainSystem.Terminate();
		_Shutdown = true;
	}
}
