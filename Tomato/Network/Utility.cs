using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Tomato.Network
{
	public static class NetworkHelper
	{
		public static IPEndPoint GetServerEndPoint( int port )
		{
			return new IPEndPoint( IPAddress.Any, port );
		}

		public static IPEndPoint GetLoopbackEndPoint( int port )
		{
			return new IPEndPoint( IPAddress.Loopback, port );
		}

		/// <summary>
		/// Resolves the IP address from the host name in text.
		/// This function resolves only IPv4 addresses.
		/// When failed to resovle, this returns null.
		/// </summary>
		/// <param name="hostName"></param>
		/// <returns></returns>
		public static IPAddress ResolveHostName( string hostName )
		{
			foreach( IPAddress ipAddress in  Dns.GetHostAddresses( hostName ) )
			{
				if( ipAddress.AddressFamily == AddressFamily.InterNetwork )
				{
					return ipAddress;
				}
			}

			return null;
		}
	}
}
