using System;
using System.Collections.Generic;
using System.Linq;

namespace Tomato.Graphics.Console
{
	public class ConsoleVariableCollection : IEnumerable<ConsoleVariable>
	{
		private Dictionary<string, ConsoleVariable> m_consoleVariables = null;

		/// <summary>
		/// Gets the name of the collection.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Raised after a ConsoleVariable is added.
		/// </summary>
		public event Action<ConsoleVariable> ItemAdded = null;

		/// <summary>
		/// Raised before a ConsoleVariable is removed.
		/// </summary>
		public event Action<string> ItemRemoving = null;
		
		/// <summary>
		/// Raised after a ConsoleVariable is removed.
		/// </summary>
		public event Action<string> ItemRemoved = null;

		/// <summary>
		/// Raised after all ConsoleVariable(s) are cleared.
		/// </summary>
		public event Action ItemsCleared = null;

		public ConsoleVariableCollection()
		{
			Name = "";
			m_consoleVariables = new Dictionary<string, ConsoleVariable>();
		}

		public ConsoleVariableCollection( string name )
		{
			Name = name;
			m_consoleVariables = new Dictionary<string, ConsoleVariable>();
		}

		public ConsoleVariableCollection( IEnumerable<ConsoleVariable> collection )
		{
			Name = "";

			m_consoleVariables = new Dictionary<string, ConsoleVariable>();
			foreach( var consoleVariable in collection )
			{
				m_consoleVariables.Add( consoleVariable.Name, consoleVariable );
			}
		}

		public ConsoleVariableCollection( string name, IEnumerable<ConsoleVariable> collection )
		{
			Name = name;

			m_consoleVariables = new Dictionary<string, ConsoleVariable>();
			foreach( var consoleVariable in collection )
			{
				m_consoleVariables.Add( consoleVariable.Name, consoleVariable );
			}
		}

		public void Add( ConsoleVariable consoleVariable )
		{
			m_consoleVariables[ consoleVariable.Name ] = consoleVariable;

			if( ItemAdded != null )
			{
				ItemAdded( consoleVariable );
			}
		}

		public void AddRange( IEnumerable<ConsoleVariable> collection )
		{
			foreach( var consoleVariable in collection )
			{
				Add( consoleVariable );
			}
		}

		public void Remove( ConsoleVariable consoleVariable )
		{
			Remove( consoleVariable.Name );
		}

		public void Remove( string consoleVariableName )
		{
			if( ItemRemoving != null )
			{
				ItemRemoving( consoleVariableName );
			}

			m_consoleVariables.Remove( consoleVariableName );

			if( ItemRemoved != null )
			{
				ItemRemoved( consoleVariableName );
			}
		}

		public void Clear()
		{
			m_consoleVariables.Clear();

			if( ItemsCleared != null )
			{
				ItemsCleared();
			}
		}

		public IEnumerator<ConsoleVariable> GetEnumerator()
		{
			return m_consoleVariables.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
