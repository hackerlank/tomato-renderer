#pragma once

namespace Tomato
{
	class Quaternion;

	class TOMATO_API Matrix4
	{
	public:
		Matrix4();
		Matrix4(
			const Vector4& r1,
			const Vector4& r2,
			const Vector4& r3,
			const Vector4& r4 );
		Matrix4(
			f32 m00, f32 m01, f32 m02, f32 m03,
			f32 m10, f32 m11, f32 m12, f32 m13,
			f32 m20, f32 m21, f32 m22, f32 m23,
			f32 m30, f32 m31, f32 m32, f32 m33 );

		~Matrix4();

	public:
		// Set
		void Set( 
			f32 m00, f32 m01, f32 m02, f32 m03,
			f32 m10, f32 m11, f32 m12, f32 m13,
			f32 m20, f32 m21, f32 m22, f32 m23,
			f32 m30, f32 m31, f32 m32, f32 m33 );
		void Set( const Matrix4& m );

		// Identity
		void SetIdentity();
		static Matrix4 CreateIdentity();

		// Transpose
		void SetTranspose();
		static Matrix4 CreateTranspose( const Matrix4& m );
		Matrix4 GetTranspose() const;

		// Scaling
		void SetScaling( const Vector3& v );
		void SetScaling( f32 x,f32 y,f32 z );
		static Matrix4 CreateScaling( f32 x, f32 y, f32 z );

		// Translatation
		void SetTranslation( const Vector3& v );
		void SetTranslation( f32 x, f32 y, f32 z );
		static Matrix4 CreateTranslation( f32 x, f32 y, f32 z );
		Vector3 GetTranslation() const;

		// Rotation
		void SetRotationX( f32 angle );
		void SetRotationY( f32 angle );
		void SetRotationZ( f32 angle );
		static Matrix4 CreateRotationX( f32 angle );
		static Matrix4 CreateRotationY( f32 angle );
		static Matrix4 CreateRotationZ( f32 angle );

		// Rotation 
		void SetFromAxisAngle( const Vector3& axis, f32 angle );
		void SetFromYawPitchRoll( f32 yaw, f32 pitch, f32 roll );
		void SetFromQuaternion( const Quaternion& q );
		static Matrix4 CreateFromAxisAngle( const Vector3& axis, f32 angle );
		static Matrix4 CreateFromYawPitchRoll( f32 yaw, f32 pitch, f32 roll );
		static Matrix4 CreateFromQuaternion( const Quaternion& q );

		// Perspective
		void SetPerspectiveLH( f32 width, f32 height, f32 nearPlaneDistance, f32 farPlaneDistance );
		void  SetPerspectiveRH( f32 width, f32 height, f32 nearPlaneDistance, f32 farPlaneDistance );
		static Matrix4 CreatePerspectiveLH( f32 width, f32 height, f32 nearPlaneDistance, f32 farPlaneDistance );
		static Matrix4 CreatePerspectiveRH( f32 width, f32 height, f32 nearPlaneDistance, f32 farPlaneDistance );
		void SetPerspectiveFovLH( f32 fov, f32 aspectRatio, f32 nearPlaneDistance, f32 farPlaneDistance );
		void SetPerspectiveFovRH( f32 fov, f32 aspectRatio, f32 nearPlaneDistance, f32 farPlaneDistance );

		// Orthographic
		void SetOrthographicLH( f32 width, f32 height, f32 zNearPlane, f32 zFarPlane );
		void SetOrthographicRH( f32 width, f32 height, f32 zNearPlane, f32 zFarPlane );
		static Matrix4 CreateOrthographicLH( f32 width, f32 height, f32 zNearPlane, f32 zFarPlane );
		static Matrix4 CreateOrthographicRH( f32 width, f32 height, f32 zNearPlane, f32 zFarPlane );

		// LookAt
		void SetLookAtLH( const Vector3& cameraPosition, const Vector3& cameraTarget, const Vector3& cameraUpVector );
		void SetLookAtRH( const Vector3& cameraPosition, const Vector3& cameraTarget, const Vector3& cameraUpVector );
		static Matrix4 CreateLookAtLH( const Vector3& cameraPosition, const Vector3& cameraTarget, const Vector3& cameraUpVector );
		static Matrix4 CreateLookAtRH( const Vector3& cameraPosition, const Vector3& cameraTarget, const Vector3& cameraUpVector );

		// Inverse
		void SetInverse();
		Matrix4 GetInverse() const;
		static Matrix4 CreateInverse( const Matrix4& m );

		// View
		Vector3 GetRightVector() const;
		Vector3 GetUpVector() const;
		Vector3 GetForwardVector() const;

		// Transformation
		static void Transform( const Matrix4& m, const Vector3& v, Vector3& result );
		static Vector3 Transform( const Matrix4& m, const Vector3& v );
		static void TransformNormal( const Matrix4& m, const Vector3& v, Vector3& result );
		static Vector3 TransformNormal( const Matrix4& m, const Vector3& v );
		static void Transform( const Matrix4& m, const Vector4& v, Vector4& result );
		static Vector4 Transform( const Matrix4& m, const Vector4& v );

		// Operators
		const Matrix4& operator = ( const Matrix4& m );
		Matrix4 operator +	(const Matrix4& m) const;
		Matrix4 operator -	( const Matrix4& m) const;
		Matrix4 operator / ( const Matrix4& m ) const;
		Matrix4 operator *	( const Matrix4& m ) const;
		void operator *= ( const Matrix4& m );
		Vector3 operator * ( const Vector3& v ) const;
		Vector4 operator * ( const Vector4& v) const;
		bool operator == ( const Matrix4& m ) const;
		bool operator != ( const Matrix4& m ) const;

	public:
#pragma warning( disable:4201 )

		union
		{
			struct  
			{
				Vector4 Row1;
				Vector4 Row2;
				Vector4 Row3;
				Vector4 Row4;
			};

			f32 E[16];
			f32 M[4][4];
		};
	};

#pragma warning( default: 4201 )

}