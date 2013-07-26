using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph
{
	public class SimpleGraphWithAdjacencyMatrix : Graph
	{
		private bool m_bDirected;
		private bool[ , ] m_adjacency;

		private uint m_vertexCount;
		private uint m_edgeCount;

		public override uint VertexCount { get { return m_vertexCount; }  }
		public override uint EdgeCount { get { return m_edgeCount; } }

		public override bool IsDirected
		{
			get { return m_bDirected; }
		}

		public SimpleGraphWithAdjacencyMatrix( uint vertexCount, bool bDirected )
		{
			m_bDirected = bDirected;
			m_adjacency = new bool[ vertexCount, vertexCount ];
			m_adjacency.Initialize();

			m_vertexCount = vertexCount;
			m_edgeCount = 0;
		}

		public override void AddEdge( uint source, uint destination )
		{
			if( !m_adjacency[ source, destination ] )
			{
				m_edgeCount++;

				m_adjacency[ source, destination ] = true;
				if( !m_bDirected )
				{
					m_adjacency[ destination, source ] = true;
				}
			}
		}

		public override void RemoveEdge( uint source, uint destination )
		{
			if( m_adjacency[ source, destination ] )
			{
				m_edgeCount--;

				m_adjacency[ source, destination ] = false;
				if( !m_bDirected )
				{
					m_adjacency[ destination, source ] = false;
				}
			}
		}

		public override List<uint>.Enumerator GetAdjacentVertices( uint v )
		{
			List<uint> adjacentVertices = new List<uint>();

			for( uint i = 0 ; i < m_vertexCount ; ++i )
			{
				if( m_adjacency[ v, i ] )
				{
					adjacentVertices.Add( i );
				}
			}

			return adjacentVertices.GetEnumerator();
		}

		public override uint GetVertexDegree( uint v )
		{
			uint degree = 0;

			for( uint i = 0 ; i < m_vertexCount ; ++i )
			{
				if( m_adjacency[ v, i ] )
				{
					degree++;
				}
			}

			return degree;
		}

		public override bool IsAdjacent( uint v, uint w )
		{
			return m_adjacency[ v, w ];
		}

		public override object Clone()
		{
			SimpleGraphWithAdjacencyMatrix copy = new SimpleGraphWithAdjacencyMatrix( m_vertexCount, m_bDirected );

			for( uint i = 0 ; i < m_vertexCount ; ++i )
			{
				for( uint j = 0 ; j < m_vertexCount ; ++j )
				{
					copy.m_adjacency[ i, j ] = m_adjacency[ i, j ];
					
					if( m_adjacency[ i, j ] )
					{
						copy.m_edgeCount++;
					}
				}
			}

			if( m_bDirected )
			{
				copy.m_edgeCount /= 2;
			}

			return copy;
		}
	}


}
