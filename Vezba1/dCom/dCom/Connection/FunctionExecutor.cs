using dCom.Modbus.ModbusFunctions;
using dCom.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace dCom.Connection
{

	public delegate void UpdatePointDelegate(PointType type, ushort pointAddres, ushort newValue);

	public class FunctionExecutor: IDisposable
	{
		private IConnection connection;
        private IStateUpdater stateUpdater;
        private ModbusFunction currentCommand;
        private bool threadCancellationSignal = true;
        private AutoResetEvent processConnection;
        private Thread connectionProcessorThread;
        private ConnectionState connectionState = ConnectionState.DISCONNECTED;
        private uint numberOfConnectionRetries = 0;

		private string RECEIVED_MESSAGE = "Point of type {0} on address {1:d5} received value: {2}";

		public event UpdatePointDelegate UpdatePointEvent;

        public FunctionExecutor(IStateUpdater stateUpdater)
		{
            this.stateUpdater = stateUpdater;
            connection = new TCPConnection(stateUpdater, this);
            this.processConnection = new AutoResetEvent(false);
            connectionProcessorThread = new Thread(new ThreadStart(ConnectionProcessorThread));
            connectionProcessorThread.Start();
        }

		public void ExecuteCommand(ModbusFunction commandToExecute)
		{
            if (this.currentCommand!= null)
            {
                this.stateUpdater.HandleCommandInProgress(currentCommand);
            }
            else
            {
                this.currentCommand = commandToExecute;
                this.processConnection.Set();           
            }
		}

        public void HandleReceivedBytes(byte[] receivedBytes)
        {
			Dictionary<Tuple<PointType, ushort>, ushort> pointsToupdate = this.currentCommand?.ParseResponse(receivedBytes);
			if (UpdatePointEvent != null)
			{
				foreach (var point in pointsToupdate)
				{
					UpdatePointEvent.Invoke(point.Key.Item1, point.Key.Item2, point.Value);
					stateUpdater.LogMessage(string.Format(RECEIVED_MESSAGE, point.Key.Item1, point.Key.Item2, point.Value));
				}
			}
        }

        private void ConnectionProcessorThread()
        {
            while (this.threadCancellationSignal)
            {
                try
                {
                    if (this.connectionState == ConnectionState.DISCONNECTED)
                    {
                        numberOfConnectionRetries = 0;
                        this.connection.Connect();
                        while (numberOfConnectionRetries < 10)
                        {
                            if (this.connection.CheckState())
                            {
                                this.connectionState = ConnectionState.CONNECTED;
                                this.stateUpdater.UpdateConnectionState(ConnectionState.CONNECTED);
                                numberOfConnectionRetries = 0;
                                break;
                            }
                            else
                            {
                                numberOfConnectionRetries++;
                                if (numberOfConnectionRetries == 10)
                                {
                                    this.connection.Disconnect();
									this.connectionState = ConnectionState.DISCONNECTED;
									this.stateUpdater.UpdateConnectionState(ConnectionState.DISCONNECTED);
								}
                            }                  
                        }
                    }
                    else
                    {
                        processConnection.WaitOne();
                        this.connection.SendBytes(this.currentCommand.PackRequest());
                        byte[] message;
                        byte[] header = this.connection.RecvBytes(7);
                        int payLoadSize = 0;
                        unchecked
                        {
                            payLoadSize = IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(header, 4));
                        }
                        byte[] payload = this.connection.RecvBytes(payLoadSize-1);
                        message = new byte[header.Length + payload.Length];
                        Buffer.BlockCopy(header, 0, message, 0, 7);
                        Buffer.BlockCopy(payload, 0, message, 7, payload.Length);
                        this.HandleReceivedBytes(message);
                        this.currentCommand = null;
                    }
                }
                catch (SocketException se)
                {
                    if (se.ErrorCode != 10054)
                    {
                        throw se;
                    }
                    currentCommand = null;
                    this.connectionState = ConnectionState.DISCONNECTED;
                    this.stateUpdater.UpdateConnectionState(ConnectionState.DISCONNECTED);
				}
				catch(Exception ex)
				{
					stateUpdater.LogMessage(ex.Message);
				}
            }
        }

		public void Dispose()
		{
			connectionProcessorThread.Abort();
        }
	}
}
