using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph
{
	public class CycleDetector
	{
		private Graph m_graph;
		
		private int[] m_depth;
		private bool m_bForest;

		public Graph Graph
		{
			get { return m_graph; }
		}

		public CycleDetector( Graph graph )
		{
			m_graph = graph;

			m_depth = new int[ graph.VertexCount ];
			for( uint i = 0 ; i < graph.VertexCount ; ++i )
			{
				m_depth[ i ] = -1;
			}

			m_bForest = true;
			for( uint i = 0 ; i < graph.VertexCount ; ++i )
			{
				if( m_depth[ i ] == -1 )
				{
					m_bForest = IsAcyclic( i, 0 );
				}
			}
		}

		public bool IsForest()
		{
			return m_bForest;
		}

		private bool IsAcyclic( uint v, int depth )
		{
			m_depth[ v ] = depth;

			List<uint>.Enumerator adjacentVertices = m_graph.GetAdjacentVertices( v );
			while( adjacentVertices.MoveNext() )
			{
				uint w = adjacentVertices.Current;
				if( m_depth[ w ] < 0 )
				{
					return IsAcyclic( w, depth + 1 );
				}
				else
				{
					break;
				}
			}

			return false;
		}
	}
}
