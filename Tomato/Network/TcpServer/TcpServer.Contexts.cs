using System;
using System.Net.Sockets;

namespace Tomato.Network
{
	partial class TcpServer
	{
		private sealed class ReceiveContext
		{
			public long ClientID { get; private set; }
			public Socket Socket { get; private set; }
			public byte[] Buffer { get; private set; }

			public ReceiveContext( long clientID, Socket socket, int bufferSize )
			{
				ClientID = clientID;
				Socket = socket;
				Buffer = new byte[ bufferSize ];
			}
		}

		private sealed class SendContext
		{
			public long ClientID { get; private set; }
			public Socket Socket { get; private set; }

			public SendContext( long clientID, Socket socket )
			{
				ClientID = clientID;
				Socket = socket;
			}
		}
	}
}