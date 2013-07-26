using System;
using System.Net.Sockets;

namespace Tomato.Network
{
	partial class TcpServer
	{
		private void OnAccept( IAsyncResult asyncResult )
		{
			Socket clientSocket = null;
			try
			{
				clientSocket = m_socket.EndAccept( asyncResult );
			}
			catch( Exception exception )
			{
				DisposeServerSocket( exception );
				return;
			}

			// Add a client context.
			ClientContext clientContext = AddClient( clientSocket );

			// Invoke ClientAccepted event.
			if( ClientAccepted != null )
			{
				ClientAccepted( clientContext.ID );
			}

			// Initiate the first Receive() call.
			BeginReceive( clientContext.ID, clientContext.Socket );

			// Initiate the next Accept() call.
			try
			{
				m_socket.BeginAccept( new AsyncCallback( OnAccept ), null );
				IsAcceptingClients = true;
			}
			catch( Exception exception )
			{
				IsAcceptingClients = false;
				DisposeServerSocket( exception );
			}
		}

		private void OnReceive( IAsyncResult asyncResult )
		{
			ReceiveContext receiveContext = ( ReceiveContext )( asyncResult.AsyncState );
			Socket clientSocket = receiveContext.Socket;

			int bytesReceived = 0;
			try
			{
				bytesReceived = clientSocket.EndReceive( asyncResult );
			}
			catch( ArgumentException )
			{
				throw;
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
			catch( InvalidOperationException )
			{
				HandleAbnormalClientDisconnection( receiveContext.ClientID );
				return;
			}
			catch( Exception exception )
			{
				DisposeServerSocket( exception );
				return;
			}

			if( bytesReceived > 0 )
			{
				m_clients[ receiveContext.ClientID ].Packetizer.AddBytes( receiveContext.Buffer, 0, bytesReceived );

				// Invoke DataReceived event.
				if( DataReceived != null )
				{
					DataReceived( receiveContext.ClientID, receiveContext.Buffer, 0, bytesReceived );
				}
			}

			// Initiate the next Receive() call.
			BeginReceive( receiveContext.ClientID, clientSocket );
		}

		private void OnSent( IAsyncResult asyncResult )
		{
			SendContext sendContext = ( SendContext )( asyncResult.AsyncState );
			Socket clientSocket = sendContext.Socket;

			int bytesSent = 0;
			try
			{
				bytesSent = clientSocket.EndSend( asyncResult );
			}
			catch( ArgumentException )
			{
				throw;
			}
			catch( SocketException socketException )
			{
				if( socketException.ErrorCode != 10054 )
				{
					Disconnect( sendContext.ClientID );
					return;
				}
				else
				{
					HandleAbnormalClientDisconnection( sendContext.ClientID );
					return;
				}				
			}
			catch( ObjectDisposedException )
			{
				HandleAbnormalClientDisconnection( sendContext.ClientID );
				return;
			}
			catch( Exception exception )
			{
				DisposeServerSocket( exception );
				return;
			}

			// Invoke DataSend event.
			if( DataSent != null )
			{
				DataSent( sendContext.ClientID, bytesSent );
			}
		}
	}
}