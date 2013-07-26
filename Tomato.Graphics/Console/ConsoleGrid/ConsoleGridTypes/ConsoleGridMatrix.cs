using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	internal class ConsoleGridMatrix
	{
		private ConsoleGridItem m_row1 = new ConsoleGridItem( "Row1", ConsoleVariableValueType.Vector4, new ConsoleGridVector4() );
		private ConsoleGridItem m_row2 = new ConsoleGridItem( "Row2", ConsoleVariableValueType.Vector4, new ConsoleGridVector4() );
		private ConsoleGridItem m_row3 = new ConsoleGridItem( "Row3", ConsoleVariableValueType.Vector4, new ConsoleGridVector4() );
		private ConsoleGridItem m_row4 = new ConsoleGridItem( "Row4", ConsoleVariableValueType.Vector4, new ConsoleGridVector4() );

		public ConsoleGridMatrix()
		{
		}

		public ConsoleGridMatrix( Matrix mat )
		{
			( m_row1.Value as ConsoleGridVector4 ).X.Value = mat.M11;
			( m_row1.Value as ConsoleGridVector4 ).Y.Value = mat.M12;
			( m_row1.Value as ConsoleGridVector4 ).Z.Value = mat.M13;
			( m_row1.Value as ConsoleGridVector4 ).W.Value = mat.M14;
			( m_row2.Value as ConsoleGridVector4 ).X.Value = mat.M21;
			( m_row2.Value as ConsoleGridVector4 ).Y.Value = mat.M22;
			( m_row2.Value as ConsoleGridVector4 ).Z.Value = mat.M23;
			( m_row2.Value as ConsoleGridVector4 ).W.Value = mat.M24;
			( m_row3.Value as ConsoleGridVector4 ).X.Value = mat.M31;
			( m_row3.Value as ConsoleGridVector4 ).Y.Value = mat.M32;
			( m_row3.Value as ConsoleGridVector4 ).Z.Value = mat.M33;
			( m_row3.Value as ConsoleGridVector4 ).W.Value = mat.M34;
			( m_row4.Value as ConsoleGridVector4 ).X.Value = mat.M41;
			( m_row4.Value as ConsoleGridVector4 ).Y.Value = mat.M42;
			( m_row4.Value as ConsoleGridVector4 ).Z.Value = mat.M43;
			( m_row4.Value as ConsoleGridVector4 ).W.Value = mat.M44;
		}

		public ConsoleGridMatrix( float[] values )
		{
			if( values.Length != 16 )
			{
				throw new InvalidOperationException();
			}

			( m_row1.Value as ConsoleGridVector4 ).X.Value = values[ 0 ];
			( m_row1.Value as ConsoleGridVector4 ).Y.Value = values[ 1 ];
			( m_row1.Value as ConsoleGridVector4 ).Z.Value = values[ 2 ];
			( m_row1.Value as ConsoleGridVector4 ).W.Value = values[ 3 ];
			( m_row2.Value as ConsoleGridVector4 ).X.Value = values[ 4 ];
			( m_row2.Value as ConsoleGridVector4 ).Y.Value = values[ 5 ];
			( m_row2.Value as ConsoleGridVector4 ).Z.Value = values[ 6 ];
			( m_row2.Value as ConsoleGridVector4 ).W.Value = values[ 7 ];
			( m_row3.Value as ConsoleGridVector4 ).X.Value = values[ 8 ];
			( m_row3.Value as ConsoleGridVector4 ).Y.Value = values[ 9 ];
			( m_row3.Value as ConsoleGridVector4 ).Z.Value = values[ 10 ];
			( m_row3.Value as ConsoleGridVector4 ).W.Value = values[ 11 ];
			( m_row4.Value as ConsoleGridVector4 ).X.Value = values[ 12 ];
			( m_row4.Value as ConsoleGridVector4 ).Y.Value = values[ 13 ];
			( m_row4.Value as ConsoleGridVector4 ).Z.Value = values[ 14 ];
			( m_row4.Value as ConsoleGridVector4 ).W.Value = values[ 15 ];
		}

		/// <summary>
		/// 1st row
		/// </summary>
		public ConsoleGridItem Row1
		{
			get { return m_row1; }
			set { m_row1 = value; }
		}

		/// <summary>
		/// 2nd row
		/// </summary>
		public ConsoleGridItem Row2
		{
			get { return m_row2; }
			set { m_row2 = value; }
		}

		/// <summary>
		/// 3rd row
		/// </summary>
		public ConsoleGridItem Row3
		{
			get { return m_row3; }
			set { m_row3 = value; }
		}

		/// <summary>
		/// 4th row
		/// </summary>
		public ConsoleGridItem Row4
		{
			get { return m_row4; }
			set { m_row4 = value; }
		}

		public Matrix AsMatrix()
		{
			return new Matrix(
				( float )( m_row1.Value as ConsoleGridVector4 ).X.Value,
				( float )( m_row1.Value as ConsoleGridVector4 ).Y.Value,
				( float )( m_row1.Value as ConsoleGridVector4 ).Z.Value,
				( float )( m_row1.Value as ConsoleGridVector4 ).W.Value,
				( float )( m_row2.Value as ConsoleGridVector4 ).X.Value,
				( float )( m_row2.Value as ConsoleGridVector4 ).Y.Value,
				( float )( m_row2.Value as ConsoleGridVector4 ).Z.Value,
				( float )( m_row2.Value as ConsoleGridVector4 ).W.Value,
				( float )( m_row3.Value as ConsoleGridVector4 ).X.Value,
				( float )( m_row3.Value as ConsoleGridVector4 ).Y.Value,
				( float )( m_row3.Value as ConsoleGridVector4 ).Z.Value,
				( float )( m_row3.Value as ConsoleGridVector4 ).W.Value,
				( float )( m_row4.Value as ConsoleGridVector4 ).X.Value,
				( float )( m_row4.Value as ConsoleGridVector4 ).Y.Value,
				( float )( m_row4.Value as ConsoleGridVector4 ).Z.Value,
				( float )( m_row4.Value as ConsoleGridVector4 ).W.Value );
		}


		/// <summary>
		/// Returns property descriptors
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		public PropertyDescriptorCollection GetPropertyDescriptors( Attribute[] attributes, ConsoleGrid propertyGrid )
		{
			ConsoleGridItemDescriptor[] descriptors = new ConsoleGridItemDescriptor[ 4 ];

			List<Attribute> newAttributes = new List<Attribute>();
			newAttributes.AddRange( attributes );
			newAttributes.Add( new RefreshPropertiesAttribute( RefreshProperties.Repaint ) );

			descriptors[ 0 ] = new ConsoleGridItemDescriptor( m_row1, newAttributes.ToArray(), propertyGrid );
			descriptors[ 1 ] = new ConsoleGridItemDescriptor( m_row2, newAttributes.ToArray(), propertyGrid );
			descriptors[ 2 ] = new ConsoleGridItemDescriptor( m_row3, newAttributes.ToArray(), propertyGrid );
			descriptors[ 3 ] = new ConsoleGridItemDescriptor( m_row4, newAttributes.ToArray(), propertyGrid );

			return new PropertyDescriptorCollection( descriptors );
		}

		/// <summary>
		/// Makes matrix from string
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static ConsoleGridMatrix FromString( string value )
		{
			string[] tokens = value.Split( ' ', ',', '(', ')' );
			float[] values = new float[ 16 ];

			for( int i = 0 ; i < values.Length ; ++i ) { values[ i ] = 0; }

			int valueCount = 0;
			for( int i = 0 ; i < tokens.Length && valueCount < 16 ; ++i )
			{
				if( tokens[ i ] != string.Empty )
				{
					try
					{
						values[ valueCount ] = Convert.ToSingle( tokens[ i ] );
						valueCount++;
					}
					catch( Exception ) { }
				}
			}

			return new ConsoleGridMatrix( values );
		}

		/// <summary>
		/// Returns text representation of vector: "(X, Y, Z, W)"
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "( " + m_row1.Value.ToString() + ", " +
				m_row2.Value.ToString() + ", " +
				m_row3.Value.ToString() + ", " +
				m_row4.Value.ToString() + ")";
		}
	}

}
