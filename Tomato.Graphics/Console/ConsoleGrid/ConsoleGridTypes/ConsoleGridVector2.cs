using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridVector2
	{
		private ConsoleGridItem m_x = new ConsoleGridItem( "X", ConsoleVariableValueType.Float, 0.0f );
		private ConsoleGridItem m_y = new ConsoleGridItem( "Y", ConsoleVariableValueType.Float, 0.0f );

		public ConsoleGridVector2()
		{
		}

		public ConsoleGridVector2( Vector2 v )
		{
			m_x.Value = v.X;
			m_y.Value = v.Y;
		}

		public ConsoleGridVector2( float x, float y )
		{
			m_x.Value = x;
			m_y.Value = y;
		}

		public Vector2 AsVector2()
		{
			return new Vector2( ( float )m_x.Value, ( float )m_y.Value );
		}

		public void ClampTo( ConsoleGridVector2 minValue, ConsoleGridVector2 maxValue )
		{
			float minX = Convert.ToSingle( minValue.X.Value );
			float maxX = Convert.ToSingle( maxValue.X.Value );
			float x = Convert.ToSingle( m_x.Value );
			if( x < minX ) { x = minX; }
			if( x > maxX ) { x = maxX; }
			m_x.Value = x;

			float minY = Convert.ToSingle( minValue.Y.Value );
			float maxY = Convert.ToSingle( maxValue.Y.Value );
			float y = Convert.ToSingle( m_y.Value );
			if( y < minY ) { x = minY; }
			if( y > maxY ) { x = maxY; }
			m_y.Value = y;
		}

		public PropertyDescriptorCollection GetPropertyDescriptors( Attribute[] attributes, ConsoleGrid propertyGrid )
		{
			ConsoleGridItemDescriptor[] descriptors = new ConsoleGridItemDescriptor[ 2 ];

			List<Attribute> newAttributes = new List<Attribute>();
			newAttributes.AddRange( attributes );
			newAttributes.Add( new RefreshPropertiesAttribute( RefreshProperties.Repaint ) );
			newAttributes.Add( new EditorAttribute( typeof( ConsoleGridFloatValueEditor ), typeof( System.Drawing.Design.UITypeEditor ) ) );

			descriptors[ 0 ] = new ConsoleGridItemDescriptor( m_x, newAttributes.ToArray(), propertyGrid );
			descriptors[ 1 ] = new ConsoleGridItemDescriptor( m_y, newAttributes.ToArray(), propertyGrid );

			return new PropertyDescriptorCollection( descriptors );
		}

		public ConsoleGridItem X
		{
			get { return m_x; }
			set { m_x = value; }
		}

		public ConsoleGridItem Y
		{
			get { return m_y; }
			set { m_y = value; }
		}

		public static ConsoleGridVector2 FromString( string value )
		{
			ConsoleGridVector2 vector = new ConsoleGridVector2();
			string[] tokens = value.Split( ' ', ',', '(', ')' );

			int valueCount = 0;
			for( int i = 0 ; i < tokens.Length ; ++i )
			{
				if( tokens[ i ] != string.Empty )
				{
					try
					{
						switch( valueCount )
						{
							case 0:
								vector.m_x.Value = Convert.ToSingle( tokens[ i ] );
								break;
							case 1:
								vector.m_y.Value = Convert.ToSingle( tokens[ i ] );
								break;
							default: return vector; // extra numbers are just ignored
						}

						valueCount++;
					}
					catch( Exception ) { }
				}
			}

			return vector;
		}

		public override string ToString()
		{
			try
			{
				return "(" + m_x.Value + ", " + m_y.Value + ")";
			}
			catch( Exception )
			{
				return "";
			}
		}
	}
}