using System;
using System.ComponentModel;

namespace Tomato.Graphics.Console.ConsoleGrid
{
	public class ConsoleGridItem
	{
		public string Category { get; set; }

		public string Name { get; set; }

		public object Value { get; set; }

		public ConsoleVariableValueType ValueType { get; set; }

		public object MinimumValue { get; set; }

		public object MaximumValue { get; set; }

		public object SteppingValue { get; set; }

		/// <summary>
		/// Creates a ConsoleGridItem instance.
		/// Note that this constructor does not convert the value into ConsoleGrid-friendly type.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="valueType"></param>
		/// <param name="value"></param>
		internal ConsoleGridItem( string name, ConsoleVariableValueType valueType, object value )
		{
			Category = null;
			Name = name;
			ValueType = valueType;
			Value = value;

			MinimumValue = null;
			MaximumValue = null;
			SteppingValue = null;
		}

		/// <summary>
		/// Creates a ConsoleGridItem instance.
		/// Note that this constructor does not convert the value into ConsoleGrid-friendly type.
		/// </summary>
		/// <param name="category"></param>
		/// <param name="name"></param>
		/// <param name="valueType"></param>
		/// <param name="value"></param>
		internal ConsoleGridItem( string category, string name, ConsoleVariableValueType valueType, object value )
		{
			Category = category;
			Name = name;
			ValueType = valueType;
			Value = value;

			MinimumValue = null;
			MaximumValue = null;
			SteppingValue = null;
		}

		/// <summary>
		/// Creates a ConsoleGridItem instance.
		/// The value of console variable is converted into ConsoleGrid-friendly type.
		/// </summary>
		/// <param name="consoleVariable"></param>
		public ConsoleGridItem( ConsoleVariable consoleVariable )
		{
			// Set the category name and item name.
			string[] nameTokens = consoleVariable.Name.Split( '|' );
			if( nameTokens.Length == 1 )
			{
				Category = null;
				Name = nameTokens[ 0 ];

			}
			else if( nameTokens.Length == 2 )
			{
				Category = nameTokens[ 0 ];
				Name = nameTokens[ 1 ];
				Value = consoleVariable.Value;
				ValueType = consoleVariable.ValueType;
			}
			else
			{
				throw new FormatException( consoleVariable.Name );
			}

			// Set (converted ) value.
			Value = GetConsoleGridItemValue( consoleVariable );

			// Set the value type.
			ValueType = consoleVariable.ValueType;

			// Working on...
			MinimumValue = null;
			MaximumValue = null;
			SteppingValue = null;
		}

		internal static object GetConsoleGridItemValue( ConsoleVariable consoleVariable )
		{
			switch( consoleVariable.ValueType )
			{
				case ConsoleVariableValueType.BoolArray: return new ConsoleGridBoolArray( consoleVariable.AsBoolArray() );
				case ConsoleVariableValueType.IntArray: return new ConsoleGridIntArray( consoleVariable.AsIntArray() );
				case ConsoleVariableValueType.UIntArray: return new ConsoleGridUIntArray( consoleVariable.AsUIntArray() );
				case ConsoleVariableValueType.FloatArray: return new ConsoleGridFloatArray( consoleVariable.AsFloatArray() );
				case ConsoleVariableValueType.DoubleArray: return new ConsoleGridDoubleArray( consoleVariable.AsDoubleArray() );
				case ConsoleVariableValueType.Vector2: return new ConsoleGridVector2( consoleVariable.AsVector2() );
				case ConsoleVariableValueType.Vector3: return new ConsoleGridVector3( consoleVariable.AsVector3() );
				case ConsoleVariableValueType.Vector4: return new ConsoleGridVector4( consoleVariable.AsVector4() );
				case ConsoleVariableValueType.Matrix: return new ConsoleGridMatrix( consoleVariable.AsMatrix() );
			}
			return consoleVariable.Value;
		}

		internal static object GetConsoleVariableValue( ConsoleGridItem consoleGridItem )
		{
			switch( consoleGridItem.ValueType )
			{
				case ConsoleVariableValueType.BoolArray: 
					{
						ConsoleGridBoolArray array = consoleGridItem.Value as ConsoleGridBoolArray;
						if( array != null )
						{
							return array.ToArray();
						}
					}
					break;

				case ConsoleVariableValueType.IntArray: 
					{
						ConsoleGridIntArray array = consoleGridItem.Value as ConsoleGridIntArray;
						if( array != null )
						{
							return array.ToArray();
						}
					}
					break;

				case ConsoleVariableValueType.UIntArray: 
					{
						ConsoleGridUIntArray array = consoleGridItem.Value as ConsoleGridUIntArray;
						if( array != null )
						{
							return array.ToArray();
						}
					}
					break;

				case ConsoleVariableValueType.FloatArray: 
					{
						ConsoleGridFloatArray array = consoleGridItem.Value as ConsoleGridFloatArray;
						if( array != null )
						{
							return array.ToArray();
						}
					}
					break;

				case ConsoleVariableValueType.DoubleArray: 
					{
						ConsoleGridDoubleArray array = consoleGridItem.Value as ConsoleGridDoubleArray;
						if( array != null )
						{
							return array.ToArray();
						}
					}
					break;

				case ConsoleVariableValueType.Vector2:
					{
						ConsoleGridVector2 vector = consoleGridItem.Value as ConsoleGridVector2;
						if( vector != null )
						{
							return vector.AsVector2();
						}
					}
					break;

				case ConsoleVariableValueType.Vector3:
					{
						ConsoleGridVector3 vector = consoleGridItem.Value as ConsoleGridVector3;
						if( vector != null )
						{
							return vector.AsVector3();
						}
					}
					break;

				case ConsoleVariableValueType.Vector4:
					{
						ConsoleGridVector4 vector = consoleGridItem.Value as ConsoleGridVector4;
						if( vector != null )
						{
							return vector.AsVector4();
						}
					}
					break;

				case ConsoleVariableValueType.Matrix:
					{
						ConsoleGridMatrix matrix = consoleGridItem.Value as ConsoleGridMatrix;
						if( matrix != null )
						{
							return matrix.AsMatrix();
						}
					}
					break;
			}
			return consoleGridItem.Value;
		}
	}
}