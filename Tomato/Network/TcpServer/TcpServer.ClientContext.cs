using System;
using System.Net.Sockets;

namespace Tomato.Network
{
	
	partial class TcpServer
	{
		/// <summary>
		/// Defines a ClientContext class.
		/// </summary>
		private sealed class ClientContext : Tomato.Concurrency.IPoolable
		{
			private Action<long, byte[]> m_packetCompletedCallback = null;

			public long ID { get; private set; }
			
			public Socket Socket { get; set; }
			
			public Packetizer Packetizer { get; private set; }

			public ClientContext( long clientID, int packetizerBufferSize, Action<long, byte[]> packetCompletedCallback )
			{
				ID = clientID;

				Packetizer = new Packetizer( packetizerBufferSize );
				Packetizer.PacketCompleted += new Action<byte[]>( OnPacketCompleted );

				m_packetCompletedCallback = packetCompletedCallback;

				Socket = null;
			}

			public void OnDrawing()
			{
				Packetizer.Reset();
			}

			public void OnReturning()
			{
				Socket = null;
			}

			private void OnPacketCompleted( byte[] bytes )
			{
				m_packetCompletedCallback( ID, bytes );
			}
		}

		private ClientContext ClientContextFactoryFunction()
		{
			return new ClientContext(
				System.Threading.Interlocked.Increment( ref m_nextClientContextID ),
				ReceivingBufferSize * 4,
				new Action<long, byte[]>( OnPacketCompleted ) );
		}

		private ClientContext AddClient( Socket clientSocket )
		{
			ClientContext clientContext = m_clientContextPool.Draw();
			clientContext.Socket = clientSocket;

			m_clients[ clientContext.ID ] = clientContext;

			return clientContext;
		}
	}
}