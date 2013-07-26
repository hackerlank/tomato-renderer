using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tomato
{
	public static class MathHelper
	{
		public const int DefaultFloatComparisonTolerance = 100;

		public const float PI = ( float )( System.Math.PI );
		public const float HalfPI = ( float )( System.Math.PI * 0.5 );

		public static double Lerp( double value1, double value2, double weight )
		{
			return ( value2 - value1 ) * weight + value1;
		}

		public static float Lerp( float value1, float value2, float weight )
		{
			return ( value2 - value1 ) * weight + value1;
		}

		public static float Clamp( float minimum, float maximum, float value )
		{
			if( value > maximum ) { value = maximum; }
			if( value < minimum ) { value = minimum; }
			return value;
		}

		public static double Clamp( double minimum, double maximum, double value )
		{
			if( value > maximum ) { value = maximum; }
			if( value < minimum ) { value = minimum; }
			return value;
		}

		public static int Clamp( int minimum, int maximum, int value )
		{
			if( value > maximum ) { value = maximum; }
			if( value < minimum ) { value = minimum; }
			return value;
		}

		public static uint Clamp( uint minimum, uint maximum, uint value )
		{
			if( value > maximum ) { value = maximum; }
			if( value < minimum ) { value = minimum; }
			return value;
		}

		public static int RoundUpToPowerOfTwo( int value )
		{
			int result = 1;
			while( result < value )
			{
				result = result << 1;
			}

			return result;
		}

		public static float ToRadian( float degree )
		{
			return ( degree * PI ) / 180.0f;
		}

		public static float ToDegree( float radian )
		{
			return ( 180.0f * radian ) / PI;
		}

		// 참고: http://www.randydillon.org/Papers/2007/everfast.htm
		public static unsafe bool CompareFloat( float a, float b )
		{
			int ai = *( int* )( &a );
			int bi = *( int* )( &b );

			int test = ( ai ^ bi ) >> 31;
			System.Diagnostics.Debug.Assert( ( 0 == test ) || ( 0xFFFFFFFF == *( uint* )( &test ) ) );

			int diff = ( ai ^ ( test & 0x7fffffff ) ) - bi;
			int v1 = DefaultFloatComparisonTolerance + diff;
			int v2 = DefaultFloatComparisonTolerance - diff;

			return ( v1 | v2 ) >= 0;
		}

		// 참고: http://www.randydillon.org/Papers/2007/everfast.htm
		public static unsafe bool CompareFloat( float a, float b, int tolerance )
		{
			int ai = *( int* )( &a );
			int bi = *( int* )( &b );

			int test = ( ai ^ bi ) >> 31;
			System.Diagnostics.Debug.Assert( ( 0 == test ) || ( 0xFFFFFFFF == *( uint* )( &test ) ) );
			
			int diff = ( ai ^ ( test & 0x7fffffff ) ) - bi;
			int v1 = tolerance + diff;
			int v2 = tolerance - diff;
			
			return ( v1 | v2 ) >= 0;
		}

		// 참고: http://www.randydillon.org/Papers/2007/everfast.htm
		public static unsafe bool CompareFloatZero( float a )
		{
			int ai = *( int* )( &a );
			return ( ai & 0x7fffffff ) <= DefaultFloatComparisonTolerance;
		}

		// 참고: http://www.randydillon.org/Papers/2007/everfast.htm
		public static unsafe bool CompareFloatZero( float a, int tolerance )
		{
			int ai = *( int* )( &a );
			return ( ai & 0x7fffffff ) <= tolerance;
		}
	}
}
