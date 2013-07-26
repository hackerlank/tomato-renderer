#include "TomatoPCH.h"

#include "BasicTypeDesc.h"

namespace Tomato
{
	//--------------------------------------------------
	// string
	//--------------------------------------------------	
	DataType::Type ConsoleBasicTypeDescString::GetType() 
	{
		return DataType::String;
	}

	String ConsoleBasicTypeDescString::GetTypeName() 
	{ 
		return L"string"; 
	}


	String ConsoleBasicTypeDescString::ToString( const String& value )
	{
		return value;
	}

	String ConsoleBasicTypeDescString::FromString( const String& string )
	{
		return string;
	}
}