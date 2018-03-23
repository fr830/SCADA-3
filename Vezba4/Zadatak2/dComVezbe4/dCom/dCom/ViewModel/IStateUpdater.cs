namespace dCom.ViewModel
{
	public interface IStateUpdater
	{
		void UpdateConnectionState(ConnectionState currentConnectionState);

		void LogMessage(string message);
	}
}