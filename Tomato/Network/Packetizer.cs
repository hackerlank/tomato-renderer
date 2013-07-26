using System;
using System.Collections.Generic;


namespace Tomato.Network
{
	/// <summary>
	/// Defines a packetizer class.
	/// A packet has a 4-bytes length header.
	/// This is not thread-safe.
	/// </summary>
	public sealed class Packetizer
	{
		private Buffer<byte> m_buffer = null;

		/// <summary>
		/// Raised when a packet is finished.
		/// </summary>
		public event Action<byte[]> PacketCompleted = null;

		/// <summary>
		/// Creates a packetizer instance.
		/// </summary>
		/// <param name="initialBufferSize"></param>
		public Packetizer( int initialBufferSize )
		{
			m_buffer = new Buffer<byte>( initialBufferSize );
		}

		/// <summary>
		/// Add bytes to the packetizer.
		/// If more than a packet is finished, PacketCompleted event will be raised for each finished packets.
		/// </summary>
		/// <param name="buffer"></param>
		public void AddBytes( byte[] buffer )
		{
			AddBytes( buffer, 0, buffer.Length );
		}

		/// <summary>
		/// Add bytes to the packetizer.
		/// If more than a packet is finished, PacketCompleted event will be raised for each finished packets.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		public void AddBytes( byte[] buffer, int offset, int length )
		{
			// Write to the internal buffer.
			m_buffer.Write( buffer, offset, length );

			// Peek the packet length.
			int availableReadBufferSize;
			while( ( availableReadBufferSize = m_buffer.GetAvailableReadBufferSize() ) > 4 )
			{
				// Read the packet length.
				byte[] sizeHeader = new byte[ 4 ];
				m_buffer.Read( sizeHeader, 0, 4, false );
				uint packetSize = BitConverter.ToUInt32( sizeHeader, 0 );

				// If the full length of the packet is available,
				if( availableReadBufferSize >= 4 + packetSize )
				{
					// Read the packet payload.
					byte[] packet = new byte[ packetSize + 4 ];
					int bytesRead = m_buffer.Read( packet, 0, ( int )( packetSize + 4 ) );

					if( bytesRead != packetSize + 4 )
					{
						throw new InvalidOperationException( "Packet length is incosistent." );
					}
					else
					{
						// Raise PacketCompleted event.
						if( PacketCompleted != null )
						{
							PacketCompleted( packet );
						}
					}
				}
				else
				{
					break;
				}

			}
		}

		/// <summary>
		/// Resets the internal buffer.
		/// </summary>
		public void Reset()
		{
			m_buffer.Reset();
		}

		/// <summary>
		/// Resets and resizes the internal buffer.
		/// </summary>
		/// <param name="bufferSize"></param>
		public void Reset( int bufferSize )
		{
			m_buffer.Reset( bufferSize );
		}
	}
}
