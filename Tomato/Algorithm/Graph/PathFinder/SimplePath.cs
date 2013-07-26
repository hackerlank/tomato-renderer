using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph
{
	public class SimplePath : PathFinder
	{
		private bool[] m_bVisited;

		public SimplePath( Graph graph, uint v, uint w )
			: base( graph )
		{
			m_bVisited = new bool[ graph.VertexCount ];
			m_bVisited.Initialize();

			if( Find( v, w ) )
			{
				m_path.AddVertex( v );
			}
		}

		private bool Find( uint v, uint w )
		{
			if( v == w )
			{
				return true;
			}

			m_bVisited[ v ] = true;

			for( uint i = 0 ; i < m_graph.VertexCount ; ++i )
			{
				if( m_graph.IsAdjacent( i, v ) && !m_bVisited[ i ] )
				{
					if( Find( i, w ) )
					{
						m_path.AddVertex( i );
						return true;
					}
				}
			}

			return false;
		}
	}
}
