using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;

namespace DailyCmdrLib;

public class MyActorBase<T> : UntypedActor
	where T : ActorBase, new()
{
	private readonly ConcurrentDictionary<Type, Lazy<Action<Message>>> _MessageHandlers = new();

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
				throw new NotImplementedException($"No 'OnReceive({messageType} message)' implemented in Actor '{GetType().ToString()}'!");
			}
		}));

		handler.Value.Invoke((Message)message);

		//if (_MessageHandlers.ContainsKey(messageType) && _MessageHandlers[messageType] != null)
		//{
		//	_MessageHandlers[messageType].Invoke((Message)message);
		//}
		//else
		//{
		//	MethodInfo? methode = typeof(T).GetMethod("OnReceive", new Type[] { messageType });
		//	if (methode != null)
		//	{
		//		_ = methode.Invoke(this, new object[] { message });
		//		_MessageHandlers[messageType] = new Action<Message>((message) => methode.Invoke(this, new object[] { message }));
		//	}
		//	else
		//	{
		//		throw new NotImplementedException($"No 'OnReceive({messageType} message)' implemented!");
		//	}
		//}
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
