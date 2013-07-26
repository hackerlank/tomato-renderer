using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridVector2Converter : ConsoleGridExpandableObjectConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is ConsoleGridVector2 )
			{
				return ConvertVector2ToString( ( ConsoleGridVector2 )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToVector2( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public static string ConvertVector2ToString( ConsoleGridVector2 value )
		{
			return value.ToString();
		}

		public static ConsoleGridVector2 ConvertStringToVector2( string value )
		{
			return ConsoleGridVector2.FromString( value );
		}

		public override bool GetPropertiesSupported( ITypeDescriptorContext context )
		{
			return true;
		}

		public override PropertyDescriptorCollection GetProperties( ITypeDescriptorContext context, object value, Attribute[] attributes )
		{
			ConsoleGridItemDescriptor desc = context.PropertyDescriptor as ConsoleGridItemDescriptor;
			if( desc != null )
			{
				ConsoleGridVector2 vector = ( ConsoleGridVector2 )value;
				return vector.GetPropertyDescriptors( attributes, desc.PropertyGrid );
			}

			return null;
		}
	}
}