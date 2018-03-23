using dCom.Configuration;
using dCom.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace dCom.Connection
{
    public class TCPConnection : IConnection
    {
        private IStateUpdater stateUpdater;
        private FunctionExecutor commandExecutor;
        private IPEndPoint remoteEP;
        private Socket socket;

        public TCPConnection(IStateUpdater stateUpdater, FunctionExecutor commandExecutor)
        {
            this.stateUpdater = stateUpdater;
            this.commandExecutor = commandExecutor;
            this.remoteEP = CreateRemoteEndpoint();

        }

        public void Connect()
        {
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.socket.Blocking = false;
            try
            {
                socket.Connect(remoteEP);
            }
            catch (SocketException se)
            {
                if (se.ErrorCode != 10035)
                {
                    throw se;
                }
            }
        }

        public void Disconnect()
        {
            if (socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
            }

            socket.Close();
            socket = null;
        }

        public byte[] RecvBytes(int numberOfBytes)
        {
            int numberOfReceivedBytes = 0;
            byte[] retval = new byte[numberOfBytes];
            int numOfReceived;
            while (numberOfReceivedBytes < numberOfBytes)
            {
                numOfReceived = 0;
                if (socket.Poll(1623, SelectMode.SelectRead))
                {
                    numOfReceived = socket.Receive(retval, numberOfReceivedBytes, (int)numberOfBytes - numberOfReceivedBytes, SocketFlags.None);
                    if (numOfReceived > 0)
                    {
                        numberOfReceivedBytes += numOfReceived;
                    }
                }
            }
            return retval;
        }

        public void SendBytes(byte[] bytesToSend)
        {
            int currentlySent = 0;

            while (currentlySent < bytesToSend.Count())
            {
                if (socket.Poll(1623, SelectMode.SelectWrite))
                {
                    currentlySent += socket.Send(bytesToSend, currentlySent,bytesToSend.Length-currentlySent,SocketFlags.None);
                }
            }
        }

        private IPEndPoint CreateRemoteEndpoint()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = null;
            foreach (IPAddress ip in ipHostInfo.AddressList)
                if ("127.0.0.1".Equals(ip.ToString()))
                    ipAddress = ip;
            return new IPEndPoint(ipAddress, ConfigReader.Instance.TcpAddress);
        }

        public bool CheckState()
        {
            return this.socket.Poll(30000, SelectMode.SelectWrite);
        }
    }
}
