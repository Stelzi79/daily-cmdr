using DailyCmdrLib.Messages;

namespace DailyCmdrLib;

public interface ISystemMessageActor
{
	void OnReceive(SystemPreStartup message);

	void OnReceive(SystemStartup message);

	void OnReceive(SystemEnd message);
}
