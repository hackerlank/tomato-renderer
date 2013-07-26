#include "TomatoPCH.h"

#include "Math.h"

#include <cmath>
#include <algorithm>

#ifdef _MSC_VER
#define SIGNMASK(i) ((i)>>31)
#else
#define SIGNMASK(i) (-(s32)(((u32)(i))>>31))
#endif

#define FLOAT_COMPARISON_TOLERANCE 100

namespace Tomato
{
	const f32 Math::PI = 3.1415926535897932f;

	const f32 Math::FloatPositiveMax = 3.402823466e+38f;
	const f32 Math::FloatPositiveMin = 1.175494351e-38f;

	// http://www.randydillon.org/Papers/2007/everfast.htm
	bool Math::CompareFloat( f32 a, f32 b )
	{
		return CompareFloat( a, b, FLOAT_COMPARISON_TOLERANCE );
	}

	// http://www.randydillon.org/Papers/2007/everfast.htm
	bool Math::CompareFloat( f32 a, f32 b, s32 tolerance )
	{
		s32 ai = *reinterpret_cast<s32*>( &a );
		s32 bi = *reinterpret_cast<s32*>( &b );

		s32 test = SIGNMASK( ai ^ bi );
		Assert( ( 0 == test ) || ( 0xFFFFFFFF == test ) );

		s32 diff = ( ai ^ ( test & 0x7fffffff ) ) - bi;
		s32 v1 = tolerance + diff;
		s32 v2 = tolerance - diff;

		return ( v1 | v2 ) >= 0;
	}

	// http://www.randydillon.org/Papers/2007/everfast.htm
	bool Math::CompareFloatZero( f32 a )
	{
		return CompareFloatZero( a, FLOAT_COMPARISON_TOLERANCE );
	}

	// http://www.randydillon.org/Papers/2007/everfast.htm
	bool Math::CompareFloatZero( f32 a, s32 tolerance )
	{
		s32 ai = *reinterpret_cast<s32*>( &a );
		return ( ai & 0x7fffffff ) <= tolerance;
	}

	f32 Math::Lerp( f32 a, f32 b, f32 w )
	{
		return a + ( ( b - a ) * w );
	}

	s32 Math::Abs( s32 value )
	{
		return std::abs( value );
	}

	f32 Math::Abs( f32 value )
	{
		return std::abs( value );
	}

	f64 Math::Abs( f64 value )
	{
		return std::abs( value );
	}

	s32 Math::Min( s32 v1, s32 v2 )
	{
		return std::min<s32>( v1, v2 );
	}

	f32 Math::Min( f32 v1, f32 v2 )
	{
		return std::min<f32>( v1, v2 );
	}

	f64 Math::Min( f64 v1, f64 v2 )
	{
		return std::min<f64>( v1, v2 );
	}

	s32 Math::Max( s32 v1, s32 v2 )
	{
		return std::max<s32>( v1, v2 );
	}

	f32 Math::Max( f32 v1, f32 v2 )
	{
		return std::max<f32>( v1, v2 );
	}

	f64 Math::Max( f64 v1, f64 v2 )
	{
		return std::max<f64>( v1, v2 );
	}
}