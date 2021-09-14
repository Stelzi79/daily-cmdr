﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;

using BackgroundService.Messages;

using DailyCmdrLib;

namespace BackgroundService;

internal class MainSystemActor : MyActorBase<MainSystemActor>
{
	public void OnReceive(SystemPreStartup message) => Console.WriteLine(message.ToString());

	public void OnReceive(SystemStartup message) => Console.WriteLine(message.ToString());

	public void OnReceive(SystemPreEnd message) => Console.WriteLine(message.ToString());

	public void OnReceive(SystemEnd message) => Console.WriteLine(message.ToString());
}