using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridBoolConverter : StringConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is bool )
			{
				return ConvertBoolToString( ( bool )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToBool( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public override bool GetStandardValuesSupported( ITypeDescriptorContext context )
		{
			return true;
		}

		public override TypeConverter.StandardValuesCollection GetStandardValues( ITypeDescriptorContext context )
		{
			return new StandardValuesCollection( new string[] { "True",  "False" } );
		}

		public static string ConvertBoolToString( bool value )
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

		public static bool ConvertStringToBool( string value )
		{
			try
			{
				return Convert.ToBoolean( value );
			}
			catch( Exception )
			{
				return false;
			}
		}
	}
}