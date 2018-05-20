using Common;

namespace Common
{
	public interface IStateUpdater
	{
		void UpdateConnectionState(ConnectionState currentConnectionState);

		void LogMessage(string message);
	}
}