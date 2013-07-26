#include "TomatoPCH.h"

#include "Quaternion.h"

#include <cmath>

namespace Tomato
{
	const Quaternion Quaternion::Identity( 0.0f, 0.0f, 0.0f, 1.0f );
	const f32 Quaternion::Epsilon = 1e-08f;

	Quaternion::Quaternion() 
		: X( 0.f )
		, Y( 0.f )
		, Z( 0.f )
		, W( 0.f )
	{   
	}

	Quaternion::Quaternion( f32 x, f32 y, f32 z, f32 w ) 
		: X( x )
		, Y( y )
		, Z( z )
		, W( w )
	{
	}

	Quaternion::Quaternion( const Quaternion& q )
		: X( q.X )
		, Y( q.Y )
		, Z( q.Z )
		, W( q.W )
	{
	}

	Quaternion::Quaternion( const Vector3 &axis, f32 fAngle )
	{
		SetFromAngleAxis( axis, fAngle );
	}

	void Quaternion::Set( f32 x, f32 y, f32 z , f32 w )
	{
		X = x;
		Y = y;
		Z = z;
		W = w;
	}	

	Quaternion Quaternion::operator - () const
	{
		return (*this) * -1;
	}

	Quaternion Quaternion::operator + ( const Quaternion& q ) const
	{
		return Quaternion( X + q.X, Y + q.Y, Z + q.Z, W + q.W );
	}

	Quaternion Quaternion::operator - ( const Quaternion& q ) const
	{
		return Quaternion( X - q.X, Y - q.Y, Z - q.Z, W - q.W );
	}

	Quaternion Quaternion::operator * ( const Quaternion& q ) const
	{
		return Quaternion( 
			W * q.W - X * q.X - Y * q.Y - Z * q.Z,
			W * q.X + X * q.W + Y * q.Z - Z * q.Y,
			W * q.Y + Y * q.W + Z * q.X - X * q.Z,
			W * q.Z + Z * q.W + X * q.Y - Y * q.X );
	}

	Quaternion Quaternion::operator * ( f32 scalar ) const
	{
		return Quaternion( X * scalar, Y * scalar, Z * scalar, W * scalar );
	}

	bool Quaternion::operator == ( const Quaternion& q ) const
	{
		return (W == q.W && X == q.X && Y == q.Y && Z == q.Z);
	}

	bool Quaternion::operator != ( const Quaternion& q ) const
	{
		return (W != q.W || X != q.X || Y != q.Y || Z != q.Z);
	}

	f32 Quaternion::GetLengthSquared() const
	{
		return ( ( X * X ) + ( Y * Y ) + ( Z * Z ) + ( W * W ) );			
	}

	f32 Quaternion::GetLength() const
	{
		return ::sqrtf( GetLengthSquared() );			
	}

	f32 Quaternion::Dot(const Quaternion& p, const Quaternion& q)
	{
		return ( ( p.X * q.X ) + ( p.Y * q.Y ) + ( p.Z * q.Z ) + ( p.W * q.W ) );	
	}

	Quaternion Quaternion::Conjugate( const Quaternion& p )
	{
		return Quaternion( -p.X, -p.Y, -p.Z, p.W );
	}

	Quaternion Quaternion::Inverse( const Quaternion& q )
	{
		f32 s = 1.0f / q.GetLengthSquared();

		return Quaternion( -q.X * s, -q.Y * s, -q.Z * s, q.W * s );
	}

	void Quaternion::SetInverse( const Quaternion& q )
	{
		f32 s = 1.0f / q.GetLengthSquared();

		X = -q.X * s;
		Y = -q.Y * s;
		Z = -q.Z * s;
		W =  q.W * s;
	}

	void Quaternion::GetAngleAxis( f32& angle, Vector3& axis ) const
	{
		f32 length = GetLength();

		if ( length < Epsilon )
		{
			angle = 0.0f;
			axis.X = 0.0f;
			axis.Y = 0.0f;
			axis.Z = 0.0f;
		}
		else
		{
			angle = 2.0f * ::acosf( W );
			f32 invLength = 1.0f/length;
			axis.X = X * invLength;
			axis.Y = Y * invLength;
			axis.Z = Z * invLength;
		}
	}

	void Quaternion::SetFromAngleAxis( const Vector3& axis, f32 angle )
	{
		f32 fHalfAngle = angle * 0.5f;
		f32 sn = ::sinf( fHalfAngle );
		Set( axis.X * sn, axis.Y * sn,axis.Z * sn, ::cosf( fHalfAngle ) );
	}

	void Quaternion::SetFromAngleAxisX( f32 fAngle )
	{
		f32 fHalfAngle = fAngle * 0.5f;
		Set( ::sinf( fHalfAngle ), 0.0f, 0.0f, ::cosf( fHalfAngle ) );
	}

	void Quaternion::SetFromAngleAxisY( f32 fAngle )
	{
		f32 fHalfAngle = fAngle * 0.5f;
		Set( 0.0f, ::sinf( fHalfAngle ), 0.0f, ::cosf( fHalfAngle ) );
	}

	void Quaternion::SetFromAngleAxisZ( f32 fAngle )
	{
		f32 fHalfAngle = fAngle * 0.5f;
		Set( 0.0f, 0.0f, ::sinf( fHalfAngle ), ::cosf( fHalfAngle ) );
	}

	void Quaternion::SetFromAngleAxesXYZ( f32 fAngleX, f32 fAngleY, f32 fAngleZ )
	{
		f32 fsin0, fcos0, fsin1, fcos1, fsin2, fcos2;
		fsin0 = ::sinf( fAngleX * 0.5f );
		fcos0 = ::cosf( fAngleX * 0.5f );

		fsin1 = ::sinf( fAngleY * 0.5f );
		fcos1 = ::cosf( fAngleY * 0.5f );

		fsin2 = ::sinf( fAngleZ * 0.5f );
		fcos2 = ::cosf( fAngleZ * 0.5f );

		Set( fcos2 * fcos1 * fsin0 - fsin2 * fsin1 * fcos0,
			fcos2 * fsin1 * fcos0 + fsin2 * fcos1 * fsin0,
			-fcos2 * fsin1 * fsin0 + fsin2 * fcos1 * fcos0,
			fcos2 * fcos1 * fcos0 + fsin2 * fsin1 * fsin0 );
	}

	Quaternion Quaternion::CreateFromYawPitchRoll( f32 yaw, f32 pitch, f32 roll )
	{
		f32 r = roll * 0.5f;
		f32 sinr = ::sinf( r );
		f32 cosr = ::cosf( r );
		f32 p = pitch * 0.5f;
		f32 sinp = ::sinf( p );
		f32 cosp = ::cosf( p );
		f32 y = yaw * 0.5f;
		f32 siny = ::sinf( y );
		f32 cosy = ::cosf( y );

		return Quaternion(
			( ( cosy * sinp ) * cosr ) + ( ( siny * cosp ) * sinr ),
			( ( siny * cosp ) * cosr ) - ( ( cosy * sinp ) * sinr ),
			( ( cosy * cosp ) * sinr ) - ( ( siny * sinp ) * cosr ),
			( ( cosy * cosp ) * cosr ) + ( ( siny * sinp ) * sinr ) );
	}

	void Quaternion::SetFromYawPitchRoll( f32 yaw, f32 pitch, f32 roll )
	{
		f32 r = roll * 0.5f;
		f32 sinr = ::sinf( r );
		f32 cosr = ::cosf( r );
		f32 p = pitch * 0.5f;
		f32 sinp = ::sinf( p );
		f32 cosp = ::cosf( p );
		f32 y = yaw * 0.5f;
		f32 siny = ::sinf( y );
		f32 cosy = ::cosf( y );

		Set(( ( cosy * sinp ) * cosr ) + ( ( siny * cosp ) * sinr ),
			( ( siny * cosp ) * cosr ) - ( ( cosy * sinp ) * sinr ),
			( ( cosy * cosp ) * sinr ) - ( ( siny * sinp ) * cosr ),
			( ( cosy * cosp ) * cosr ) + ( ( siny * sinp ) * sinr ) );
	}


	Quaternion Quaternion::CreateFromRotationMatrix( const Matrix4& mat )
	{
		f32 d = mat.M[0][0] + mat.M[1][1] + mat.M[2][2];

		if( d > 0.f )
		{
			f32 s = ::sqrtf( d + 1.0f );
			f32 s2 = 0.5f / s;

			return Quaternion(
				( mat.M[1][2] - mat.M[2][1] ) * s2,
				( mat.M[2][0] - mat.M[0][2] ) * s2,
				( mat.M[0][1] - mat.M[1][0] ) * s2,
				0.5f * s );
		}
		else if( ( mat.M[0][0] >= mat.M[1][1] ) && ( mat.M[0][0] >= mat.M[2][2] ) )
		{
			f32 s = ::sqrtf(( ( 1.0f + mat.M[0][0] ) - mat.M[1][1] ) - mat.M[2][2] );
			f32 s2 = 0.5f / s;

			return Quaternion(
				0.5f * s,
				( mat.M[0][1] + mat.M[1][0] ) * s2,
				( mat.M[0][2] + mat.M[2][0] ) * s2,
				( mat.M[1][2] - mat.M[2][1] ) * s2 );

		}
		else if( mat.M[1][1] > mat.M[2][2] )
		{
			f32 s = ::sqrtf( ( ( 1.0f + mat.M[1][1] ) - mat.M[0][0] ) - mat.M[2][2] );
			f32 s2 = 0.5f / s;

			return Quaternion(
				( mat.M[1][0] + mat.M[0][1] ) * s2,
				0.5f * s,
				( mat.M[2][1] + mat.M[1][2] ) * s2,
				( mat.M[2][0] - mat.M[0][2] ) * s2 );
		}
		else
		{
			f32 s = ::sqrtf( ( ( 1.0f + mat.M[2][2] ) - mat.M[0][0] ) - mat.M[1][1] );
			f32 s2 = 0.5f / s;

			return Quaternion(
				( mat.M[2][0] + mat.M[0][2] ) * s2,
				( mat.M[2][1] + mat.M[1][2] ) * s2,
				0.5f * s,
				( mat.M[0][1] - mat.M[1][0] ) * s2 );
		}
	}	

	void Quaternion::SetFromRotationMatrix( const Matrix4& mat )
	{
		f32 d = mat.M[0][0] + mat.M[1][1] + mat.M[2][2];

		if( d > 0.f )
		{
			f32 s = ::sqrtf( d + 1.0f );
			f32 s2 = 0.5f / s;

			Set(
				( mat.M[1][2] - mat.M[2][1] ) * s2,
				( mat.M[2][0] - mat.M[0][2] ) * s2,
				( mat.M[0][1] - mat.M[1][0] ) * s2,
				0.5f * s );
		}
		else if( ( mat.M[0][0] >= mat.M[1][1] ) && ( mat.M[0][0] >= mat.M[2][2] ) )
		{
			f32 s = ::sqrtf(( ( 1.0f + mat.M[0][0] ) - mat.M[1][1] ) - mat.M[2][2] );
			f32 s2 = 0.5f / s;

			Set(
				0.5f * s,
				( mat.M[0][1] + mat.M[1][0] ) * s2,
				( mat.M[0][2] + mat.M[2][0] ) * s2,
				( mat.M[1][2] - mat.M[2][1] ) * s2 );

		}
		else if( mat.M[1][1] > mat.M[2][2] )
		{
			f32 s = ::sqrtf( ( ( 1.0f + mat.M[1][1] ) - mat.M[0][0] ) - mat.M[2][2] );
			f32 s2 = 0.5f / s;

			Set(
				( mat.M[1][0] + mat.M[0][1] ) * s2,
				0.5f * s,
				( mat.M[2][1] + mat.M[1][2] ) * s2,
				( mat.M[2][0] - mat.M[0][2] ) * s2 );
		}
		else
		{
			f32 s = ::sqrtf( ( ( 1.0f + mat.M[2][2] ) - mat.M[0][0] ) - mat.M[1][1] );
			f32 s2 = 0.5f / s;

			Set(
				( mat.M[2][0] + mat.M[0][2] ) * s2,
				( mat.M[2][1] + mat.M[1][2] ) * s2,
				0.5f * s,
				( mat.M[0][1] - mat.M[1][0] ) * s2 );
		}
	}

	

	Quaternion Quaternion::Slerp( const Quaternion& q1, const Quaternion& q2, f32 w )
	{
		f32 w1, w2;
		f32 dot = Quaternion::Dot(q1,q2);

		bool bNegative = false;
		if( dot < 0.f )
		{
			bNegative = true;
			dot = -dot;
		}

		if( dot > 0.999999f )
		{
			w1 = 1.0f - w;
			w2 = bNegative ? -w : w;
		}
		else
		{
			f32 acos = ::acosf( dot );
			f32 invSin = ( 1.0f / ::sinf( acos ) );
			w1 = ( ::sinf( ( ( 1.0f - w ) * acos ) ) ) * invSin;
			w2 = bNegative 
				? ( ( -::sinf( w * acos ) ) * invSin ) 
				: ( (  ::sinf( w * acos ) ) * invSin );
		}

		return Quaternion( 
			( q1.X * w1 ) + ( q2.X * w2 ),
			( q1.Y * w1 ) + ( q2.Y * w2 ),
			( q1.Z * w1 ) + ( q2.Z * w2 ),
			( q1.W * w1 ) + ( q2.W * w2 ) );
	}

	Quaternion Quaternion::Lerp(const Quaternion& q1,const Quaternion& q2, f32 w )
	{
		Quaternion quaternion;

		f32 invW = 1.0f - w;
		f32 dot = Quaternion::Dot(q1,q2);
		if( dot >= 0.f )
		{
			quaternion = Quaternion(
				(q1.X * invW) + ( w * q2.X ),
				(q1.Y * invW) + ( w * q2.Y ),
				(q1.Z * invW) + ( w * q2.Z ),
				(q1.W * invW) + ( w * q2.W ) );
		}
		else
		{
			quaternion = Quaternion(
				(q1.X * invW) - ( w * q2.X ),
				(q1.Y * invW) - ( w * q2.Y ),
				(q1.Z * invW) - ( w * q2.Z ),
				(q1.W * invW) - ( w * q2.W ) );
		}

		f32 s = 1.0f / quaternion.GetLength();
		quaternion.X *= s;
		quaternion.Y *= s;
		quaternion.Z *= s;
		quaternion.W *= s;
		return quaternion;
	}

	void Quaternion::SetNegate()
	{
		Set( -X, -Y, -Z, -W );
	}

	void Quaternion::Snap()
	{			
		if( fabs( X ) <= Epsilon && X != 0.0f)
		{
			X = 0.0f;
		}

		if( fabs( Y ) <= Epsilon  && Y != 0.0f)
		{
			Y = 0.0f;
		}

		if( fabs( Z ) <= Epsilon && Z != 0.0f)
		{
			Z = 0.0f;
		}

		if( fabs( W ) <= Epsilon && W != 0.0f)
		{
			W = 0.0f;
		}
	}

	void Quaternion::Normalize()
	{			
		f32 fInvLength = 1.0f / GetLength();
		X *= fInvLength;
		Y *= fInvLength;
		Z *= fInvLength;
		W *= fInvLength;
	}

	Quaternion operator * ( f32 scalar, const Quaternion& q )
	{
		return ( q * scalar );
	}
}

