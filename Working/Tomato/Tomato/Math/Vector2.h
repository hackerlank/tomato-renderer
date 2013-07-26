#pragma once

namespace Tomato
{
	class TOMATO_API Vector2
	{
	public:
		Vector2();
		Vector2( f32 x, f32 y );
		~Vector2();

		f32& operator[] ( s32 index );
		const f32& operator[] ( s32 index ) const;

		void SetX( f32 x );
		void SetY( f32 y );
		void Set( f32 x, f32 y );

		Vector2& operator = ( const Vector2& v );
		Vector2& operator += ( const Vector2& v );
		Vector2& operator -= ( const Vector2& v );
		Vector2& operator *= ( const Vector2& v );
		Vector2& operator /= ( const Vector2& v );
		Vector2& operator *= ( f32 scalar );
		Vector2& operator /= ( f32 scalar );

		bool operator == ( const Vector2& v ) const;
		bool operator != ( const Vector2& v ) const;

		Vector2 operator + ( const Vector2& v ) const;
		Vector2 operator - ( const Vector2& v ) const;
		Vector2 operator * ( const Vector2& v ) const;
		Vector2 operator / ( const Vector2& v ) const;
		Vector2 operator * ( f32 scalar ) const;
		Vector2 operator / ( f32 scalar ) const;

		const Vector2& operator + () const;
		Vector2 operator - () const;

		bool operator < ( const Vector2& v ) const;
		bool operator > ( const Vector2& v ) const;

		void SetZero();

		f32 GetLength() const;
		f32 GetLengthSquared() const;

		void Normalize();

		static f32 GetDistance( const Vector2& v1, const Vector2& v2 );
		static f32 GetDistanceSquared( const Vector2& v1, const Vector2& v2 );

		static f32 Dot( const Vector2& v1, const Vector2& v2 );

		static Vector2 Normalize( const Vector2& v );

		static Vector2 Reflect( const Vector2& v, const Vector2& normal );

		static Vector2 Min( const Vector2& v1, const Vector2& v2 );
		static Vector2 Max( const Vector2& v1, const Vector2& v2 );
		static Vector2 Clamp( const Vector2& v, const Vector2& min, const Vector2& max  );

		static Vector2 Lerp( const Vector2& v1, const Vector2& v2, f32 w );
		static Vector2 Barycentric( const Vector2& v1, const Vector2& v2, const Vector2& v3, f32 w1, f32 w2 );
		static Vector2 SmoothStep( const Vector2& v1, const Vector2& v2, f32 w );
		static Vector2 CatmullRom( const Vector2& v1, const Vector2& v2, const Vector2& v3, const Vector2& v4, f32 w );
		static Vector2 Hermite( const Vector2& v1, const Vector2& tangent1, const Vector2& v2, const Vector2& tangent2, f32 w );

		static Vector2 Zero();
		static Vector2 One();
		static Vector2 UnitX();
		static Vector2 UnitY();

	public:

#pragma warning( disable:4201 )

		union
		{
			f32 V[2];

			struct  
			{
				f32 X;
				f32 Y;
			};
		};

#pragma warning( default: 4201 )
	};
}