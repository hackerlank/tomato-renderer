using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridVector4Converter : ConsoleGridExpandableObjectConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is ConsoleGridVector4 )
			{
				return ConvertVector4ToString( ( ConsoleGridVector4 )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToVector4( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public static string ConvertVector4ToString( ConsoleGridVector4 value )
		{
			return value.ToString();
		}

		public static ConsoleGridVector4 ConvertStringToVector4( string value )
		{
			return ConsoleGridVector4.FromString( value );
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
				ConsoleGridVector4 vector = ( ConsoleGridVector4 )value;
				return vector.GetPropertyDescriptors( attributes, desc.PropertyGrid );
			}

			return null;
		}
	}
}