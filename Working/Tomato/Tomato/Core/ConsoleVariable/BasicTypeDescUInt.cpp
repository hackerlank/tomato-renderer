#include "TomatoPCH.h"

#include "BasicTypeDesc.h"

namespace Tomato
{
	//--------------------------------------------------
	// u32
	//--------------------------------------------------	
	DataType::Type ConsoleBasicTypeDescUInt::GetType() 
	{
		return DataType::UInt;
	}

	String ConsoleBasicTypeDescUInt::GetTypeName() 
	{ 
		return L"uint"; 
	}

	String ConsoleBasicTypeDescUInt::ToString( const u32& value )
	{
		return StringFormatter( L"%d", value ).GetString();
	}

	u32 ConsoleBasicTypeDescUInt::FromString( const String& string )
	{
		return string.GetInt();
	}

	//--------------------------------------------------
	// u32 array
	//--------------------------------------------------	
	DataType::Type ConsoleBasicTypeDescUIntArray::GetType() 
	{
		return DataType::UIntArray;
	}

	String ConsoleBasicTypeDescUIntArray::GetTypeName() 
	{ 
		return L"uint-array"; 
	}

	String ConsoleBasicTypeDescUIntArray::ToString( const std::vector<u32>& value )
	{
		String string = L"";

		std::vector<u32>::const_iterator it;
		for( it=value.begin(); it!=value.end(); ++it )
		{
			string += StringFormatter( L"%u ", ( *it ) ).GetString();
		}

		string.TrimEnd();
		return string;
	}

	std::vector<u32> ConsoleBasicTypeDescUIntArray::FromString( const String& string )
	{
		std::vector<u32> value;

		StringTokenizer tokenizer(string );
		tokenizer.AddPunctuator( L' ');
		tokenizer.AddPunctuator( L',');

		String token;
		while( tokenizer.GetNext(token) )
		{
			value.push_back(static_cast<u32>(token.GetInt()));
		}

		return value;
	}
}