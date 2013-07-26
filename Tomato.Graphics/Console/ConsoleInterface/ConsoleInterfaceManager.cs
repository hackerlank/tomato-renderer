using System;
using System.Collections.Generic;

namespace Tomato.Graphics.Console
{
	/// <summary>
	/// Defines the console interface manager class.
	/// </summary>
	public class ConsoleInterfaceManager
	{
		private Dictionary<string, ConsoleInterface> m_consoleInterfaces = null;

		internal ConsoleInterfaceManager()
		{
			m_consoleInterfaces = new Dictionary<string, ConsoleInterface>();
		}

		/// <summary>
		/// Creates or gets a ConsoleInterface instance.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ConsoleInterface Create( string name )
		{
			ConsoleInterface consoleInterface;
			if( m_consoleInterfaces.TryGetValue( name, out consoleInterface ) )
			{
				return consoleInterface;
			}

			consoleInterface = new ConsoleInterface( name );
			m_consoleInterfaces.Add( name, consoleInterface );

			return consoleInterface;
		}

		/// <summary>
		/// Deletes a ConosleInterface.
		/// </summary>
		/// <param name="consoleInterface"></param>
		public void Delete( ConsoleInterface consoleInterface )
		{
			if( consoleInterface != null )
			{
				consoleInterface.Dispose();

				m_consoleInterfaces.Remove( consoleInterface.Name );
			}
		}

		/// <summary>
		/// Gets a ConsoleInterface instance.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ConsoleInterface Get( string name )
		{
			ConsoleInterface consoleInterface;
			if( m_consoleInterfaces.TryGetValue( name, out consoleInterface ) )
			{
				return consoleInterface;
			}

			return null;
		}
	}
}
