using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridUIntConverter : StringConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is uint )
			{
				return ConvertUIntToString( ( uint )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToUInt( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public static string ConvertUIntToString( uint value )
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

		public static uint ConvertStringToUInt( string value )
		{
			try
			{
				return Convert.ToUInt32( value );
			}
			catch( Exception )
			{
				return 0;
			}
		}
	}
}