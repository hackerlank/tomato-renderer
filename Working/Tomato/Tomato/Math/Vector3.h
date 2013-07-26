#pragma once

namespace Tomato
{
	class TOMATO_API Vector3
	{
	public:
		Vector3();
		Vector3( f32 x, f32 y, f32 z );
		Vector3( const Vector2& v, f32 z = 0 );
		~Vector3();

		f32& operator[] ( s32 index );
		const f32& operator[] ( s32 index ) const;

		void SetX( f32 x );
		void SetY( f32 y );
		void SetZ( f32 z );
		void Set( f32 x, f32 y, f32 z );

		Vector3& operator = ( const Vector3& v );
		Vector3& operator = ( const Vector2& v );
		Vector3& operator += ( const Vector3& v );
		Vector3& operator -= ( const Vector3& v );
		Vector3& operator *= ( const Vector3& v );
		Vector3& operator /= ( const Vector3& v );
		Vector3& operator *= ( f32 scalar );
		Vector3& operator /= ( f32 scalar );

		bool operator == ( const Vector3& v ) const;
		bool operator != ( const Vector3& v ) const;

		Vector3 operator + ( const Vector3& v ) const;
		Vector3 operator - ( const Vector3& v ) const;
		Vector3 operator * ( const Vector3& v ) const;
		Vector3 operator / ( const Vector3& v ) const;
		Vector3 operator * ( f32 scalar ) const;
		Vector3 operator / ( f32 scalar ) const;

		const Vector3& operator + () const;
		Vector3 operator - () const;

		bool operator < ( const Vector3& v ) const;
		bool operator > ( const Vector3& v ) const;

		void SetZero();

		f32 GetLength() const;
		f32 GetLengthSquared() const;

		void Normalize();

		static f32 GetDistance( const Vector3& v1, const Vector3& v2 );
		static f32 GetDistanceSquared( const Vector3& v1, const Vector3& v2 );

		static f32 Dot( const Vector3& v1, const Vector3& v2 );

		static Vector3 Normalize( const Vector3& v );

		static Vector3 Cross( const Vector3& v1, const Vector3& v2 );

		static Vector3 Reflect( const Vector3& v, const Vector3& normal );

		static Vector3 Min( const Vector3& v1, const Vector3& v2 );
		static Vector3 Max( const Vector3& v1, const Vector3& v2 );
		static Vector3 Clamp( const Vector3& v, const Vector3& min, const Vector3& max );
		
		static Vector3 Lerp( const Vector3& v1, const Vector3& v2, f32 w );
		static Vector3 Barycentric( const Vector3& v1, const Vector3& v2, const Vector3& v3, f32 w1, f32 w2 );
		static Vector3 SmoothStep( const Vector3& v1, const Vector3& v2, f32 w );
		static Vector3 CatmullRom( const Vector3& v1, const Vector3& v2, const Vector3& v3, const Vector3& v4, f32 w );
		static Vector3 Hermite( const Vector3& v1, const Vector3& tangent1, const Vector3& v2, const Vector3& tangent2, f32 w );

		// Build an orthonormal basis from vector v1.
		// Assume that v1 is normalized.
		static void BuildOrthonormalBasis( const Vector3& v1, Vector3& v2, Vector3& v3 );
		
		static Vector3 Zero();
		static Vector3 One();
		static Vector3 UnitX();
		static Vector3 UnitY();
		static Vector3 UnitZ();

	public:

#pragma warning( disable: 4201 )

		union
		{
			f32 V[3];

			struct  
			{
				f32 X;
				f32 Y;
				f32 Z;
			};
		};

#pragma warning( default: 4201 )		
	};
}
