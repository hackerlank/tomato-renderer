#include "TomatoPCH.h"

#include "BasicTypeDesc.h"

namespace Tomato
{
	DataType::Type ConsoleBasicTypeDescFloat::GetType() 
	{
		return DataType::Float;
	}

	String ConsoleBasicTypeDescFloat::GetTypeName() 
	{ 
		return L"f32"; 
	}

	String ConsoleBasicTypeDescFloat::ToString( const f32& value )
	{
		return StringFormatter( L"%f", value ).GetString();
	}

	f32 ConsoleBasicTypeDescFloat::FromString( const String& string )
	{
		return string.GetFloat();
	}

	//--------------------------------------------------
	// f32 array
	//--------------------------------------------------	
	DataType::Type ConsoleBasicTypeDescFloatArray::GetType() 
	{
		return DataType::FloatArray;
	}

	String ConsoleBasicTypeDescFloatArray::GetTypeName() 
	{ 
		return L"f32-array"; 
	}

	String ConsoleBasicTypeDescFloatArray::ToString( const std::vector<f32>& value )
	{
		String string = L"";

		std::vector<f32>::const_iterator it;
		for( it=value.begin(); it!=value.end(); ++it )
		{
			string += StringFormatter( L"%f ", ( *it ) ).GetString();
		}

		string.TrimEnd();
		return string;
	}

	std::vector<f32> ConsoleBasicTypeDescFloatArray::FromString( const String& string )
	{
		std::vector<f32> value;

		StringTokenizer tokenizer(string );
		tokenizer.AddPunctuator( L' ');
		tokenizer.AddPunctuator( L',');

		String token;
		while( tokenizer.GetNext(token) )
		{
			value.push_back(token.GetFloat());
		}

		return value;
	}
}