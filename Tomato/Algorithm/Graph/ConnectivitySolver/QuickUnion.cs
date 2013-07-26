using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph.Connectivity
{
	public class QuickUnion : IConnectivitySolver<uint>
	{
		private uint[] m_connected;

		public QuickUnion( uint size )
		{
			m_connected = new uint[ size ];

			for( uint i = 0 ; i < size ; ++i )
			{
				m_connected[ i ] = i;
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

			m_connected[ i ] = j;

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

	public class QuickUnion16 : IConnectivitySolver<ushort>
	{
		private ushort[] m_connected;

		public QuickUnion16( ushort size )
		{
			m_connected = new ushort[ size ];

			for( ushort i = 0 ; i < size ; ++i )
			{
				m_connected[ i ] = i;
			}
		}

		public bool Connect( ushort p, ushort q )
		{
			ushort i, j;

			for( i = p ; i != m_connected[ i ] ; i = m_connected[ i ] ) ;
			for( j = q ; j != m_connected[ j ] ; j = m_connected[ j ] ) ;

			if( i == j )
			{
				return true;
			}

			m_connected[ i ] = j;

			return false;
		}

		public bool IsConnected( ushort p, ushort q )
		{
			ushort i, j;

			for( i = p ; i != m_connected[ i ] ; i = m_connected[ i ] ) ;
			for( j = q ; j != m_connected[ j ] ; j = m_connected[ j ] ) ;

			return ( i == j );
		}
	}
}
