using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DailyCmdrLib;
using DailyCmdrLib.Messages;

namespace DailyCmdrLib.Messages;

public abstract record SystemMsg() : Message { }
public record SystemPreStartup() : SystemMsg { }
public record SystemStartup() : SystemMsg { }
public record SystemEnd() : SystemMsg { }
