using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridDoubleConverter : StringConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is double )
			{
				return ConvertDoubleToString( ( double )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToDouble( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public static string ConvertDoubleToString( double value )
		{
			try
			{
				return Convert.ToString( value );
			}
			catch( Exception )
			{
				return "0.0";
			}
		}

		public static double ConvertStringToDouble( string value )
		{
			try
			{
				return Convert.ToDouble( value );
			}
			catch( Exception )
			{
				return 0;
			}
		}
	}
}