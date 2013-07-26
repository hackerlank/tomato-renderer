using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph
{
	public class SimpleGraphWithAdjacencyList : Graph
	{
		private bool m_bDirected;
		private List<uint>[] m_adjacency;

		private uint m_vertexCount;
		private uint m_edgeCount;

		public override uint VertexCount { get { return m_vertexCount; }  }
		public override uint EdgeCount { get { return m_edgeCount; } }

		public override bool IsDirected
		{
			get { return m_bDirected; }
		}

		public SimpleGraphWithAdjacencyList( uint vertexCount, bool bDirected )
		{
			m_bDirected = bDirected;
			m_adjacency = new List<uint>[ vertexCount ];
			for( uint i = 0 ; i < vertexCount ; ++i )
			{
				m_adjacency[ i ] = new List<uint>();
			}

			m_vertexCount = vertexCount;
			m_edgeCount = 0;
		}

		public override void AddEdge( uint source, uint destination )
		{
			if( !m_adjacency[ source ].Contains( destination ) )
			{
				m_edgeCount++;

				m_adjacency[ source ].Add( destination );
				if( !m_bDirected )
				{
					m_adjacency[ destination ].Add( source );
				}
			}
		}

		public override void RemoveEdge( uint source, uint destination )
		{
			if( m_adjacency[ source ].Contains( destination ) )
			{
				m_edgeCount--;

				m_adjacency[ source ].Remove( destination );
				if( !m_bDirected )
				{
					m_adjacency[ destination ].Remove( source );
				}
			}
		}

		public override List<uint>.Enumerator GetAdjacentVertices( uint v )
		{
			return m_adjacency[ v ].GetEnumerator();
		}

		public override uint GetVertexDegree( uint v )
		{
			return (uint)( m_adjacency[ v ].Count );
		}

		public override bool IsAdjacent( uint v, uint w )
		{
			return m_adjacency[ v ].Contains( w );
		}

		public override object Clone()
		{
			SimpleGraphWithAdjacencyList copy = new SimpleGraphWithAdjacencyList( m_vertexCount, m_bDirected );

			for( uint i = 0 ; i < m_vertexCount ; ++i )
			{
				copy.m_adjacency[ i ].AddRange( m_adjacency[ i ] );
			}

			return copy;
		}
	}


}
