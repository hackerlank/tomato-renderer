using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridIntConverter : StringConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is int )
			{
				return ConvertIntToString( ( int )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToInt( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public static string ConvertIntToString( int value )
		{
			try
			{
				return Convert.ToString( value );
			}
			catch( Exception )
			{
				return "0";
			}
		}

		public static int ConvertStringToInt( string value )
		{
			try
			{
				return Convert.ToInt32( value );
			}
			catch( Exception )
			{
				return 0;
			}
		}
	}
}