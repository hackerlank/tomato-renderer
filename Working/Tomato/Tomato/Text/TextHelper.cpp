#include "TomatoPCH.h"

#include "TextHelper.h"

#include <vector>
#include <string>
#include <cstring>
#include <cwchar>
#include <cassert>

namespace Tomato
{
	StringW TextHelper::ConvertToUnicodeString( const StringA& sourceString, u32 sourceEncoding )
	{
		s32 sourceLength = sourceString.GetLength();
		if( sourceLength == 0 )
		{
			return StringW();
		}		

		const char* pSource = sourceString.GetCString();
		assert( pSource != 0 );

		s32 length = ::MultiByteToWideChar( sourceEncoding, 0, pSource, sourceLength, NULL, 0 );
		if( length == 0 )
		{
			return StringW();
		}

		std::vector<wchar> buffer( length + 1 );
		::MultiByteToWideChar( sourceEncoding, 0, pSource, sourceLength, &buffer[ 0 ], length );
		buffer[ length ] = L'\0';

		return StringW( &buffer[ 0 ] );
	}

	std::wstring TextHelper::ConvertToUnicodeString( const std::string& sourceString, u32 sourceEncoding )
	{
		s32 sourceLength = sourceString.length();
		if( sourceLength == 0 )
		{
			return std::wstring();
		}		

		const char* pSource = sourceString.c_str();
		assert( pSource != 0 );

		s32 length = ::MultiByteToWideChar( sourceEncoding, 0, pSource, sourceLength, NULL, 0 );
		if( length == 0 )
		{
			return std::wstring();
		}

		std::vector<wchar> buffer( length );
		::MultiByteToWideChar( sourceEncoding, 0, pSource, sourceLength, &buffer[ 0 ], length );

		return std::wstring( &buffer[ 0 ], length );
	}

	StringA TextHelper::ConvertToMultiByteString( const StringW& sourceString, u32 targetEncoding )
	{
		s32 sourceLength = sourceString.GetLength();
		if( sourceLength == 0 )
		{
			return StringA();
		}

		const wchar* pSource = sourceString.GetCString();
		assert( pSource != 0 );

		s32 length = ::WideCharToMultiByte( targetEncoding, 0, pSource, sourceLength, NULL, 0, NULL, NULL );
		if( length == 0 )
		{
			return StringA();
		}

		std::vector<char> buffer( length + 1 );
		::WideCharToMultiByte( targetEncoding, 0, pSource, sourceLength, &buffer[ 0 ], length, NULL, NULL );
		buffer[ length ] = '\0';

		return StringA( &buffer[ 0 ] );
	}

	std::string TextHelper::ConvertToMultiByteString( const std::wstring& sourceString, u32 targetEncoding )
	{
		s32 sourceLength = sourceString.length();
		if( sourceLength == 0 )
		{
			return std::string();
		}

		const wchar* pSource = sourceString.c_str();
		assert( pSource != 0 );

		s32 length = ::WideCharToMultiByte( targetEncoding, 0, pSource, sourceLength, NULL, 0, NULL, NULL );
		if( length == 0 )
		{
			return std::string();
		}

		std::vector<char> buffer( length );
		::WideCharToMultiByte( targetEncoding, 0, pSource, sourceLength, &buffer[ 0 ], length, NULL, NULL );

		return std::string( &buffer[ 0 ], length );
	}

	StringA TextHelper::ConvertToAnsiString( const StringW& sourceString  )
	{
		s32 sourceLength = sourceString.GetLength();
		if( sourceLength == 0 )
		{
			return StringA();
		}

		const wchar* pSource = sourceString.GetCString();
		assert( pSource != 0 );

		s32 length = ::WideCharToMultiByte( CP_ACP, 0, pSource, sourceLength, NULL, 0, NULL, NULL );
		if( length == 0 )
		{
			return StringA();
		}

		std::vector<char> buffer( length + 1 );
		::WideCharToMultiByte( CP_ACP, 0, pSource, sourceLength, &buffer[ 0 ], length, NULL, NULL );
		buffer[ length ] = '\0';

		return StringA( &buffer[ 0 ] );
	}

	std::string TextHelper::ConvertToAnsiString( const std::wstring& sourceString  )
	{
		s32 sourceLength = sourceString.length();
		if( sourceLength == 0 )
		{
			return std::string();
		}

		const wchar* pSource = sourceString.c_str();
		assert( pSource != 0 );

		s32 length = ::WideCharToMultiByte( CP_ACP, 0, pSource, sourceLength, NULL, 0, NULL, NULL );
		if( length == 0 )
		{
			return std::string();
		}

		std::vector<char> buffer( length );
		::WideCharToMultiByte( CP_ACP, 0, pSource, sourceLength, &buffer[ 0 ], length, NULL, NULL );

		return std::string( &buffer[ 0 ], length );
	}
}