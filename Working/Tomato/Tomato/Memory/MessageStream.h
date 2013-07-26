#pragma once

namespace Tomato
{
	template<u32 HeaderLength>
	class MessageStreamBase
	{
	private:
		enum { DefaultPayloadLength = 1024 - HeaderLength };

	public:
		// Write-only
		MessageStreamBase( s32 payloadLength = DefaultPayloadLength )
			: m_buffer( payloadLength + HeaderLength )
			, m_readPosition( 0 )
			, m_writePosition( 0 )
		{
		}

		// Read-only
		MessageStreamBase( const MessageStreamBase& copy )
			: m_buffer( copy.m_buffer )
			, m_readPosition( 0 )
			, m_writePosition( copy.m_writePosition )
		{
		}

		virtual ~MessageStreamBase()
		{
#ifndef _RELEASE
			if( ( m_readPosition != 0 ) 
				&& ( m_readPosition < m_writePosition ) )
			{
				// Some bytes are left unread.
			}
#endif
		}

	public:

		MessageStreamBase& operator = ( const MessageStreamBase& copy )
		{
			m_buffer = copy.m_buffer;
			m_readPosition = 0;
			m_writePosition = copy.m_writePosition;

			return *this;
		}

	public:

		// Buffer = Header + Payload
		u8* GetBuffer() { return &m_buffer[ 0 ]; }
		const u8* GetBuffer() const { return &m_buffer[ 0 ]; }
		u32 GetLength() const
		{
			return HeaderLength + m_writePosition;
		}

		// Header
		u8* GetHeader() { return &m_buffer[ 0 ]; }
		const u8* GetHeader() const { return &m_buffer[ 0 ]; }
		u32 GetHeaderLength() const
		{
			return HeaderLength;
		}

		// Payload
		u8* GetPayload() { return &m_buffer[ HeaderLength ]; }
		const u8* GetPayload() const { return &m_buffer[ HeaderLength ]; }
		u32 GetPayloadLength() const
		{
			return m_writePosition;
		}

		// Reset the reading and writing states of the stream.
		// Argument bClear indicates whether to clear(zero) internal memory or not.
		void Reset( bool bClear = false )
		{
			m_readPosition = 0;
			m_writePosition = 0;
			
			if( bClear )
			{
				m_buffer.Reset();
			}
		}

		void ResetReadPosition( u32 readPosition = 0 ) const
		{
			m_readPosition = readPosition;
		}

		u32 GetCapacity() const
		{
			return m_buffer.GetSize();
		}

		void Resize( u32 payloadLength, bool bReset = true, bool bClear = false )
		{
			m_buffer.Resize( HeaderLength + payloadLength );

			if( bReset )
			{
				Reset( bClear );
			}
		}

		u32 GetReadPosition() const
		{
			return m_readPosition;
		}

		u32 GetWritePosition() const
		{
			return m_writePosition;
		}

	public:

		void Write( const void* pBuffer, u32 size )
		{
			if( size > 0 )
			{
				u32 bufferSize = static_cast<u32>( m_buffer.GetSize() );
				if( HeaderLength + m_writePosition + size > bufferSize )
				{
					do
					{
						bufferSize *= 2;
					} 
					while( HeaderLength + m_writePosition + size > bufferSize );

					m_buffer.Resize( bufferSize );
				}

				::CopyMemory( &( m_buffer[ HeaderLength + m_writePosition ] ), pBuffer, size );
				m_writePosition += size;
			}
		}

		void WriteHeader( const void* pBuffer, u32 size )
		{
			Assert( size <= HeaderLength );

			if( ( size > 0 )
				&& ( size <= HeaderLength ) )
			{
				::CopyMemory( &( m_buffer[ 0 ] ), pBuffer, size );
			}
		}

		void WriteU8( unsigned __int8 value ) { Write<unsigned __int8>( value ); }
		void WriteS8( __int8 value ) { Write<__int8>( value ); }
		void WriteU16( unsigned __int16 value ) { Write<unsigned __int16>( value ); }
		void WriteS16( __int16 value ) { Write<__int16>( value ); }
		void WriteU32( unsigned __int32 value ) { Write<unsigned __int32>( value ); }
		void WriteS32( __int32 value ) { Write<__int32>( value ); }
		void WriteU64( u64 value ) { Write<u64>( value ); }
		void WriteS64( s64 value ) { Write<s64>( value ); }
		void WriteF32( f32 value ) { Write<f32>( value ); }
		void WriteF64( f64 value ) { Write<f64>( value ); }
		void WriteBool( bool value ) { Write<bool>( value ); }

		void WriteHeaderU8( unsigned __int8 value ) { WriteHeader<unsigned __int8>( value ); }
		void WriteHeaderS8( __int8 value ) { WriteHeader<__int8>( value ); }
		void WriteHeaderU16( unsigned __int16 value ) { WriteHeader<unsigned __int16>( value ); }
		void WriteHeaderS16( __int16 value ) { WriteHeader<__int16>( value ); }
		void WriteHeaderU32( unsigned __int32 value ) { WriteHeader<unsigned __int32>( value ); }
		void WriteHeaderS32( __int32 value ) { WriteHeader<__int32>( value ); }
		void WriteHeaderU64( u64 value ) { WriteHeader<u64>( value ); }
		void WriteHeaderS64( s64 value ) { WriteHeader<s64>( value ); }
		void WriteHeaderF32( f32 value ) { WriteHeader<f32>( value ); }
		void WriteHeaderF64( f64 value ) { WriteHeader<f64>( value ); }
		void WriteHeaderBool( bool value ) { WriteHeader<bool>( value ); }

		void Read( void* pBuffer, u32 size ) const
		{
			Assert( m_readPosition + size <= m_writePosition );

			if( size > 0 )
			{
				::CopyMemory( pBuffer, &( m_buffer[ HeaderLength + m_readPosition ] ), size );
				m_readPosition += size;
			}
		}

		void ReadHeader( void* pBuffer, u32 size ) const
		{
			Assert( size <= HeaderLength );

			if( size > 0 )
			{
				::CopyMemory( pBuffer, &( m_buffer[ 0 ] ), size );
			}
		}

		unsigned __int8 ReadU8() const { return Read<unsigned __int8>(); }
		__int8 ReadS8() const { return Read<__int8>(); }
		unsigned __int16 ReadU16() const { return Read<unsigned __int16>(); }
		__int16 ReadS16() const { return Read<__int16>(); }
		unsigned __int32 ReadU32() const { return Read<unsigned __int32>(); }
		__int32 ReadS32() const { return Read<__int32>(); }
		u64 ReadU64() const { return Read<u64>(); }
		s64 ReadS64() const { return Read<s64>(); }
		f32 ReadF32() const { return Read<f32>(); }
		f64 ReadF64() const { return Read<f64>(); }
		bool ReadBool() const { return Read<bool>(); }

		void ReadU8( unsigned __int8& value ) const { return Read<unsigned __int8>( value ); }
		void ReadS8( __int8& value ) const { return Read<__int8>( value ); }
		void ReadU16( unsigned __int16& value ) const { return Read<unsigned __int16>( value ); }
		void ReadS16( __int16& value ) const { return Read<__int16>( value ); }
		void ReadU32( unsigned __int32& value ) const { return Read<unsigned __int32>( value ); }
		void ReadS32( __int32& value ) const { return Read<__int32>( value ); }
		void ReadU64( u64& value ) const { return Read<u64>( value ); }
		void ReadS64( s64& value ) const { return Read<s64>( value ); }
		void ReadF32( f32& value ) const { return Read<f32>( value ); }
		void ReadF64( f64& value ) const { return Read<f64>( value ); }
		void ReadBool( bool& value ) const { return Read<bool>( value ); }

		unsigned __int8 ReadHeaderU8() const { return ReadHeader<unsigned __int8>(); }
		__int8 ReadHeaderS8() const { return ReadHeader<__int8>(); }
		unsigned __int16 ReadHeaderU16() const { return ReadHeader<unsigned __int16>(); }
		__int16 ReadHeaderS16() const { return ReadHeader<__int16>(); }
		unsigned __int32 ReadHeaderU32() const { return ReadHeader<unsigned __int32>(); }
		__int32 ReadHeaderS32() const { return ReadHeader<__int32>(); }
		u64 ReadHeaderU64() const { return ReadHeader<u64>(); }
		s64 ReadHeaderS64() const { return ReadHeader<s64>(); }
		f32 ReadHeaderF32() const { return ReadHeader<f32>(); }
		f64 ReadHeaderF64() const { return ReadHeader<f64>(); }
		bool ReadHeaderBool() const { return ReadHeader<bool>(); }

		void ReadHeaderU8( unsigned __int8& value ) const { return ReadHeader<unsigned __int8>( value ); }
		void ReadHeaderS8( __int8& value ) const { return ReadHeader<__int8>( value ); }
		void ReadHeaderU16( unsigned __int16& value ) const { return ReadHeader<unsigned __int16>( value ); }
		void ReadHeaderS16( __int16& value ) const { return ReadHeader<__int16>( value ); }
		void ReadHeaderU32( unsigned __int32& value ) const { return ReadHeader<unsigned __int32>( value ); }
		void ReadHeaderS32( __int32& value ) const { return ReadHeader<__int32>( value ); }
		void ReadHeaderU64( u64& value ) const { return ReadHeader<u64>( value ); }
		void ReadHeaderS64( s64& value ) const { return ReadHeader<s64>( value ); }
		void ReadHeaderF32( f32& value ) const { return ReadHeader<f32>( value ); }
		void ReadHeaderF64( f64& value ) const { return ReadHeader<f64>( value ); }
		void ReadHeaderBool( bool& value ) const { return ReadHeader<bool>( value ); }

	public:

		void WriteString( const StringW& value )
		{
			s32 bytesCount = value.GetLength() * sizeof( StringW::Character );
			if( bytesCount > 0xFFFF )
			{
				do
				{
					u16 count = static_cast<u16>( Math::Min( bytesCount, 0xFFFF ) );

					WriteU16( count );

					Write( value.GetBuffer(), count );

					bytesCount -= count;
				}
				while( bytesCount > 0 );
			}
			else
			{
				WriteU16( static_cast<u16>( bytesCount ) );

				Write( value.GetBuffer(), bytesCount );
			}
		}

		void WriteString( const StringW& value, Encoding::Type encoding )
		{
			StringA conversion( value, encoding );

			s32 bytesCount = conversion.GetLength() * sizeof( StringA::Character );
			if( bytesCount > 0xFFFF )
			{
				do
				{
					u16 count = static_cast<u16>( Math::Min( bytesCount, 0xFFFF ) );

					WriteU16( count );

					Write( value.GetBuffer(), count );

					bytesCount -= count;
				}
				while( bytesCount > 0 );
			}
			else
			{
				WriteU16( static_cast<u16>( bytesCount ) );

				Write( conversion.GetBuffer(), bytesCount );
			}
		}

		void WriteString( const StringA& value )
		{
			s32 bytesCount = value.GetLength() * sizeof( StringA::Character );
			if( bytesCount > 0xFFFF )
			{
				do
				{
					u16 count = static_cast<u16>( Math::Min( bytesCount, 0xFFFF ) );

					WriteU16( count );

					Write( value.GetBuffer(), count );

					bytesCount -= count;
				}
				while( bytesCount > 0 );
			}
			else
			{
				WriteU16( static_cast<u16>( bytesCount ) );

				Write( value.GetBuffer(), bytesCount );
			}
		}

		StringW ReadString() const
		{
			u16 bytesCount = ReadU16();

			StringW buffer;
			buffer.Resize( bytesCount / sizeof( StringW::Character ) );
			Read( static_cast<void*>( buffer.GetBuffer() ), bytesCount );

			return buffer;
		}

		StringW ReadMultiByteString( Encoding::Type encoding ) const
		{
			u16 bytesCount = ReadU16();

			StringA buffer;
			buffer.Resize( bytesCount / sizeof( StringA::Character ) );
			Read( static_cast<void*>( buffer.GetBuffer() ), bytesCount );

			return StringW( buffer, encoding );
		}

		StringA ReadAnsiString() const
		{
			u16 bytesCount = ReadU16();

			StringA buffer;
			buffer.Resize( bytesCount / sizeof( StringA::Character ) );
			Read( static_cast<void*>( buffer.GetBuffer() ), bytesCount );

			return buffer;
		}

		StringA ReadMultiByteString() const
		{
			u16 bytesCount = ReadU16();

			StringA buffer;
			buffer.Resize( bytesCount / sizeof( StringA::Character ) );
			Read( static_cast<void*>( buffer.GetBuffer() ), bytesCount );

			return buffer;
		}

	public:
		template<typename T>
		MessageStreamBase& operator << ( const T& value )
		{
			Write<T>( value );
			return *this;
		}

		template<typename T>
		MessageStreamBase& operator >> ( T& value )
		{
			Read<T>( value );
			return *this;
		}

	private:
		template<typename T> 
		void Write( const T& value )
		{
			Write( &value, sizeof( T ) );
		}

		template<> 
		void Write( const bool& value )
		{
			Write<u8>( static_cast<u8>( value ? 1 : 0 ) );
		}

		template<typename T> 
		void WriteHeader( const T& value )
		{
			WriteHeader( &value, sizeof( T ) );
		}

		template<> 
		void WriteHeader( const bool& value )
		{
			WriteHeader<u8>( static_cast<u8>( value ? 1 : 0 ) );
		}


		template<typename T> 
		void Read( T& value ) const
		{
			Read( static_cast<void*>( &value ), sizeof( T ) );
		}

		template<typename T> 
		T Read() const
		{
			T value;
			Read( static_cast<void*>( &value ), sizeof( T ) );
			return value;
		}

		template<> 
		void Read( bool& value ) const
		{
			return ( ( Read<u8>() != 0 ) ? true : false );
		}

		template<typename T> 
		void ReadHeader( T& value ) const
		{
			ReadHeader( static_cast<void*>( &value ), sizeof( T ) );
		}

		template<typename T> 
		T ReadHeader() const
		{
			T value;
			ReadHeader( static_cast<void*>( &value ), sizeof( T ) );
			return value;
		}

		template<> 
		void ReadHeader( bool& value ) const
		{
			return ( ( ReadHeader<u8>() != 0 ) ? true : false );
		}

	private:
		MemoryBlock m_buffer;

		mutable u32 m_readPosition;
		u32 m_writePosition;
	};

	typedef MessageStreamBase<2> MessageStream;
}
