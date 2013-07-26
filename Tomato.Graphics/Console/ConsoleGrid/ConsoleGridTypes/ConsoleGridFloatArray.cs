using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	[Editor( typeof( UITypeEditor ), typeof( UITypeEditor ) )]
	internal class ConsoleGridFloatArray : List<float>
	{
		public ConsoleGridFloatArray()
			: base()
		{
		}

		public ConsoleGridFloatArray( IEnumerable<float> collection )
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

		public static ConsoleGridFloatArray FromString( string value )
		{
			string[] tokens = value.Split( ' ', ',', '(', ')' );
			ConsoleGridFloatArray list = new ConsoleGridFloatArray();

			for( int i = 0 ; i < tokens.Length ; ++i )
			{
				if( tokens[ i ] != string.Empty )
				{
					try
					{
						list.Add( Convert.ToSingle( tokens[ i ] ) );
					}
					catch( Exception ) { }
				}
			}

			return list;
		}
	}
}