using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph.Connectivity
{
	public class WeightedQuickUnionWithHalving : IConnectivitySolver<uint>
	{
		private uint[] m_connected;
		private uint[] m_length;

		public WeightedQuickUnionWithHalving( uint size )
		{
			m_connected = new uint[ size ];
			m_length = new uint[ size ];

			for( uint i = 0 ; i < size ; ++i )
			{
				m_connected[ i ] = i;
				m_length[ i ] = 1;
			}
		}

		public bool Connect( uint p, uint q )
		{
			uint i, j;

			for( i = p ; i != m_connected[ i ] ; i = m_connected[ i ] )
			{
				m_connected[ i ] = m_connected[ m_connected[ i ] ];
			}

			for( j = q ; j != m_connected[ j ] ; j = m_connected[ j ] )
			{
				m_connected[ j ] = m_connected[ m_connected[ j ] ];
			}

			if( i == j )
			{
				return true;
			}

			if( m_length[ i ] < m_length[ j ] )
			{
				m_connected[ i ] = j;
				m_length[ i ] += m_length[ i ];
			}
			else
			{
				m_connected[ j ] = i;
				m_length[ i ] += m_length[ j ];
			}

			return false;
		}

		public bool IsConnected( uint p, uint q )
		{
			uint i, j;

			for( i = p ; i != m_connected[ i ] ; i = m_connected[ i ] )
			{
				m_connected[ i ] = m_connected[ m_connected[ i ] ];
			}

			for( j = q ; j != m_connected[ j ] ; j = m_connected[ j ] )
			{
				m_connected[ j ] = m_connected[ m_connected[ j ] ];
			}

			return ( i == j );
		}
	}

	public class WeightedQuickUnionWithHalving16 : IConnectivitySolver<ushort>
	{
		private ushort[] m_connected;
		private ushort[] m_length;

		public WeightedQuickUnionWithHalving16( ushort size )
		{
			m_connected = new ushort[ size ];
			m_length = new ushort[ size ];

			for( ushort i = 0 ; i < size ; ++i )
			{
				m_connected[ i ] = i;
				m_length[ i ] = 1;
			}
		}

		public bool Connect( ushort p, ushort q )
		{
			ushort i, j;

			for( i = p ; i != m_connected[ i ] ; i = m_connected[ i ] )
			{
				m_connected[ i ] = m_connected[ m_connected[ i ] ];
			}

			for( j = q ; j != m_connected[ j ] ; j = m_connected[ j ] )
			{
				m_connected[ j ] = m_connected[ m_connected[ j ] ];
			}

			if( i == j )
			{
				return true;
			}

			if( m_length[ i ] < m_length[ j ] )
			{
				m_connected[ i ] = j;
				m_length[ i ] += m_length[ i ];
			}
			else
			{
				m_connected[ j ] = i;
				m_length[ i ] += m_length[ j ];
			}

			return false;
		}

		public bool IsConnected( ushort p, ushort q )
		{
			ushort i, j;

			for( i = p ; i != m_connected[ i ] ; i = m_connected[ i ] )
			{
				m_connected[ i ] = m_connected[ m_connected[ i ] ];
			}

			for( j = q ; j != m_connected[ j ] ; j = m_connected[ j ] )
			{
				m_connected[ j ] = m_connected[ m_connected[ j ] ];
			}

			return ( i == j );
		}
	}
}
