using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DailyCmdrLib;

namespace BackgroundService.Messages;

public abstract record SystemMessages(DateTime DateTime) : Message(DateTime) { }

public record SystemPreStartup(DateTime DateTime) : SystemMessages(DateTime) { }
public record SystemStartup(DateTime DateTime) : SystemMessages(DateTime) { }
public record SystemPreEnd(DateTime DateTime) : SystemMessages(DateTime) { }
public record SystemEnd(DateTime DateTime) : SystemMessages(DateTime) { }
