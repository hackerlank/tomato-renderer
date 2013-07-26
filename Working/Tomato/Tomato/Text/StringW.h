#pragma once

#include <string>

namespace Tomato
{
	class TOMATO_API StringW
	{
	public:
		StringW();

		StringW( wchar c, s32 count = 1 );

		StringW( const StringW& str );
		StringW( const wchar* str );
		StringW( const std::wstring& str );

		bool operator ==( const StringW& str ) const;
		bool operator ==( const wchar* str ) const;
		bool operator ==( const std::wstring& str ) const;
		
		bool operator !=( const StringW& str ) const;
		bool operator !=( const wchar* str ) const;
		bool operator !=( const std::wstring& str ) const;

		bool operator <( const StringW& str ) const;
		bool operator <( const wchar* str ) const;
		bool operator <( const std::wstring& str ) const;

		bool operator >( const StringW& str ) const;
		bool operator >( const wchar* str ) const;
		bool operator >( const std::wstring& str ) const;
		
		bool operator <=( const StringW& str ) const;
		bool operator <=( const wchar* str ) const;
		bool operator <=( const std::wstring& str ) const;

		bool operator >=( const StringW& str ) const;
		bool operator >=( const wchar* str ) const;
		bool operator >=( const std::wstring& str ) const;

		StringW& operator =( const StringW& str );
		StringW& operator =( const wchar* str );
		StringW& operator =( const std::wstring& str );

		StringW& operator +=( const StringW& str );
		StringW& operator +=( const wchar* str );
		StringW& operator +=( const std::wstring& str );
		
		StringW operator +( const StringW& str ) const;
		StringW operator +( const wchar* str ) const;
		StringW operator +( const std::wstring& str ) const;
		
		// Do not assume that internal storage of the StringW is contiguous or null-terminated.
		wchar& operator []( s32 index );
		const wchar& operator []( s32 index ) const;

		StringW& Append( const StringW& str );
		StringW& Append( const wchar* str );
		StringW& Append( const std::wstring& str );

		StringW& Trim();

		StringW& TrimStart();

		StringW& TrimEnd();

		StringW& RemoveWhiteSpaces();

		static StringW ToUpper( StringW str );
		static StringW ToUpper( const wchar* str );
		static StringW ToUpper( const std::wstring& str );

		static StringW ToLower( StringW str );
		static StringW ToLower( const wchar* str );
		static StringW ToLower( const std::wstring& str );

		s32 CompareTo( const StringW& str ) const;
		s32 CompareTo( const wchar* str ) const;
		s32 CompareTo( const std::wstring& str ) const;

		static s32 Compare( const StringW& str1, const StringW& str2 );
		static s32 Compare( const wchar* str1, const wchar* str2 );
		static s32 Compare( const std::wstring& str1, const std::wstring& str2 );
		
		bool EqualTo( const StringW& str ) const;
		bool EqualTo( const wchar* str ) const;
		bool EqualTo( const std::wstring& str ) const;
		
		bool EqualTo( const StringW& str, bool bCaseSensitive ) const;
		bool EqualTo( const wchar* str, bool bCaseSensitive ) const;
		bool EqualTo( const std::wstring& str, bool bCaseSensitive ) const;

		s32 GetInt() const;
		f32 GetFloat() const;
		f64 GetDouble() const;
		const wchar* GetCString() const;

		StringW Left( s32 count ) const;
		StringW Right( s32 count ) const;
		StringW SubString( s32 index ) const;
		StringW SubString( s32 index, s32 count ) const;

		s32 IndexOf( const StringW& str, s32 offset = 0 ) const;
		s32 IndexOf( const wchar* str, s32 offset = 0 ) const;
		s32 IndexOf( const std::wstring& str, s32 offset = 0 ) const;
		s32 IndexOf( wchar c, s32 offset = 0 ) const;
		s32 LastIndexOf( const StringW& str, s32 offset = -1 ) const;
		s32 LastIndexOf( const wchar* str, s32 offset = -1 ) const;
		s32 LastIndexOf( const std::wstring& str, s32 offset = -1 ) const;
		s32 LastIndexOf( wchar c, s32 offset = -1 ) const;

		StringW& Remove( const StringW& str );
		StringW& Remove( const wchar* str );
		StringW& Remove( const std::wstring& str );
		
		StringW& Replace( const StringW& findStr, const StringW& replaceStr);
		StringW& Replace( const wchar* findStr, const wchar* replaceStr);
		StringW& Replace( const std::wstring& findStr, const std::wstring& replaceStr);

		bool StartWith( const StringW& findStr ) const;
		bool StartWith( const wchar* findStr ) const;
		bool StartWith( const std::wstring& findStr ) const;
		
		bool EndWith( const StringW& findStr ) const;
		bool EndWith( const wchar* findStr ) const;
		bool EndWith( const std::wstring& findStr ) const;
		
		StringW& Resize( s32 size );
		s32 GetLength() const;
		bool IsEmpty() const;

	private:
		std::wstring m_string;
	};
}