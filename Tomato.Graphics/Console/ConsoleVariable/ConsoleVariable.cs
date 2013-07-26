using System;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics.Console
{
	/// <summary>
	/// Defines the console variable class.
	/// </summary>
	public partial class ConsoleVariable
	{
		/// <summary>
		/// Gets or sets the name of the console variable.
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// Gets or sets the type of the console variable's value.
		/// </summary>
		public ConsoleVariableValueType ValueType { get; set; }

		/// <summary>
		/// Gets or sets the value of the console variable.
		/// </summary>
		public object Value { get; set; }

		/// <summary>
		/// Internal use only.
		/// </summary>
		private ConsoleVariable()
		{
			Name = null;
			ValueType = ConsoleVariableValueType.None;
			Value = null;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, bool value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.Bool;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, bool[] value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.BoolArray;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, int value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.Int;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, int[] value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.IntArray;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, uint value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.UInt;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, uint[] value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.UIntArray;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, float value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.Float;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, float[] value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.FloatArray;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, double value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.Double;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, double[] value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.DoubleArray;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, string value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.String;
			Value = value;
		}
		
		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, string[] value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.StringArray;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, Vector2 value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.Vector2;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, Vector2[] value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.Vector2Array;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, Vector3 value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.Vector3;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, Vector3[] value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.Vector3Array;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, Vector4 value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.Vector4;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, Vector4[] value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.Vector4Array;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, Matrix value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.Matrix;
			Value = value;
		}

		/// <summary>
		/// Creates a ConsoleVariable instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public ConsoleVariable( string name, Matrix[] value )
		{
			Name = name;
			ValueType = ConsoleVariableValueType.MatrixArray;
			Value = value;
		}

		/// <summary>
		/// Gets the value as a bool.
		/// </summary>
		/// <returns></returns>
		public bool AsBool()
		{
			return Convert.ToBoolean( Value );
		}

		/// <summary>
		/// Gets the value as a bool array.
		/// </summary>
		/// <returns></returns>
		public bool[] AsBoolArray()
		{
			if( ValueType == ConsoleVariableValueType.BoolArray ) { return ( bool[] )( Value ); }
			return null;
		}

		/// <summary>
		/// Gets the value as an int.
		/// </summary>
		/// <returns></returns>
		public int AsInt()
		{
			return Convert.ToInt32( Value );
		}

		/// <summary>
		/// Gets the value as an int array.
		/// </summary>
		/// <returns></returns>
		public int[] AsIntArray()
		{
			if( ValueType == ConsoleVariableValueType.IntArray ) { return ( int[] )( Value ); }
			return null;
		}

		/// <summary>
		/// Gets the value as an unsigned int.
		/// </summary>
		/// <returns></returns>
		public uint AsUInt()
		{
			return Convert.ToUInt32( Value );
		}

		/// <summary>
		/// Gets the value as an unsigned int array.
		/// </summary>
		/// <returns></returns>
		public uint[] AsUIntArray()
		{
			if( ValueType == ConsoleVariableValueType.UIntArray ) { return ( uint[] )( Value ); }
			return null;
		}

		/// <summary>
		/// Gets the value as a float.
		/// </summary>
		/// <returns></returns>
		public float AsFloat()
		{
			return Convert.ToSingle( Value );
		}

		/// <summary>
		/// Gets the value as a float array.
		/// </summary>
		/// <returns></returns>
		public float[] AsFloatArray()
		{
			if( ValueType == ConsoleVariableValueType.FloatArray ) { return ( float[] )( Value ); }
			return null;
		}

		/// <summary>
		/// Gets the value as a double.
		/// </summary>
		/// <returns></returns>
		public double AsDouble()
		{
			return Convert.ToDouble( Value );
		}

		/// <summary>
		/// Gets the value as a double array.
		/// </summary>
		/// <returns></returns>
		public double[] AsDoubleArray()
		{
			if( ValueType == ConsoleVariableValueType.DoubleArray ) { return ( double[] )( Value ); }
			return null;
		}

		/// <summary>
		/// Gets the value as a Vector2.
		/// </summary>
		/// <returns></returns>
		public Vector2 AsVector2()
		{
			if( ValueType == ConsoleVariableValueType.Vector2 ) { return ( Vector2 )( Value ); }
			return Vector2.Zero;
		}

		/// <summary>
		/// Gets the value as a Vector2 array.
		/// </summary>
		/// <returns></returns>
		public Vector2[] AsVector2Array()
		{
			if( ValueType == ConsoleVariableValueType.Vector2Array ) { return ( Vector2[] )( Value ); }
			return null;
		}

		/// <summary>
		/// Gets the value as a Vector3.
		/// </summary>
		/// <returns></returns>
		public Vector3 AsVector3()
		{
			if( ValueType == ConsoleVariableValueType.Vector3 ) { return ( Vector3 )( Value ); }
			return Vector3.Zero;
		}

		/// <summary>
		/// Gets the value as a Vector3 array.
		/// </summary>
		/// <returns></returns>
		public Vector3[] AsVector3Array()
		{
			if( ValueType == ConsoleVariableValueType.Vector3Array ) { return ( Vector3[] )( Value ); }
			return null;
		}

		/// <summary>
		/// Gets the value as a Vector4.
		/// </summary>
		/// <returns></returns>
		public Vector4 AsVector4()
		{
			if( ValueType == ConsoleVariableValueType.Vector4 ) { return ( Vector4 )( Value ); }
			return Vector4.Zero;
		}

		/// <summary>
		/// Gets the value as a Vector4 array.
		/// </summary>
		/// <returns></returns>
		public Vector4[] AsVector4Array()
		{
			if( ValueType == ConsoleVariableValueType.Vector4Array ) { return ( Vector4[] )( Value ); }
			return null;
		}

		/// <summary>
		/// Gets the value as a Matrix.
		/// </summary>
		/// <returns></returns>
		public Matrix AsMatrix()
		{
			if( ValueType == ConsoleVariableValueType.Matrix ) { return ( Matrix )( Value ); }
			return Matrix.Identity;
		}

		/// <summary>
		/// Gets the value as a Matrix array.
		/// </summary>
		/// <returns></returns>
		public Matrix[] AsMatrixArray()
		{
			if( ValueType == ConsoleVariableValueType.MatrixArray ) { return ( Matrix[] )( Value ); }
			return null;
		}

		/// <summary>
		/// Gets the value as a string.
		/// </summary>
		/// <returns></returns>
		public string AsString()
		{
			return Convert.ToString( Value );
		}

		/// <summary>
		/// Gets the value as a string array.
		/// </summary>
		/// <returns></returns>
		public string[] AsStringArray()
		{
			if( ValueType == ConsoleVariableValueType.StringArray ) { return ( string[] )( Value ); }
			return null;
		}
	}
}
