using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;
using Akka.Logger.Serilog;

using BackgroundService.Messages;

using DailyCmdrLib;

namespace BackgroundService.Actors;

internal class MainSystem : MyActorBase<MainSystem>
{
	public void OnReceive(SystemPreStartup message) => Logger.Debug(message.ToString());

	public void OnReceive(SystemStartup message) => Logger.Debug(message.ToString());

	public void OnReceive(SystemEnd message)
	{
		Logger.Debug(message.ToString());
		_ = Self.GracefulStop(new TimeSpan(0, 0, 10));
	}

	public override void AroundPostStop() => Logger.Debug("AroundPostStop()");
}
