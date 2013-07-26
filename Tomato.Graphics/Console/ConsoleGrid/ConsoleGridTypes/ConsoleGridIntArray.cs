using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	[Editor( typeof( UITypeEditor ), typeof( UITypeEditor ) )]
	internal class ConsoleGridIntArray : List<int>
	{
		public ConsoleGridIntArray()
			: base()
		{
		}

		public ConsoleGridIntArray( IEnumerable<int> collection )
			: base( collection )
		{
		}

		public override string ToString()
		{
			string conv = "";
			for( int i = 0 ; i < base.Count ; ++i )
			{
				conv += Convert.ToString( base[ i ] );
				if( i < base.Count - 1 ) { conv += ", "; }
			}
			return conv;
		}

		public static ConsoleGridIntArray FromString( string value )
		{
			string[] tokens = value.Split( ' ', ',', '(', ')' );
			ConsoleGridIntArray list = new ConsoleGridIntArray();

			for( int i = 0 ; i < tokens.Length ; ++i )
			{
				if( tokens[ i ] != string.Empty )
				{
					try
					{
						list.Add( Convert.ToInt32( tokens[ i ] ) );
					}
					catch( Exception ) { }
				}
			}

			return list;
		}
	}
}