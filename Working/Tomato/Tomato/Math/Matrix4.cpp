#include "TomatoPCH.h"

#include "Matrix4.h"

#include <cmath>

namespace Tomato
{
	Matrix4::Matrix4()
		: Row1()
		, Row2()
		, Row3()
		, Row4()
	{
	}

	Matrix4::Matrix4(
		const Vector4& r1,
		const Vector4& r2,
		const Vector4& r3,
		const Vector4& r4 )
		: Row1(r1)
		, Row2(r2)
		, Row3(r3)
		, Row4(r4)
	{
	}

	Matrix4::Matrix4(
		f32 m00, f32 m01, f32 m02, f32 m03,
		f32 m10, f32 m11, f32 m12, f32 m13,
		f32 m20, f32 m21, f32 m22, f32 m23,
		f32 m30, f32 m31, f32 m32, f32 m33 )
	{
		Set( m00, m01, m02, m03,
			m10, m11, m12, m13,
			m20, m21, m22, m23,
			m30, m31, m32, m33 );
	}

	Matrix4::~Matrix4()
	{
	}

	void Matrix4::Set( 
		f32 m00, f32 m01, f32 m02, f32 m03,
		f32 m10, f32 m11, f32 m12, f32 m13,
		f32 m20, f32 m21, f32 m22, f32 m23,
		f32 m30, f32 m31, f32 m32, f32 m33 )
	{
		M[0][0] = m00;  M[0][1] = m01;  M[0][2] = m02;  M[0][3] = m03;
		M[1][0] = m10;  M[1][1] = m11;  M[1][2] = m12;  M[1][3] = m13;
		M[2][0] = m20;  M[2][1] = m21;  M[2][2] = m22;  M[2][3] = m23;
		M[3][0] = m30;  M[3][1] = m31;  M[3][2] = m32;  M[3][3] = m33;
	}

	void Matrix4::Set( const Matrix4& m )
	{
		M[0][0] = m.M[0][0];  M[0][1] = m.M[0][1];  M[0][2] = m.M[0][2];  M[0][3] = m.M[0][3];
		M[1][0] = m.M[1][0];  M[1][1] = m.M[1][1];  M[1][2] = m.M[1][2];  M[1][3] = m.M[1][3];
		M[2][0] = m.M[2][0];  M[2][1] = m.M[2][1];  M[2][2] = m.M[2][2];  M[2][3] = m.M[2][3];
		M[3][0] = m.M[3][0];  M[3][1] = m.M[3][1];  M[3][2] = m.M[3][2];  M[3][3] = m.M[3][3];
	}

	void Matrix4::SetIdentity() 
	{
		Set( 1.f, 0.f, 0.f, 0.f,
			0.f, 1.f, 0.f, 0.f,
			0.f, 0.f, 1.f, 0.f,
			0.f, 0.f, 0.f, 1.f );
	}

	Matrix4 Matrix4::CreateIdentity()
	{
		return Matrix4( 
			1.f, 0.f, 0.f, 0.f,
			0.f, 1.f, 0.f, 0.f,
			0.f, 0.f, 1.f, 0.f,
			0.f, 0.f, 0.f, 1.f );
	}

	void Matrix4::SetTranspose()
	{
		Set(
			M[0][0], M[1][0], M[2][0], M[3][0],
			M[0][1], M[1][1], M[2][1], M[3][1],
			M[0][2], M[1][2], M[2][2], M[3][2],
			M[0][3], M[1][3], M[2][3], M[3][3] );
	}

	Matrix4 Matrix4::CreateTranspose( const Matrix4& m )
	{
		return Matrix4(	
			m.M[0][0], m.M[1][0], m.M[2][0], m.M[3][0],
			m.M[0][1], m.M[1][1], m.M[2][1], m.M[3][1],
			m.M[0][2], m.M[1][2], m.M[2][2], m.M[3][2],
			m.M[0][3], m.M[1][3], m.M[2][3], m.M[3][3] );
	}

	Matrix4 Matrix4::GetTranspose() const
	{
		return Matrix4(	
			M[0][0], M[1][0], M[2][0], M[3][0],
			M[0][1], M[1][1], M[2][1], M[3][1],
			M[0][2], M[1][2], M[2][2], M[3][2],
			M[0][3], M[1][3], M[2][3], M[3][3] );
	}

	void Matrix4::SetScaling( const Vector3& v )
	{
		Set(
			v.X, 0, 0, 0,
			0, v.Y, 0, 0,
			0, 0, v.Z, 0,
			0, 0, 0, 1.0f );
	}

	void Matrix4::SetScaling( f32 x, f32 y, f32 z )
	{
		Set(
			x, 0, 0, 0,
			0, y, 0, 0,
			0, 0, z, 0,
			0, 0, 0, 1 );
	}

	Matrix4 Matrix4::CreateScaling( f32 x, f32 y, f32 z ) 
	{
		return Matrix4(   
			x, 0.f, 0.f, 0.f,
			0.f,   y, 0.f, 0.f,
			0.f, 0.f,   z, 0.f,
			0.f, 0.f, 0.f, 1.f );
	}

	void Matrix4::SetTranslation( const Vector3& v )
	{
		Set( 
			1.f, 0.f, 0.f, 0.f,
			0.f, 1.f, 0.f, 0.f,
			0.f, 0.f, 1.f, 0.f,
			v.X, v.Y, v.Z, 1.f );
	}

	void Matrix4::SetTranslation( f32 x, f32 y, f32 z ) 
	{
		Set( 
			1.f, 0.f, 0.f, 0.f,
			0.f, 1.f, 0.f, 0.f,
			0.f, 0.f, 1.f, 0.f,
			x, y, z, 1.f );
	}

	Matrix4 Matrix4::CreateTranslation( f32 x, f32 y, f32 z ) 
	{
		return Matrix4( 
			1.f, 0.f, 0.f, 0.f,
			0.f, 1.f, 0.f, 0.f,
			0.f, 0.f, 1.f, 0.f,
			x, y, z, 1.f );
	}

	Vector3 Matrix4::GetTranslation() const
	{
		return Vector3( M[3][0], M[3][1], M[3][2] );
	}

	void Matrix4::SetRotationX( f32 angle )
	{
		f32 sin = ::sinf( angle );
		f32 cos = ::cosf( angle );

		Set( 
			1.f,  0.f, 0.f, 0.f,
			0.f,  cos, sin, 0.f,
			0.f, -sin, cos, 0.f,
			0.f,  0.f, 0.f, 1.f );
	}

	void Matrix4::SetRotationY( f32 angle )
	{
		f32 sin = ::sinf( angle );
		f32 cos = ::cosf( angle );

		Set(
			cos, 0.f, -sin, 0.f,
			0.f, 1.f,  0.f,	0.f,
			sin, 0.f,  cos, 0.f,
			0.f, 0.f,  0.f, 1.0f );
	}

	void Matrix4::SetRotationZ( f32 angle )
	{
		f32 sin = ::sinf( angle );
		f32 cos = ::cosf( angle );

		Set( 
			cos, sin, 0.f, 0.f,
			-sin, cos, 0.f, 0.f,
			0.f, 0.f, 1.f, 0.f,
			0.f, 0.f, 0.f, 1.f );
	}

	Matrix4 Matrix4::CreateRotationX( f32 angle )
	{
		f32 sin = ::sinf( angle );
		f32 cos = ::cosf( angle );

		return Matrix4( 
			1.f,  0.f, 0.f, 0.f,
			0.f,  cos, sin, 0.f,
			0.f, -sin, cos, 0.f,
			0.f,  0.f, 0.f, 1.f );
	}

	Matrix4 Matrix4::CreateRotationY( f32 angle )
	{
		f32 sin = ::sinf( angle );
		f32 cos = ::cosf( angle );

		return Matrix4(
			cos, 0.f, -sin, 0.f,
			0.f, 1.f,  0.f,	0.f,
			sin, 0.f,  cos, 0.f,
			0.f, 0.f,  0.f, 1.0f );
	}

	Matrix4 Matrix4::CreateRotationZ( f32 angle )
	{
		f32 sin = ::sinf( angle );
		f32 cos = ::cosf( angle );

		return Matrix4( 
			cos, sin, 0.f, 0.f,
			-sin, cos, 0.f, 0.f,
			0.f, 0.f, 1.f, 0.f,
			0.f, 0.f, 0.f, 1.f );		
	}

	void Matrix4::SetFromAxisAngle( const Vector3& axis, f32 angle )
	{
		Assert( Math::CompareFloat( axis.GetLength(), 1.0f ) );

		f32 x = axis.X;
		f32 y = axis.Y;
		f32 z = axis.Z;
		f32 x2 = x * x;
		f32 y2 = y * y;
		f32 z2 = z * z;
		f32 xy = x * y;
		f32 xz = x * z;
		f32 yz = y * z;
		f32 sin = ::sinf( angle );
		f32 cos = ::cosf( angle );

		Set(
			x2 + ( cos * ( 1.f - x2 ) ), ( xy - ( cos * xy ) ) + ( sin * z ), ( xz - ( cos * xz ) ) - ( sin * y ), 0.f,
			( xy - ( cos * xy ) ) - ( sin * z ), y2 + ( cos * ( 1.f - y2 ) ), ( yz - ( cos * yz ) ) + ( sin * x ), 0.f,
			( xz - ( cos * xz ) ) + ( sin * y ), ( yz - ( cos * yz ) ) - ( sin * x ), z2 + ( cos * ( 1.f - z2 ) ), 0.f,
			0.f, 0.f, 0.f, 1.f );
	}

	void Matrix4::SetFromYawPitchRoll( f32 yaw, f32 pitch, f32 roll )
	{
		Quaternion quaternion;
		quaternion.SetFromYawPitchRoll( yaw, pitch, roll );

		SetFromQuaternion( quaternion );
	}

	void Matrix4::SetFromQuaternion( const Quaternion& q )
	{
		f32 xx = q.X * q.X;
		f32 yy = q.Y * q.Y;
		f32 zz = q.Z * q.Z;
		f32 xy = q.X * q.Y;
		f32 zw = q.Z * q.W;
		f32 zx = q.Z * q.X;
		f32 yw = q.Y * q.W;
		f32 yz = q.Y * q.Z;
		f32 xw = q.X * q.W;

		Set( 
			1.0f - ( 2.0f * ( yy + zz ) ), 2.0f * ( xy + zw ), 2.0f * ( zx - yw ), 0.0f,
			2.0f * ( xy - zw ), 1.0f - ( 2.0f * ( zz + xx ) ), 2.0f * ( yz + xw ), 0.0f,
			2.0f * ( zx + yw ), 2.0f * ( yz - xw ), 1.0f - ( 2.0f * ( yy + xx ) ), 0.0f,
			0.0f, 0.0f, 0.0f, 1.0f );
	}

	Matrix4 Matrix4::CreateFromAxisAngle( const Vector3& axis, f32 angle )
	{
#ifdef _DEBUG
		Assert( Math::CompareFloat( axis.GetLength(), 1.0f ) );
#endif

		f32 x = axis.X;
		f32 y = axis.Y;
		f32 z = axis.Z;
		f32 x2 = x * x;
		f32 y2 = y * y;
		f32 z2 = z * z;
		f32 xy = x * y;
		f32 xz = x * z;
		f32 yz = y * z;
		f32 sin = ::sinf( angle );
		f32 cos = ::cosf( angle );

		return Matrix4(
			x2 + ( cos * ( 1.f - x2 ) ), ( xy - ( cos * xy ) ) + ( sin * z ), ( xz - ( cos * xz ) ) - ( sin * y ), 0.f,
			( xy - ( cos * xy ) ) - ( sin * z ), y2 + ( cos * ( 1.f - y2 ) ), ( yz - ( cos * yz ) ) + ( sin * x ), 0.f,
			( xz - ( cos * xz ) ) + ( sin * y ), ( yz - ( cos * yz ) ) - ( sin * x ), z2 + ( cos * ( 1.f - z2 ) ), 0.f,
			0.f, 0.f, 0.f, 1.f );
	}

	Matrix4 Matrix4::CreateFromYawPitchRoll( f32 yaw, f32 pitch, f32 roll )
	{
		Quaternion quaternion;
		quaternion.SetFromYawPitchRoll( yaw, pitch, roll );

		return CreateFromQuaternion( quaternion );
	}

	Matrix4 Matrix4::CreateFromQuaternion( const Quaternion& q )
	{
		f32 xx = q.X * q.X;
		f32 yy = q.Y * q.Y;
		f32 zz = q.Z * q.Z;
		f32 xy = q.X * q.Y;
		f32 zw = q.Z * q.W;
		f32 zx = q.Z * q.X;
		f32 yw = q.Y * q.W;
		f32 yz = q.Y * q.Z;
		f32 xw = q.X * q.W;

		return Matrix4(
			1.0f - ( 2.0f * ( yy + zz ) ), 2.0f * ( xy + zw ), 2.0f * ( zx - yw ), 0.0f,
			2.0f * ( xy - zw ), 1.0f - ( 2.0f * ( zz + xx ) ), 2.0f * ( yz + xw ), 0.0f,
			2.0f * ( zx + yw ), 2.0f * ( yz - xw ), 1.0f - ( 2.0f * ( yy + xx ) ), 0.0f,
			0.0f, 0.0f, 0.0f, 1.0f );
	}

	void Matrix4::SetPerspectiveLH( f32 width, f32 height, f32 nearPlaneDistance, f32 farPlaneDistance )
	{
		Assert( nearPlaneDistance > 0.0f );
		Assert( farPlaneDistance > 0.0f );
		Assert( nearPlaneDistance < farPlaneDistance );

		Set(
			( 2.f * nearPlaneDistance ) / width, 0.f, 0.f, 0.f,
			0.f, ( 2.f * nearPlaneDistance ) / height, 0.f, 0.f,
			0.f, 0.f, farPlaneDistance / ( farPlaneDistance - nearPlaneDistance ), 1.f,
			0.f, 0.f, ( nearPlaneDistance * farPlaneDistance ) / ( nearPlaneDistance - farPlaneDistance ), 0.f );
	}

	Matrix4 Matrix4::CreatePerspectiveLH( f32 width, f32 height, f32 nearPlaneDistance, f32 farPlaneDistance )
	{
		Assert( nearPlaneDistance > 0.0f );
		Assert( farPlaneDistance > 0.0f );
		Assert( nearPlaneDistance < farPlaneDistance );

		return Matrix4(
			( 2.f * nearPlaneDistance ) / width, 0.f, 0.f, 0.f,
			0.f, ( 2.f * nearPlaneDistance ) / height, 0.f, 0.f,
			0.f, 0.f, farPlaneDistance / ( farPlaneDistance - nearPlaneDistance ), 1.f,
			0.f, 0.f, ( nearPlaneDistance * farPlaneDistance ) / ( nearPlaneDistance - farPlaneDistance ), 0.f );
	}

	void Matrix4::SetPerspectiveRH( f32 width, f32 height, f32 nearPlaneDistance, f32 farPlaneDistance )
	{
		Assert( nearPlaneDistance > 0.0f );
		Assert( farPlaneDistance > 0.0f );
		Assert( nearPlaneDistance < farPlaneDistance );

		Set(
			( 2.f * nearPlaneDistance ) / width, 0.f, 0.f, 0.f,
			0.f, ( 2.f * nearPlaneDistance ) / height, 0, 0, 
			0.f, 0.f, farPlaneDistance / ( nearPlaneDistance - farPlaneDistance ), -1.f,
			0.f, 0.f, ( nearPlaneDistance * farPlaneDistance ) / ( nearPlaneDistance - farPlaneDistance ), 0.f );
	}

	Matrix4 Matrix4::CreatePerspectiveRH( f32 width, f32 height, f32 nearPlaneDistance, f32 farPlaneDistance )
	{
		Assert( nearPlaneDistance > 0.0f );
		Assert( farPlaneDistance > 0.0f );
		Assert( nearPlaneDistance < farPlaneDistance );

		return Matrix4(
			( 2.f * nearPlaneDistance ) / width, 0.f, 0.f, 0.f,
			0.f, ( 2.f * nearPlaneDistance ) / height, 0, 0, 
			0.f, 0.f, farPlaneDistance / ( nearPlaneDistance - farPlaneDistance ), -1.f,
			0.f, 0.f, ( nearPlaneDistance * farPlaneDistance ) / ( nearPlaneDistance - farPlaneDistance ), 0.f );
	}

	void Matrix4::SetPerspectiveFovLH( f32 fov, f32 aspectRatio, f32 nearPlaneDistance, f32 farPlaneDistance )
	{
		Assert( fov > 0.0f );
		Assert( aspectRatio > 0.0f );
		Assert( nearPlaneDistance > 0.0f );
		Assert( farPlaneDistance > 0.0f );
		Assert( nearPlaneDistance < farPlaneDistance );

		f32 yScale = 1.0f / tanf( fov / 2.0f );
		f32 xScale = yScale / aspectRatio;

		Set(
			xScale, 0.f, 0.f, 0.f,
			0.f, yScale, 0.f, 0.f,
			0.f, 0.f, farPlaneDistance / ( farPlaneDistance - nearPlaneDistance ), 1.f,
			0.f, 0.f, -( nearPlaneDistance * farPlaneDistance ) / ( farPlaneDistance - nearPlaneDistance ), 0.f );
	}

	void Matrix4::SetPerspectiveFovRH( f32 fov, f32 aspectRatio, f32 nearPlaneDistance, f32 farPlaneDistance )
	{
		Assert( fov > 0.0f );
		Assert( aspectRatio > 0.0f );
		Assert( nearPlaneDistance > 0.0f );
		Assert( farPlaneDistance > 0.0f );
		Assert( nearPlaneDistance < farPlaneDistance );

		f32 yScale = 1.0f / tanf( fov / 2.0f );
		f32 xScale = yScale / aspectRatio;

		Set(
			xScale, 0.f, 0.f, 0.f,
			0.f, yScale, 0.f, 0.f,
			0.f, 0.f, farPlaneDistance / (  nearPlaneDistance - farPlaneDistance ), -1.f,
			0.f, 0.f, ( nearPlaneDistance * farPlaneDistance ) / ( nearPlaneDistance - farPlaneDistance ), 0.f );
	}

	void Matrix4::SetOrthographicLH( f32 width, f32 height, f32 zNearPlane, f32 zFarPlane )
	{
		Assert( zNearPlane < zFarPlane );

		Set( 
			2.f / width, 0.f, 0.f, 0.f,
			0.f, 2.f / height, 0.f, 0.f,
			0.f, 0.f, 1.f / ( zFarPlane - zNearPlane ), 0.f,
			0.f, 0.f, -zNearPlane / ( zFarPlane - zNearPlane ), 1.f );
	}

	void Matrix4::SetOrthographicRH( f32 width, f32 height, f32 zNearPlane, f32 zFarPlane )
	{
		Assert( zNearPlane < zFarPlane );

		Set(
			2.f / width, 0.f, 0.f, 0.f,
			0.f, 2.0f / height, 0.f, 0.f,
			0.f, 0.f, 1.f / ( zNearPlane - zFarPlane ), 0.f,
			0.f, 0.f, zNearPlane / ( zNearPlane - zFarPlane ), 1.f );
	}

	Matrix4 Matrix4::CreateOrthographicLH( f32 width, f32 height, f32 zNearPlane, f32 zFarPlane )
	{
		Assert( zNearPlane < zFarPlane );

		return Matrix4( 
			2.f / width, 0.f, 0.f, 0.f,
			0.f, 2.f / height, 0.f, 0.f,
			0.f, 0.f, 1.f / ( zFarPlane - zNearPlane ), 0.f,
			0.f, 0.f, -zNearPlane / ( zFarPlane - zNearPlane ), 1.f );
	}

	Matrix4 Matrix4::CreateOrthographicRH( f32 width, f32 height, f32 zNearPlane, f32 zFarPlane )
	{
		Assert( zNearPlane < zFarPlane );

		return Matrix4(
			2.f / width, 0.f, 0.f, 0.f,
			0.f, 2.0f / height, 0.f, 0.f,
			0.f, 0.f, 1.f / ( zNearPlane - zFarPlane ), 0.f,
			0.f, 0.f, zNearPlane / ( zNearPlane - zFarPlane ), 1.f );
	}

	void Matrix4::SetLookAtLH( const Vector3& cameraPosition, const Vector3& cameraTarget, const Vector3& cameraUpVector )
	{
		Vector3 zAxis = Vector3::Normalize( cameraTarget - cameraPosition );
		Vector3 xAxis = Vector3::Normalize( Vector3::Cross( cameraUpVector, zAxis ) );
		Vector3 yAxis = Vector3::Cross( zAxis, xAxis );

		Set( 
			xAxis.X, yAxis.X, zAxis.X, 0.f,
			xAxis.Y, yAxis.Y, zAxis.Y, 0.f,
			xAxis.Z, yAxis.Z, zAxis.Z, 0.f,
			-Vector3::Dot( xAxis, cameraPosition ), -Vector3::Dot( yAxis, cameraPosition ), -Vector3::Dot( zAxis, cameraPosition ), 1.f );
	}

	void Matrix4::SetLookAtRH( const Vector3& cameraPosition, const Vector3& cameraTarget, const Vector3& cameraUpVector )
	{
		Vector3 zAxis = Vector3::Normalize( cameraPosition - cameraTarget );
		Vector3 xAxis = Vector3::Normalize( Vector3::Cross( cameraUpVector, zAxis ) );
		Vector3 yAxis = Vector3::Cross( zAxis, xAxis );

		Set(
			xAxis.X, yAxis.X, zAxis.X, 0.f,
			xAxis.Y, yAxis.Y, zAxis.Y, 0.f,
			xAxis.Z, yAxis.Z, zAxis.Z, 0.f,
			-Vector3::Dot( xAxis, cameraPosition ), -Vector3::Dot( yAxis, cameraPosition ), -Vector3::Dot( zAxis, cameraPosition ), 1.f );
	}

	Matrix4 Matrix4::CreateLookAtLH( const Vector3& cameraPosition, const Vector3& cameraTarget, const Vector3& cameraUpVector )
	{
		Vector3 zAxis = Vector3::Normalize( cameraTarget - cameraPosition );
		Vector3 xAxis = Vector3::Normalize( Vector3::Cross( cameraUpVector, zAxis ) );
		Vector3 yAxis = Vector3::Cross( zAxis, xAxis );

		return Matrix4( 
			xAxis.X, yAxis.X, zAxis.X, 0.f,
			xAxis.Y, yAxis.Y, zAxis.Y, 0.f,
			xAxis.Z, yAxis.Z, zAxis.Z, 0.f,
			-Vector3::Dot( xAxis, cameraPosition ), -Vector3::Dot( yAxis, cameraPosition ), -Vector3::Dot( zAxis, cameraPosition ), 1.f );
	}

	Matrix4 Matrix4::CreateLookAtRH( const Vector3& cameraPosition, const Vector3& cameraTarget, const Vector3& cameraUpVector )
	{
		Vector3 zAxis = Vector3::Normalize( cameraPosition - cameraTarget );
		Vector3 xAxis = Vector3::Normalize( Vector3::Cross( cameraUpVector, zAxis ) );
		Vector3 yAxis = Vector3::Cross( zAxis, xAxis );

		return Matrix4(
			xAxis.X, yAxis.X, zAxis.X, 0.f,
			xAxis.Y, yAxis.Y, zAxis.Y, 0.f,
			xAxis.Z, yAxis.Z, zAxis.Z, 0.f,
			-Vector3::Dot( xAxis, cameraPosition ), -Vector3::Dot( yAxis, cameraPosition ), -Vector3::Dot( zAxis, cameraPosition ), 1.f );
	}

	void Matrix4::SetInverse()
	{
		f32 s3344 = ( M[2][2] * M[3][3] ) - ( M[2][3] * M[3][2] );
		f32 s3244 = ( M[2][1] * M[3][3] ) - ( M[2][3] * M[3][1] );
		f32 s3243 = ( M[2][1] * M[3][2] ) - ( M[2][2] * M[3][1] );
		f32 s3144 = ( M[2][0] * M[3][3] ) - ( M[2][3] * M[3][0] );
		f32 s3143 = ( M[2][0] * M[3][2] ) - ( M[2][2] * M[3][0] );
		f32 s3142 = ( M[2][0] * M[3][1] ) - ( M[2][1] * M[3][0] );

		f32 s2344 = ( M[1][2] * M[3][3] ) - ( M[1][3] * M[3][2] );
		f32 s2244 = ( M[1][1] * M[3][3] ) - ( M[1][3] * M[3][1] );
		f32 s2243 = ( M[1][1] * M[3][2] ) - ( M[1][2] * M[3][1] );
		f32 s2144 = ( M[1][0] * M[3][3] ) - ( M[1][3] * M[3][0] );
		f32 s2143 = ( M[1][0] * M[3][2] ) - ( M[1][2] * M[3][0] );
		f32 s2142 = ( M[1][0] * M[3][1] ) - ( M[1][1] * M[3][0] );

		f32 s2343 = ( M[1][2] * M[2][3] ) - ( M[1][3] * M[2][2] );
		f32 s2234 = ( M[1][1] * M[2][3] ) - ( M[1][3] * M[2][1] );
		f32 s2233 = ( M[1][1] * M[2][2] ) - ( M[1][2] * M[2][1] );
		f32 s2134 = ( M[1][0] * M[2][3] ) - ( M[1][3] * M[2][0] );
		f32 s2133 = ( M[1][0] * M[2][2] ) - ( M[1][2] * M[2][0] );
		f32 s2132 = ( M[1][0] * M[2][1] ) - ( M[1][1] * M[2][0] );

		f32 d11 =  (( M[1][1] * s3344 ) - ( M[1][2] * s3244 )) + ( M[1][3] * s3243 );
		f32 d22 = -(((M[1][0] * s3344 ) - ( M[1][2] * s3144 )) + ( M[1][3] * s3143 ));
		f32 d33 =  (( M[1][0] * s3244 ) - ( M[1][1] * s3144 )) + ( M[1][3] * s3142 );
		f32 d44 = -(((M[1][0] * s3243 ) - ( M[1][1] * s3143 )) + ( M[1][2] * s3142 ));

		f32 det = 1.f / ( ( ( ( M[0][0] * d11 ) + ( M[0][1] * d22 ) ) + ( M[0][2] * d33 ) ) + ( M[0][3] * d44 ) );

		Set(
			d11 * det,
			-((( M[0][1] * s3344 ) - ( M[0][2] * s3244 )) + ( M[0][3] * s3243 )) * det,
			((( M[0][1] * s2344 ) - ( M[0][2] * s2244 )) + ( M[0][3] * s2243 )) * det,
			-((( M[0][1] * s2343 ) - ( M[0][2] * s2234 )) + ( M[0][3] * s2233 )) * det,
			d22 * det,
			((( M[0][0] * s3344 ) - ( M[0][2] * s3144 )) + ( M[0][3] * s3143 )) * det,
			-((( M[0][0] * s2344 ) - ( M[0][2] * s2144 )) + ( M[0][3] * s2143 )) * det,
			((( M[0][0] * s2343 ) - ( M[0][2] * s2134 )) + ( M[0][3] * s2133 )) * det,
			d33 * det,
			-((( M[0][0] * s3244 ) - ( M[0][1] * s3144 )) + ( M[0][3] * s3142 )) * det,
			((( M[0][0] * s2244 ) - ( M[0][1] * s2144 )) + ( M[0][3] * s2142 )) * det,
			-((( M[0][0] * s2234 ) - ( M[0][1] * s2134 )) + ( M[0][3] * s2132 )) * det,
			d44 * det,
			((( M[0][0] * s3243 ) - ( M[0][1] * s3143 )) + ( M[0][2] * s3142 )) * det,
			-((( M[0][0] * s2243 ) - ( M[0][1] * s2143 )) + ( M[0][2] * s2142 )) * det,
			((( M[0][0] * s2233 ) - ( M[0][1] * s2133 )) + ( M[0][2] * s2132 )) * det );
	}

	Matrix4 Matrix4::GetInverse() const
	{
		f32 s3344 = ( M[2][2] * M[3][3] ) - ( M[2][3] * M[3][2] );
		f32 s3244 = ( M[2][1] * M[3][3] ) - ( M[2][3] * M[3][1] );
		f32 s3243 = ( M[2][1] * M[3][2] ) - ( M[2][2] * M[3][1] );
		f32 s3144 = ( M[2][0] * M[3][3] ) - ( M[2][3] * M[3][0] );
		f32 s3143 = ( M[2][0] * M[3][2] ) - ( M[2][2] * M[3][0] );
		f32 s3142 = ( M[2][0] * M[3][1] ) - ( M[2][1] * M[3][0] );

		f32 s2344 = ( M[1][2] * M[3][3] ) - ( M[1][3] * M[3][2] );
		f32 s2244 = ( M[1][1] * M[3][3] ) - ( M[1][3] * M[3][1] );
		f32 s2243 = ( M[1][1] * M[3][2] ) - ( M[1][2] * M[3][1] );
		f32 s2144 = ( M[1][0] * M[3][3] ) - ( M[1][3] * M[3][0] );
		f32 s2143 = ( M[1][0] * M[3][2] ) - ( M[1][2] * M[3][0] );
		f32 s2142 = ( M[1][0] * M[3][1] ) - ( M[1][1] * M[3][0] );

		f32 s2343 = ( M[1][2] * M[2][3] ) - ( M[1][3] * M[2][2] );
		f32 s2234 = ( M[1][1] * M[2][3] ) - ( M[1][3] * M[2][1] );
		f32 s2233 = ( M[1][1] * M[2][2] ) - ( M[1][2] * M[2][1] );
		f32 s2134 = ( M[1][0] * M[2][3] ) - ( M[1][3] * M[2][0] );
		f32 s2133 = ( M[1][0] * M[2][2] ) - ( M[1][2] * M[2][0] );
		f32 s2132 = ( M[1][0] * M[2][1] ) - ( M[1][1] * M[2][0] );

		f32 d11 =  (( M[1][1] * s3344 ) - ( M[1][2] * s3244 )) + ( M[1][3] * s3243 );
		f32 d22 = -(((M[1][0] * s3344 ) - ( M[1][2] * s3144 )) + ( M[1][3] * s3143 ));
		f32 d33 =  (( M[1][0] * s3244 ) - ( M[1][1] * s3144 )) + ( M[1][3] * s3142 );
		f32 d44 = -(((M[1][0] * s3243 ) - ( M[1][1] * s3143 )) + ( M[1][2] * s3142 ));

		f32 det = 1.f / ( ( ( ( M[0][0] * d11 ) + ( M[0][1] * d22 ) ) + ( M[0][2] * d33 ) ) + ( M[0][3] * d44 ) );

		return Matrix4(
			d11 * det,
			-((( M[0][1] * s3344 ) - ( M[0][2] * s3244 )) + ( M[0][3] * s3243 )) * det,
			((( M[0][1] * s2344 ) - ( M[0][2] * s2244 )) + ( M[0][3] * s2243 )) * det,
			-((( M[0][1] * s2343 ) - ( M[0][2] * s2234 )) + ( M[0][3] * s2233 )) * det,
			d22 * det,
			((( M[0][0] * s3344 ) - ( M[0][2] * s3144 )) + ( M[0][3] * s3143 )) * det,
			-((( M[0][0] * s2344 ) - ( M[0][2] * s2144 )) + ( M[0][3] * s2143 )) * det,
			((( M[0][0] * s2343 ) - ( M[0][2] * s2134 )) + ( M[0][3] * s2133 )) * det,
			d33 * det,
			-((( M[0][0] * s3244 ) - ( M[0][1] * s3144 )) + ( M[0][3] * s3142 )) * det,
			((( M[0][0] * s2244 ) - ( M[0][1] * s2144 )) + ( M[0][3] * s2142 )) * det,
			-((( M[0][0] * s2234 ) - ( M[0][1] * s2134 )) + ( M[0][3] * s2132 )) * det,
			d44 * det,
			((( M[0][0] * s3243 ) - ( M[0][1] * s3143 )) + ( M[0][2] * s3142 )) * det,
			-((( M[0][0] * s2243 ) - ( M[0][1] * s2143 )) + ( M[0][2] * s2142 )) * det,
			((( M[0][0] * s2233 ) - ( M[0][1] * s2133 )) + ( M[0][2] * s2132 )) * det );
	}

	Matrix4 Matrix4::CreateInverse( const Matrix4& m )
	{
		f32 s3344 = ( m.M[2][2] * m.M[3][3] ) - ( m.M[2][3] * m.M[3][2] );
		f32 s3244 = ( m.M[2][1] * m.M[3][3] ) - ( m.M[2][3] * m.M[3][1] );
		f32 s3243 = ( m.M[2][1] * m.M[3][2] ) - ( m.M[2][2] * m.M[3][1] );
		f32 s3144 = ( m.M[2][0] * m.M[3][3] ) - ( m.M[2][3] * m.M[3][0] );
		f32 s3143 = ( m.M[2][0] * m.M[3][2] ) - ( m.M[2][2] * m.M[3][0] );
		f32 s3142 = ( m.M[2][0] * m.M[3][1] ) - ( m.M[2][1] * m.M[3][0] );

		f32 s2344 = ( m.M[1][2] * m.M[3][3] ) - ( m.M[1][3] * m.M[3][2] );
		f32 s2244 = ( m.M[1][1] * m.M[3][3] ) - ( m.M[1][3] * m.M[3][1] );
		f32 s2243 = ( m.M[1][1] * m.M[3][2] ) - ( m.M[1][2] * m.M[3][1] );
		f32 s2144 = ( m.M[1][0] * m.M[3][3] ) - ( m.M[1][3] * m.M[3][0] );
		f32 s2143 = ( m.M[1][0] * m.M[3][2] ) - ( m.M[1][2] * m.M[3][0] );
		f32 s2142 = ( m.M[1][0] * m.M[3][1] ) - ( m.M[1][1] * m.M[3][0] );

		f32 s2343 = ( m.M[1][2] * m.M[2][3] ) - ( m.M[1][3] * m.M[2][2] );
		f32 s2234 = ( m.M[1][1] * m.M[2][3] ) - ( m.M[1][3] * m.M[2][1] );
		f32 s2233 = ( m.M[1][1] * m.M[2][2] ) - ( m.M[1][2] * m.M[2][1] );
		f32 s2134 = ( m.M[1][0] * m.M[2][3] ) - ( m.M[1][3] * m.M[2][0] );
		f32 s2133 = ( m.M[1][0] * m.M[2][2] ) - ( m.M[1][2] * m.M[2][0] );
		f32 s2132 = ( m.M[1][0] * m.M[2][1] ) - ( m.M[1][1] * m.M[2][0] );

		f32 d11 =  (( m.M[1][1] * s3344 ) - ( m.M[1][2] * s3244 )) + ( m.M[1][3] * s3243 );
		f32 d22 = -(((m.M[1][0] * s3344 ) - ( m.M[1][2] * s3144 )) + ( m.M[1][3] * s3143 ));
		f32 d33 =  (( m.M[1][0] * s3244 ) - ( m.M[1][1] * s3144 )) + ( m.M[1][3] * s3142 );
		f32 d44 = -(((m.M[1][0] * s3243 ) - ( m.M[1][1] * s3143 )) + ( m.M[1][2] * s3142 ));

		f32 det = 1.f / ( ( ( ( m.M[0][0] * d11 ) + ( m.M[0][1] * d22 ) ) + ( m.M[0][2] * d33 ) ) + ( m.M[0][3] * d44 ) );

		return Matrix4(
			d11 * det,
			-((( m.M[0][1] * s3344 ) - ( m.M[0][2] * s3244 )) + ( m.M[0][3] * s3243 )) * det,
			((( m.M[0][1] * s2344 ) - ( m.M[0][2] * s2244 )) + ( m.M[0][3] * s2243 )) * det,
			-((( m.M[0][1] * s2343 ) - ( m.M[0][2] * s2234 )) + ( m.M[0][3] * s2233 )) * det,
			d22 * det,
			((( m.M[0][0] * s3344 ) - ( m.M[0][2] * s3144 )) + ( m.M[0][3] * s3143 )) * det,
			-((( m.M[0][0] * s2344 ) - ( m.M[0][2] * s2144 )) + ( m.M[0][3] * s2143 )) * det,
			((( m.M[0][0] * s2343 ) - ( m.M[0][2] * s2134 )) + ( m.M[0][3] * s2133 )) * det,
			d33 * det,
			-((( m.M[0][0] * s3244 ) - ( m.M[0][1] * s3144 )) + ( m.M[0][3] * s3142 )) * det,
			((( m.M[0][0] * s2244 ) - ( m.M[0][1] * s2144 )) + ( m.M[0][3] * s2142 )) * det,
			-((( m.M[0][0] * s2234 ) - ( m.M[0][1] * s2134 )) + ( m.M[0][3] * s2132 )) * det,
			d44 * det,
			((( m.M[0][0] * s3243 ) - ( m.M[0][1] * s3143 )) + ( m.M[0][2] * s3142 )) * det,
			-((( m.M[0][0] * s2243 ) - ( m.M[0][1] * s2143 )) + ( m.M[0][2] * s2142 )) * det,
			((( m.M[0][0] * s2233 ) - ( m.M[0][1] * s2133 )) + ( m.M[0][2] * s2132 )) * det );
	}

	Vector3 Matrix4::GetRightVector() const
	{
		return Vector3::Normalize( Vector3( Row1[0], Row2[0], Row3[0] ) );
	}

	Vector3 Matrix4::GetUpVector() const
	{
		return Vector3::Normalize( Vector3( Row1[1], Row2[1], Row3[1] ) );
	}

	Vector3 Matrix4::GetForwardVector() const
	{
		return Vector3::Normalize( Vector3( Row1[2], Row2[2], Row3[2] ) );
	}

	const Matrix4& Matrix4::operator = ( const Matrix4& m )
	{
		Set( m );
		return *this;
	}

	Matrix4 Matrix4::operator +	( const Matrix4& m ) const
	{
		return Matrix4( 
			M[0][0] + m.M[0][0], M[0][1] + m.M[0][1], M[0][2] + m.M[0][2], M[0][3] + m.M[0][3],
			M[1][0] + m.M[1][0], M[1][1] + m.M[1][1], M[1][2] + m.M[1][2], M[1][3] + m.M[1][3],
			M[2][0] + m.M[2][0], M[2][1] + m.M[2][1], M[2][2] + m.M[2][2], M[2][3] + m.M[2][3],
			M[3][0] + m.M[3][0], M[3][1] + m.M[3][1], M[3][2] + m.M[3][2], M[3][3] + m.M[3][3] );
	}

	Matrix4 Matrix4::operator -	( const Matrix4& m ) const
	{
		return Matrix4( 
			M[0][0] - m.M[0][0], M[0][1] - m.M[0][1], M[0][2] - m.M[0][2], M[0][3] - m.M[0][3],
			M[1][0] - m.M[1][0], M[1][1] - m.M[1][1], M[1][2] - m.M[1][2], M[1][3] - m.M[1][3],
			M[2][0] - m.M[2][0], M[2][1] - m.M[2][1], M[2][2] - m.M[2][2], M[2][3] - m.M[2][3],
			M[3][0] - m.M[3][0], M[3][1] - m.M[3][1], M[3][2] - m.M[3][2], M[3][3] - m.M[3][3] );
	}

	Matrix4 Matrix4::operator / ( const Matrix4& m ) const
	{
		return Matrix4(
			M[0][0] / m.M[0][0], M[0][1] / m.M[0][1], M[0][2] / m.M[0][2], M[0][3] / m.M[0][3],
			M[1][0] / m.M[1][0], M[1][1] / m.M[1][1], M[1][2] / m.M[1][2], M[1][3] / m.M[1][3],
			M[2][0] / m.M[2][0], M[2][1] / m.M[2][1], M[2][2] / m.M[2][2], M[2][3] / m.M[2][3],
			M[3][0] / m.M[3][0], M[3][1] / m.M[3][1], M[3][2] / m.M[3][2], M[3][3] / m.M[3][3] );
	}

	Matrix4 Matrix4::operator *	( const Matrix4& m ) const
	{
		return Matrix4(
			M[0][0] * m.M[0][0] + M[0][1] * m.M[1][0] + M[0][2] * m.M[2][0] + M[0][3] * m.M[3][0],
			M[0][0] * m.M[0][1] + M[0][1] * m.M[1][1] + M[0][2] * m.M[2][1] + M[0][3] * m.M[3][1],
			M[0][0] * m.M[0][2] + M[0][1] * m.M[1][2] + M[0][2] * m.M[2][2] + M[0][3] * m.M[3][2],
			M[0][0] * m.M[0][3] + M[0][1] * m.M[1][3] + M[0][2] * m.M[2][3] + M[0][3] * m.M[3][3],

			M[1][0] * m.M[0][0] + M[1][1] * m.M[1][0] + M[1][2] * m.M[2][0] + M[1][3] * m.M[3][0],
			M[1][0] * m.M[0][1] + M[1][1] * m.M[1][1] + M[1][2] * m.M[2][1] + M[1][3] * m.M[3][1],
			M[1][0] * m.M[0][2] + M[1][1] * m.M[1][2] + M[1][2] * m.M[2][2] + M[1][3] * m.M[3][2],
			M[1][0] * m.M[0][3] + M[1][1] * m.M[1][3] + M[1][2] * m.M[2][3] + M[1][3] * m.M[3][3],

			M[2][0] * m.M[0][0] + M[2][1] * m.M[1][0] + M[2][2] * m.M[2][0] + M[2][3] * m.M[3][0],
			M[2][0] * m.M[0][1] + M[2][1] * m.M[1][1] + M[2][2] * m.M[2][1] + M[2][3] * m.M[3][1],
			M[2][0] * m.M[0][2] + M[2][1] * m.M[1][2] + M[2][2] * m.M[2][2] + M[2][3] * m.M[3][2],
			M[2][0] * m.M[0][3] + M[2][1] * m.M[1][3] + M[2][2] * m.M[2][3] + M[2][3] * m.M[3][3],

			M[3][0] * m.M[0][0] + M[3][1] * m.M[1][0] + M[3][2] * m.M[2][0] + M[3][3] * m.M[3][0],
			M[3][0] * m.M[0][1] + M[3][1] * m.M[1][1] + M[3][2] * m.M[2][1] + M[3][3] * m.M[3][1],
			M[3][0] * m.M[0][2] + M[3][1] * m.M[1][2] + M[3][2] * m.M[2][2] + M[3][3] * m.M[3][2],
			M[3][0] * m.M[0][3] + M[3][1] * m.M[1][3] + M[3][2] * m.M[2][3] + M[3][3] * m.M[3][3] );
	}

	void Matrix4::operator *= ( const Matrix4& m )
	{
		Set(
			M[0][0] * m.M[0][0] + M[0][1] * m.M[1][0] + M[0][2] * m.M[2][0] + M[0][3] * m.M[3][0],
			M[0][0] * m.M[0][1] + M[0][1] * m.M[1][1] + M[0][2] * m.M[2][1] + M[0][3] * m.M[3][1],
			M[0][0] * m.M[0][2] + M[0][1] * m.M[1][2] + M[0][2] * m.M[2][2] + M[0][3] * m.M[3][2],
			M[0][0] * m.M[0][3] + M[0][1] * m.M[1][3] + M[0][2] * m.M[2][3] + M[0][3] * m.M[3][3],

			M[1][0] * m.M[0][0] + M[1][1] * m.M[1][0] + M[1][2] * m.M[2][0] + M[1][3] * m.M[3][0],
			M[1][0] * m.M[0][1] + M[1][1] * m.M[1][1] + M[1][2] * m.M[2][1] + M[1][3] * m.M[3][1],
			M[1][0] * m.M[0][2] + M[1][1] * m.M[1][2] + M[1][2] * m.M[2][2] + M[1][3] * m.M[3][2],
			M[1][0] * m.M[0][3] + M[1][1] * m.M[1][3] + M[1][2] * m.M[2][3] + M[1][3] * m.M[3][3],

			M[2][0] * m.M[0][0] + M[2][1] * m.M[1][0] + M[2][2] * m.M[2][0] + M[2][3] * m.M[3][0],
			M[2][0] * m.M[0][1] + M[2][1] * m.M[1][1] + M[2][2] * m.M[2][1] + M[2][3] * m.M[3][1],
			M[2][0] * m.M[0][2] + M[2][1] * m.M[1][2] + M[2][2] * m.M[2][2] + M[2][3] * m.M[3][2],
			M[2][0] * m.M[0][3] + M[2][1] * m.M[1][3] + M[2][2] * m.M[2][3] + M[2][3] * m.M[3][3],

			M[3][0] * m.M[0][0] + M[3][1] * m.M[1][0] + M[3][2] * m.M[2][0] + M[3][3] * m.M[3][0],
			M[3][0] * m.M[0][1] + M[3][1] * m.M[1][1] + M[3][2] * m.M[2][1] + M[3][3] * m.M[3][1],
			M[3][0] * m.M[0][2] + M[3][1] * m.M[1][2] + M[3][2] * m.M[2][2] + M[3][3] * m.M[3][2],
			M[3][0] * m.M[0][3] + M[3][1] * m.M[1][3] + M[3][2] * m.M[2][3] + M[3][3] * m.M[3][3] );
	}

	Vector3 Matrix4::operator * ( const Vector3& v ) const
	{
		f32 fInvW = 1.f / ( M[3][0] * v.X + M[3][1] * v.Y + M[3][2] * v.Z + M[3][3] );

		return Vector3( 
			( M[0][0] * v.X + M[0][1] * v.Y + M[0][2] * v.Z + M[0][3] ) * fInvW,
			( M[1][0] * v.X + M[1][1] * v.Y + M[1][2] * v.Z + M[1][3] ) * fInvW,
			( M[2][0] * v.X + M[2][1] * v.Y + M[2][2] * v.Z + M[2][3] ) * fInvW );
	}

	Vector4 Matrix4::operator * ( const Vector4& v ) const
	{
		return Vector4(
			M[0][0] * v.X + M[0][1] * v.Y + M[0][2] * v.Z + M[0][3] * v.W, 
			M[1][0] * v.X + M[1][1] * v.Y + M[1][2] * v.Z + M[1][3] * v.W,
			M[2][0] * v.X + M[2][1] * v.Y + M[2][2] * v.Z + M[2][3] * v.W,
			M[3][0] * v.X + M[3][1] * v.Y + M[3][2] * v.Z + M[3][3] * v.W );
	}

	bool Matrix4::operator == ( const Matrix4& m ) const
	{
		if( M[0][0] != m.M[0][0] || M[0][1] != m.M[0][1] || M[0][2] != m.M[0][2] || M[0][3] != m.M[0][3] ||
			M[1][0] != m.M[1][0] || M[1][1] != m.M[1][1] || M[1][2] != m.M[1][2] || M[1][3] != m.M[1][3] ||
			M[2][0] != m.M[2][0] || M[2][1] != m.M[2][1] || M[2][2] != m.M[2][2] || M[2][3] != m.M[2][3] ||
			M[3][0] != m.M[3][0] || M[3][1] != m.M[3][1] || M[3][2] != m.M[3][2] || M[3][3] != m.M[3][3] )
		{
			return false;
		}

		return true;
	}

	bool Matrix4::operator != ( const Matrix4& m ) const
	{
		if( M[0][0] != m.M[0][0] || M[0][1] != m.M[0][1] || M[0][2] != m.M[0][2] || M[0][3] != m.M[0][3] ||
			M[1][0] != m.M[1][0] || M[1][1] != m.M[1][1] || M[1][2] != m.M[1][2] || M[1][3] != m.M[1][3] ||
			M[2][0] != m.M[2][0] || M[2][1] != m.M[2][1] || M[2][2] != m.M[2][2] || M[2][3] != m.M[2][3] ||
			M[3][0] != m.M[3][0] || M[3][1] != m.M[3][1] || M[3][2] != m.M[3][2] || M[3][3] != m.M[3][3] )
		{
			return true;
		}

		return false;
	}

	Vector3 Matrix4::Transform( const Matrix4& m, const Vector3& v )
	{
		Vector3 vector;
		vector.X = ( ( ( v.X * m.M[0][0] ) + ( v.Y * m.M[1][0] ) ) + ( v.Z * m.M[2][0] ) ) + m.M[3][0];
		vector.Y = ( ( ( v.X * m.M[0][1] ) + ( v.Y * m.M[1][1] ) ) + ( v.Z * m.M[2][1] ) ) + m.M[3][1];
		vector.Z = ( ( ( v.X * m.M[0][2] ) + ( v.Y * m.M[1][2] ) ) + ( v.Z * m.M[2][2] ) ) + m.M[3][2];

		return vector;
	}

	void Matrix4::Transform( const Matrix4& m, const Vector3& v, Vector3& result )
	{
		result.X = ( ( ( v.X * m.M[0][0] ) + ( v.Y * m.M[1][0] ) ) + ( v.Z * m.M[2][0] ) ) + m.M[3][0];
		result.Y = ( ( ( v.X * m.M[0][1] ) + ( v.Y * m.M[1][1] ) ) + ( v.Z * m.M[2][1] ) ) + m.M[3][1];
		result.Z = ( ( ( v.X * m.M[0][2] ) + ( v.Y * m.M[1][2] ) ) + ( v.Z * m.M[2][2] ) ) + m.M[3][2];
	}

	Vector3 Matrix4::TransformNormal( const Matrix4& m, const Vector3& v )
	{
		Vector3 vector;
		vector.X = ( ( v.X * m.M[0][0] ) + ( v.Y * m.M[1][0] ) ) + ( v.Z * m.M[2][0] );
		vector.Y = ( ( v.X * m.M[0][1] ) + ( v.Y * m.M[1][1] ) ) + ( v.Z * m.M[2][1] );
		vector.Z = ( ( v.X * m.M[0][2] ) + ( v.Y * m.M[1][2] ) ) + ( v.Z * m.M[2][2] );

		return vector;
	}

	void Matrix4::TransformNormal( const Matrix4& m, const Vector3& v, Vector3& result )
	{
		result.X = ( ( v.X * m.M[0][0] ) + ( v.Y * m.M[1][0] ) ) + ( v.Z * m.M[2][0] );
		result.Y = ( ( v.X * m.M[0][1] ) + ( v.Y * m.M[1][1] ) ) + ( v.Z * m.M[2][1] );
		result.Z = ( ( v.X * m.M[0][2] ) + ( v.Y * m.M[1][2] ) ) + ( v.Z * m.M[2][2] );
	}

	void Matrix4::Transform( const Matrix4& m, const Vector4& v, Vector4& result )
	{
		result.X = ( ( ( v.X * m.M[0][0] ) + ( v.Y * m.M[1][0] ) ) + ( v.Z * m.M[2][0] ) ) + ( v.W * m.M[3][0] );
		result.Y = ( ( ( v.X * m.M[0][1] ) + ( v.Y * m.M[1][1] ) ) + ( v.Z * m.M[2][1] ) ) + ( v.W * m.M[3][1] );
		result.Z = ( ( ( v.X * m.M[0][2] ) + ( v.Y * m.M[1][2] ) ) + ( v.Z * m.M[2][2] ) ) + ( v.W * m.M[3][2] );
		result.W = ( ( ( v.X * m.M[0][3] ) + ( v.Y * m.M[1][3] ) ) + ( v.Z * m.M[2][3] ) ) + ( v.W * m.M[3][3] );
	}

	Vector4 Matrix4::Transform( const Matrix4& m, const Vector4& v )
	{
		return Vector4(
			( ( ( v.X * m.M[0][0] ) + ( v.Y * m.M[1][0] ) ) + ( v.Z * m.M[2][0] ) ) + ( v.W * m.M[3][0] ),
			( ( ( v.X * m.M[0][1] ) + ( v.Y * m.M[1][1] ) ) + ( v.Z * m.M[2][1] ) ) + ( v.W * m.M[3][1] ),
			( ( ( v.X * m.M[0][2] ) + ( v.Y * m.M[1][2] ) ) + ( v.Z * m.M[2][2] ) ) + ( v.W * m.M[3][2] ),
			( ( ( v.X * m.M[0][3] ) + ( v.Y * m.M[1][3] ) ) + ( v.Z * m.M[2][3] ) ) + ( v.W * m.M[3][3] ) );
	}


}