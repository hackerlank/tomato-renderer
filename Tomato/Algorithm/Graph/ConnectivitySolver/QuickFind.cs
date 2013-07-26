using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph.Connectivity
{
	public class QuickFind16 : IConnectivitySolver<ushort>
	{
		private ushort[] m_connected;

		public QuickFind16( ushort size )
		{
			m_connected = new ushort[ size ];

			for( ushort i = 0 ; i < size ; ++i )
			{
				m_connected[ i ] = i;
			}
		}

		public bool Connect( ushort p, ushort q )
		{
			ushort t = m_connected[ p ];
			if( t == m_connected[ q ] )
			{
				return true;
			}

			for( ushort i = 0 ; i < m_connected.Length ; ++i )
			{
				if( m_connected[ i ] == t )
				{
					m_connected[ i ] = m_connected[ q ];
				}
			}

			return false;
		}

		public bool IsConnected( ushort p, ushort q )
		{
			return ( m_connected[ p ] == m_connected[ q ] );
		}
	}
}
