using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridIntArrayConverter : ConsoleGridStringConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is ConsoleGridIntArray )
			{
				return ConvertIntArrayToString( ( ConsoleGridIntArray )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToIntArray( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public static string ConvertIntArrayToString( ConsoleGridIntArray value )
		{
			return value.ToString();
		}

		public static ConsoleGridIntArray ConvertStringToIntArray( string value )
		{
			return ConsoleGridIntArray.FromString( value );
		}
	}
}