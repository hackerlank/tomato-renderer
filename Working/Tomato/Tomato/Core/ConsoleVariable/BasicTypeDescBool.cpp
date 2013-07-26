#include "TomatoPCH.h"

#include "BasicTypeDesc.h"

namespace Tomato
{
	//--------------------------------------------------
	// bool
	//--------------------------------------------------	
	DataType::Type ConsoleBasicTypeDescBool::GetType() 
	{
		return DataType::Bool;
	}

	String ConsoleBasicTypeDescBool::GetTypeName() 
	{ 
		return L"bool"; 
	}

	String ConsoleBasicTypeDescBool::ToString( const bool& value )
	{
		if( value ) { return L"true"; }
		else { return L"false"; }
	}

	bool ConsoleBasicTypeDescBool::FromString( const String& string )
	{
		String conv(string );
		conv.Trim();

		if( conv.EqualTo( L"true", false ) ) { return true; }
		else if( conv.GetInt() != 0 ) { return true; }
		else { return false; }
	}

	//--------------------------------------------------
	// bool array
	//--------------------------------------------------	
	DataType::Type ConsoleBasicTypeDescBoolArray::GetType() 
	{
		return DataType::BoolArray;
	}

	String ConsoleBasicTypeDescBoolArray::GetTypeName() 
	{ 
		return L"bool-array"; 
	}

	String ConsoleBasicTypeDescBoolArray::ToString( const std::vector<bool>& value )
	{
		String string = L"";

		std::vector<bool>::const_iterator it;
		for( it=value.begin(); it!=value.end(); ++it )
		{
			if( *it ) { string += L"true "; }
			else { string += L"false "; }
		}

		string.TrimEnd();
		return string;
	}

	std::vector<bool> ConsoleBasicTypeDescBoolArray::FromString( const String& string )
	{
		std::vector<bool> value;

		StringTokenizer tokenizer(string );
		tokenizer.AddPunctuator( L' ');
		tokenizer.AddPunctuator( L',');

		String token;
		while( tokenizer.GetNext(token) )
		{
			if( token.EqualTo( L"true", false ) ) { value.push_back(true); }
			else if( token.GetInt() != 0 ) { value.push_back(true); }
			else { value.push_back(false); }
		}

		return value;
	}
}