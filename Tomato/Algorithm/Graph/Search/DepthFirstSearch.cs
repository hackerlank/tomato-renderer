using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph
{
	public class DepthFirstSearch
	{
		protected Graph m_graph;
		protected int[] m_depth;
		
		public Graph Graph
		{
			get { return m_graph; }
		}

		public DepthFirstSearch( Graph graph )
		{
			m_graph = graph;

			m_depth = new int[ graph.VertexCount ];
			for( uint i = 0 ; i < graph.VertexCount ; ++i )
			{
				m_depth[ i ] = -1;
			}
		}

		public bool Search( uint v, GraphSearch searchFunction )
		{
			if( searchFunction == null )
			{
				throw new ArgumentNullException( "searchFunction" );
			}

			if( searchFunction( m_graph, v, 0, v ) )
			{
				return SearchInternal( v, 0, searchFunction );
			}

			return false;
		}

		private bool SearchInternal( uint v, int depth, GraphSearch searchFuction )
		{
			m_depth[ v ] = depth;

			List<uint>.Enumerator adjacentVertices = m_graph.GetAdjacentVertices( v );
			while( adjacentVertices.MoveNext() )
			{
				uint w = adjacentVertices.Current;
				if( m_depth[ w ] < 0 )
				{
					if( searchFuction( m_graph, w, depth + 1, v ) )
					{
						SearchInternal( w, depth + 1, searchFuction );
					}
					else
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}
