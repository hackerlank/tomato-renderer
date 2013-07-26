using System;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics.Console
{
	partial class ConsoleInterface
	{
		/// <summary>
		/// Gets a ConsoleVariable.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ConsoleVariable Get( string name )
		{
			return m_consoleVariables.Find( v => v.Name == name );
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a bool.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool GetBool( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsBool(); }
			return false;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a bool array.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool[] GetBoolArray( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsBoolArray(); }
			return null;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as an int.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public int GetInt( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsInt(); }
			return 0;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as an int array.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public int[] GetIntArray( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsIntArray(); }
			return null;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as an unsigned int.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public uint GetUInt( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsUInt(); }
			return 0;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as an unsigned int array.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public uint[] GetUIntArray( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsUIntArray(); }
			return null;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a float.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public float GetFloat( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsFloat(); }
			return 0;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a float array.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public float[] GetFloatArray( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsFloatArray(); }
			return null;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a double.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public double GetDouble( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsDouble(); }
			return 0;
		}
		
		/// <summary>
		/// Gets the value of a ConsoleVariable as a double array.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public double[] GetDoubleArray( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsDoubleArray(); }
			return null;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a string.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public string GetString( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsString(); }
			return null;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a string array.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public string[] GetStringArray( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsStringArray(); }
			return null;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a Vector2.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Vector2 GetVector2( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsVector2(); }
			return Vector2.Zero;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a Vector2 array.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Vector2[] GetVector2Array( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsVector2Array(); }
			return null;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a Vector3.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Vector3 GetVector3( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsVector3(); }
			return Vector3.Zero;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a Vector3 array.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Vector3[] GetVector3Array( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsVector3Array(); }
			return null;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a Vector4.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Vector4 GetVector4( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsVector4(); }
			return Vector4.Zero;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a Vector4 array.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Vector4[] GetVector4Array( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsVector4Array(); }
			return null;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a Matrix.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Matrix GetMatrix( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsMatrix(); }
			return Matrix.Identity;
		}

		/// <summary>
		/// Gets the value of a ConsoleVariable as a Matrix array.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Matrix[] GetMatrixArray( string name )
		{
			ConsoleVariable consoleVariable = Get( name );
			if( consoleVariable != null ) { return consoleVariable.AsMatrixArray(); }
			return null;
		}
	}
}