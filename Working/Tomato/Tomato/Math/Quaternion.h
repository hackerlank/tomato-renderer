#pragma once

namespace Tomato
{	
	class TOMATO_API Quaternion
	{
	public:
		Quaternion();
		Quaternion( f32 x, f32 y, f32 z, f32 w );
		Quaternion( const Quaternion& q );
		Quaternion( const Vector3 &axis, f32 fAngle );

		void Set( f32 x, f32 y, f32 z , f32 w );
		Quaternion operator - () const;
		Quaternion operator + ( const Quaternion& q ) const;
		Quaternion operator - ( const Quaternion& q ) const;
		Quaternion operator * ( const Quaternion& q ) const;
		Quaternion operator * ( f32 scalar ) const;

		bool operator == ( const Quaternion& q ) const;
		bool operator != ( const Quaternion& q ) const;

		f32 GetLengthSquared() const;
		f32 GetLength() const;

		static f32 Dot(const Quaternion& p, const Quaternion& q );
		
		static Quaternion Conjugate( const Quaternion& p );

		static Quaternion Inverse( const Quaternion& q );

		void SetInverse( const Quaternion& q );

		void GetAngleAxis( f32& angle, Vector3& axis ) const;
		void SetFromAngleAxis( const Vector3& axis, f32 angle );
		void SetFromAngleAxisX( f32 fAngle );
		void SetFromAngleAxisY( f32 fAngle );
		void SetFromAngleAxisZ( f32 fAngle );
		void SetFromAngleAxesXYZ( f32 fAngleX, f32 fAngleY, f32 fAngleZ );

		static Quaternion CreateFromYawPitchRoll( f32 yaw, f32 pitch, f32 roll );
		void SetFromYawPitchRoll( f32 yaw, f32 pitch, f32 roll );

		static Quaternion CreateFromRotationMatrix( const Matrix4& mat );

		void SetFromRotationMatrix( const Matrix4& mat );

		static Quaternion Slerp( const Quaternion& q1, const Quaternion& q2, f32 w );
		static Quaternion Lerp(const Quaternion& q1,const Quaternion& q2, f32 w );
		
		void SetNegate();
		
		void Snap();

		void Normalize();

		static const Quaternion Identity;
		static const f32 Epsilon;

	public:
#pragma warning( disable : 4201 )

		union
		{
			f32 V[4];

			struct
			{
				f32 X;
				f32 Y;
				f32 Z;
				f32 W;
			};
		};

#pragma warning( default : 4201 )

	};

	Quaternion operator * ( f32 scalar, const Quaternion& q );
}