#pragma once

namespace Tomato
{
	class TOMATO_API Math
	{
	public:
		static const f32 PI;

		static const f32 FloatPositiveMax;
		static const f32 FloatPositiveMin;

		static bool CompareFloat( f32 a, f32 b );
		static bool CompareFloatZero( f32 a );
		static bool CompareFloat( f32 a, f32 b, s32 tolerance );
		static bool CompareFloatZero( f32 a, s32 tolerance );		

		static f32 ToRadian( f32 degree );
		static f32 ToDegree( f32 radian );
		
		static f32 Lerp( f32 a, f32 b, f32 w );

		static s32 Abs( s32 value );
		static f32 Abs( f32 value );
		static f64 Abs( f64 value );

		static s32 Min( s32 v1, s32 v2 );
		static f32 Min( f32 v1, f32 v2 );
		static f64 Min( f64 v1, double v2 );

		static s32 Max( s32 v1, s32 v2 );
		static f32 Max( f32 v1, f32 v2 );
		static f64 Max( f64 v1, f64 v2 );
	};	
}