#include "TomatoPCH.h"

#include "BasicTypeDesc.h"

namespace Tomato
{
	//--------------------------------------------------
	// f64
	//--------------------------------------------------	
	DataType::Type ConsoleBasicTypeDescDouble::GetType() 
	{
		return DataType::Double;
	}

	String ConsoleBasicTypeDescDouble::GetTypeName() 
	{ 
		return L"f64"; 
	}

	String ConsoleBasicTypeDescDouble::ToString( const f64& value )
	{
		return StringFormatter( L"%f", value ).GetString();
	}

	f64 ConsoleBasicTypeDescDouble::FromString( const String& string )
	{
		return string.GetDouble();
	}

	//--------------------------------------------------
	// f64 array
	//--------------------------------------------------	
	DataType::Type ConsoleBasicTypeDescDoubleArray::GetType() 
	{
		return DataType::DoubleArray;
	}


	String ConsoleBasicTypeDescDoubleArray::GetTypeName() 
	{ 
		return L"f64-array"; 
	}

	String ConsoleBasicTypeDescDoubleArray::ToString( const std::vector<f64>& value )
	{
		String string = L"";

		std::vector<f64>::const_iterator it;
		for( it=value.begin(); it!=value.end(); ++it )
		{
			string += StringFormatter( L"%f ", ( *it)).GetString();
		}

		string.TrimEnd();
		return string;
	}

	std::vector<f64> ConsoleBasicTypeDescDoubleArray::FromString( const String& string )
	{
		std::vector<f64> value;

		StringTokenizer tokenizer(string );
		tokenizer.AddPunctuator( L' ');
		tokenizer.AddPunctuator( L',');

		String token;
		while( tokenizer.GetNext(token) )
		{
			value.push_back(token.GetDouble());
		}

		return value;
	}
}