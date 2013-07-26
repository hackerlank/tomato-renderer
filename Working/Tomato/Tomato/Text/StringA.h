#pragma once

#include <string>

namespace Tomato
{
	class TOMATO_API StringA
	{
	public:
		StringA();

		StringA( char c, s32 count = 1 );

		StringA( const StringA& str );
		StringA( const char* str );
		StringA( const std::string& str );

		bool operator ==( const StringA& str ) const;
		bool operator ==( const char* str ) const;
		bool operator ==( const std::string& str ) const;
		
		bool operator !=( const StringA& str ) const;
		bool operator !=( const char* str ) const;
		bool operator !=( const std::string& str ) const;

		bool operator <( const StringA& str ) const;
		bool operator <( const char* str ) const;
		bool operator <( const std::string& str ) const;

		bool operator >( const StringA& str ) const;
		bool operator >( const char* str ) const;
		bool operator >( const std::string& str ) const;
		
		bool operator <=( const StringA& str ) const;
		bool operator <=( const char* str ) const;
		bool operator <=( const std::string& str ) const;

		bool operator >=( const StringA& str ) const;
		bool operator >=( const char* str ) const;
		bool operator >=( const std::string& str ) const;

		StringA& operator =( const StringA& str );
		StringA& operator =( const char* str );
		StringA& operator =( const std::string& str );

		StringA& operator +=( const StringA& str );
		StringA& operator +=( const char* str );
		StringA& operator +=( const std::string& str );
		
		StringA operator +( const StringA& str ) const;
		StringA operator +( const char* str ) const;
		StringA operator +( const std::string& str ) const;
		
		// Do not assume that internal storage of the StringA is contiguous or null-terminated.
		char& operator []( s32 index );
		const char& operator []( s32 index ) const;

		StringA& Append( const StringA& str );
		StringA& Append( const char* str );
		StringA& Append( const std::string& str );

		StringA& Trim();

		StringA& TrimStart();

		StringA& TrimEnd();

		StringA& RemoveWhiteSpaces();

		static StringA ToUpper( StringA str );
		static StringA ToUpper( const char* str );
		static StringA ToUpper( const std::string& str );

		static StringA ToLower( StringA str );
		static StringA ToLower( const char* str );
		static StringA ToLower( const std::string& str );

		s32 CompareTo( const StringA& str ) const;
		s32 CompareTo( const char* str ) const;
		s32 CompareTo( const std::string& str ) const;

		static s32 Compare( const StringA& str1, const StringA& str2 );
		static s32 Compare( const char* str1, const char* str2 );
		static s32 Compare( const std::string& str1, const std::string& str2 );
		
		bool EqualTo( const StringA& str ) const;
		bool EqualTo( const char* str ) const;
		bool EqualTo( const std::string& str ) const;
		
		bool EqualTo( const StringA& str, bool bCaseSensitive ) const;
		bool EqualTo( const char* str, bool bCaseSensitive ) const;
		bool EqualTo( const std::string& str, bool bCaseSensitive ) const;

		s32 GetInt() const;
		f32 GetFloat() const;
		f64 GetDouble() const;
		const char* GetCString() const;

		StringA Left( s32 count ) const;
		StringA Right( s32 count ) const;
		StringA SubString( s32 index ) const;
		StringA SubString( s32 index, s32 count ) const;

		s32 IndexOf( const StringA& str, s32 offset = 0 ) const;
		s32 IndexOf( const char* str, s32 offset = 0 ) const;
		s32 IndexOf( const std::string& str, s32 offset = 0 ) const;
		s32 IndexOf( char c, s32 offset = 0 ) const;
		s32 LastIndexOf( const StringA& str, s32 offset = -1 ) const;
		s32 LastIndexOf( const char* str, s32 offset = -1 ) const;
		s32 LastIndexOf( const std::string& str, s32 offset = -1 ) const;
		s32 LastIndexOf( char c, s32 offset = -1 ) const;

		StringA& Remove( const StringA& str );
		StringA& Remove( const char* str );
		StringA& Remove( const std::string& str );
		
		StringA& Replace( const StringA& findStr, const StringA& replaceStr);
		StringA& Replace( const char* findStr, const char* replaceStr);
		StringA& Replace( const std::string& findStr, const std::string& replaceStr);

		bool StartWith( const StringA& findStr ) const;
		bool StartWith( const char* findStr ) const;
		bool StartWith( const std::string& findStr ) const;
		
		bool EndWith( const StringA& findStr ) const;
		bool EndWith( const char* findStr ) const;
		bool EndWith( const std::string& findStr ) const;
		
		StringA& Resize( s32 size );
		s32 GetLength() const;
		bool IsEmpty() const;

	private:
		std::string m_string;
	};
}