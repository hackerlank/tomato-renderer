using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridUIntArrayConverter : ConsoleGridStringConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is ConsoleGridUIntArray )
			{
				return ConvertUIntArrayToString( ( ConsoleGridUIntArray )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToUIntArray( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public static string ConvertUIntArrayToString( ConsoleGridUIntArray value )
		{
			return value.ToString();
		}

		public static ConsoleGridUIntArray ConvertStringToUIntArray( string value )
		{
			return ConsoleGridUIntArray.FromString( value );
		}
	}
}