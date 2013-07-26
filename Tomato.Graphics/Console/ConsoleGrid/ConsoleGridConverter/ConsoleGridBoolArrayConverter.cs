using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridBoolArrayConverter : ConsoleGridStringConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is ConsoleGridBoolArray )
			{
				return ConvertBoolArrayToString( ( ConsoleGridBoolArray )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToBoolArray( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public static string ConvertBoolArrayToString( ConsoleGridBoolArray value )
		{
			return value.ToString();
		}

		public static ConsoleGridBoolArray ConvertStringToBoolArray( string value )
		{
			return ConsoleGridBoolArray.FromString( value );
		}
	}
}