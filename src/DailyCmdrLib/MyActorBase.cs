using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;

namespace DailyCmdrLib;

public abstract class MyActorBase<T> : UntypedActor
	where T : ActorBase, new()
{
	private readonly ConcurrentDictionary<Type, Lazy<Action<Message>>> _MessageHandlers = new();

	protected ILoggingAdapter Logger => Context.GetLogger();

	protected override void OnReceive(object message)
	{
		Type messageType = message.GetType();
		Lazy<Action<Message>> handler = _MessageHandlers.GetOrAdd(messageType, (_) => new Lazy<Action<Message>>(() =>
		{
			MethodInfo? methode = typeof(T).GetMethod("OnReceive", new Type[] { messageType });
			if (methode != null)
			{
				return new Action<Message>((message) => methode.Invoke(this, new object[] { message }));
			}
			else
			{
#pragma warning disable RCS1079 // Throwing of new NotImplementedException.
				throw new NotImplementedException($"No 'OnReceive({messageType} message)' implemented in Actor '{GetType()}'!");
#pragma warning restore RCS1079 // Throwing of new NotImplementedException.
			}
		}));
		handler.Value.Invoke((Message)message);
	}

	public static IActorRef Props(ActorSystem system, string actorName = "")
	{
		if (string.IsNullOrWhiteSpace(actorName))
		{
			actorName = typeof(T).Name;
		}

		return system.ActorOf<T>(actorName);
	}
}
