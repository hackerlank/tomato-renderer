using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics.Console
{
	partial class ConsoleVariable
	{
		/// <summary>
		/// Writes the console variable to the stream.
		/// </summary>
		/// <param name="stream"></param>
		public void WriteToStream( MessageStream stream )
		{
			// Write the name.
			stream.WriteString( Name );

			// Write the value type.
			stream.WriteU8( ( byte )ValueType );

			// Write the value.
			switch( ValueType )
			{
				case ConsoleVariableValueType.Bool:
					{
						stream.WriteBool( (bool)Value );
					}
					break;

				case ConsoleVariableValueType.BoolArray:
					{
						bool[] values = ( bool[] )Value;
						stream.WriteU32( ( uint )values.Count() );
						for( int i = 0 ; i < values.Count() ; ++i )
						{
							stream.WriteBool( values[ i ] );
						}
					}
					break;

				case ConsoleVariableValueType.Int:
					{
						stream.WriteS32( ( int )Value );
					}
					break;

				case ConsoleVariableValueType.IntArray:
					{
						int[] values = ( int[] )Value;
						uint valueCount = ( uint )values.Count();
						stream.WriteU32( valueCount );
						for( uint i = 0 ; i < valueCount ; ++i )
						{
							stream.WriteS32( values[ i ] );
						}
					}
					break;

				case ConsoleVariableValueType.UInt:
					{
						stream.WriteU32( ( uint )Value );
					}
					break;

				case ConsoleVariableValueType.UIntArray:
					{
						uint[] values = ( uint[] )Value;
						uint valueCount = ( uint )values.Count();
						stream.WriteU32( valueCount );
						for( uint i = 0 ; i < valueCount ; ++i )
						{
							stream.WriteU32( values[ i ] );
						}
					}
					break;

				case ConsoleVariableValueType.Float:
					{
						stream.WriteF32( ( float )Value );
					}
					break;

				case ConsoleVariableValueType.FloatArray:
					{
						float[] values = ( float[] )Value;
						uint valueCount = ( uint )values.Count();
						stream.WriteU32( valueCount );
						for( uint i = 0 ; i < valueCount ; ++i )
						{
							stream.WriteF32( values[ i ] );
						}
					}
					break;

				case ConsoleVariableValueType.Double:
					{
						stream.WriteF64( ( double )Value );
					}
					break;

				case ConsoleVariableValueType.DoubleArray:
					{
						double[] values = ( double[] )Value;
						uint valueCount = ( uint )values.Count();
						stream.WriteU32( valueCount );
						for( int i = 0 ; i < valueCount ; ++i )
						{
							stream.WriteF64( values[ i ] );
						}
					}
					break;

				case ConsoleVariableValueType.String:
					{
						stream.WriteString( ( string )Value );
					}
					break;

				case ConsoleVariableValueType.Vector2:
					{
						Vector2 vector2 = ( Vector2 )Value;
						stream.WriteF32( vector2.X );
						stream.WriteF32( vector2.Y );
					}
					break;

				case ConsoleVariableValueType.Vector3:
					{
						Vector3 vector3 = ( Vector3 )Value;
						stream.WriteF32( vector3.X );
						stream.WriteF32( vector3.Y );
						stream.WriteF32( vector3.Z );

					}
					break;

				case ConsoleVariableValueType.Vector4:
					{
						Vector4 vector4 = ( Vector4 )Value;
						stream.WriteF32( vector4.X );
						stream.WriteF32( vector4.Y );
						stream.WriteF32( vector4.Z );
						stream.WriteF32( vector4.W );
					}
					break;

				case ConsoleVariableValueType.Matrix:
					{
						Matrix matrix4 = ( Matrix )Value;
						stream.WriteF32( matrix4.M11 );
						stream.WriteF32( matrix4.M12 );
						stream.WriteF32( matrix4.M13 );
						stream.WriteF32( matrix4.M14 );
						stream.WriteF32( matrix4.M21 );
						stream.WriteF32( matrix4.M22 );
						stream.WriteF32( matrix4.M23 );
						stream.WriteF32( matrix4.M24 );
						stream.WriteF32( matrix4.M31 );
						stream.WriteF32( matrix4.M32 );
						stream.WriteF32( matrix4.M33 );
						stream.WriteF32( matrix4.M34 );
						stream.WriteF32( matrix4.M41 );
						stream.WriteF32( matrix4.M42 );
						stream.WriteF32( matrix4.M43 );
						stream.WriteF32( matrix4.M44 );
					}
					break;

				default:
					throw new NotImplementedException(
						string.Format( "Streaming of the value type {0} is not implemented.",
						Enum.GetName( typeof( ConsoleVariableValueType ), ValueType ) ) );
			}
		}

		/// <summary>
		/// Reads an console variable from the stream.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		public static ConsoleVariable ReadFromStream( MessageStream stream )
		{
			ConsoleVariable consoleVariable = new ConsoleVariable();

			// Read the name.
			consoleVariable.Name = stream.ReadString();

			// Read the value type.
			consoleVariable.ValueType = ( ConsoleVariableValueType )stream.ReadU8();

			// Read the value.
			switch( consoleVariable.ValueType )
			{
				case ConsoleVariableValueType.Bool:
					{
						consoleVariable.Value = stream.ReadBool();
					}
					break;

				case ConsoleVariableValueType.BoolArray:
					{
						uint valueCount = stream.ReadU32();
						bool[] valueArray = new bool[ valueCount ];
						for( uint i = 0 ; i < valueCount ; ++i )
						{
							valueArray[ i ] = stream.ReadBool();
						}
						consoleVariable.Value = valueArray;
					}
					break;

				case ConsoleVariableValueType.Int:
					{
						consoleVariable.Value = stream.ReadS32();
					}
					break;

				case ConsoleVariableValueType.IntArray:
					{
						uint valueCount = stream.ReadU32();
						int[] valueArray = new int[ valueCount ];
						for( uint i = 0 ; i < valueCount ; ++i )
						{
							valueArray[ i ] = stream.ReadS32();
						}
						consoleVariable.Value = valueArray;
					}
					break;

				case ConsoleVariableValueType.UInt:
					{
						consoleVariable.Value = stream.ReadU32();
					}
					break;

				case ConsoleVariableValueType.UIntArray:
					{
						uint valueCount = stream.ReadU32();
						uint[] valueArray = new uint[ valueCount ];
						for( uint i = 0 ; i < valueCount ; ++i )
						{
							valueArray[ i ] = stream.ReadU32();
						}
						consoleVariable.Value = valueArray;
					}
					break;

				case ConsoleVariableValueType.Float:
					{
						consoleVariable.Value = stream.ReadF32();
					}
					break;

				case ConsoleVariableValueType.FloatArray:
					{
						uint valueCount = stream.ReadU32();
						float[] valueArray = new float[ valueCount ];
						for( uint i = 0 ; i < valueCount ; ++i )
						{
							valueArray[ i ] = stream.ReadF32();
						}
						consoleVariable.Value = valueArray;
					}
					break;

				case ConsoleVariableValueType.Double:
					{
						consoleVariable.Value = stream.ReadF64();
					}
					break;

				case ConsoleVariableValueType.DoubleArray:
					{
						uint valueCount = stream.ReadU32();
						double[] valueArray = new double[ valueCount ];
						for( uint i = 0 ; i < valueCount ; ++i )
						{
							valueArray[ i ] = stream.ReadF64();
						}
						consoleVariable.Value = valueArray;
					}
					break;

				case ConsoleVariableValueType.String:
					{
						consoleVariable.Value = stream.ReadString();
					}
					break;

				case ConsoleVariableValueType.Vector2:
					{
						float X = stream.ReadF32();
						float Y = stream.ReadF32();
						consoleVariable.Value = new Vector2( X, Y );
					}
					break;

				case ConsoleVariableValueType.Vector3:
					{
						float X = stream.ReadF32();
						float Y = stream.ReadF32();
						float Z = stream.ReadF32();
						consoleVariable.Value = new Vector3( X, Y, Z );
					}
					break;

				case ConsoleVariableValueType.Vector4:
					{
						float X = stream.ReadF32();
						float Y = stream.ReadF32();
						float Z = stream.ReadF32();
						float W = stream.ReadF32();
						consoleVariable.Value = new Vector4( X, Y, Z, W );
					}
					break;

				case ConsoleVariableValueType.Matrix:
					{
						float[] elements = new float[ 16 ];
						for( uint i = 0 ; i < 16 ; ++i )
						{
							elements[ i ] = stream.ReadF32();
						}
						consoleVariable.Value = new Matrix(
							elements[ 0 ], elements[ 1 ], elements[ 2 ], elements[ 3 ],
							elements[ 4 ], elements[ 5 ], elements[ 6 ], elements[ 7 ],
							elements[ 8 ], elements[ 9 ], elements[ 10 ], elements[ 11 ],
							elements[ 12 ], elements[ 13 ], elements[ 14 ], elements[ 15 ] );
					}
					break;

				default:
					throw new NotImplementedException(
						string.Format( "Streaming of the value type {0} is not implemented.",
						Enum.GetName( typeof( ConsoleVariableValueType ), consoleVariable.ValueType ) ) );
			}

			return consoleVariable;
		}
	}
}
