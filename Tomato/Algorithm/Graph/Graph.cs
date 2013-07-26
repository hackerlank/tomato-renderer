using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph
{
	public class Graph : ICloneable
	{
		/// <summary>
		/// The number of vertices.
		/// </summary>
		public virtual uint VertexCount { get { return 0; } }
		
		/// <summary>
		/// The number of edges.
		/// </summary>
		public virtual uint EdgeCount { get { return 0; } }

		/// <summary>
		/// Returns whether vertex v and w are adjacent or not.
		/// Internally it invokes IsAdjanct(v, w) funtion.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		public bool this[ uint v, uint w ]
		{
			get { return IsAdjacent( v, w ); }
		}

		public virtual bool IsDirected
		{
			get { return false; }
		}

		public virtual bool AllowSelfLoop
		{
			get { return false; }
		}

		public virtual bool AllowParallelEdge
		{
			get { return false; }
		}

		/// <summary>
		/// Returns whether vertex v and w are adjacent or not.
		/// </summary>
		/// <param name="v"></param>
		/// <param name="w"></param>
		/// <returns></returns>
		public virtual bool IsAdjacent( uint v, uint w )
		{
			return false;
		}

		public virtual void AddEdge( uint source, uint destination )
		{
		}

		public virtual void RemoveEdge( uint source, uint destination )
		{
		}

		public virtual List<uint>.Enumerator GetAdjacentVertices( uint v )
		{
			return ( new List<uint>() ).GetEnumerator();
		}

		public virtual uint GetVertexDegree( uint v )
		{
			return 0;
		}

		public virtual object Clone()
		{
			return null;
		}
	}	
}
