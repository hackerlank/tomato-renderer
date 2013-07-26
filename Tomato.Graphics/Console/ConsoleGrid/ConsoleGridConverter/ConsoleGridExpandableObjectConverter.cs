using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridExpandableObjectConverter : System.ComponentModel.ExpandableObjectConverter
	{
		public override bool CanConvertTo( ITypeDescriptorContext context, Type destinationType )
		{
			if( destinationType == typeof( string ) ) { return true; }
			return base.CanConvertTo( context, destinationType );
		}

		public override bool CanConvertFrom( ITypeDescriptorContext context, Type sourceType )
		{
			if( sourceType == typeof( string ) ) { return true; }
			return base.CanConvertFrom( context, sourceType );
		}
	}
}