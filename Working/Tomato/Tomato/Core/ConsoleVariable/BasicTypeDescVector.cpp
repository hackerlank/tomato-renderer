#include "TomatoPCH.h"

#include "BasicTypeDesc.h"

namespace Tomato
{
	DataType::Type ConsoleBasicTypeDescVector2::GetType() 
	{
		return DataType::Vector2;
	}

	String ConsoleBasicTypeDescVector2::GetTypeName() 
	{ 
		return L"vector2"; 
	}

	String ConsoleBasicTypeDescVector2::ToString( const Vector2& value )
	{
		String conv = StringFormatter(
			L"(%f %f)", 
			value.X, value.Y).GetString();

		conv.TrimEnd();
		return conv;
	}

	Vector2 ConsoleBasicTypeDescVector2::FromString( const String& string )
	{
		Vector2 value(0, 0);

		// copy original data removing white spaces
		String copy = string;
		copy.Trim();

		// prepare tokenizer ignoring parenthesess, comman and blank
		StringTokenizer tokenizer(copy);
		tokenizer.AddPunctuator( L' ');
		tokenizer.AddPunctuator( L'(');
		tokenizer.AddPunctuator( L')');
		tokenizer.AddPunctuator( L',');

		// assign elements converting to f32 type
		u32 tokenCount = tokenizer.GetTokenCount();
		for( u32 i=0; i<tokenCount && i<2; ++i )
		{
			value.V[ i ] = tokenizer.GetNext().GetFloat();
		}

		return value;
	}

	DataType::Type ConsoleBasicTypeDescVector3::GetType() 
	{
		return DataType::Vector3;
	}

	String ConsoleBasicTypeDescVector3::GetTypeName() 
	{ 
		return L"vector3"; 
	}

	String ConsoleBasicTypeDescVector3::ToString( const Vector3& value )
	{
		String conv = StringFormatter(
			L"(%f %f %f)", 
			value.X, value.Y, value.Z).GetString();

		conv.TrimEnd();
		return conv;
	}

	Vector3 ConsoleBasicTypeDescVector3::FromString( const String& string )
	{
		Vector3 value(0, 0, 0);

		// copy original data removing white spaces
		String copy = string;
		copy.Trim();

		// prepare tokenizer ignoring parenthesess, comman and blank
		StringTokenizer tokenizer(copy);
		tokenizer.AddPunctuator( L' ');
		tokenizer.AddPunctuator( L'(');
		tokenizer.AddPunctuator( L')');
		tokenizer.AddPunctuator( L',');

		// assign elements converting to f32 type
		u32 tokenCount = tokenizer.GetTokenCount();
		for( u32 i=0; i<tokenCount && i<3; ++i )
		{
			value.V[ i ] = tokenizer.GetNext().GetFloat();
		}

		return value;
	}

	DataType::Type ConsoleBasicTypeDescVector4::GetType() 
	{
		return DataType::Vector4;
	}

	String ConsoleBasicTypeDescVector4::GetTypeName() { 
		return L"vector4"; 
	}

	String ConsoleBasicTypeDescVector4::ToString( const Vector4& value )
	{
		String conv = StringFormatter(
			L"(%f %f %f %f)", 
			value.X, value.Y, value.Z, value.W).GetString();

		conv.TrimEnd();
		return conv;
	}

	Vector4 ConsoleBasicTypeDescVector4::FromString( const String& string )
	{
		Vector4 value(0, 0, 0, 0);

		// copy original data removing white spaces
		String copy = string;
		copy.Trim();

		// prepare tokenizer ignoring parenthesess, comman and blank
		StringTokenizer tokenizer(copy);
		tokenizer.AddPunctuator( L' ');
		tokenizer.AddPunctuator( L'(');
		tokenizer.AddPunctuator( L')');
		tokenizer.AddPunctuator( L',');

		// assign elements converting to f32 type
		u32 tokenCount = tokenizer.GetTokenCount();
		for( u32 i=0; i<tokenCount && i<4; ++i )
		{
			value.V[ i ] = tokenizer.GetNext().GetFloat();
		}

		return value;
	}

}