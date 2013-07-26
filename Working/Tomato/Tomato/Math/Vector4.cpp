#include "TomatoPCH.h"

#include "Vector4.h"

#include <cmath>

namespace Tomato
{	
	Vector4::Vector4()
		: X( 0 )
		, Y( 0 )
		, Z( 0 )
		, W( 0 )
	{
	}

	Vector4::Vector4( f32 x, f32 y, f32 z, f32 w )
		: X( x )
		, Y( y )
		, Z( z )
		, W( w )
	{
	}

	Vector4::Vector4( const Vector3& v, f32 w )
		: X( v.X )
		, Y( v.Y )
		, Z( v.Z )
		, W( w )
	{
	}

	Vector4::~Vector4()
	{
	}

	f32& Vector4::operator[] ( s32 index )
	{
		Assert( index >= 0 );
		Assert( index < 4 );
		return V[ index ];
	}

	const f32& Vector4::operator[] ( s32 index ) const
	{
		Assert( index >= 0 );
		Assert( index < 4 );
		return V[ index ];
	}

	void Vector4::SetX( f32 x )
	{
		X = x;
	}

	void Vector4::SetY( f32 y )
	{
		Y = y;
	}

	void Vector4::SetZ( f32 z )
	{
		Z = z;
	}

	void Vector4::SetW( f32 w )
	{
		W = w;
	}

	void Vector4::Set( f32 x, f32 y, f32 z, f32 w )
	{
		SetX( x );
		SetY( y );
		SetZ( z );
		SetW( w );
	}

	Vector4& Vector4::operator = ( const Vector4& v )
	{
		Set( v.X, v.Y, v.Z, v.W );
		return *this;
	}

	Vector4& Vector4::operator = ( const Vector3& v )
	{
		Set( v.X, v.Y, v.Z, 0 );
		return *this;
	}

	Vector4& Vector4::operator += ( const Vector4& v )
	{
		X += v.X;
		Y += v.Y;
		Z += v.Z;
		W += v.W;

		return *this;
	}

	Vector4& Vector4::operator -= ( const Vector4& v )
	{
		X -= v.X;
		Y -= v.Y;
		Z -= v.Z;
		W -= v.W;

		return *this;
	}

	Vector4& Vector4::operator *= ( const Vector4& v )
	{
		X *= v.X;
		Y *= v.Y;
		Z *= v.Z;
		W *= v.W;

		return *this;
	}

	Vector4& Vector4::operator /= ( const Vector4& v )
	{
		X /= v.X;
		Y /= v.Y;
		Z /= v.Z;
		W /= v.W;

		return *this;
	}

	Vector4& Vector4::operator *= ( f32 scalar )
	{
		X *= scalar;
		Y *= scalar;
		Z *= scalar;
		W *= scalar;

		return *this;
	}

	Vector4& Vector4::operator /= ( f32 scalar )
	{
		Assert( scalar != 0 );

		f32 inv = 1 / scalar;

		X *= inv;
		Y *= inv;
		Z *= inv;
		W *= inv;

		return *this;
	}

	bool Vector4::operator == ( const Vector4& v ) const
	{
		return ( v.X == X && v.Y == Y && v.Z == Z && v.W == W );
	}

	bool Vector4::operator != ( const Vector4& v ) const
	{
		return !( (*this) == v );
	}

	Vector4 Vector4::operator + ( const Vector4& v ) const
	{
		Vector4 result = *this;
		result += v;
		return result;
	}

	Vector4 Vector4::operator - ( const Vector4& v ) const
	{
		Vector4 result = *this;
		result -= v;
		return result;
	}

	Vector4 Vector4::operator * ( const Vector4& v ) const
	{
		Vector4 result = *this;
		result *= v;
		return result;
	}

	Vector4 Vector4::operator / ( const Vector4& v ) const
	{
		Vector4 result = *this;
		result /= v;
		return result;
	}

	Vector4 Vector4::operator * ( f32 scalar ) const
	{
		Vector4 result = *this;
		result *= scalar;
		return result;
	}

	Vector4 Vector4::operator / ( f32 scalar ) const
	{
		Vector4 result = *this;
		result /= scalar;
		return result;
	}

	const Vector4& Vector4::operator + () const
	{
		return *this;
	}

	Vector4 Vector4::operator - () const
	{
		return (*this) * -1;
	}

	bool Vector4::operator < ( const Vector4& v ) const
	{
		return ( X < v.X && Y < v.Y && Z < v.Z && W < v.W );
	}

	bool Vector4::operator > ( const Vector4& v ) const
	{
		return ( X > v.X && Y > v.Y && Z > v.Z && W > v.W );
	}

	void Vector4::SetZero()
	{
		Set( 0, 0, 0, 0 );
	}

	f32 Vector4::GetLength() const
	{
		return ::sqrtf( GetLengthSquared() );
	}

	f32 Vector4::GetLengthSquared() const
	{
		return ( X * X + Y * Y + Z * Z + W * W );
	}

	void Vector4::Normalize()
	{
		f32 length = GetLength();

		if( length != 0 )
		{
			f32 invLength = 1 / length;
			X *= invLength;
			Y *= invLength;
			Z *= invLength;
			W *= invLength;
		}
	}

	f32 Vector4::GetDistance( const Vector4& v1, const Vector4& v2 )
	{
		return ( v1 - v2 ).GetLength();
	}

	f32 Vector4::GetDistanceSquared( const Vector4& v1, const Vector4& v2 )
	{
		return ( v1 - v2 ).GetLengthSquared();
	}

	f32 Vector4::Dot( const Vector4& v1, const Vector4& v2 )
	{
		return ( v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z + v1.W * v2.W );
	}

	Vector4 Vector4::Normalize( const Vector4& v )
	{
		Vector4 vector = v;
		vector.Normalize();
		return vector;
	}

	Vector4 Vector4::Min( const Vector4& v1, const Vector4& v2 )
	{
		return Vector4(
			Math::Min( v1.X, v2.X ),
			Math::Min( v1.Y, v2.Y ),
			Math::Min( v1.Z, v2.Z ),
			Math::Min( v1.W, v2.W ) );
	}

	Vector4 Vector4::Max( const Vector4& v1, const Vector4& v2 )
	{
		return Vector4(
			Math::Max( v1.X, v2.X ),
			Math::Max( v1.Y, v2.Y ),
			Math::Max( v1.Z, v2.Z ),
			Math::Max( v1.W, v2.W ) );
	}

	Vector4 Vector4::Clamp( const Vector4& v, const Vector4& min, const Vector4& max  )
	{
		Vector4 vector = v;
		vector.SetX( ( vector.X > max.X ) ? max.X : vector.X );
		vector.SetX( ( vector.X < min.X ) ? min.X : vector.X );
		vector.SetY( ( vector.Y > max.Y ) ? max.Y : vector.Y );
		vector.SetY( ( vector.Y < min.Y ) ? min.Y : vector.Y );
		vector.SetZ( ( vector.Z > max.Z ) ? max.Z : vector.Z );
		vector.SetZ( ( vector.Z < min.Z ) ? min.Z : vector.Z );
		vector.SetW( ( vector.W > max.W ) ? max.W : vector.W );
		vector.SetW( ( vector.W < min.W ) ? min.W : vector.W );
		return vector;
	}

	Vector4 Vector4::Lerp( const Vector4& v1, const Vector4& v2, f32 w )
	{
		return Vector4(
			v1.X + ( ( v2.X - v1.X ) * w ),
			v1.Y + ( ( v2.Y - v1.Y ) * w ),
			v1.Z + ( ( v2.Z - v1.Z ) * w ),
			v1.W + ( ( v2.W - v1.W ) * w ) );
	}

	Vector4 Vector4::Barycentric( const Vector4& v1, const Vector4& v2, const Vector4& v3, f32 w1, f32 w2 )
	{
		return Vector4(
			( v1.X + ( w1 * ( v2.X - v1.X ) ) ) + ( w2 * ( v3.X - v1.X ) ),
			( v1.Y + ( w1 * ( v2.Y - v1.Y ) ) ) + ( w2 * ( v3.Y - v1.Y ) ),
			( v1.Z + ( w1 * ( v2.Z - v1.Z ) ) ) + ( w2 * ( v3.Z - v1.Z ) ),
			( v1.W + ( w1 * ( v2.W - v1.W ) ) ) + ( w2 * ( v3.W - v1.W ) ) );
	}

	Vector4 Vector4::SmoothStep( const Vector4& v1, const Vector4& v2, f32 w )
	{
		w = ( w > 1 ) ? 1 : ( ( w < 0 ) ? 0 : w );
		w = ( w * w ) * ( static_cast<f32>( 3 ) - ( static_cast<f32>( 2 ) * w ) );

		return Vector4(
			v1.X + ( ( v2.X - v1.X ) * w ),
			v1.Y + ( ( v2.Y - v1.Y ) * w ),
			v1.Z + ( ( v2.Z - v1.Z ) * w ),
			v1.W + ( ( v2.W - v1.W ) * w ) );
	}

	Vector4 Vector4::CatmullRom( const Vector4& v1, const Vector4& v2, const Vector4& v3, const Vector4& v4, f32 w )
	{
		f32 amountSqaured = w * w;
		f32 amountCubed = w * amountSqaured;

		return Vector4(
			static_cast<f32>( 0.5 ) * ( ( ( ( static_cast<f32>( 2 ) * v2.X ) + ( ( -v1.X + v3.X ) * w ) ) + ( ( ( ( ( static_cast<f32>( 2 ) * v1.X ) - ( static_cast<f32>( 5 ) * v2.X ) ) + ( static_cast<f32>( 4 ) * v3.X ) ) - v4.X ) * amountSqaured ) ) + ( ( ( ( -v1.X + ( static_cast<f32>( 3 ) * v2.X ) ) - ( static_cast<f32>( 3 ) * v3.X ) ) + v4.X ) * amountCubed ) ),
			static_cast<f32>( 0.5 ) * ( ( ( ( static_cast<f32>( 2 ) * v2.Y ) + ( ( -v1.Y + v3.Y ) * w ) ) + ( ( ( ( ( static_cast<f32>( 2 ) * v1.Y ) - ( static_cast<f32>( 5 ) * v2.Y ) ) + ( static_cast<f32>( 4 ) * v3.Y ) ) - v4.Y ) * amountSqaured ) ) + ( ( ( ( -v1.Y + ( static_cast<f32>( 3 ) * v2.Y ) ) - ( static_cast<f32>( 3 ) * v3.Y ) ) + v4.Y ) * amountCubed ) ),
			static_cast<f32>( 0.5 ) * ( ( ( ( static_cast<f32>( 2 ) * v2.Z ) + ( ( -v1.Z + v3.Z ) * w ) ) + ( ( ( ( ( static_cast<f32>( 2 ) * v1.Z ) - ( static_cast<f32>( 5 ) * v2.Z ) ) + ( static_cast<f32>( 4 ) * v3.Z ) ) - v4.Z ) * amountSqaured ) ) + ( ( ( ( -v1.Z + ( static_cast<f32>( 3 ) * v2.Z ) ) - ( static_cast<f32>( 3 ) * v3.Z ) ) + v4.Z ) * amountCubed ) ),
			static_cast<f32>( 0.5 ) * ( ( ( ( static_cast<f32>( 2 ) * v2.W ) + ( ( -v1.W + v3.W ) * w ) ) + ( ( ( ( ( static_cast<f32>( 2 ) * v1.W ) - ( static_cast<f32>( 5 ) * v2.W ) ) + ( static_cast<f32>( 4 ) * v3.W ) ) - v4.W ) * amountSqaured ) ) + ( ( ( ( -v1.W + ( static_cast<f32>( 3 ) * v2.W ) ) - ( static_cast<f32>( 3 ) * v3.W ) ) + v4.W ) * amountCubed ) ) );
	}

	Vector4 Vector4::Hermite( const Vector4& v1, const Vector4& tangent1, const Vector4& v2, const Vector4& tangent2, f32 w )
	{
		f32 amountSquared = w * w;
		f32 amountCubed = w * amountSquared;

		f32 s1 = ( ( static_cast<f32>( 2 ) * amountCubed ) - ( static_cast<f32>( 3 ) * amountSquared ) ) + 1;
		f32 s2 = ( static_cast<f32>( -2 ) * amountCubed ) + ( static_cast<f32>( 3 ) * amountSquared );
		f32 s3 = ( amountCubed - ( static_cast<f32>( 2 ) * amountSquared ) ) + w;
		f32 s4 = amountCubed - amountSquared;

		return Vector4(
			( ( ( v1.X * s1 ) + ( v2.X * s2 ) ) + ( tangent1.X * s3 ) ) + ( tangent2.X * s4 ),
			( ( ( v1.Y * s1 ) + ( v2.Y * s2 ) ) + ( tangent1.Y * s3 ) ) + ( tangent2.Y * s4 ),
			( ( ( v1.Z * s1 ) + ( v2.Z * s2 ) ) + ( tangent1.Z * s3 ) ) + ( tangent2.Z * s4 ),
			( ( ( v1.W * s1 ) + ( v2.W * s2 ) ) + ( tangent1.W * s3 ) ) + ( tangent2.W * s4 ) );
	}		

	Vector4 Vector4::Zero()
	{
		return Vector4( 0, 0, 0, 0 );
	}

	Vector4 Vector4::One()
	{
		return Vector4( 1, 1, 1, 1 );
	}

	Vector4 Vector4::UnitX()
	{
		return Vector4( 1, 0, 0, 0 );
	}

	Vector4 Vector4::UnitY()
	{
		return Vector4( 0, 1, 0, 0 );
	}

	Vector4 Vector4::UnitZ()
	{
		return Vector4( 0, 0, 1, 0 );
	}

	Vector4 Vector4::UnitW()
	{
		return Vector4( 0, 0, 0, 1 );
	}

}