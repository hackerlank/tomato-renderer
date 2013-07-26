using System;

namespace Tomato.Concurrency
{
	/// <summary>
	/// Defines an interface for poolable objects for the Pool class.
	/// </summary>
	public interface IPoolable
	{
		/// <summary>
		/// Invoked whenever drawn from the Pool.
		/// </summary>
		void OnDrawing();
		
		/// <summary>
		/// Invoked whenever returned to the pool.
		/// </summary>
		void OnReturning();
	}
}
