using System;
using System.Net;
using System.Net.Sockets;
using System.Security;

namespace Tomato.Network
{
	/// <summary>
	/// Defines a TCP client socket class.
	/// </summary>
	public sealed class TcpClient
	{
		private Socket m_socket = null;

		private int m_bufferSize = 0;

		private Packetizer m_packetizer = null;

		/// <summary>
		/// Raised after successfully connected to the server.
		/// </summary>
		public event Action ServerConnected = null;

		/// <summary>
		/// Raised when failed to connecting to the server.
		/// </summary>
		public event Action<Exception> ServerConnectionFailed = null;

		/// <summary>
		/// Raised after a finished packet was received from the server.
		/// </summary>
		public event Action<byte[]> PacketReceived = null;

		/// <summary>
		/// Raised after a chunk of data was received from the server.
		/// </summary>
		public event Action<byte[], int, int> DataReceived = null;

		/// <summary>
		/// Raised after a pakcet was sent to the server.
		/// </summary>
		public event Action<int> DataSent = null;

		/// <summary>
		/// Raised when the internal socket was closed for any reasons.
		/// </summary>
		public event Action SocketClosed = null;

		/// <summary>
		/// Gets whether the client is now connected to the server or not.
		/// </summary>
		public bool IsConnected { get { return ( m_socket != null ) && m_socket.Connected; } }

		/// <summary>
		/// Creates a TcpClient instance.
		/// </summary>
		/// <param name="bufferSize"></param>
		public TcpClient( int bufferSize )
		{
			m_bufferSize = bufferSize;

			// Create the packetizer with an enough buffer length.
			m_packetizer = new Packetizer( bufferSize * 4 );
			m_packetizer.PacketCompleted += new Action<byte[]>( OnPacketCompleted );

			m_socket = null;
		}

		/// <summary>
		/// Try connecting to the server.
		/// </summary>
		/// <param name="serverEndPoint"></param>
		public void Connect( EndPoint serverEndPoint )
		{
			if( m_socket != null )
			{
				// If the socket is already connected to a server.
				if( m_socket.Connected )
				{
					throw new InvalidOperationException( "Socket is currenlty connected to the server." );
				}
				else
				{
					// Dispose the disconnected internal socket.
					try
					{
						m_socket.Close();
					}
					catch( Exception exception )
					{
						System.Diagnostics.Debug.WriteLine( exception.Message );
					}
					finally
					{
						m_socket = null;
					}
				}
			}

			try
			{
				// Intialize the internal socket.
				m_socket = new Socket( AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp );

				// Begin connecting.
				m_socket.BeginConnect( serverEndPoint, new AsyncCallback( OnConnected ), null );
			}
			catch( Exception exception )
			{
				// An exception has occured.
				// Close the socket.
				Close();
				OnConnectionFailed( exception );
			}
		}

		/// <summary>
		/// Closes the client socket.
		/// </summary>
		public void Close()
		{
			m_packetizer.Reset();

			if( m_socket != null )
			{
				try
				{
					m_socket.Close();
				}
				catch( Exception exception )
				{
					System.Diagnostics.Debug.WriteLine( exception.Message );
				}
				finally
				{
					m_socket = null;

					// Invoke SocketClosed event.
					if( SocketClosed != null )
					{
						SocketClosed();
					}
				}
			}
		}

		/// <summary>
		/// Sends the data to the server.
		/// </summary>
		/// <param name="message"></param>
		public void Send( MessageStream message )
		{
			if( IsConnected )
			{
				byte[] messageBuffer = message.GetBuffer();
				uint messageLength = ( uint )( message.Position );

				// Prepare the intermediate buffer.
				byte[] buffer = new byte[ messageLength + 4 ];
				
				// Write the length header.
				Buffer.BlockCopy( BitConverter.GetBytes( messageLength ), 0, buffer, 0, 4 );
				
				// Write the payload.
				Buffer.BlockCopy( messageBuffer, 0, buffer, 4, ( int )( messageLength ) );

				// Send the finished bytes.
				Send( buffer, 0, buffer.Length );
			}
		}

		/// <summary>
		/// Sends the data to the server.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		public void Send( byte[] buffer, int offset, int length )
		{
			if( IsConnected )
			{
				// Begin sending.
				m_socket.BeginSend( buffer, offset, length, SocketFlags.None, new AsyncCallback( OnSent ), null );
			}
		}

		private void OnConnected( IAsyncResult asyncResult )
		{
			try
			{
				m_socket.EndConnect( asyncResult );
			}
			catch( Exception exception )
			{
				Close();
				OnConnectionFailed( exception );

				return;
			}

			/// Invoke ServerConnected event.
			if( ServerConnected != null )
			{
				ServerConnected();
			}

			// Initiate a receiving call.
			StartReceiving();
		}

		private void OnConnectionFailed( Exception exception )
		{
			// Invoke ServerConectionFailed event.
			if( ServerConnectionFailed != null )
			{
				ServerConnectionFailed( exception );
			}
		}

		private void OnReceived( IAsyncResult asyncResult )
		{
			ReceivedData receivedData = ( ReceivedData )( asyncResult.AsyncState );
			
			int bytesReceived = 0;
			try
			{
				bytesReceived = m_socket.EndReceive( asyncResult );
			}
			catch( Exception )
			{
				Close();
				return;
			}

			if( bytesReceived > 0 )
			{
				// Add bytes to the packetizer.
				m_packetizer.AddBytes( receivedData.Buffer, 0, bytesReceived );

				// Invoke DataReceived event.
				if( DataReceived != null )
				{
					DataReceived( receivedData.Buffer, 0, bytesReceived );
				}
			}

			// Keep receiving!
			StartReceiving();
		}

		private void OnSent( IAsyncResult asyncResult )
		{
			int bytesSent = 0;
			try
			{
				bytesSent = m_socket.EndSend( asyncResult );
			}
			catch( Exception )
			{
				Close();
				return;
			}

			// Invoke DataSent event.
			if( DataSent != null )
			{
				DataSent( bytesSent );
			}
		}

		private void OnPacketCompleted( byte[] packet )
		{
			if( PacketReceived != null )
			{
				PacketReceived( packet );
			}
		}

		private void StartReceiving()
		{
			try
			{
				// Request Receive() to the socket.
				ReceivedData receivedData = new ReceivedData( m_socket, m_bufferSize );
				m_socket.BeginReceive(
					receivedData.Buffer,
					0,
					m_bufferSize,
					SocketFlags.None,
					new AsyncCallback( OnReceived ),
					receivedData );
			}
			catch( Exception )
			{
				Close();
			}
		}

		private class ReceivedData
		{
			public Socket Socket { get; private set; }
			public byte[] Buffer { get; private set; }

			public ReceivedData( Socket socket, int bufferSize )
			{
				Socket = socket;
				Buffer = new byte[ bufferSize ];
			}
		}
	}
}