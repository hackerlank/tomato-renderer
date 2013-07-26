using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics.Console
{
	partial class ConsoleVariable
	{
		private static string ConvertCollectionValueToString<T>( IEnumerable<T> collection )
		{
			StringBuilder text = new StringBuilder();

			foreach( T item in collection )
			{
				text.AppendFormat( "{0}, ", item.ToString() );
			}

			if( text.Length > 0 )
			{
				return text.ToString( 0, text.Length - 2 );
			}
			else
			{
				return "";
			}
		}
		
		/// <summary>
		/// Creates a ConsoleVariable instance from a text value.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="valueType"></param>
		/// <param name="textValue"></param>
		/// <returns></returns>
		public static ConsoleVariable FromTextValue( string name, ConsoleVariableValueType valueType, string textValue )
		{
			ConsoleVariable consoleVariable = new ConsoleVariable() { Name = name, ValueType = valueType, Value = null };

			switch( valueType )
			{
				case ConsoleVariableValueType.String: { consoleVariable.Value = textValue; } break;

				case ConsoleVariableValueType.Bool: { consoleVariable.Value = bool.Parse( textValue ); } break;
				case ConsoleVariableValueType.Double: { consoleVariable.Value = double.Parse( textValue ); } break;
				case ConsoleVariableValueType.Float: { consoleVariable.Value = float.Parse( textValue ); } break;
				case ConsoleVariableValueType.Int: { consoleVariable.Value = int.Parse( textValue ); } break;
				case ConsoleVariableValueType.UInt: { consoleVariable.Value = uint.Parse( textValue ); } break;

				case ConsoleVariableValueType.BoolArray:
				case ConsoleVariableValueType.DoubleArray:
				case ConsoleVariableValueType.FloatArray:
				case ConsoleVariableValueType.IntArray:
				case ConsoleVariableValueType.UIntArray:
					{
						char[] separators = new char[] { ' ', '\t', ',', '(', ')' };
						string[] tokens = textValue.Split( separators, StringSplitOptions.RemoveEmptyEntries );
						if( valueType == ConsoleVariableValueType.BoolArray ) { consoleVariable.Value = ( from token in tokens select bool.Parse( token ) ).ToArray(); }
						if( valueType == ConsoleVariableValueType.DoubleArray ) { consoleVariable.Value = ( from token in tokens select double.Parse( token ) ).ToArray(); }
						if( valueType == ConsoleVariableValueType.FloatArray ) { consoleVariable.Value = ( from token in tokens select float.Parse( token ) ).ToArray(); }
						if( valueType == ConsoleVariableValueType.IntArray ) { consoleVariable.Value = ( from token in tokens select int.Parse( token ) ).ToArray(); }
						if( valueType == ConsoleVariableValueType.UIntArray ) { consoleVariable.Value = ( from token in tokens select uint.Parse( token ) ).ToArray(); }
					}
					break;

				case ConsoleVariableValueType.Vector2: { consoleVariable.Value = ParseVector2( textValue ); } break;
				case ConsoleVariableValueType.Vector3: { consoleVariable.Value = ParseVector3( textValue ); } break;
				case ConsoleVariableValueType.Vector4: { consoleVariable.Value = ParseVector4( textValue ); } break;
				case ConsoleVariableValueType.Matrix: { consoleVariable.Value = ParseMatrix( textValue ); } break;

				default:
					throw new NotImplementedException(
						string.Format( "Conversion of the value type {0} is not implemented.",
						Enum.GetName( typeof( ConsoleVariableValueType ), valueType ) ) );
			}

			return consoleVariable;
		}

		private static Vector2 ParseVector2( string text )
		{
			if( string.IsNullOrWhiteSpace( text ) )
			{
				throw new ArgumentNullException( "text" );
			}

			List<string> tokens = new List<string>( ( from t in text.Split( ' ', ',', '(', ')' )
													  where !string.IsNullOrEmpty( t )
													  select t ) );
			if( tokens.Count == 2 )
			{
				Vector2 vector = Vector2.Zero;
				if( float.TryParse( tokens[ 0 ], out vector.X )
					&& float.TryParse( tokens[ 1 ], out vector.Y ) )
				{
					return vector;
				}
			}

			throw new FormatException( string.Format( "Failed to parse Vector2 from the text {0}.", text ) );
		}

		private static Vector3 ParseVector3( string text )
		{
			if( string.IsNullOrWhiteSpace( text ) )
			{
				throw new ArgumentNullException( "text" );
			}

			List<string> tokens = new List<string>( ( from t in text.Split( ' ', ',', '(', ')' )
													  where !string.IsNullOrEmpty( t )
													  select t ) );
			if( tokens.Count == 3 )
			{
				Vector3 vector = Vector3.Zero;
				if( float.TryParse( tokens[ 0 ], out vector.X )
					&& float.TryParse( tokens[ 1 ], out vector.Y ) 
					&& float.TryParse( tokens[ 2 ], out vector.Z ) )
				{
					return vector;
				}
			}

			throw new FormatException( string.Format( "Failed to parse Vector3 from the text {0}.", text ) );
		}

		private static Vector4 ParseVector4( string text )
		{
			if( string.IsNullOrWhiteSpace( text ) )
			{
				throw new ArgumentNullException( "text" );
			}

			List<string> tokens = new List<string>( ( from t in text.Split( ' ', ',', '(', ')' )
													  where !string.IsNullOrEmpty( t )
													  select t ) );
			if( tokens.Count == 4 )
			{
				Vector4 vector = Vector4.Zero;
				if( float.TryParse( tokens[ 0 ], out vector.X )
					&& float.TryParse( tokens[ 1 ], out vector.Y )
					&& float.TryParse( tokens[ 2 ], out vector.Z )
					&& float.TryParse( tokens[ 3 ], out vector.W ) )
				{
					return vector;
				}
			}

			throw new FormatException( string.Format( "Failed to parse Vector4 from the text {0}.", text ) );
		}

		private static Matrix ParseMatrix( string text )
		{
			if( string.IsNullOrEmpty( text ) )
			{
				throw new ArgumentNullException( "text" );
			}

			List<string> tokens = new List<string>( ( from t in text.Split( ' ', ',', '(', ')' )
													  where !string.IsNullOrEmpty( t )
													  select t ) );
			if( tokens.Count == 16 )
			{
				Matrix matrix = new Matrix();
				if( float.TryParse( tokens[ 0 ], out matrix.M11 )
					&& float.TryParse( tokens[ 1 ], out matrix.M12 )
					&& float.TryParse( tokens[ 2 ], out matrix.M13 )
					&& float.TryParse( tokens[ 3 ], out matrix.M14 )
					&& float.TryParse( tokens[ 4 ], out matrix.M21 )
					&& float.TryParse( tokens[ 5 ], out matrix.M22 )
					&& float.TryParse( tokens[ 6 ], out matrix.M23 )
					&& float.TryParse( tokens[ 7 ], out matrix.M24 )
					&& float.TryParse( tokens[ 8 ], out matrix.M31 )
					&& float.TryParse( tokens[ 9 ], out matrix.M32 )
					&& float.TryParse( tokens[ 10 ], out matrix.M33 )
					&& float.TryParse( tokens[ 11 ], out matrix.M34 )
					&& float.TryParse( tokens[ 12 ], out matrix.M41 )
					&& float.TryParse( tokens[ 13 ], out matrix.M42 )
					&& float.TryParse( tokens[ 14 ], out matrix.M43 )
					&& float.TryParse( tokens[ 15 ], out matrix.M44 ) )
				{
					return matrix;
				}
			}

			throw new FormatException( string.Format( "Failed to parse Matrix from the text {0}.", text ) );
		}
	}
}
