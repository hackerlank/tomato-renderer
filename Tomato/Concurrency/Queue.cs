using System;
using System.Collections.Generic;

namespace Tomato.Concurrency
{
	/// <summary>
	/// Defines a thread-safe queue collection.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public sealed class Queue<T>
	{
		// Internal queue collection.
		private System.Collections.Generic.Queue<T> m_queue = null;

		/// <summary>
		/// Gets the number of elements.
		/// </summary>
		public int Count
		{
			get { return m_queue.Count; }
		}

		/// <summary>
		/// Creates a Queue instance.
		/// </summary>
		public Queue()
		{
			m_queue = new System.Collections.Generic.Queue<T>();
		}

		/// <summary>
		/// Enqueues an item to the queue.
		/// </summary>
		/// <param name="item"></param>
		public void Enqueue( T item )
		{
			lock( this )
			{
				m_queue.Enqueue( item );
			}
		}

		/// <summary>
		/// Deqeues an item from the queue.
		/// </summary>
		/// <returns></returns>
		public T Dequeue()
		{
			lock( this )
			{
				return m_queue.Dequeue();
			}
		}

		/// <summary>
		/// Dequeus an item from the queue.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool TryDequeue( out T item )
		{
			lock( this )
			{
				if( m_queue.Count > 0 )
				{
					item = m_queue.Dequeue();
					return true;
				}

				item = default(T);
				return false;
			}
		}

		/// <summary>
		/// Clears the queue.
		/// </summary>
		public void Clear()
		{
			lock( this )
			{
				m_queue.Clear();
			}
		}
	}
}
