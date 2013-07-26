#include "TomatoPCH.h"

#include "Vector2.h"

#include <cmath>

namespace Tomato
{
	Vector2::Vector2()
		: X( 0 )
		, Y( 0 )
	{
	}

	Vector2::Vector2( f32 x, f32 y )
		: X( x )
		, Y( y )
	{
	}

	Vector2::~Vector2()
	{
	}

	f32& Vector2::operator[] ( s32 index )
	{
		Assert( index >= 0 );
		Assert( index < 2 );

		return V[ index ];
	}

	const f32& Vector2::operator[] ( s32 index ) const
	{
		Assert( index >= 0 );
		Assert( index < 2 );

		return V[ index ];
	}

	void Vector2::SetX( f32 x )
	{
		X = x;
	}

	void Vector2::SetY( f32 y )
	{
		Y = y;
	}

	void Vector2::Set( f32 x, f32 y )
	{
		SetX( x );
		SetY( y );
	}

	Vector2& Vector2::operator = ( const Vector2& v )
	{
		Set( v.X, v.Y );
		return *this;
	}

	Vector2& Vector2::operator += ( const Vector2& v )
	{
		X += v.X;
		Y += v.Y;

		return *this;
	}

	Vector2& Vector2::operator -= ( const Vector2& v )
	{
		X -= v.X;
		Y -= v.Y;

		return *this;
	}

	Vector2& Vector2::operator *= ( const Vector2& v )
	{
		X *= v.X;
		Y *= v.Y;

		return *this;
	}

	Vector2& Vector2::operator /= ( const Vector2& v )
	{
		X /= v.X;
		Y /= v.Y;

		return *this;
	}

	Vector2& Vector2::operator *= ( f32 scalar )
	{
		X *= scalar;
		Y *= scalar;

		return *this;
	}

	Vector2& Vector2::operator /= ( f32 scalar )
	{
		Assert( scalar != 0 );

		f32 inv = 1 / scalar;

		X *= inv;
		Y *= inv;

		return *this;
	}

	bool Vector2::operator == ( const Vector2& v ) const
	{
		return ( v.X == X && v.Y == Y );
	}

	bool Vector2::operator != ( const Vector2& v ) const
	{
		return !( (*this) == v );
	}

	Vector2 Vector2::operator + ( const Vector2& v ) const
	{
		Vector2 result = *this;
		result += v;
		return result;
	}

	Vector2 Vector2::operator - ( const Vector2& v ) const
	{
		Vector2 result = *this;
		result -= v;
		return result;
	}

	Vector2 Vector2::operator * ( const Vector2& v ) const
	{
		Vector2 result = *this;
		result *= v;
		return result;
	}

	Vector2 Vector2::operator / ( const Vector2& v ) const
	{
		Vector2 result = *this;
		result /= v;
		return result;
	}

	Vector2 Vector2::operator * ( f32 scalar ) const
	{
		Vector2 result = *this;
		result *= scalar;
		return result;
	}

	Vector2 Vector2::operator / ( f32 scalar ) const
	{
		Vector2 result = *this;
		result /= scalar;
		return result;
	}

	const Vector2& Vector2::operator + () const
	{
		return *this;
	}

	Vector2 Vector2::operator - () const
	{
		return (*this) * -1.0f;
	}

	bool Vector2::operator < ( const Vector2& v ) const
	{
		return ( X < v.X && Y < v.Y );
	}

	bool Vector2::operator > ( const Vector2& v ) const
	{
		return ( X > v.X && Y > v.Y );
	}

	void Vector2::SetZero()
	{
		Set( 0, 0 );
	}

	f32 Vector2::GetLength() const
	{
		return sqrtf( GetLengthSquared() );
	}

	f32 Vector2::GetLengthSquared() const
	{
		return ( X * X + Y * Y );
	}

	void Vector2::Normalize()
	{
		f32 length = GetLength();
		
		if( length != 0 )
		{
			f32 invLength = 1 / length;
			X *= invLength;
			Y *= invLength;
		}
	}

	f32 Vector2::GetDistance( const Vector2& v1, const Vector2& v2 )
	{
		return ( v1 - v2 ).GetLength();
	}

	f32 Vector2::GetDistanceSquared( const Vector2& v1, const Vector2& v2 )
	{
		return ( v1 - v2 ).GetLengthSquared();
	}

	f32 Vector2::Dot( const Vector2& v1, const Vector2& v2 )
	{
		return ( ( v1.X * v2.X ) + ( v1.Y * v2.Y ) );
	}

	Vector2 Vector2::Normalize( const Vector2& v )
	{
		Vector2 vector = v;
		vector.Normalize();
		return vector;
	}

	Vector2 Vector2::Reflect( const Vector2& v, const Vector2& normal )
	{
		f32 dot = Dot( v, normal );

		return Vector2(
			v.X - ( ( static_cast<f32>( 2 ) * dot ) * normal.X ),
			v.Y - ( ( static_cast<f32>( 2 ) * dot ) * normal.Y ) );
	}


	Vector2 Vector2::Min( const Vector2& v1, const Vector2& v2 )
	{
		return Vector2(
			Math::Min( v1.X, v2.X ),
			Math::Min( v1.Y, v2.Y ) );
	}

	Vector2 Vector2::Max( const Vector2& v1, const Vector2& v2 )
	{
		return Vector2(
			Math::Max( v1.X, v2.X ),
			Math::Max( v1.Y, v2.Y ) );
	}

	Vector2 Vector2::Clamp( const Vector2& v, const Vector2& min, const Vector2& max  )
	{
		Vector2 vector = v;
		vector.SetX( ( vector.X > max.X ) ? max.X : vector.X );
		vector.SetX( ( vector.X < min.X ) ? min.X : vector.X );
		vector.SetY( ( vector.Y > max.Y ) ? max.Y : vector.Y );
		vector.SetY( ( vector.Y < min.Y ) ? min.Y : vector.Y );
		return vector;
	}

	Vector2 Vector2::Lerp( const Vector2& v1, const Vector2& v2, f32 w )
	{
		return Vector2(
			v1.X + ( ( v2.X - v1.X ) * w ),
			v1.Y + ( ( v2.Y - v1.Y ) * w ) );
	}

	Vector2 Vector2::Barycentric( const Vector2& v1, const Vector2& v2, const Vector2& v3, f32 w1, f32 w2 )
	{
		return Vector2(
			( v1.X + ( w1 * ( v2.X - v1.X ) ) ) + ( w2 * ( v3.X - v1.X ) ),
			( v1.Y + ( w1 * ( v2.Y - v1.Y ) ) ) + ( w2 * ( v3.Y - v1.Y ) ) );
	}

	Vector2 Vector2::SmoothStep( const Vector2& v1, const Vector2& v2, f32 w )
	{
		w = ( w > 1 ) ? 1 : ( ( w < 0 ) ? 0 : w );
		w = ( w * w ) * ( static_cast<f32>( 3 ) - ( static_cast<f32>( 2 ) * w ) );

		return Vector2(
			v1.X + ( ( v2.X - v1.X ) * w ),
			v1.Y + ( ( v2.Y - v1.Y ) * w ) );
	}

	Vector2 Vector2::CatmullRom( const Vector2& v1, const Vector2& v2, const Vector2& v3, const Vector2& v4, f32 w )
	{
		f32 amountSqaured = w * w;
		f32 amountCubed = w * amountSqaured;

		return Vector2(
			static_cast<f32>( 0.5 ) * ( ( ( ( static_cast<f32>( 2 ) * v2.X ) + ( ( -v1.X + v3.X ) * w ) ) + ( ( ( ( ( static_cast<f32>( 2 ) * v1.X ) - ( static_cast<f32>( 5 ) * v2.X ) ) + ( static_cast<f32>( 4 ) * v3.X ) ) - v4.X ) * amountSqaured ) ) + ( ( ( ( -v1.X + ( static_cast<f32>( 3 ) * v2.X ) ) - ( static_cast<f32>( 3 ) * v3.X ) ) + v4.X ) * amountCubed ) ),
			static_cast<f32>( 0.5 ) * ( ( ( ( static_cast<f32>( 2 ) * v2.Y ) + ( ( -v1.Y + v3.Y ) * w ) ) + ( ( ( ( ( static_cast<f32>( 2 ) * v1.Y ) - ( static_cast<f32>( 5 ) * v2.Y ) ) + ( static_cast<f32>( 4 ) * v3.Y ) ) - v4.Y ) * amountSqaured ) ) + ( ( ( ( -v1.Y + ( static_cast<f32>( 3 ) * v2.Y ) ) - ( static_cast<f32>( 3 ) * v3.Y ) ) + v4.Y ) * amountCubed ) ) );
	}

	Vector2 Vector2::Hermite( const Vector2& v1, const Vector2& tangent1, const Vector2& v2, const Vector2& tangent2, f32 w )
	{
		f32 amountSquared = w * w;
		f32 amountCubed = w * amountSquared;

		f32 s1 = ( ( static_cast<f32>( 2 ) * amountCubed ) - ( static_cast<f32>( 3 ) * amountSquared ) ) + 1;
		f32 s2 = ( static_cast<f32>( -2 ) * amountCubed ) + ( static_cast<f32>( 3 ) * amountSquared );
		f32 s3 = ( amountCubed - ( static_cast<f32>( 2 ) * amountSquared ) ) + w;
		f32 s4 = amountCubed - amountSquared;

		return Vector2(
			( ( ( v1.X * s1 ) + ( v2.X * s2 ) ) + ( tangent1.X * s3 ) ) + ( tangent2.X * s4 ),
			( ( ( v1.Y * s1 ) + ( v2.Y * s2 ) ) + ( tangent1.Y * s3 ) ) + ( tangent2.Y * s4 ) );
	}

	Vector2 Vector2::Zero()
	{
		return Vector2( 0, 0 );
	}

	Vector2 Vector2::One()
	{
		return Vector2( 1, 1 );
	}

	Vector2 Vector2::UnitX()
	{
		return Vector2( 1, 0 );
	}

	Vector2 Vector2::UnitY()
	{
		return Vector2( 0, 1 );
	}
}