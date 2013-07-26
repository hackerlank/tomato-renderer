using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph.Connectivity
{
	public interface IConnectivitySolver<T>
	{
		/// <summary>
		/// Connect p and q.
		/// </summary>
		/// <returns>
		/// If true, they are already connected.
		/// If false, they are newly connected.
		/// </returns>
		bool Connect( T p, T q );

		/// <summary>
		/// Check whether they are conencted
		/// </summary>
		bool IsConnected( T p, T q );
	}
}
