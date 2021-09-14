using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DailyCmdrLib;

namespace BackgroundService.Messages;

public abstract record SystemMessages() : Message { }

public record SystemPreStartup() : SystemMessages { }
public record SystemStartup() : SystemMessages { }
public record SystemPreEnd() : SystemMessages { }
public record SystemEnd() : SystemMessages { }
