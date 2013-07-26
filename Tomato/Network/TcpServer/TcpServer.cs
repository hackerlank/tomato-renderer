using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Tomato.Network
{
	/// <summary>
	/// Defines a TCP socket server class.
	/// </summary>
	public sealed partial class TcpServer
	{
		private static long m_nextClientContextID;

		// Default socket closing timeout.
		private int m_closeTimeout = 1000;

		// Internal socket.
		private Socket m_socket = null;

		// Client context pool.
		private Tomato.Concurrency.Pool<ClientContext> m_clientContextPool = null;

		// Client context map.
		private Dictionary<long, ClientContext> m_clients = null;

		// Received packet queue.
		private Tomato.Concurrency.Queue<IPacket> m_packetQueue = null;

		/// <summary>
		/// Gets whether the server is accepting the clients or not.
		/// </summary>
		public bool IsAcceptingClients { get; private set; }

		/// <summary>
		/// Gets the maximum length of the receiving buffer.
		/// </summary>
		public int ReceivingBufferSize { get; private set; }

		/// <summary>
		/// Raised after a client was accepted.
		/// </summary>
		public event Action<long> ClientAccepted = null;

		/// <summary>
		/// Raised after a chunk of data was received from a client.
		/// </summary>
		public event Action<long, byte[], int, int> DataReceived = null;

		/// <summary>
		/// Raised after data was sent to a client.
		/// </summary>
		public event Action<long, int> DataSent = null;

		/// <summary>
		/// Raised after a packet was finished.
		/// </summary>
		public event Action PacketReceived = null;

		static TcpServer()
		{
			m_nextClientContextID = 0;
		}

		/// <summary>
		/// Creates a TCPServer instance.
		/// </summary>
		/// <param name="receivingBufferSize"></param>
		public TcpServer( int receivingBufferSize )
		{
			IsAcceptingClients = false;

			ReceivingBufferSize = receivingBufferSize;

			m_clientContextPool = new Tomato.Concurrency.Pool<ClientContext>( new Func<ClientContext>( ClientContextFactoryFunction ) );
			m_clients = new Dictionary<long, ClientContext>();

			m_packetQueue = new Tomato.Concurrency.Queue<IPacket>();
		}

		/// <summary>
		/// Starts the server.
		/// </summary>
		/// <param name="localEndPoint"></param>
		/// <param name="backlog"></param>
		/// <returns></returns>
		public bool Start( IPEndPoint localEndPoint, int backlog )
		{
			if( m_socket != null )
			{
				throw new InvalidOperationException( "Internal socket object is already in use." );
			}

			if( localEndPoint == null )
			{
				throw new ArgumentNullException( "localEndPoint" );
			}

			// Initliaze the internal socket.
			m_socket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

			// Bind and start listening.
			try
			{
				m_socket.Bind( localEndPoint );
				m_socket.Listen( backlog );
			}
			catch( Exception exception )
			{
				DisposeServerSocket( exception );
				return false;
			}

			// Initiate the first Accept() call.
			try
			{
				m_socket.BeginAccept( new AsyncCallback( OnAccept ), null );
				IsAcceptingClients = true;
			}
			catch( Exception exception )
			{
				DisposeServerSocket( exception );
				return false;
			}

			return true;
		}

		/// <summary>
		/// Sends the data to a client.
		/// </summary>
		/// <param name="clientID"></param>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public bool Send( long clientID, byte[] buffer, int offset, int length )
		{
			if( buffer == null )
			{
				throw new ArgumentNullException( "buffer" );
			}

			ClientContext clientContext = null;
			if( m_clients.TryGetValue( clientID, out clientContext ) )
			{
				Socket clientSocket = clientContext.Socket;
				SendContext sendContext = new SendContext( clientID, clientSocket );

				try
				{
					clientSocket.BeginSend( buffer, offset, length, SocketFlags.None, new AsyncCallback( OnSent ), sendContext );
				}
				catch( ArgumentOutOfRangeException )
				{
					// Invalid arguments.
					throw;
				}
				catch( ObjectDisposedException )
				{
					// The client socket has been closed.
					HandleAbnormalClientDisconnection( clientID );
					return false;
				}
				catch( SocketException socketException )
				{
					if( socketException.ErrorCode != 10054 )
					{
						// The client might be disconnected already.
						Disconnect( clientID );
					}
					else
					{
						// Other types of socket error has occured.
						HandleAbnormalClientDisconnection( clientID );
					}
					
					return false;
				}
				catch( Exception exception )
				{
					DisposeServerSocket( exception );
					return false;
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Sends the data to all accepted clients.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		/// <returns>The number of successful Send() operation.</returns>
		public int SendToAll( byte[] buffer, int offset, int length )
		{
			int count = 0;

			var clientKeys = from k in m_clients.Keys select k;
			foreach( var clientID in clientKeys )
			{
				if( Send( clientID, buffer, offset, length ) )
				{
					count++;
				}
			}

			return count;
		}

		/// <summary>
		/// Disconnects a client.
		/// </summary>
		/// <param name="clientID"></param>
		public void Disconnect( long clientID )
		{
			ClientContext clientContext = null;
			if( m_clients.TryGetValue( clientID, out clientContext ) )
			{
				// Close the client socket.
				Socket clientSocket = clientContext.Socket;
				DisposeClientSocket( clientSocket );

				// Return client context for reuse and remove from the internal client list.
				m_clientContextPool.Return( m_clients[ clientID ] );
				m_clients.Remove( clientID );
			}
		}

		/// <summary>
		/// Disconnects all accepted clients.
		/// </summary>
		public void DisconnectAll()
		{
			List<ClientContext> contextsToDelete = new List<ClientContext>( m_clients.Count );
			foreach( ClientContext clientContext in m_clients.Values )
			{
				// Close the client socket.
				DisposeClientSocket( clientContext.Socket );
				contextsToDelete.Add( clientContext );
			}

			// Clear the list.
			m_clients.Clear();
			foreach( ClientContext clientContext in contextsToDelete )
			{
				// Return client context for reuse.
				m_clientContextPool.Return( clientContext );
			}
		}

		/// <summary>
		/// Closes the server.
		/// </summary>
		public void Close()
		{
			DisconnectAll();
			DisposeServerSocket( null );
		}

		/// <summary>
		/// Gets a newly received packet.
		/// If no packet was received, this returns null.
		/// </summary>
		/// <returns></returns>
		public IPacket GetPacket()
		{
			IPacket packet;
			if( m_packetQueue.TryDequeue( out packet ) )
			{
				return packet;
			}

			return null;
		}

		private void BeginReceive( long clientID, Socket clientSocket )
		{
			ReceiveContext receiveContext = new ReceiveContext( clientID, clientSocket, ReceivingBufferSize );

			try
			{
				clientSocket.BeginReceive(
					receiveContext.Buffer,
					0,
					ReceivingBufferSize,
					SocketFlags.None,
					new AsyncCallback( OnReceive ),
					receiveContext );
			}
			catch( SocketException socketException )
			{
				if( socketException.ErrorCode != 10054 )
				{
					Disconnect( receiveContext.ClientID );
					return;
				}
				else
				{
					HandleAbnormalClientDisconnection( receiveContext.ClientID );
					return;
				}
				
			}
			catch( ObjectDisposedException )
			{
				HandleAbnormalClientDisconnection( receiveContext.ClientID );
				return;
			}
			catch( Exception )
			{
				throw;
			}
		}

		/// <summary>
		/// Closes the server socket.
		/// </summary>
		/// <param name="exception"></param>
		private void DisposeServerSocket( Exception exception )
		{
			if( exception != null )
			{
				System.Diagnostics.Debug.WriteLine( "Error: Disposing server socket. Exception: {0}", exception.Message );
			}

			try
			{
				if( m_socket != null )
				{
					m_socket.Close();
				}
			}
			catch( Exception ) 
			{ 
			}
			finally
			{
				m_socket = null;
			}
		}

		/// <summary>
		/// Closes the client socket.
		/// </summary>
		/// <param name="socket"></param>
		private void DisposeClientSocket( Socket socket )
		{
			try
			{
				socket.Shutdown( SocketShutdown.Both );
			}
			catch( Exception )
			{
			}

			try
			{
				socket.Close( m_closeTimeout );
			}
			catch( Exception )
			{
			}
		}

		private void OnPacketCompleted( long clientID, byte[] packet )
		{
			AddPacket( new ClientPacket( clientID, packet ) );
		}

		private void HandleAbnormalClientDisconnection( long clientID )
		{
			// Disconnect the client.
			Disconnect( clientID );

			// Notify this abnormal client disconnection to the user.
			AddPacket( new AbnormalClientDisconnectionSystemEventPacket( clientID ) );
		}

		private void AddPacket( IPacket packet )
		{
			m_packetQueue.Enqueue( packet );

			// Invoke PacketReceived event.
			if( PacketReceived != null )
			{
				PacketReceived();
			}
		}
	}
}
