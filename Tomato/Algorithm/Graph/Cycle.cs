using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph
{
	public class VertexDegree
	{
		private Graph m_graph;
		private uint[] m_degrees;

		public Graph Graph
		{
			get { return m_graph; }
		}

		public uint this[ uint v ]
		{
			get { return m_degrees[ v ]; }
		} 

		public VertexDegree( Graph graph )
		{
			m_graph = graph;

			m_degrees = new uint[ graph.VertexCount ];
			for( uint i = 0 ; i < graph.VertexCount ; ++i )
			{
				m_degrees[ i ] = graph.GetVertexDegree( i );
			}
		}
	}
}
