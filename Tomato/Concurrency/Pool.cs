using System;
using System.Collections.Generic;

namespace Tomato.Concurrency
{
	/// <summary>
	/// Defines the Pool class.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class Pool<T> where T : IPoolable
	{
		private Queue<T> m_queue = null;

		private Func<T> m_factoryFunction = null;

		/// <summary>
		/// Creates a Pool instance with the factory function.
		/// The factory functino is called whenever creating a new instance.
		/// </summary>
		/// <param name="factoryFunction"></param>
		public Pool( Func<T> factoryFunction )
		{
			m_queue = new Queue<T>();

			m_factoryFunction = factoryFunction;
		}

		/// <summary>
		/// Creates a Pool instance with the factory function.
		/// The factory functino is called whenever creating a new instance.
		/// </summary>
		/// <param name="factoryFunction"></param>
		public Pool( int initialCount, Func<T> factoryFunction )
		{
			m_queue = new Queue<T>();

			m_factoryFunction = factoryFunction;

			for( int i = 0 ; i < initialCount ; ++i )
			{
				Return( m_factoryFunction() );
			}
		}

		/// <summary>
		/// Gets an object from the pool.
		/// </summary>
		/// <returns></returns>
		public T Draw()
		{
			T item;
			if( !m_queue.TryDequeue( out item ) )
			{
				item = m_factoryFunction();
			}

			item.OnDrawing();

			return item;
		}

		/// <summary>
		/// Returns an used object to the pool.
		/// </summary>
		/// <param name="item"></param>
		public void Return( T item )
		{
			item.OnReturning();

			m_queue.Enqueue( item );
		}
	}
}
