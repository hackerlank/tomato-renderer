#include "TomatoPCH.h"

#include "StringW.h"

#include <cwctype>
#include <cstdlib>
#include <algorithm>
#include <cstring>
#include <cwchar>

namespace Tomato
{
	StringW::StringW()
		: m_string()
	{
	}

	StringW::StringW( wchar c, s32 count )
		: m_string( count, c )
	{
	}

	StringW::StringW( const StringW& str )
		: m_string( str.m_string )
	{
	}

	StringW::StringW( const wchar* str )
		: m_string( ( str != NULL ) ? str : L"" )
	{
	}

	StringW::StringW( const std::wstring& str )
		: m_string( str )
	{
	}

	bool StringW::operator ==( const StringW& str ) const
	{
		return m_string.compare( str.m_string ) == 0;
	}

	bool StringW::operator ==( const wchar* str ) const
	{
		return m_string.compare( str ) == 0;
	}

	bool StringW::operator ==( const std::wstring& str ) const
	{
		return m_string.compare( str ) == 0;
	}

	bool StringW::operator !=( const StringW& str ) const
	{
		return m_string.compare( str.m_string ) != 0;
	}

	bool StringW::operator !=( const wchar* str ) const
	{
		return m_string.compare( str ) != 0;
	}

	bool StringW::operator !=( const std::wstring& str ) const
	{
		return m_string.compare( str ) != 0;
	}

	bool StringW::operator <( const StringW& str ) const
	{
		return m_string.compare( str.m_string ) < 0;
	}

	bool StringW::operator <( const wchar* str ) const
	{
		return m_string.compare( str ) < 0;
	}

	bool StringW::operator <( const std::wstring& str ) const
	{
		return m_string.compare( str ) < 0;
	}
	
	bool StringW::operator >( const StringW& str ) const
	{
		return m_string.compare( str.m_string ) > 0;
	}

	bool StringW::operator >( const wchar* str ) const
	{
		return m_string.compare( str ) > 0;
	}

	bool StringW::operator >( const std::wstring& str ) const
	{
		return m_string.compare( str ) > 0;
	}

	bool StringW::operator <=( const StringW& str ) const
	{
		return m_string.compare( str.m_string ) <= 0;
	}

	bool StringW::operator <=( const wchar* str ) const
	{
		return m_string.compare( str ) <= 0;
	}

	bool StringW::operator <=( const std::wstring& str ) const
	{
		return m_string.compare( str ) <= 0;
	}

	bool StringW::operator >=( const StringW& str ) const
	{
		return m_string.compare( str.m_string ) >= 0;
	}

	bool StringW::operator >=( const wchar* str ) const
	{
		return m_string.compare( str ) >= 0;
	}

	bool StringW::operator >=( const std::wstring& str ) const
	{
		return m_string.compare( str ) >= 0;
	}

	StringW& StringW::operator =( const StringW& str )
	{
		m_string = str.m_string;
		return *this;
	}

	StringW& StringW::operator =( const wchar* str )
	{
		m_string = str;
		return *this;
	}

	StringW& StringW::operator =( const std::wstring& str )
	{
		m_string = str;
		return *this;
	}

	StringW& StringW::operator +=( const StringW& str )
	{
		m_string += str.m_string;
		return *this;
	}

	StringW& StringW::operator +=( const wchar* str )
	{
		m_string += str;
		return *this;
	}

	StringW& StringW::operator +=( const std::wstring& str )
	{
		m_string += str;
		return *this;
	}

	StringW StringW::operator +( const StringW& str ) const
	{
		StringW result( m_string );
		result.m_string.append( str.m_string );
		return result;
	}

	StringW StringW::operator +( const wchar* str ) const
	{
		StringW result( m_string );
		result.m_string.append( str );
		return result;
	}

	StringW StringW::operator +( const std::wstring& str ) const
	{
		StringW result( m_string );
		result.m_string.append( str );
		return result;
	}

	wchar& StringW::operator []( s32 index ) 
	{
		return m_string[ index ];
	}

	const wchar& StringW::operator []( s32 index ) const
	{
		return m_string[ index ];
	}

	StringW& StringW::Append( const StringW& str )
	{
		m_string.append( str.m_string );
		return *this;
	}

	StringW& StringW::Append( const wchar* str )
	{
		m_string.append( str );
		return *this;
	}

	StringW& StringW::Append( const std::wstring& str )
	{
		m_string.append( str );
		return *this;
	}

	StringW& StringW::Trim()
	{
		TrimStart();
		TrimEnd();
		return *this;
	}

	StringW& StringW::TrimStart()
	{
#if 1
		if( m_string.empty() ) { return *this; }

		for( std::size_t i = 0, j = m_string.length() ; i != j ; ++i )
		{
			if( !std::iswspace( m_string[ i ] ) )
			{
				m_string.erase( 0, i );
				return *this;
			}
		}

		m_string.erase();
		return *this;
#else
		m_string.erase( 
			m_string.begin(), 
			std::find_if( m_string.begin(), m_string.end(), std::not1( std::ptr_fun<s32, s32>( std::iswspace ) ) ) );
		return *this;
#endif
	}

	StringW& StringW::TrimEnd()
	{
#if 1
		if( m_string.empty() ) { return *this; }

		for( std::size_t i = m_string.length() - 1 ; i != std::wstring::npos ; --i )
		{
			if( !std::iswspace( m_string[ i ] ) )
			{
				m_string.erase( i + 1 );
				return *this;
			}
		}

		m_string.erase();
		return *this;
#else
		m_string.erase( 
			std::find_if( m_string.rbegin(), m_string.rend(), std::not1( std::ptr_fun<s32, s32>( std::iswspace ) ) ).base(), 
			m_string.end() );
		return *this;
#endif
	}

	StringW& StringW::RemoveWhiteSpaces()
	{
		m_string.erase( std::remove_if( m_string.begin(), m_string.end(), std::ptr_fun<std::wint_t, s32>( std::iswspace ) ), m_string.end() );
		return *this;
	}

	StringW StringW::ToUpper( StringW source )
	{
		for( std::size_t i = 0, j = source.m_string.length() ; i != j ; ++i )
		{
			source.m_string[ i ] = static_cast<wchar>( std::towupper( source.m_string[ i ] ) );
		}

		return source;
	}

	StringW StringW::ToUpper( const wchar* pSource )
	{
		std::size_t length = std::wcslen( pSource );

		StringW result( ' ', length );
		for( std::size_t i = 0, j = length ; i != j ; ++i )
		{
			result.m_string[ i ] = static_cast<wchar>( std::towupper( pSource[ i ] ) );
		}

		return result;
	}

	StringW StringW::ToLower( StringW source )
	{
		for( std::size_t i = 0, j = source.m_string.length() ; i != j ; ++i )
		{
			source.m_string[ i ] = static_cast<wchar>( std::towlower( source.m_string[ i ] ) );
		}

		return source;
	}

	StringW StringW::ToUpper( const std::wstring& source )
	{
		StringW result( ' ', source.length() );
		for( std::size_t i = 0, j = source.length() ; i != j ; ++i )
		{
			result.m_string[ i ] = static_cast<wchar>( std::towupper( source[ i ] ) );
		}

		return result;
	}

	StringW StringW::ToLower( const wchar* pSource )
	{
		std::size_t length = std::wcslen( pSource );

		StringW result( ' ', length );
		for( std::size_t i = 0, j = length ; i != j ; ++i )
		{
			result.m_string[ i ] = static_cast<wchar>( std::towlower( pSource[ i ] ) );
		}

		return result;
	}

	StringW StringW::ToLower( const std::wstring& source )
	{
		StringW result( ' ', source.length() );
		for( std::size_t i = 0, j = source.length() ; i != j ; ++i )
		{
			result.m_string[ i ] = static_cast<wchar>( std::towlower( source[ i ] ) );
		}

		return result;
	}

	s32 StringW::CompareTo( const StringW& str ) const
	{
		return m_string.compare( str.m_string );
	}

	s32 StringW::CompareTo( const wchar* str ) const
	{
		return m_string.compare( str );
	}

	s32 StringW::CompareTo( const std::wstring& str ) const
	{
		return m_string.compare( str );
	}

	s32 StringW::Compare( const StringW& str1, const StringW& str2 )
	{
		return str1.m_string.compare( str2.m_string );
	}

	s32 StringW::Compare( const wchar* str1, const wchar* str2 )
	{
		return std::wcscmp( str1, str2 );
	}

	s32 StringW::Compare( const std::wstring& str1, const std::wstring& str2 )
	{
		return str1.compare( str2 );
	}

	bool StringW::EqualTo( const StringW& str ) const
	{
		return m_string == str.m_string;
	}

	bool StringW::EqualTo( const wchar* str ) const
	{
		return m_string.compare( str ) == 0;
	}

	bool StringW::EqualTo( const std::wstring& str ) const
	{
		return m_string == str;
	}

	bool StringW::EqualTo( const StringW& str, bool bCaseSensitive ) const
	{
		if( bCaseSensitive )
		{
			return m_string == str.m_string;
		}
		else
		{
			if( m_string.length() != str.m_string.length() )
			{
				return false;
			}

			for( std::size_t i = 0, j = m_string.length() ; i != j ; ++i )
			{
				if( std::towlower( m_string[ i ] ) != std::towlower( str.m_string[ i ] ) )
				{
					return false;
				}
			}

			return true;
		}
	}

	bool StringW::EqualTo( const wchar* str, bool bCaseSensitive ) const
	{
		if( bCaseSensitive )
		{
			return m_string.compare( str ) == 0;
		}
		else
		{
			std::size_t length = std::wcslen( str );
			if( m_string.length() != length  )
			{
				return false;
			}

			for( std::size_t i = 0, j = m_string.length() ; i != j ; ++i )
			{
				if( std::towlower( m_string[ i ] ) != std::towlower( str[ i ] ) )
				{
					return false;
				}
			}

			return true;
		}
	}

	bool StringW::EqualTo( const std::wstring& str, bool bCaseSensitive ) const
	{
		if( bCaseSensitive )
		{
			return m_string == str;
		}
		else
		{
			if( m_string.length() != str.length() )
			{
				return false;
			}

			for( std::size_t i = 0, j = m_string.length() ; i != j ; ++i )
			{
				if( std::towlower( m_string[ i ] ) != std::towlower( str[ i ] ) )
				{
					return false;
				}
			}

			return true;
		}
	}

	s32 StringW::GetInt() const
	{
		return ::_wtoi( m_string.c_str() );
	}

	f32 StringW::GetFloat() const
	{
		return static_cast<f32>( ::_wtof( m_string.c_str() ) );
	}

	f64 StringW::GetDouble() const
	{
		return ::_wtof( m_string.c_str() );
	}

	const wchar* StringW::GetCString() const
	{
		return m_string.c_str();
	}

	StringW StringW::Left( s32 count ) const
	{
		return StringW( m_string.substr( 0, count) );
	}

	StringW StringW::Right( s32 count ) const
	{
		return StringW( m_string.substr( m_string.length() - count ) );
	}

	StringW StringW::SubString( s32 index ) const
	{
		return StringW( m_string.substr( index ) );
	}

	StringW StringW::SubString( s32 index, s32 count ) const
	{
		return StringW( m_string.substr( index, count ) );
	}

	s32 StringW::IndexOf( const StringW& str, s32 offset ) const
	{
		return static_cast<s32>( m_string.find( str.m_string, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringW::IndexOf( const wchar* str, s32 offset ) const
	{
		return static_cast<s32>( m_string.find( str, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringW::IndexOf( const std::wstring& str, s32 offset ) const
	{
		return static_cast<s32>( m_string.find( str, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringW::IndexOf( wchar c, s32 offset ) const
	{
		return static_cast<s32>( m_string.find_first_of( c, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringW::LastIndexOf( const StringW& str, s32 offset ) const
	{
		return static_cast<s32>( m_string.rfind( str.m_string, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringW::LastIndexOf( const wchar* str, s32 offset ) const
	{
		return static_cast<s32>( m_string.rfind( str, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringW::LastIndexOf( const std::wstring& str, s32 offset ) const
	{
		return static_cast<s32>( m_string.rfind( str, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringW::LastIndexOf( wchar c, s32 offset ) const
	{
		return static_cast<s32>( m_string.find_last_of( c, static_cast<std::size_t>( offset ) ) );
	}

	StringW& StringW::Remove( const StringW& str )
	{
		if( m_string.empty() || str.m_string.empty() ) { return *this; }

		std::wstring copy( m_string );
		m_string.erase();

		std::size_t prev = 0;
		std::size_t idx = copy.find( str.m_string, prev );
		while( idx != std::wstring::npos )
		{
			m_string.append( copy.substr( prev, idx - prev ) );

			prev = idx + str.m_string.length();
			idx = copy.find( str.m_string, prev );
		}
		m_string.append( copy.substr( prev ) );

		return *this;
	}

	StringW& StringW::Remove( const wchar* str )
	{
		std::size_t length = std::wcslen( str );

		if( m_string.empty() || length == 0 ) { return *this; }

		std::wstring copy( m_string );
		m_string.erase();

		std::size_t prev = 0;
		std::size_t idx = copy.find( str, prev );
		while( idx != std::wstring::npos )
		{
			m_string.append( copy.substr( prev, idx - prev ) );

			prev = idx + length;
			idx = copy.find( str, prev );
		}
		m_string.append( copy.substr( prev ) );

		return *this;
	}

	StringW& StringW::Remove( const std::wstring& str )
	{
		if( m_string.empty() || str.empty() ) { return *this; }

		std::wstring copy( m_string );
		m_string.erase();

		std::size_t prev = 0;
		std::size_t idx = copy.find( str, prev );
		while( idx != std::wstring::npos )
		{
			m_string.append( copy.substr( prev, idx - prev ) );

			prev = idx + str.length();
			idx = copy.find( str, prev );
		}
		m_string.append( copy.substr( prev ) );

		return *this;
	}

	StringW& StringW::Replace( const StringW& findStr, const StringW& replaceStr )
	{
		if( m_string.empty() || findStr.m_string.empty() ) { return *this; }

		std::wstring copy( m_string );
		m_string.erase();

		std::size_t prev = 0;
		std::size_t idx = copy.find( findStr.m_string, prev );
		while( idx != std::wstring::npos )
		{
			m_string.append( copy.substr( prev, idx - prev ) );
			m_string.append( replaceStr.m_string );

			prev = idx + findStr.m_string.length();
			idx = copy.find( findStr.m_string, prev );
		}
		m_string.append( copy.substr( prev ) );

		return *this;
	}

	StringW& StringW::Replace( const wchar* findStr, const wchar* replaceStr )
	{
		std::size_t length = std::wcslen( findStr );
		if( m_string.empty() || length == 0 ) { return *this; }

		std::wstring copy( m_string );
		m_string.erase();

		std::size_t prev = 0;
		std::size_t idx = copy.find( findStr, prev );
		while( idx != std::wstring::npos )
		{
			m_string.append( copy.substr( prev, idx - prev ) );
			m_string.append( replaceStr );

			prev = idx + length;
			idx = copy.find( findStr, prev );
		}
		m_string.append( copy.substr( prev ) );

		return *this;
	}

	StringW& StringW::Replace( const std::wstring& findStr, const std::wstring& replaceStr )
	{
		if( m_string.empty() || findStr.empty() ) { return *this; }

		std::wstring copy( m_string );
		m_string.erase();

		std::size_t prev = 0;
		std::size_t idx = copy.find( findStr, prev );
		while( idx != std::wstring::npos )
		{
			m_string.append( copy.substr( prev, idx - prev ) );
			m_string.append( replaceStr );

			prev = idx + findStr.length();
			idx = copy.find( findStr, prev );
		}
		m_string.append( copy.substr( prev ) );

		return *this;
	}

	bool StringW::StartWith( const StringW& findStr ) const
	{
		if( findStr.m_string.empty() ) { return true; }
		if( m_string.length() < findStr.m_string.length() ) { return false; }

		for( std::size_t i = 0, j = findStr.m_string.length() ; i != j ; ++i )
		{
			if( m_string[ i ] != findStr.m_string[ i ] )
			{
				return false;
			}
		}

		return true;
	}

	bool StringW::StartWith( const wchar* findStr ) const
	{
		std::size_t length = std::wcslen( findStr );
		if( length == 0 ) { return true; }
		if( m_string.length() < length ) { return false; }

		for( std::size_t i = 0, j = length ; i != j ; ++i )
		{
			if( m_string[ i ] != findStr[ i ] )
			{
				return false;
			}
		}

		return true;
	}

	bool StringW::StartWith( const std::wstring& findStr ) const
	{
		if( findStr.empty() ) { return true; }
		if( m_string.length() < findStr.length() ) { return false; }

		for( std::size_t i = 0, j = findStr.length() ; i != j ; ++i )
		{
			if( m_string[ i ] != findStr[ i ] )
			{
				return false;
			}
		}

		return true;
	}

	bool StringW::EndWith( const StringW& findStr ) const
	{
		if( findStr.m_string.empty() ) { return true; }
		if( m_string.length() < findStr.m_string.length() ) { return false; }

		std::size_t base = m_string.length() - findStr.m_string.length();
		for( std::size_t i = 0, j = findStr.m_string.length() ; i != j ; ++i )
		{
			if( m_string[ base + i ] != findStr.m_string[ i ] )
			{
				return false;
			}
		}

		return true;
	}

	bool StringW::EndWith( const wchar* findStr ) const
	{
		std::size_t length = std::wcslen( findStr );
		if( length == 0 ) { return true; }
		if( m_string.length() < length ) { return false; }

		std::size_t base = m_string.length() - length;
		for( std::size_t i = 0, j = length ; i != j ; ++i )
		{
			if( m_string[ base + i ] != findStr[ i ] )
			{
				return false;
			}
		}

		return true;
	}

	bool StringW::EndWith( const std::wstring& findStr ) const
	{
		if( findStr.empty() ) { return true; }
		if( m_string.length() < findStr.length() ) { return false; }

		std::size_t base = m_string.length() - findStr.length();
		for( std::size_t i = 0, j = findStr.length() ; i != j ; ++i )
		{
			if( m_string[ base + i ] != findStr[ i ] )
			{
				return false;
			}
		}

		return true;
	}

	StringW& StringW::Resize( s32 size )
	{
		m_string.resize( static_cast<std::size_t>( size ) );
		return *this;
	}

	s32 StringW::GetLength() const
	{
		return static_cast<s32>( m_string.length() );
	}

	bool StringW::IsEmpty() const
	{
		return m_string.empty();
	}	
}
