using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridMatrixConverter : ConsoleGridExpandableObjectConverter
	{
		public override object ConvertTo( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType )
		{
			if( destinationType == typeof( string ) && value is ConsoleGridMatrix )
			{
				return ConvertMatrixToString( ( ConsoleGridMatrix )value );
			}

			return base.ConvertTo( context, culture, value, destinationType );
		}

		public override object ConvertFrom( ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value )
		{
			if( value is string )
			{
				return ConvertStringToMatrix( ( string )value );
			}

			return base.ConvertFrom( context, culture, value );
		}

		public static string ConvertMatrixToString( ConsoleGridMatrix value )
		{
			return value.ToString();
		}

		public static ConsoleGridMatrix ConvertStringToMatrix( string value )
		{
			return ConsoleGridMatrix.FromString( value );
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
				ConsoleGridMatrix matrix = ( ConsoleGridMatrix )value;
				return matrix.GetPropertyDescriptors( attributes, desc.PropertyGrid );
			}

			return null;
		}
	}


}
