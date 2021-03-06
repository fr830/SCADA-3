﻿namespace dCom.Connection
{
	public interface IConnection
	{
		void Connect();

		void Disconnect();

		byte[] RecvBytes(int numberOfBytes);

		void SendBytes(byte[] bytesToSend);

		bool CheckState();
	}
}