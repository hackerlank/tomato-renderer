using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridFloatArrayConverter : ConsoleGridStringConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is ConsoleGridFloatArray )
			{
				return ConvertFloatArrayToString( ( ConsoleGridFloatArray )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToFloatArray( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public static string ConvertFloatArrayToString( ConsoleGridFloatArray value )
		{
			return value.ToString();
		}

		public static ConsoleGridFloatArray ConvertStringToFloatArray( string value )
		{
			return ConsoleGridFloatArray.FromString( value );
		}
	}
}