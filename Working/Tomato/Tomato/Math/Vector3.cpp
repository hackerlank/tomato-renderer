#include "TomatoPCH.h"

#include "Vector3.h"

#include <cmath>

namespace Tomato
{
	Vector3::Vector3()
		: X( 0 )
		, Y( 0 )
		, Z( 0 )
	{
	}

	Vector3::Vector3( f32 x, f32 y, f32 z )
		: X( x )
		, Y( y )
		, Z( z )
	{
	}

	Vector3::Vector3( const Vector2& v, f32 z )
		: X( v.X )
		, Y( v.Y )
		, Z( z )
	{
	}


	Vector3::~Vector3()
	{
	}

	f32& Vector3::operator[] ( s32 index )
	{
		Assert( index >= 0 );
		Assert( index < 3 );
		return V[ index ];
	}

	const f32& Vector3::operator[] ( s32 index ) const
	{
		Assert( index >= 0 );
		Assert( index < 3 );
		return V[ index ];
	}

	void Vector3::SetX( f32 x )
	{
		X = x;
	}

	void Vector3::SetY( f32 y )
	{
		Y = y;
	}

	void Vector3::SetZ( f32 z )
	{
		Z = z;
	}

	void Vector3::Set( f32 x, f32 y, f32 z )
	{
		SetX( x );
		SetY( y );
		SetZ( z );
	}

	Vector3& Vector3::operator = ( const Vector3& v )
	{
		Set( v.X, v.Y, v.Z );
		return *this;
	}

	Vector3& Vector3::operator = ( const Vector2& v )
	{
		Set( v.X, v.Y, 0 );
		return *this;
	}

	Vector3& Vector3::operator += ( const Vector3& v )
	{
		X += v.X;
		Y += v.Y;
		Z += v.Z;

		return *this;
	}

	Vector3& Vector3::operator -= ( const Vector3& v )
	{
		X -= v.X;
		Y -= v.Y;
		Z -= v.Z;

		return *this;
	}

	Vector3& Vector3::operator *= ( const Vector3& v )
	{
		X *= v.X;
		Y *= v.Y;
		Z *= v.Z;

		return *this;
	}

	Vector3& Vector3::operator /= ( const Vector3& v )
	{
		X /= v.X;
		Y /= v.Y;
		Z /= v.Z;

		return *this;
	}

	Vector3& Vector3::operator *= ( f32 scalar )
	{
		X *= scalar;
		Y *= scalar;
		Z *= scalar;

		return *this;
	}

	Vector3& Vector3::operator /= ( f32 scalar )
	{
		Assert( scalar != 0 );

		f32 inv = 1 / scalar;

		X *= inv;
		Y *= inv;
		Z *= inv;

		return *this;
	}

	bool Vector3::operator == ( const Vector3& v ) const
	{
		return ( v.X == X && v.Y == Y && v.Z == Z );
	}

	bool Vector3::operator != ( const Vector3& v ) const
	{
		return !( (*this) == v );
	}

	Vector3 Vector3::operator + ( const Vector3& v ) const
	{
		Vector3 result = *this;
		result += v;
		return result;
	}

	Vector3 Vector3::operator - ( const Vector3& v ) const
	{
		Vector3 result = *this;
		result -= v;
		return result;
	}

	Vector3 Vector3::operator * ( const Vector3& v ) const
	{
		Vector3 result = *this;
		result *= v;
		return result;
	}

	Vector3 Vector3::operator / ( const Vector3& v ) const
	{
		Vector3 result = *this;
		result /= v;
		return result;
	}

	Vector3 Vector3::operator * ( f32 scalar ) const
	{
		Vector3 result = *this;
		result *= scalar;
		return result;
	}

	Vector3 Vector3::operator / ( f32 scalar ) const
	{
		Vector3 result = *this;
		result /= scalar;
		return result;
	}

	const Vector3& Vector3::operator + () const
	{
		return *this;
	}

	Vector3 Vector3::operator - () const
	{
		return (*this) * static_cast<f32>( -1 );
	}

	bool Vector3::operator < ( const Vector3& v ) const
	{
		return ( X < v.X && Y < v.Y && Z < v.Z );
	}

	bool Vector3::operator > ( const Vector3& v ) const
	{
		return ( X > v.X && Y > v.Y && Z > v.Z );
	}

	void Vector3::SetZero()
	{
		Set( 0, 0, 0 );
	}

	f32 Vector3::GetLength() const
	{
		return sqrtf( GetLengthSquared() );
	}

	f32 Vector3::GetLengthSquared() const
	{
		return ( X * X + Y * Y + Z * Z );
	}

	void Vector3::Normalize()
	{
		f32 length = GetLength();

		if( length != 0 )
		{
			f32 invLength = 1 / length;
			X *= invLength;
			Y *= invLength;
			Z *= invLength;
		}
	}

	f32 Vector3::GetDistance( const Vector3& v1, const Vector3& v2 )
	{
		return ( v1 - v2 ).GetLength();
	}

	f32 Vector3::GetDistanceSquared( const Vector3& v1, const Vector3& v2 )
	{
		return ( v1 - v2 ).GetLengthSquared();
	}

	f32 Vector3::Dot( const Vector3& v1, const Vector3& v2 )
	{
		return ( ( v1.X * v2.X ) + ( v1.Y * v2.Y ) + ( v1.Z * v2.Z ) );
	}

	Vector3 Vector3::Normalize( const Vector3& v )
	{
		Vector3 vector = v;
		vector.Normalize();
		return vector;
	}

	Vector3 Vector3::Cross( const Vector3& v1, const Vector3& v2 )
	{
		return Vector3(
			( v1.Y * v2.Z ) - ( v1.Z * v2.Y ),
			( v1.Z * v2.X ) - ( v1.X * v2.Z ),
			( v1.X * v2.Y ) - ( v1.Y * v2.X ) );
	}

	Vector3 Vector3::Reflect( const Vector3& v, const Vector3& normal )
	{
		f32 dot = Dot( v, normal );

		return Vector3(
			v.X - ( ( 2 * dot ) * normal.X ),
			v.Y - ( ( 2 * dot ) * normal.Y ),
			v.Z - ( ( 2 * dot ) * normal.Z ) );
	}


	Vector3 Vector3::Min( const Vector3& v1, const Vector3& v2 )
	{
		return Vector3(
			Math::Min( v1.X, v2.X ),
			Math::Min( v1.Y, v2.Y ),
			Math::Min( v1.Z, v2.Z ) );
	}

	Vector3 Vector3::Max( const Vector3& v1, const Vector3& v2 )
	{
		return Vector3(
			Math::Max( v1.X, v2.X ),
			Math::Max( v1.Y, v2.Y ),
			Math::Max( v1.Z, v2.Z ) );
	}

	Vector3 Vector3::Clamp( const Vector3& v, const Vector3& min, const Vector3& max )
	{
		Vector3 vector = v;
		vector.SetX( ( vector.X > max.X ) ? max.X : vector.X );
		vector.SetX( ( vector.X < min.X ) ? min.X : vector.X );
		vector.SetY( ( vector.Y > max.Y ) ? max.Y : vector.Y );
		vector.SetY( ( vector.Y < min.Y ) ? min.Y : vector.Y );
		vector.SetZ( ( vector.Z > max.Z ) ? max.Z : vector.Z );
		vector.SetZ( ( vector.Z < min.Z ) ? min.Z : vector.Z );
		return vector;
	}

	Vector3 Vector3::Lerp( const Vector3& v1, const Vector3& v2, f32 w )
	{
		return Vector3(
			v1.X + ( ( v2.X - v1.X ) * w ),
			v1.Y + ( ( v2.Y - v1.Y ) * w ),
			v1.Z + ( ( v2.Z - v1.Z ) * w ) );
	}

	Vector3 Vector3::Barycentric( const Vector3& v1, const Vector3& v2, const Vector3& v3, f32 w1, f32 w2 )
	{
		return Vector3(
			( v1.X + ( w1 * ( v2.X - v1.X ) ) ) + ( w2 * ( v3.X - v1.X ) ),
			( v1.Y + ( w1 * ( v2.Y - v1.Y ) ) ) + ( w2 * ( v3.Y - v1.Y ) ),
			( v1.Z + ( w1 * ( v2.Z - v1.Z ) ) ) + ( w2 * ( v3.Z - v1.Z ) ) );
	}

	Vector3 Vector3::SmoothStep( const Vector3& v1, const Vector3& v2, f32 w )
	{
		w = ( w > 1 ) ? 1 : ( ( w < 0 ) ? 0 : w );
		w = ( w * w ) * ( static_cast<f32>( 3 ) - ( 2 * w ) );

		return Vector3(
			v1.X + ( ( v2.X - v1.X ) * w ),
			v1.Y + ( ( v2.Y - v1.Y ) * w ),
			v1.Z + ( ( v2.Z - v1.Z ) * w ) );
	}

	Vector3 Vector3::CatmullRom( const Vector3& v1, const Vector3& v2, const Vector3& v3, const Vector3& v4, f32 w )
	{
		f32 amountSqaured = w * w;
		f32 amountCubed = w * amountSqaured;

		return Vector3(
			static_cast<f32>( 0.5 ) * ( ( ( ( 2 * v2.X ) + ( ( -v1.X + v3.X ) * w ) ) + ( ( ( ( ( 2 * v1.X ) - ( static_cast<f32>( 5 ) * v2.X ) ) + ( static_cast<f32>( 4 ) * v3.X ) ) - v4.X ) * amountSqaured ) ) + ( ( ( ( -v1.X + ( static_cast<f32>( 3 ) * v2.X ) ) - ( static_cast<f32>( 3 ) * v3.X ) ) + v4.X ) * amountCubed ) ),
			static_cast<f32>( 0.5 ) * ( ( ( ( 2 * v2.Y ) + ( ( -v1.Y + v3.Y ) * w ) ) + ( ( ( ( ( 2 * v1.Y ) - ( static_cast<f32>( 5 ) * v2.Y ) ) + ( static_cast<f32>( 4 ) * v3.Y ) ) - v4.Y ) * amountSqaured ) ) + ( ( ( ( -v1.Y + ( static_cast<f32>( 3 ) * v2.Y ) ) - ( static_cast<f32>( 3 ) * v3.Y ) ) + v4.Y ) * amountCubed ) ),
			static_cast<f32>( 0.5 ) * ( ( ( ( 2 * v2.Z ) + ( ( -v1.Z + v3.Z ) * w ) ) + ( ( ( ( ( 2 * v1.Z ) - ( static_cast<f32>( 5 ) * v2.Z ) ) + ( static_cast<f32>( 4 ) * v3.Z ) ) - v4.Z ) * amountSqaured ) ) + ( ( ( ( -v1.Z + ( static_cast<f32>( 3 ) * v2.Z ) ) - ( static_cast<f32>( 3 ) * v3.Z ) ) + v4.Z ) * amountCubed ) ) );
	}

	Vector3 Vector3::Hermite( const Vector3& v1, const Vector3& tangent1, const Vector3& v2, const Vector3& tangent2, f32 w )
	{
		f32 amountSquared = w * w;
		f32 amountCubed = w * amountSquared;

		f32 s1 = ( ( 2 * amountCubed ) - ( static_cast<f32>( 3 ) * amountSquared ) ) + 1;
		f32 s2 = ( static_cast<f32>( -2 ) * amountCubed ) + ( static_cast<f32>( 3 ) * amountSquared );
		f32 s3 = ( amountCubed - ( 2 * amountSquared ) ) + w;
		f32 s4 = amountCubed - amountSquared;

		return Vector3(
			( ( ( v1.X * s1 ) + ( v2.X * s2 ) ) + ( tangent1.X * s3 ) ) + ( tangent2.X * s4 ),
			( ( ( v1.Y * s1 ) + ( v2.Y * s2 ) ) + ( tangent1.Y * s3 ) ) + ( tangent2.Y * s4 ),
			( ( ( v1.Z * s1 ) + ( v2.Z * s2 ) ) + ( tangent1.Z * s3 ) ) + ( tangent2.Z * s4 ) );
	}

	// Build an orthonormal basis from vector v1.
	// Assume that v1 is normalized.
	void Vector3::BuildOrthonormalBasis( const Vector3& v1, Vector3& v2, Vector3& v3 )
	{			
		if( ::abs( v1.X ) < ::abs( v1.Y ) )
		{
			if( ::abs( v1.X ) < ::abs( v1.Z ) )
			{
				v2.Set( 0, -v1.Z, v1.Y );
			}
			else
			{
				v2.Set( v1.Y, -v1.X, 0 );
			}
		}
		else
		{
			if( ::abs( v1.Y ) < ::abs( v1.Z ) )
			{
				v2.Set( v1.Z, 0, -v1.X );
			}
			else
			{
				v2.Set( v1.Y, -v1.X, 0 );
			}
		}

		v2.Normalize();

		v3 = Vector3::Cross( v1, v2 );
	}

	Vector3 Vector3::Zero()
	{
		return Vector3( 0, 0, 0 );
	}

	Vector3 Vector3::One()
	{
		return Vector3( 1, 1, 1 );
	}

	Vector3 Vector3::UnitX()
	{
		return Vector3( 1, 0, 0 );
	}

	Vector3 Vector3::UnitY()
	{
		return Vector3( 0, 1, 0 );
	}

	Vector3 Vector3::UnitZ()
	{
		return Vector3( 0, 0, 1 );
	}

}
