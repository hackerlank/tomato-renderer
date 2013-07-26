#pragma once

#include <string>

namespace Tomato
{
	class TOMATO_API TextHelper
	{
	public:
		// Convert the source string into the Unicode(UTF16) string.
		static StringW ConvertToUnicodeString( const StringA& sourceString, u32 sourceEncoding = 0 );
		static std::wstring ConvertToUnicodeString( const std::string& sourceString, u32 sourceEncoding = 0 );

		// Convert the Unicode(UTF16) string into the ANSI string.
		static StringA ConvertToAnsiString( const StringW& sourceString );
		static std::string ConvertToAnsiString( const std::wstring& sourceString );

		// Convert the Unicode(UTF16) string into the multi-byte string.
		static StringA ConvertToMultiByteString( const StringW& sourceString, u32 targetEncoding );
		static std::string ConvertToMultiByteString( const std::wstring& sourceString, u32 targetEncoding );
	};
}