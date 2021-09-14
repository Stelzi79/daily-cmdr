using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;

using BackgroundService.Messages;

namespace BackgroundService
{
	public static class Program
	{
		public static ActorSystem? MainSystem { get; private set; }

		public static void Main(string[] args)
		{
			Console.WriteLine("Starting up ActorSystem ...");
			MainSystem = ActorSystem.Create("daily-cmd");
			IActorRef mainActor = MainSystemActor.Props(MainSystem);
			mainActor.Tell(new SystemPreStartup(DateTime.Now));
			mainActor.Tell(new SystemStartup(DateTime.Now));
			mainActor.Tell(new SystemPreEnd(DateTime.Now));
			mainActor.Tell(new SystemEnd(DateTime.Now));

			Console.ReadLine();
		}
	}
}
