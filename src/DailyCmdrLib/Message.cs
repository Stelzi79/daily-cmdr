using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyCmdrLib;

public abstract record Message
{
	public DateTime DateTime { get; init; }

	public Message(DateTime? dateTime = null)
	{
		DateTime = dateTime ?? DateTime.Now;
	}
}
