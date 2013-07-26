#include "TomatoPCH.h"

#include "BasicTypeDesc.h"

namespace Tomato
{
	//--------------------------------------------------
	// Matrix4
	//--------------------------------------------------	
	DataType::Type ConsoleBasicTypeDescMatrix4::GetType() 
	{
		return DataType::Matrix;
	}

	String ConsoleBasicTypeDescMatrix4::GetTypeName() 
	{ 
		return L"matrix4"; 
	}

	String ConsoleBasicTypeDescMatrix4::ToString( const Matrix4& value )
	{
		String conv = StringFormatter(
			L"(%f %f %f %f) (%f %f %f %f) (%f %f %f %f) (%f %f %f %f)", 
			value.Row1[0], value.Row1[1], value.Row1[2], value.Row1[3],
			value.Row2[0], value.Row2[1], value.Row2[2], value.Row2[3],
			value.Row3[0], value.Row3[1], value.Row3[2], value.Row3[3],
			value.Row4[0], value.Row4[1], value.Row4[2], value.Row4[3]).GetString();

		conv.TrimEnd();
		return conv;
	}

	Matrix4 ConsoleBasicTypeDescMatrix4::FromString( const String& string )
	{
		Matrix4 value;
		for( s32 i = 0 ; i < 16 ; ++i ) { value.E[ i ] = 0; }

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
		for( u32 i=0; i<tokenCount && i<16; ++i )
		{
			value.E[ i ] = tokenizer.GetNext().GetFloat();
		}

		return value;
	}
}