using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	[Editor( typeof( UITypeEditor ), typeof( UITypeEditor ) )]
	internal class ConsoleGridBoolArray : List<bool>
	{
		public ConsoleGridBoolArray()
			: base()
		{
		}

		public ConsoleGridBoolArray( IEnumerable<bool> collection )
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

		public static ConsoleGridBoolArray FromString( string value )
		{
			string[] tokens = value.Split( ' ', ',', '(', ')' );
			ConsoleGridBoolArray list = new ConsoleGridBoolArray();

			for( int i = 0 ; i < tokens.Length ; ++i )
			{
				if( tokens[ i ] != string.Empty )
				{
					try
					{
						list.Add( Convert.ToBoolean( tokens[ i ] ) );
					}
					catch( Exception ) { }
				}
			}

			return list;
		}
	}
}
