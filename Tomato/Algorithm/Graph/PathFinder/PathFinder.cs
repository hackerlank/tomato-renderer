using System;
using System.Collections.Generic;
using System.Text;

namespace Tomato.Algorithm.Graph
{
	public class PathFinder
	{
		protected Graph m_graph;
		protected Path m_path;

		public PathFinder( Graph graph )
		{
			m_graph = graph;
			m_path = new Path();
		}

		public virtual Path GetPath()
		{
			return m_path;
		}
	}
}
