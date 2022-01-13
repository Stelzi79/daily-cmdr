using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka;
using Akka.Actor;
using Akka.Configuration;

using Ardalis.GuardClauses;

using BackgroundService;
using BackgroundService.Actors;

using DailyCmdrLib;

using Serilog;

namespace BackgroundService;

public static class Program
{
	public static void Main(string[] args)
	{
		var baseDirectory = args.ElementAtOrDefault(1) ?? Environment.CurrentDirectory;

		Console.WriteLine("Starting up ActorSystem ...");

		DailCmdr.Init(baseDirectory);

		Console.WriteLine("Application Ended!");
	}
}
