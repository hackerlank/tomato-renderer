using System;
using System.IO;
using System.Text;

namespace Tomato
{
	public class MessageStream
	{
		private byte[] m_internalBuffer = null;

		protected MemoryStream m_memoryStream = null;

		public long Position
		{
			get { return m_memoryStream.Position; }
		}

		public MessageStream()
			: this( 0 )
		{
		}

		public MessageStream( int initialCapacity )
		{
			m_internalBuffer = new byte[ 8 ];

			m_memoryStream = new MemoryStream( initialCapacity );
		}

		public MessageStream( byte[] buffer )
		{
			m_internalBuffer = new byte[ 8 ];

			m_memoryStream = new MemoryStream( buffer );

			ReadU32();
		}

		public void Reset()
		{
			m_memoryStream.Seek( 0, SeekOrigin.Begin );
		}

		public void WriteBool( bool value )
		{
			m_memoryStream.WriteByte( value ? ( byte )1 : ( byte )0 );
		}

		public void WriteU8( byte value )
		{
			m_memoryStream.WriteByte( value );
		}

		public void WriteU16( ushort value )
		{
			m_memoryStream.WriteByte( ( byte )( value & 0xFF ) );
			m_memoryStream.WriteByte( ( byte )( ( value >> 8 ) & 0xFF ) );
		}

		public void WriteU32( uint value )
		{
			m_memoryStream.WriteByte( ( byte )( value & 0xFF ) );
			m_memoryStream.WriteByte( ( byte )( ( value >> 8 ) & 0xFF ) );
			m_memoryStream.WriteByte( ( byte )( ( value >> 16 ) & 0xFF ) );
			m_memoryStream.WriteByte( ( byte )( ( value >> 24 ) & 0xFF ) );
		}

		public void WriteU64( ulong value )
		{
			m_memoryStream.Write( BitConverter.GetBytes( value ), 0, 8 );
		}

		public void WriteS8( sbyte value )
		{
			m_memoryStream.WriteByte( ( byte )( value ) );
		}

		public void WriteS16( short value )
		{
			m_memoryStream.WriteByte( ( byte )( value & 0xFF ) );
			m_memoryStream.WriteByte( ( byte )( ( value >> 8 ) & 0xFF ) );
		}

		public void WriteS32( int value )
		{
			m_memoryStream.WriteByte( ( byte )( value & 0xFF ) );
			m_memoryStream.WriteByte( ( byte )( ( value >> 8 ) & 0xFF ) );
			m_memoryStream.WriteByte( ( byte )( ( value >> 16 ) & 0xFF ) );
			m_memoryStream.WriteByte( ( byte )( ( value >> 24 ) & 0xFF ) );
		}

		public void WriteS64( long value )
		{
			m_memoryStream.Write( BitConverter.GetBytes( value ), 0, 8 );
		}

		public void WriteF32( float value )
		{
			m_memoryStream.Write( BitConverter.GetBytes( value ), 0, 4 );
		}

		public void WriteF64( double value )
		{
			m_memoryStream.Write( BitConverter.GetBytes( value ), 0, 8 );
		}

		public void WriteString( string value )
		{
			byte[] bytes = Encoding.Unicode.GetBytes( value );
			WriteU16( ( ushort )( bytes.Length ) );
			m_memoryStream.Write( bytes, 0, bytes.Length );
		}

		public void WriteString( string value, Encoding encoding )
		{
			byte[] bytes = encoding.GetBytes( value );
			WriteU16( ( ushort )( bytes.Length ) );
			m_memoryStream.Write( bytes, 0, bytes.Length );
		}

		public bool ReadBool()
		{
			return ( m_memoryStream.ReadByte() != 0 );
		}

		public byte ReadU8()
		{
			return ( byte )( m_memoryStream.ReadByte() );
		}

		public ushort ReadU16()
		{
#if DEBUG
			int length = m_memoryStream.Read( m_internalBuffer, 0, 2 );
			if( length == 2 )
			{
				return BitConverter.ToUInt16( m_internalBuffer, 0 );
			}
			else
			{
				throw new Exception();
			}
#else
			m_memoryStream.Read( m_internalBuffer, 0, 2 );
			return BitConverter.ToUInt16( m_internalBuffer, 0 );
#endif
		}

		public uint ReadU32()
		{
#if DEBUG
			int length = m_memoryStream.Read( m_internalBuffer, 0, 4 );
			if( length == 4 )
			{
				return BitConverter.ToUInt32( m_internalBuffer, 0 );
			}
			else
			{
				throw new Exception();
			}
#else
			m_memoryStream.Read( m_internalBuffer, 0, 4 );
			return BitConverter.ToUInt32( m_internalBuffer, 0 );
#endif
		}

		public ulong ReadU64()
		{
#if DEBUG
			int length = m_memoryStream.Read( m_internalBuffer, 0, 8 );
			if( length == 8 )
			{
				return BitConverter.ToUInt64( m_internalBuffer, 0 );
			}
			else
			{
				throw new Exception();
			}
#else
			m_memoryStream.Read( m_internalBuffer, 0, 8 );
			return BitConverter.ToUInt64( m_internalBuffer, 0 );
#endif
		}

		public sbyte ReadS8()
		{
			return ( sbyte )( m_memoryStream.ReadByte() );
		}

		public short ReadS16()
		{
#if DEBUG
			int length = m_memoryStream.Read( m_internalBuffer, 0, 2 );
			if( length == 2 )
			{
				return BitConverter.ToInt16( m_internalBuffer, 0 );
			}
			else
			{
				throw new Exception();
			}
#else
			m_memoryStream.Read( m_internalBuffer, 0, 2 );
			return BitConverter.ToInt16( m_internalBuffer, 0 );
#endif
		}

		public int ReadS32()
		{
#if DEBUG
			int length = m_memoryStream.Read( m_internalBuffer, 0, 4 );
			if( length == 4 )
			{
				return BitConverter.ToInt32( m_internalBuffer, 0 );
			}
			else
			{
				throw new Exception();
			}
#else
			m_memoryStream.Read( m_internalBuffer, 0, 4 );
			return BitConverter.ToInt32( m_internalBuffer, 0 );
#endif
		}

		public long ReadS64()
		{
#if DEBUG
			int length = m_memoryStream.Read( m_internalBuffer, 0, 8 );
			if( length == 8 )
			{
				return BitConverter.ToInt64( m_internalBuffer, 0 );
			}
			else
			{
				throw new Exception();
			}
#else
			m_memoryStream.Read( m_internalBuffer, 0, 8 );
			return BitConverter.ToInt64( m_internalBuffer, 0 );
#endif
		}

		public float ReadF32()
		{
#if DEBUG
			int length = m_memoryStream.Read( m_internalBuffer, 0, 4 );
			if( length == 4 )
			{
				return BitConverter.ToSingle( m_internalBuffer, 0 );
			}
			else
			{
				throw new Exception();
			}
#else
			m_memoryStream.Read( m_internalBuffer, 0, 4 );
			return BitConverter.ToSingle( m_internalBuffer, 0 );
#endif
		}

		public double ReadF64()
		{
#if DEBUG
			int length = m_memoryStream.Read( m_internalBuffer, 0, 8 );
			if( length == 8 )
			{
				return BitConverter.ToDouble( m_internalBuffer, 0 );
			}
			else
			{
				throw new Exception();
			}
#else
			m_memoryStream.Read( m_internalBuffer, 0, 8 );
			return BitConverter.ToDouble( m_internalBuffer, 0 );
#endif
		}

		public string ReadString()
		{
			ushort length = ReadU16();
			byte[] buffer = new byte[ length ];
			m_memoryStream.Read( buffer, 0, length );
			return Encoding.Unicode.GetString( buffer );
		}

		public string ReadString( Encoding encoding )
		{
			ushort length = ReadU16();
			byte[] buffer = new byte[ length ];
			m_memoryStream.Read( buffer, 0, length );
			return encoding.GetString( buffer );
		}

		public byte[] GetBuffer()
		{
			return m_memoryStream.GetBuffer();
		}
	}
}