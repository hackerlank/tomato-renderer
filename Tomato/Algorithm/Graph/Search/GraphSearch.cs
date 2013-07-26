using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph
{
	/// <summary>
	/// Method executed on each visiting vertex while searching the graph.
	/// </summary>
	/// <param name="graph">Graph that is searched.</param>
	/// <param name="vertex">Current visiting vertex.</param>
	/// <param name="depth">Depth of search tree. Depth of staring vertex is 0.</param>
	/// <param name="previousVertex">Previously visited vertex. If the current vertex is starting vertex, 'previousVertex' has the same value as 'vertex'.</param>
	/// <returns>If this returns false, searching on the current path is stopped.</returns>
	public delegate bool GraphSearch( Graph graph, uint vertex, int depth, uint previousVertex );

	public enum TraversingEdgeType
	{
		/// <summary>
		/// 
		/// </summary>
		TreeLink,

		/// <summary>
		/// 
		/// </summary>
		ParentLink,

		/// <summary>
		/// 
		/// </summary>
		BackLink,

		/// <summary>
		/// 
		/// </summary>
		DownLink,

		/// <summary>
		/// 
		/// </summary>
		InvalidLink
	}

	public delegate bool GraphTraverse( Graph graph, uint v, uint w );
}
