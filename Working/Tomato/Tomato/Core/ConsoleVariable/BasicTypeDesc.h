#pragma once

namespace Tomato
{
	class TOMATO_API ConsoleBasicTypeDescBool
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const bool& value );
		static bool FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescBoolArray
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const std::vector<bool>& value );
		static std::vector<bool> FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescInt
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const s32& value );
		static s32 FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescIntArray
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const std::vector<s32>& value );
		static std::vector<s32> FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescUInt
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const u32& value );
		static u32 FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescUIntArray
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const std::vector<u32>& value );
		static std::vector<u32> FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescFloat
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const f32& value );
		static f32 FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescFloatArray
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const std::vector<f32>& value );
		static std::vector<f32> FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescDouble
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const f64& value );
		static f64 FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescDoubleArray
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const std::vector<f64>& value );
		static std::vector<f64> FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescString
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const String& value );
		static String FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescVector2
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const Vector2& value );
		static Vector2 FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescVector3
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const Vector3& value );
		static Vector3 FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescVector4
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const Vector4& value );
		static Vector4 FromString( const String& string );
	};

	class TOMATO_API ConsoleBasicTypeDescMatrix4
	{
	public:
		static DataType::Type GetType();
		static String GetTypeName();
		static String ToString( const Matrix4& value );
		static Matrix4 FromString( const String& string );
	};

}