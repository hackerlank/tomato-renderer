using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridFloatConverter : StringConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is float )
			{
				return ConvertFloatToString( ( float )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToFloat( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public static string ConvertFloatToString( float value )
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

		public static float ConvertStringToFloat( string value )
		{
			try
			{
				return Convert.ToSingle( value );
			}
			catch( Exception )
			{
				return 0;
			}
		}
	}
}