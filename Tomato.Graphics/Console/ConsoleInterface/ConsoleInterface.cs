using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Threading;
using System.Text;
using Tomato.Network;

namespace Tomato.Graphics.Console
{
	/// <summary>
	/// Defines the console interface class.
	/// </summary>
	public sealed partial class ConsoleInterface : IDisposable
	{
		// A singleton manager.
		private static ConsoleInterfaceManager s_manager = null;

		// Name of the interface.
		private string m_name = null;

		// List of console variables.
		private List<ConsoleVariable> m_consoleVariables = null;

		// Host information
		private string m_hostName;
		private int m_hostPort;

		// Socket for connection to the host.
		private TcpClient m_tcpSocket = null;
		
		private long m_nextUserCommandID = 0;
		private MessageStream m_syncMessageOutputStream = null;
		private AutoResetEvent m_syncMessageEvent = null;

		/// <summary>
		/// Raised after a response for a user command received.
		/// </summary>
		public event Action<long, bool, MessageStream> UserCommandResponseReceived = null;

		/// <summary>
		/// Raised after connected to the host successfully.
		/// </summary>
		public event Action HostConnected = null;

		/// <summary>
		/// Raised when the connecting to the host failed.
		/// </summary>
		public event Action HostConnectionFailed = null;

		/// <summary>
		/// Raised when the connection to the host was disconnected.
		/// </summary>
		public event Action HostDisconnected = null;

		/// <summary>
		/// Raised when a ConsoleVariable was downloaded from the host.
		/// </summary>
		public event Action<ConsoleVariable> ConsoleVariableDownloaded = null;

		/// <summary>
		/// Raised when the value of a ConsoleVariable was changed.
		/// </summary>
		public event Action<ConsoleVariable> ConsoleVariableValueChanged = null;

		/// <summary>
		/// Gets the name of the interface.
		/// </summary>
		public string Name { get { return m_name; } }

		/// <summary>
		/// Gets the host address.
		/// </summary>
		public string HostName { get { return m_hostName; } }

		/// <summary>
		/// Gets the port number of the host.
		/// </summary>
		public int HostPort { get { return m_hostPort; } }

		/// <summary>
		/// Gets whether the interface is connected to the host or not.
		/// </summary>
		public bool IsConnected
		{
			get { return ( m_tcpSocket != null ) && ( m_tcpSocket.IsConnected ); }
		}

		/// <summary>
		/// Gets the singleton manager instance.
		/// </summary>
		public static ConsoleInterfaceManager Manager
		{
			get { return s_manager; }
		}

		static ConsoleInterface()
		{
			s_manager = new ConsoleInterfaceManager();
		}

		/// <summary>
		/// Internal use only.
		/// </summary>
		/// <param name="name"></param>
		internal ConsoleInterface( string name )
		{
			m_name = name;

			m_consoleVariables = new List<ConsoleVariable>();

			// Intialize the client socket for connection to the host.
			m_tcpSocket = new TcpClient( 4096 );
			m_tcpSocket.PacketReceived += new Action<byte[]>( OnReceivePacket );
			m_tcpSocket.ServerConnected += new Action( OnHostConnected );
			m_tcpSocket.SocketClosed += new Action( OnHostDisconnected );
			m_tcpSocket.ServerConnectionFailed += new Action<Exception>( OnHostConnectionFailed );
		}

		/// <summary>
		/// Connects to the host.
		/// </summary>
		/// <param name="hostName"></param>
		/// <param name="hostPort"></param>
		public void ConnectTo( string hostName, int hostPort )
		{
			if( !IsConnected )
			{
				try
				{
					m_hostName = hostName;
					m_hostPort = hostPort;

					IPEndPoint endPoint = new IPEndPoint( Tomato.Network.NetworkHelper.ResolveHostName( hostName ), hostPort );
					m_tcpSocket.Connect( endPoint );
				}
				catch( Exception ) { }
			}
		}

		/// <summary>
		/// Disconnects from the host.
		/// </summary>
		public void Disconnect()
		{
			if( IsConnected )
			{
				m_tcpSocket.Close();
				m_consoleVariables.Clear();
			}
		}

		/// <summary>
		/// Disposes the interface.
		/// </summary>
		public void Dispose()
		{
			// Disconnect from the host.
			Disconnect();

			m_consoleVariables.Clear();

			// Cancel all pending requests.
			if( m_syncMessageEvent != null )
			{
				m_syncMessageOutputStream = null;
				m_syncMessageEvent.Set();
			}

			// Dispose the client socket.
			if( m_tcpSocket != null )
			{
				m_tcpSocket.PacketReceived -= new Action<byte[]>( OnReceivePacket );
				m_tcpSocket.ServerConnected -= new Action( OnHostConnected );
				m_tcpSocket.SocketClosed -= new Action( OnHostDisconnected );
				m_tcpSocket.ServerConnectionFailed -= new Action<Exception>( OnHostConnectionFailed );
				m_tcpSocket.Close();
			}
		}
		
		/// <summary>
		/// Requests downloading all console variables from the host.
		/// </summary>
		public void DownloadAll()
		{
			if( IsConnected )
			{
				MessageStream message = new MessageStream();
				message.WriteU16( ( ushort )0 );
				message.WriteU16( ( ushort )ConsoleMessageType.List );
				message.WriteBool( false );
				m_tcpSocket.Send( message );
			}
		}

		/// <summary>
		/// Requests downloading a console variable from the host.
		/// </summary>
		/// <param name="consoleVariableName"></param>
		public void Download( string consoleVariableName )
		{
			if( IsConnected )
			{
				MessageStream message = new MessageStream();
				message.WriteU16( ( ushort )0 );
				message.WriteU16( ( ushort )ConsoleMessageType.Get );
				message.WriteString( consoleVariableName );
				m_tcpSocket.Send( message );
			}
		}

		/// <summary>
		/// Requests the file-save to the host.
		/// </summary>
		public void RequestSaveToFile()
		{
			if( IsConnected )
			{
				MessageStream message = new MessageStream();
				message.WriteU16( ( ushort )0 );
				message.WriteU16( ( ushort )ConsoleMessageType.SaveToFile );
				m_tcpSocket.Send( message );
			}
		}

		/// <summary>
		/// Requests the file-delete to the host.
		/// </summary>
		public void RequestDeleteSaveFile()
		{
			if( IsConnected )
			{
				MessageStream message = new MessageStream();
				message.WriteU16( ( ushort )0 );
				message.WriteU16( ( ushort )ConsoleMessageType.DeleteSaveFile );
				m_tcpSocket.Send( message );
			}
		}

		/// <summary>
		/// Sets the value of a ConsoleVariable using text value.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="valueType"></param>
		/// <param name="textValue"></param>
		public void SetByText( string name, ConsoleVariableValueType valueType, string textValue )
		{
			Set( ConsoleVariable.FromTextValue( name, valueType, textValue ) );
		}

		/// <summary>
		/// Sets the value of a ConsoleVariable.
		/// </summary>
		/// <param name="consoleVariable"></param>
		public void Set( ConsoleVariable consoleVariable )
		{
			m_consoleVariables.RemoveAll( v => v.Name == consoleVariable.Name );
			m_consoleVariables.Add( consoleVariable );

			// Invoke ConsoleVariableChanged event.
			if( ConsoleVariableValueChanged != null )
			{
				ConsoleVariableValueChanged( consoleVariable );
			}

			Upload( consoleVariable );
		}

		/// <summary>
		/// Uploads a ConsoleVariable.
		/// </summary>
		/// <param name="consoleVariable"></param>
		public void Upload( ConsoleVariable consoleVariable )
		{
			if( IsConnected
				&& ( consoleVariable != null ) )
			{
				MessageStream message = new MessageStream();
				message.WriteU16( ( ushort )0 );
				message.WriteU16( ( ushort )ConsoleMessageType.Set );
				consoleVariable.WriteToStream( message );
				m_tcpSocket.Send( message );
			}
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public object GetAsValue( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null )
			{
				return consoleVariable.Value;
			}

			return null;
		}

		/// <summary>
		/// Sends a user command to the host.
		/// This function waits for the host to send the response.
		/// </summary>
		/// <param name="userCommand"></param>
		/// <param name="timeOut"></param>
		/// <param name="outputStream"></param>
		/// <returns></returns>
		public bool SendUserCommand( string userCommand, float timeOut, out MessageStream outputStream )
		{
			if( IsConnected
				&& ( m_syncMessageEvent == null ) )
			{
				long packetID = GetNextPacketID();

				MessageStream message = new MessageStream();
				message.WriteU16( ( ushort )0 );
				message.WriteU16( ( ushort )ConsoleMessageType.UserCommand );
				message.WriteS64( packetID );
				message.WriteString( userCommand.Trim() );
				m_tcpSocket.Send( message );

				// Wait for the response.
				m_syncMessageEvent = new AutoResetEvent( false );
				if( !m_syncMessageEvent.WaitOne( ( int )( timeOut * 1000.0f ) ) )
				{
					m_syncMessageEvent.Close();
					m_syncMessageEvent = null;
					outputStream = null;
					return false;
				}

				// Got a response.
				m_syncMessageEvent.Close();
				m_syncMessageEvent = null;
				outputStream = m_syncMessageOutputStream;

				return ( outputStream != null );
			}

			outputStream = null;
			return false;
		}

		/// <summary>
		/// Sends a user command asynchronously.
		/// This function does not wait for the response from the host.
		/// </summary>
		/// <param name="userCommand"></param>
		/// <returns></returns>
		public long? SendAsyncUserCommand( string userCommand )
		{
			if( IsConnected )
			{
				long userCommandID = GetNextPacketID();

				MessageStream message = new MessageStream();
				message.WriteU16( ( ushort )0 );
				message.WriteU16( ( ushort )ConsoleMessageType.UserCommand );
				message.WriteS64( userCommandID );
				message.WriteString( userCommand.Trim() );
				m_tcpSocket.Send( message );

				return userCommandID;
			}

			return null;
		}

		private long GetNextPacketID()
		{
			return System.Threading.Interlocked.Increment( ref m_nextUserCommandID );
		}

		private void OnHostConnected()
		{
			System.Diagnostics.Debug.WriteLine( string.Format( "Info: ConsoleInterface [{0}] connected to {1}:{2}.", m_name, m_hostName, m_hostPort ) );

			// Invoke HostConnected event.
			if( HostConnected != null )
			{
				HostConnected();
			}

			// Request downloading all variables.
			DownloadAll();
		}

		private void OnHostConnectionFailed( Exception exception )
		{
			System.Diagnostics.Debug.WriteLine( string.Format( "Info: ConsoleInterface [{0}] failed to connect to {1}:{2}.", m_name, m_hostName, m_hostPort ) );

			OnConnectionClosed();

			// Invoke HostConnectionFailed event.
			if( HostConnectionFailed != null )
			{
				HostConnectionFailed();
			}
		}

		private void OnHostDisconnected()
		{
			System.Diagnostics.Debug.WriteLine( string.Format( "Info: ConsoleInterface [{0}] disconnected from {1}:{2}.", m_name, m_hostName, m_hostPort ) );

			OnConnectionClosed();

			// Invoke HostDisconnected event.
			if( HostDisconnected != null )
			{
				HostDisconnected();
			}
		}

		private void OnConnectionClosed()
		{
			m_consoleVariables.Clear();

			// Canceul pending requests.
			if( m_syncMessageEvent != null )
			{
				m_syncMessageOutputStream = null;
				m_syncMessageEvent.Set();
			}
		}

		private void OnReceivePacket( byte[] packet )
		{
			MessageStream stream = new MessageStream( packet );

			// Read the 2 bytes header and ignore.
			stream.ReadU16();

			// Read the message type.
			ConsoleMessageType messageType = ( ConsoleMessageType )stream.ReadU16();
			switch( messageType )
			{
				case ConsoleMessageType.List:
					{
						bool bTypeInfo = stream.ReadBool();
						if( !bTypeInfo )
						{
							uint valueCount = stream.ReadU32();
							for( uint i = 0 ; i < valueCount ; ++i )
							{
								// Read a ConsoleVariable from the stream.
								HandleGetMessage( stream );
							}
						}
					}
					break;

				case ConsoleMessageType.Get:
					{
						bool bResult = stream.ReadBool();
						if( bResult )
						{
							// Read a ConsoleVariable from the stream.
							HandleGetMessage( stream );
						}
						else
						{
							// Failed.
							// Read the name and ignore.
							string variableName = stream.ReadString();
						}
					}
					break;

				case ConsoleMessageType.Set:
					{
						// Read the name and the result.
						// Then just ignore.
						string valueName = stream.ReadString();
						bool bResult = stream.ReadBool();
					}
					break;

				case ConsoleMessageType.UserCommand:
					{
						// Read the user command id and the result.
						long userCommandID = stream.ReadS64();
						bool bResult = stream.ReadBool();

						// Wake the pending thread if exists.
						if( m_syncMessageEvent != null )
						{
							m_syncMessageOutputStream = stream;
							m_syncMessageEvent.Set();
						}
						else
						{
							// Otherwise, invoke UserCommandResponseReceived event.
							if( UserCommandResponseReceived != null )
							{
								UserCommandResponseReceived( userCommandID, bResult, stream );
							}
						}
					}
					break;
			}
		}

		private void HandleGetMessage( MessageStream stream )
		{
			ConsoleVariable consoleVariable = ConsoleVariable.ReadFromStream( stream );
			if( consoleVariable != null )
			{
				// Add (replace) to the list.
				m_consoleVariables.RemoveAll( v => v.Name == consoleVariable.Name );
				m_consoleVariables.Add( consoleVariable );

				// Invoke ConsoleVariableDownloaded event.
				if( ConsoleVariableDownloaded != null )
				{
					ConsoleVariableDownloaded( consoleVariable );
				}

				// Invoke ConsoleVariableValueChanged event.
				if( ConsoleVariableValueChanged != null )
				{
					ConsoleVariableValueChanged( consoleVariable );
				}
			}
		}

		/// <summary>
		/// Writes the console variable collection to the file.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public bool SaveToFile( string filePath )
		{
			TextWriter textWriter = new StreamWriter( filePath, false, Encoding.Unicode );

			foreach( ConsoleVariable consoleVariable in m_consoleVariables )
			{
				textWriter.WriteLine( string.Format( "{0}|{1}|{2}",
					consoleVariable.Name,
					Enum.GetName( typeof( ConsoleVariableValueType ), consoleVariable.ValueType ).ToLower(),
					consoleVariable.Value ) );
			}

			textWriter.Flush();
			textWriter.Close();

			return true;
		}

		/// <summary>
		/// Reads the collection of console variables from the file.
		/// </summary>
		/// <param name="filePath"></param>
		public void LoadFromFile( string filePath )
		{
			TextReader textReader = new StreamReader( filePath, Encoding.Unicode );

			string line;
			while( ( line = textReader.ReadLine() ) != null )
			{
				string[] tokens = line.Split( '|' );
				if( tokens.Length >= 3 )
				{
					Set( ConsoleVariable.FromTextValue(
						tokens[ 0 ],
						( ConsoleVariableValueType )Enum.Parse( typeof( ConsoleVariableValueType ), tokens[ 1 ] ),
						tokens[ 2 ] ) );
				}
			}

			textReader.Close();
		}
	}
}
