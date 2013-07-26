#pragma once

namespace Tomato
{	
	class TOMATO_API Vector4
	{
	public:
		Vector4();
		Vector4( f32 x, f32 y, f32 z, f32 w );
		Vector4( const Vector3& v, f32 w = 0 );
		~Vector4();

		f32& operator[] ( s32 index );
		const f32& operator[] ( s32 index ) const;

		void SetX( f32 x );
		void SetY( f32 y );
		void SetZ( f32 z );
		void SetW( f32 w );
		void Set( f32 x, f32 y, f32 z, f32 w );

		Vector4& operator = ( const Vector4& v );
		Vector4& operator = ( const Vector3& v );
		Vector4& operator += ( const Vector4& v );
		Vector4& operator -= ( const Vector4& v );
		Vector4& operator *= ( const Vector4& v );
		Vector4& operator /= ( const Vector4& v );
		Vector4& operator *= ( f32 scalar );
		Vector4& operator /= ( f32 scalar );

		bool operator == ( const Vector4& v ) const;
		bool operator != ( const Vector4& v ) const;

		Vector4 operator + ( const Vector4& v ) const;
		Vector4 operator - ( const Vector4& v ) const;
		Vector4 operator * ( const Vector4& v ) const;
		Vector4 operator / ( const Vector4& v ) const;
		Vector4 operator * ( f32 scalar ) const;
		Vector4 operator / ( f32 scalar ) const;

		const Vector4& operator + () const;
		Vector4 operator - () const;

		bool operator < ( const Vector4& v ) const;
		bool operator > ( const Vector4& v ) const;

		void SetZero();

		f32 GetLength() const;
		f32 GetLengthSquared() const;

		void Normalize();

		static f32 GetDistance( const Vector4& v1, const Vector4& v2 );
		static f32 GetDistanceSquared( const Vector4& v1, const Vector4& v2 );

		static f32 Dot( const Vector4& v1, const Vector4& v2 );

		static Vector4 Normalize( const Vector4& v );

		static Vector4 Min( const Vector4& v1, const Vector4& v2 );
		static Vector4 Max( const Vector4& v1, const Vector4& v2 );
		static Vector4 Clamp( const Vector4& v, const Vector4& min, const Vector4& max );

		static Vector4 Lerp( const Vector4& v1, const Vector4& v2, f32 w );
		static Vector4 Barycentric( const Vector4& v1, const Vector4& v2, const Vector4& v3, f32 w1, f32 w2 );
		static Vector4 SmoothStep( const Vector4& v1, const Vector4& v2, f32 w );
		static Vector4 CatmullRom( const Vector4& v1, const Vector4& v2, const Vector4& v3, const Vector4& v4, f32 w );
		static Vector4 Hermite( const Vector4& v1, const Vector4& tangent1, const Vector4& v2, const Vector4& tangent2, f32 w );

		static Vector4 Zero();
		static Vector4 One();
		static Vector4 UnitX();
		static Vector4 UnitY();
		static Vector4 UnitZ();
		static Vector4 UnitW();

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
}