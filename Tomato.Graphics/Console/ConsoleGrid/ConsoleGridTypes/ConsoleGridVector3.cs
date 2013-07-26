using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	/// <summary>
	/// Vector of three elements
	/// </summary>
	internal class ConsoleGridVector3
	{
		//private Float m_x;
		private ConsoleGridItem m_x = new ConsoleGridItem( "X", ConsoleVariableValueType.Float, 0.0f );
		private ConsoleGridItem m_y = new ConsoleGridItem( "Y", ConsoleVariableValueType.Float, 0.0f );
		private ConsoleGridItem m_z = new ConsoleGridItem( "Z", ConsoleVariableValueType.Float, 0.0f );

		/// <summary>
		/// Constructor
		/// </summary>
		public ConsoleGridVector3()
		{
		}

		public ConsoleGridVector3( Vector3 v )
		{
			m_x.Value = v.X;
			m_y.Value = v.Y;
			m_z.Value = v.Z;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public ConsoleGridVector3( float x, float y, float z )
		{
			m_x.Value = x;
			m_y.Value = y;
			m_z.Value = z;
		}

		public Vector3 AsVector3()
		{
			return new Vector3( ( float )m_x.Value, ( float )m_y.Value, ( float )m_z.Value );
		}

		public void ClampTo( ConsoleGridVector3 minValue, ConsoleGridVector3 maxValue )
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

			float minZ = Convert.ToSingle( minValue.Z.Value );
			float maxZ = Convert.ToSingle( maxValue.Z.Value );
			float z = Convert.ToSingle( m_z.Value );
			if( z < minZ ) { z = minZ; }
			if( z > maxZ ) { z = maxZ; }
			m_z.Value = z;
		}

		/// <summary>
		/// Returns property descriptors
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public PropertyDescriptorCollection GetPropertyDescriptors( Attribute[] attributes, ConsoleGrid propertyGrid )
		{
			ConsoleGridItemDescriptor[] descriptors = new ConsoleGridItemDescriptor[ 3 ];

			List<Attribute> newAttributes = new List<Attribute>();
			newAttributes.AddRange( attributes );
			newAttributes.Add( new RefreshPropertiesAttribute( RefreshProperties.Repaint ) );
			newAttributes.Add( new EditorAttribute( typeof( ConsoleGridFloatValueEditor ), typeof( System.Drawing.Design.UITypeEditor ) ) );

			descriptors[ 0 ] = new ConsoleGridItemDescriptor( m_x, newAttributes.ToArray(), propertyGrid );
			descriptors[ 1 ] = new ConsoleGridItemDescriptor( m_y, newAttributes.ToArray(), propertyGrid );
			descriptors[ 2 ] = new ConsoleGridItemDescriptor( m_z, newAttributes.ToArray(), propertyGrid );

			return new PropertyDescriptorCollection( descriptors );
		}

		/// <summary>
		/// X
		/// </summary>
		public ConsoleGridItem X
		{
			get { return m_x; }
			set { m_x = value; }
		}

		/// <summary>
		/// Y
		/// </summary>
		public ConsoleGridItem Y
		{
			get { return m_y; }
			set { m_y = value; }
		}

		/// <summary>
		/// Z
		/// </summary>
		public ConsoleGridItem Z
		{
			get { return m_z; }
			set { m_z = value; }
		}

		/// <summary>
		/// Makes vector from string
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static ConsoleGridVector3 FromString( string value )
		{
			ConsoleGridVector3 vector = new ConsoleGridVector3();
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
							case 2:
								vector.m_z.Value = Convert.ToSingle( tokens[ i ] );
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
				return "(" + m_x.Value + ", " + m_y.Value + ", " + m_z.Value + ")";
			}
			catch( Exception )
			{
				return "";
			}
		}
	}
}