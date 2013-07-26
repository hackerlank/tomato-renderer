using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph
{
	public class EulerPath : PathFinder
	{
		private Graph m_graphCopy;
		private Stack<uint> m_stack;

		public EulerPath( Graph graph, uint v )
			: base( graph )
		{
			m_graphCopy = (Graph)( graph.Clone() );
			m_stack = new Stack<uint>();

			if( ThereExistsEulerPath() )
			{
				m_path.AddVertex( v );

				while( Find( v ) == v && m_stack.Count > 0 )
				{
					v = m_stack.Pop();
					m_path.AddVertex( v );
				}
			}
		}

		private bool ThereExistsEulerPath()
		{
			VertexDegree degrees = new VertexDegree( m_graph );

			for( uint i = 0 ; i < m_graph.VertexCount ; ++i )
			{
				if( degrees[ i ] % 2 != 0 )
				{
					return false;
				}
			}

			return true;
		}

		private uint Find( uint v )
		{
			while( true )
			{
				List<uint>.Enumerator adjacentVertices = m_graphCopy.GetAdjacentVertices( v );
				if( adjacentVertices.MoveNext() )
				{
					uint w = adjacentVertices.Current;

					m_stack.Push( v );
					m_graphCopy.RemoveEdge( v, w );

					v = w;
				}
				else
				{
					break;
				}
			}

			return v;
		}
	}
}
