using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridVector3Converter : ConsoleGridExpandableObjectConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is ConsoleGridVector3 )
			{
				return ConvertVector3ToString( ( ConsoleGridVector3 )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToVector3( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public static string ConvertVector3ToString( ConsoleGridVector3 value )
		{
			return value.ToString();
		}

		public static ConsoleGridVector3 ConvertStringToVector3( string value )
		{
			return ConsoleGridVector3.FromString( value );
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
				ConsoleGridVector3 vector = ( ConsoleGridVector3 )value;
				return vector.GetPropertyDescriptors( attributes, desc.PropertyGrid );
			}

			return null;
		}
	}
}