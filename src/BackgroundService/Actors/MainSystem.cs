using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;

using BackgroundService.Messages;

using DailyCmdrLib;

namespace BackgroundService.Actors;

internal class MainSystem : MyActorBase<MainSystem>
{
	public void OnReceive(SystemPreStartup message) => Console.WriteLine(message.ToString());

	public void OnReceive(SystemStartup message) => Console.WriteLine(message.ToString());

	public void OnReceive(SystemEnd message)
	{
		Console.WriteLine(message.ToString());

		_ = Self.GracefulStop(new TimeSpan(0, 0, 10));
	}

#pragma warning disable RCS1132 // Remove redundant overriding member.
	public override void AroundPostStop() => base.AroundPostStop();
#pragma warning restore RCS1132 // Remove redundant overriding member.
}
