#include "TomatoPCH.h"

#include "BasicTypeDesc.h"

namespace Tomato
{
	//--------------------------------------------------
	// s32
	//--------------------------------------------------	
	DataType::Type ConsoleBasicTypeDescInt::GetType() 
	{
		return DataType::Int;
	}

	String ConsoleBasicTypeDescInt::GetTypeName() 
	{ 
		return L"s32"; 
	}

	String ConsoleBasicTypeDescInt::ToString( const s32& value )
	{
		return StringFormatter( L"%d", value ).GetString();
	}

	s32 ConsoleBasicTypeDescInt::FromString( const String& string )
	{
		return string.GetInt();
	}

	//--------------------------------------------------
	// s32 array
	//--------------------------------------------------	
	DataType::Type ConsoleBasicTypeDescIntArray::GetType() 
	{
		return DataType::IntArray;
	}

	String ConsoleBasicTypeDescIntArray::GetTypeName() 
	{ 
		return L"s32-array"; 
	}

	String ConsoleBasicTypeDescIntArray::ToString( const std::vector<s32>& value )
	{
		String string = L"";

		std::vector<s32>::const_iterator it;
		for( it=value.begin(); it!=value.end(); ++it )
		{
			string += StringFormatter( L"%d ", ( *it ) ).GetString();
		}

		string.TrimEnd();
		return string;
	}

	std::vector<s32> ConsoleBasicTypeDescIntArray::FromString( const String& string )
	{
		std::vector<s32> value;

		StringTokenizer tokenizer(string );
		tokenizer.AddPunctuator( L' ');
		tokenizer.AddPunctuator( L',');

		String token;
		while( tokenizer.GetNext(token) )
		{
			value.push_back(token.GetInt());
		}

		return value;
	}

}