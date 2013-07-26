#pragma once

#include <cstdlib>

namespace Tomato
{
	class MemoryBlock 
	{
	public:
		MemoryBlock( s32 initialCapacity )
			: m_buffer( initialCapacity )
		{
		}

		MemoryBlock( const MemoryBlock& copy )
			: m_buffer( copy.m_buffer )
		{
		}

		MemoryBlock( const byte* pBuffer, s32 bufferLength )
			: m_buffer( bufferLength )
		{
			std::memcpy( 
				reinterpret_cast<void*>( &( m_buffer[ 0 ] ) ),
				reinterpret_cast<const void*>( pBuffer ),
				static_cast<std::size_t>( bufferLength ) );
		}

		MemoryBlock( const char* pBuffer, s32 bufferLength )
			: m_buffer( bufferLength )
		{
			std::memcpy( 
				reinterpret_cast<void*>( &( m_buffer[ 0 ] ) ),
				reinterpret_cast<const void*>( pBuffer ),
				static_cast<std::size_t>( bufferLength ) );
		}

		// .dtor: non-virtual means "DO NOT INHERIT".
		~MemoryBlock()
		{
		}

	public:
		MemoryBlock& operator = ( const MemoryBlock& copy )
		{
			m_buffer = copy.m_buffer;
			return *this;
		}

		void Reset()
		{
			std::memset( &m_buffer[ 0 ], 0, m_buffer.size() );
		}

		s32 GetSize() const
		{
			return m_buffer.size();
		}

		void Resize( s32 size )
		{
			m_buffer.resize( size );
		}

		byte& operator [] ( s32 index ) { return m_buffer[ index ]; }
		const byte& operator [] ( s32 index ) const { return m_buffer[ index ]; }

	private:
		std::vector<byte> m_buffer;
	};
}