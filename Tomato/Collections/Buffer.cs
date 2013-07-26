using System;
using System.Collections.Generic;

namespace Tomato
{
	/// <summary>
	/// Defines a memory buffer consisting of elements of type T.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class Buffer<T>
	{
		private int m_bufferSize = 0;
		private T[] m_buffer = null;

		private int m_readPosition = 0;
		private int m_writePosition = 0;

		/// <summary>
		/// Creates a Buffer instance.
		/// </summary>
		/// <param name="bufferSize"></param>
		public Buffer( int bufferSize )
		{
			if( bufferSize <= 1 )
			{
				throw new ArgumentException( "Argument bufferSize must be at least 1." );
			}

			// Allocate the memory.
			m_bufferSize = bufferSize;
			m_buffer = new T[ bufferSize ];

			// Reset the positions.
			Reset();
		}

		/// <summary>
		/// Resets the buffer.
		/// This resets both the reading position and the writing position to zero.
		/// </summary>
		public void Reset()
		{
			m_readPosition = 0;
			m_writePosition = 0;
		}

		/// <summary>
		/// Resizes the internal buffer with the length of bufferSize and resets the buffer.
		/// </summary>
		/// <param name="bufferSize"></param>
		public void Reset( int bufferSize )
		{
			// Reallocate the internal buffer.
			if( bufferSize != m_bufferSize )
			{
				m_bufferSize = bufferSize;
				m_buffer = new T[ bufferSize ];
			}

			// Reset the positions.
			Reset();
		}

		/// <summary>
		/// Gets the length of readable written memory data.
		/// </summary>
		/// <returns></returns>
		public int GetAvailableReadBufferSize()
		{
			if( m_readPosition <= m_writePosition )
			{
				return m_writePosition - m_readPosition;
			}
			else
			{
				return m_bufferSize - m_readPosition + m_writePosition;
			}
		}

		/// <summary>
		/// Gets the length of writable memory space.
		/// </summary>
		/// <returns></returns>
		public int GetAvailableWriteBufferSize()
		{
			if( m_writePosition < m_readPosition )
			{
				return m_readPosition - m_writePosition;
			}
			else
			{
				return m_bufferSize - m_writePosition + m_readPosition;
			}
		}

		/// <summary>
		/// Writes the data.
		/// </summary>
		/// <param name="data"></param>
		public void Write( T[] data )
		{
			Write( data, 0, data.Length, true );
		}

		/// <summary>
		/// Writes the data.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="bUpdatePosition"></param>
		public void Write( T[] data, bool bUpdatePosition )
		{
			Write( data, 0, data.Length, bUpdatePosition );
		}

		/// <summary>
		/// Writes the data.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		public void Write( T[] data, int offset, int length )
		{
			Write( data, offset, length, true );
		}

		/// <summary>
		/// Writes the data.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		/// <param name="bUpdatePosition"></param>
		public void Write( T[] data, int offset, int length, bool bUpdatePosition )
		{
			// Validate parameters.
			ValidateParameters( data, offset, length );

			// Check if writable space is enough.
			int writableSize = GetAvailableWriteBufferSize();
			if( length > writableSize )
			{
				// Resize the buffer to have an enough space.
				int readableSize = GetAvailableReadBufferSize();
				ResizeBuffer( MathHelper.RoundUpToPowerOfTwo( readableSize + length ) );
			}

			// Copy the requested data to the right.
			int rightHandSize = m_bufferSize - m_writePosition;
			Buffer.BlockCopy(
				data, offset,
				m_buffer, m_writePosition,
				System.Math.Min( rightHandSize, length ) );

			// If necessary, copy the data from the left.
			if( length - rightHandSize > 0 )
			{
				Buffer.BlockCopy(
					data, offset + rightHandSize,
					m_buffer, 0,
					length - rightHandSize );
			}

			// Update the writing position.
			if( bUpdatePosition )
			{
				m_writePosition += length;
				if( m_writePosition >= m_bufferSize )
				{
					m_writePosition -= m_bufferSize;
				}
			}
		}

		/// <summary>
		/// Reads the data.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public int Read( T[] data )
		{
			return Read( data, 0, data.Length, true );
		}

		/// <summary>
		/// Reads the data.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="bUpdatePosition"></param>
		/// <returns></returns>
		public int Read( T[] data, bool bUpdatePosition )
		{
			return Read( data, 0, data.Length, bUpdatePosition );
		}

		/// <summary>
		/// Reads the data.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public int Read( T[] data, int offset, int length )
		{
			return Read( data, offset, length, true );
		}

		/// <summary>
		/// Reads the data.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		/// <param name="bUpdatePosition"></param>
		/// <returns></returns>
		public int Read( T[] data, int offset, int length, bool bUpdatePosition )
		{
			// Validate parameters.
			ValidateParameters( data, offset, length );

			// If available read buffer length is less than the requested length,
			// we just read only the availble data.
			length = System.Math.Min( length, GetAvailableReadBufferSize() );
			
			// Read the data to the right.
			int rightHandSize = m_bufferSize - m_readPosition;
			Buffer.BlockCopy(
				m_buffer, m_readPosition,
				data, offset,
				System.Math.Min( rightHandSize, length ) );

			// If necessary, read the data from the left.
			if( length - rightHandSize > 0 )
			{
				Buffer.BlockCopy(
					m_buffer, 0,
					data, offset + rightHandSize,
					length - rightHandSize );
			}

			// Update the reading position.
			if( bUpdatePosition )
			{
				m_readPosition += length;
				if( m_readPosition >= m_bufferSize )
				{
					m_readPosition -= m_bufferSize;
				}
			}

			// Return the length of data read.
			return length;
		}

		/// <summary>
		/// Skips the reading position.
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public int ConsumeReadBuffer( int length )
		{
			if( length > 0 )
			{
				length = System.Math.Min( length, GetAvailableReadBufferSize() );

				m_readPosition += length;
				if( m_readPosition >= m_bufferSize )
				{
					m_readPosition -= m_bufferSize;
				}
			}

			return length;
		}

		private void ValidateParameters( T[] data, int offset, int length )
		{
			if( data == null )
			{
				throw new ArgumentNullException( "data" );
			}

			if( data.Length < offset + length )
			{
				throw new ArgumentOutOfRangeException( "data" );
			}
		}

		private void ResizeBuffer( int size )
		{
			if( size <= 1 )
			{
				throw new ArgumentException( "Argument size must be at least 1." );
			}

			if( size != m_bufferSize )
			{
				// Allocate a new memory space.
				T[] newBuffer = new T[ size ];

				// Read the currently written memory data as much as possible.
				int availableReadBufferSize = GetAvailableReadBufferSize();
				int properSize = System.Math.Min( availableReadBufferSize, size );
				Read( newBuffer, 0, properSize, false );

				// Reset the buffer size and buffer reference.
				m_bufferSize = size;
				m_buffer = newBuffer;

				// Reset the reading position and the writing position.
				m_readPosition = 0;
				m_writePosition = properSize;
			}
		}
	}
}