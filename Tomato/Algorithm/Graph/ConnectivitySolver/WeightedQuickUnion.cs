using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph.Connectivity
{
	public class WeightedQuickUnion : IConnectivitySolver<uint>
	{
		private uint[] m_connected;
		private uint[] m_length;

		public WeightedQuickUnion( uint size )
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

			for( i = p ; i != m_connected[ i ] ; i = m_connected[ i ] ) ;
			for( j = q ; j != m_connected[ j ] ; j = m_connected[ j ] ) ;

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

			for( i = p ; i != m_connected[ i ] ; i = m_connected[ i ] ) ;
			for( j = q ; j != m_connected[ j ] ; j = m_connected[ j ] ) ;

			return ( i == j );
		}
	}
}
