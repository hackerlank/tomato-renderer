using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph
{
	public class Path
	{
		private List<uint> m_path;

		public uint Source
		{
			get
			{
				if( !IsEmpty )
				{
					return m_path[ 0 ];
				}

				throw new InvalidOperationException();
			}
		}

		public uint Destination
		{
			get
			{
				if( !IsEmpty )
				{
					return m_path[ m_path.Count - 1 ];
				}

				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// The number of edges.
		/// </summary>
		public int Length
		{
			get
			{
				if( !IsEmpty )
				{
					return m_path.Count - 1;
				}

				throw new InvalidOperationException();
			}
		}

		public bool IsEmpty
		{
			get
			{
				return ( m_path.Count == 0 );
			}
		}

		public Path()
		{
			m_path = new List<uint>();
		}

		public void AddVertex( uint v )
		{
			m_path.Add( v );
		}

		public List<uint>.Enumerator GetEnumerator()
		{
			return m_path.GetEnumerator();
		}
	}
}
