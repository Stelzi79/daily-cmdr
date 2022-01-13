using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DailyCmdrLib;
using DailyCmdrLib.Messages;

namespace BackgroundService;

internal class TestSystem : MyActorBase, ISystemMessageActor
{
	public void OnReceive(SystemPreStartup message) => Logger.Debug("TestSystem => " + message.ToString());

	public void OnReceive(SystemStartup message)
	{
		Logger.Debug("TestSystem => " + message.ToString());
	}

	public void OnReceive(SystemEnd message) => Logger.Debug("TestSystem => " + message.ToString());
}
