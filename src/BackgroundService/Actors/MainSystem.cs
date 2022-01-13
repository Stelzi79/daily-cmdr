using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;

using DailyCmdrLib;
using DailyCmdrLib.Messages;

namespace BackgroundService.Actors;

internal class MainSystem : MyActorBase, ISystemMessageActor
{
	private static readonly List<Type> _SystemActorTypes = new();
	private static readonly List<IActorRef> _SystemActors = new();

	public static void AddSystemActor<T>()
		where T : ISystemMessageActor => _SystemActorTypes.Add(typeof(T));

	private void ForwardSystemMessages(SystemMsg message)
	{
		foreach (var systemActor in _SystemActors)
		{
			systemActor.Forward(message);
		}
	}

	public void OnReceive(SystemPreStartup message)
	{
		Logger.Debug(message.ToString());
		ForwardSystemMessages(message);
	}

	public void OnReceive(SystemStartup message)
	{
		Logger.Debug(message.ToString());
		ForwardSystemMessages(message);
	}

	public void OnReceive(SystemEnd message)
	{
		Logger.Debug(message.ToString());
		ForwardSystemMessages(message);
		_ = Self.GracefulStop(new TimeSpan(0, 0, 10));
	}

	public override void AroundPostStop() => Logger.Debug("AroundPostStop()");

	protected override void PostStop() => Logger.Debug("PostStop()");

	protected override void PreStart()
	{
		Console.WriteLine("PreStart()");

		// Add SystemActors
		foreach (Type actor in _SystemActorTypes)
		{
			var prop = Akka.Actor.Props.Create(actor);
			_SystemActors.Add(Context.ActorOf(prop, actor.Name));
		}

		Self.Tell(new SystemPreStartup());
		Self.Tell(new SystemStartup());
	}
}
