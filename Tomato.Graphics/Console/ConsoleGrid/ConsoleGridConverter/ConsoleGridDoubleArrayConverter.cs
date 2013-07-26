using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridDoubleArrayConverter : ConsoleGridStringConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is ConsoleGridDoubleArray )
			{
				return ConvertDoubleArrayToString( ( ConsoleGridDoubleArray )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToDoubleArray( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public static string ConvertDoubleArrayToString( ConsoleGridDoubleArray value )
		{
			return value.ToString();
		}

		public static ConsoleGridDoubleArray ConvertStringToDoubleArray( string value )
		{
			return ConsoleGridDoubleArray.FromString( value );
		}
	}
}