using dCom.Modbus.ModbusFunctions;

namespace dCom.ViewModel
{


	public interface IStateUpdater
    {
        void UpdateConnectionState(ConnectionState currentConnectionState);

        void HandleCommandInProgress(ModbusFunction currentCommand);

		void LogMessage(string message);
    }
}
