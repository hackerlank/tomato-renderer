using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	[Editor( typeof( UITypeEditor ), typeof( UITypeEditor ) )]
	internal class ConsoleGridDoubleArray : List<double>
	{
		public ConsoleGridDoubleArray()
			: base()
		{
		}

		public ConsoleGridDoubleArray( IEnumerable<double> collection )
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

		public static ConsoleGridDoubleArray FromString( string value )
		{
			string[] tokens = value.Split( ' ', ',', '(', ')' );
			ConsoleGridDoubleArray list = new ConsoleGridDoubleArray();

			for( int i = 0 ; i < tokens.Length ; ++i )
			{
				if( tokens[ i ] != string.Empty )
				{
					try
					{
						list.Add( Convert.ToDouble( tokens[ i ] ) );
					}
					catch( Exception ) { }
				}
			}

			return list;
		}
	}
}