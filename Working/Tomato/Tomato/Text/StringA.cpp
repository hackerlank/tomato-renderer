#include "TomatoPCH.h"

#include "StringA.h"

#include <cctype>
#include <cstdlib>
#include <algorithm>
#include <cstring>

namespace Tomato
{
	StringA::StringA()
		: m_string()
	{
	}

	StringA::StringA( char c, s32 count )
		: m_string( count, c )
	{
	}

	StringA::StringA( const StringA& str )
		: m_string( str.m_string )
	{
	}

	StringA::StringA( const char* str )
		: m_string( ( str != NULL ) ? str : "" )
	{
	}

	StringA::StringA( const std::string& str )
		: m_string( str )
	{
	}

	bool StringA::operator ==( const StringA& str ) const
	{
		return m_string.compare( str.m_string ) == 0;
	}

	bool StringA::operator ==( const char* str ) const
	{
		return m_string.compare( str ) == 0;
	}

	bool StringA::operator ==( const std::string& str ) const
	{
		return m_string.compare( str ) == 0;
	}

	bool StringA::operator !=( const StringA& str ) const
	{
		return m_string.compare( str.m_string ) != 0;
	}

	bool StringA::operator !=( const char* str ) const
	{
		return m_string.compare( str ) != 0;
	}

	bool StringA::operator !=( const std::string& str ) const
	{
		return m_string.compare( str ) != 0;
	}

	bool StringA::operator <( const StringA& str ) const
	{
		return m_string.compare( str.m_string ) < 0;
	}

	bool StringA::operator <( const char* str ) const
	{
		return m_string.compare( str ) < 0;
	}

	bool StringA::operator <( const std::string& str ) const
	{
		return m_string.compare( str ) < 0;
	}
	
	bool StringA::operator >( const StringA& str ) const
	{
		return m_string.compare( str.m_string ) > 0;
	}

	bool StringA::operator >( const char* str ) const
	{
		return m_string.compare( str ) > 0;
	}

	bool StringA::operator >( const std::string& str ) const
	{
		return m_string.compare( str ) > 0;
	}

	bool StringA::operator <=( const StringA& str ) const
	{
		return m_string.compare( str.m_string ) <= 0;
	}

	bool StringA::operator <=( const char* str ) const
	{
		return m_string.compare( str ) <= 0;
	}

	bool StringA::operator <=( const std::string& str ) const
	{
		return m_string.compare( str ) <= 0;
	}

	bool StringA::operator >=( const StringA& str ) const
	{
		return m_string.compare( str.m_string ) >= 0;
	}

	bool StringA::operator >=( const char* str ) const
	{
		return m_string.compare( str ) >= 0;
	}

	bool StringA::operator >=( const std::string& str ) const
	{
		return m_string.compare( str ) >= 0;
	}

	StringA& StringA::operator =( const StringA& str )
	{
		m_string = str.m_string;
		return *this;
	}

	StringA& StringA::operator =( const char* str )
	{
		m_string = str;
		return *this;
	}

	StringA& StringA::operator =( const std::string& str )
	{
		m_string = str;
		return *this;
	}

	StringA& StringA::operator +=( const StringA& str )
	{
		m_string += str.m_string;
		return *this;
	}

	StringA& StringA::operator +=( const char* str )
	{
		m_string += str;
		return *this;
	}

	StringA& StringA::operator +=( const std::string& str )
	{
		m_string += str;
		return *this;
	}

	StringA StringA::operator +( const StringA& str ) const
	{
		StringA result( m_string );
		result.m_string.append( str.m_string );
		return result;
	}

	StringA StringA::operator +( const char* str ) const
	{
		StringA result( m_string );
		result.m_string.append( str );
		return result;
	}

	StringA StringA::operator +( const std::string& str ) const
	{
		StringA result( m_string );
		result.m_string.append( str );
		return result;
	}

	char& StringA::operator []( s32 index ) 
	{
		return m_string[ index ];
	}

	const char& StringA::operator []( s32 index ) const
	{
		return m_string[ index ];
	}

	StringA& StringA::Append( const StringA& str )
	{
		m_string.append( str.m_string );
		return *this;
	}

	StringA& StringA::Append( const char* str )
	{
		m_string.append( str );
		return *this;
	}

	StringA& StringA::Append( const std::string& str )
	{
		m_string.append( str );
		return *this;
	}

	StringA& StringA::Trim()
	{
		TrimStart();
		TrimEnd();
		return *this;
	}

	StringA& StringA::TrimStart()
	{
#if 1
		if( m_string.empty() ) { return *this; }

		for( std::size_t i = 0, j = m_string.length() ; i != j ; ++i )
		{
			if( !std::isspace( m_string[ i ] ) )
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
			std::find_if( m_string.begin(), m_string.end(), std::not1( std::ptr_fun<s32, s32>( std::isspace ) ) ) );
		return *this;
#endif
	}

	StringA& StringA::TrimEnd()
	{
#if 1
		if( m_string.empty() ) { return *this; }

		for( std::size_t i = m_string.length() - 1 ; i != std::string::npos ; --i )
		{
			if( !std::isspace( m_string[ i ] ) )
			{
				m_string.erase( i + 1 );
				return *this;
			}
		}

		m_string.erase();
		return *this;
#else
		m_string.erase( 
			std::find_if( m_string.rbegin(), m_string.rend(), std::not1( std::ptr_fun<s32, s32>( std::isspace ) ) ).base(), 
			m_string.end() );
		return *this;
#endif
	}

	StringA& StringA::RemoveWhiteSpaces()
	{
		m_string.erase( std::remove_if( m_string.begin(), m_string.end(), std::ptr_fun<s32, s32>( std::isspace ) ), m_string.end() );
		return *this;
	}

	StringA StringA::ToUpper( StringA source )
	{
		for( std::size_t i = 0, j = source.m_string.length() ; i != j ; ++i )
		{
			source.m_string[ i ] = static_cast<char>( std::toupper( source.m_string[ i ] ) );
		}

		return source;
	}

	StringA StringA::ToUpper( const char* pSource )
	{
		std::size_t length = std::strlen( pSource );

		StringA result( ' ', length );
		for( std::size_t i = 0, j = length ; i != j ; ++i )
		{
			result.m_string[ i ] = static_cast<char>( std::toupper( pSource[ i ] ) );
		}

		return result;
	}

	StringA StringA::ToLower( StringA source )
	{
		for( std::size_t i = 0, j = source.m_string.length() ; i != j ; ++i )
		{
			source.m_string[ i ] = static_cast<char>( std::tolower( source.m_string[ i ] ) );
		}

		return source;
	}

	StringA StringA::ToUpper( const std::string& source )
	{
		StringA result( ' ', source.length() );
		for( std::size_t i = 0, j = source.length() ; i != j ; ++i )
		{
			result.m_string[ i ] = static_cast<char>( std::toupper( source[ i ] ) );
		}

		return result;
	}

	StringA StringA::ToLower( const char* pSource )
	{
		std::size_t length = std::strlen( pSource );

		StringA result( ' ', length );
		for( std::size_t i = 0, j = length ; i != j ; ++i )
		{
			result.m_string[ i ] = static_cast<char>( std::tolower( pSource[ i ] ) );
		}

		return result;
	}

	StringA StringA::ToLower( const std::string& source )
	{
		StringA result( ' ', source.length() );
		for( std::size_t i = 0, j = source.length() ; i != j ; ++i )
		{
			result.m_string[ i ] = static_cast<char>( std::tolower( source[ i ] ) );
		}

		return result;
	}

	s32 StringA::CompareTo( const StringA& str ) const
	{
		return m_string.compare( str.m_string );
	}

	s32 StringA::CompareTo( const char* str ) const
	{
		return m_string.compare( str );
	}

	s32 StringA::CompareTo( const std::string& str ) const
	{
		return m_string.compare( str );
	}

	s32 StringA::Compare( const StringA& str1, const StringA& str2 )
	{
		return str1.m_string.compare( str2.m_string );
	}

	s32 StringA::Compare( const char* str1, const char* str2 )
	{
		return std::strcmp( str1, str2 );
	}

	s32 StringA::Compare( const std::string& str1, const std::string& str2 )
	{
		return str1.compare( str2 );
	}

	bool StringA::EqualTo( const StringA& str ) const
	{
		return m_string == str.m_string;
	}

	bool StringA::EqualTo( const char* str ) const
	{
		return m_string.compare( str ) == 0;
	}

	bool StringA::EqualTo( const std::string& str ) const
	{
		return m_string == str;
	}

	bool StringA::EqualTo( const StringA& str, bool bCaseSensitive ) const
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
				if( std::tolower( m_string[ i ] ) != std::tolower( str.m_string[ i ] ) )
				{
					return false;
				}
			}

			return true;
		}
	}

	bool StringA::EqualTo( const char* str, bool bCaseSensitive ) const
	{
		if( bCaseSensitive )
		{
			return m_string.compare( str ) == 0;
		}
		else
		{
			std::size_t length = std::strlen( str );
			if( m_string.length() != length  )
			{
				return false;
			}

			for( std::size_t i = 0, j = m_string.length() ; i != j ; ++i )
			{
				if( std::tolower( m_string[ i ] ) != std::tolower( str[ i ] ) )
				{
					return false;
				}
			}

			return true;
		}
	}

	bool StringA::EqualTo( const std::string& str, bool bCaseSensitive ) const
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
				if( std::tolower( m_string[ i ] ) != std::tolower( str[ i ] ) )
				{
					return false;
				}
			}

			return true;
		}
	}

	s32 StringA::GetInt() const
	{
		return ::atoi( m_string.c_str() );
	}

	f32 StringA::GetFloat() const
	{
		return static_cast<f32>( ::atof( m_string.c_str() ) );
	}

	f64 StringA::GetDouble() const
	{
		return ::atof( m_string.c_str() );
	}

	const char* StringA::GetCString() const
	{
		return m_string.c_str();
	}

	StringA StringA::Left( s32 count ) const
	{
		return StringA( m_string.substr( 0, count) );
	}

	StringA StringA::Right( s32 count ) const
	{
		return StringA( m_string.substr( m_string.length() - count ) );
	}

	StringA StringA::SubString( s32 index ) const
	{
		return StringA( m_string.substr( index ) );
	}

	StringA StringA::SubString( s32 index, s32 count ) const
	{
		return StringA( m_string.substr( index, count ) );
	}

	s32 StringA::IndexOf( const StringA& str, s32 offset ) const
	{
		return static_cast<s32>( m_string.find( str.m_string, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringA::IndexOf( const char* str, s32 offset ) const
	{
		return static_cast<s32>( m_string.find( str, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringA::IndexOf( const std::string& str, s32 offset ) const
	{
		return static_cast<s32>( m_string.find( str, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringA::IndexOf( char c, s32 offset ) const
	{
		return static_cast<s32>( m_string.find_first_of( c, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringA::LastIndexOf( const StringA& str, s32 offset ) const
	{
		return static_cast<s32>( m_string.rfind( str.m_string, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringA::LastIndexOf( const char* str, s32 offset ) const
	{
		return static_cast<s32>( m_string.rfind( str, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringA::LastIndexOf( const std::string& str, s32 offset ) const
	{
		return static_cast<s32>( m_string.rfind( str, static_cast<std::size_t>( offset ) ) );
	}

	s32 StringA::LastIndexOf( char c, s32 offset ) const
	{
		return static_cast<s32>( m_string.find_last_of( c, static_cast<std::size_t>( offset ) ) );
	}

	StringA& StringA::Remove( const StringA& str )
	{
		if( m_string.empty() || str.m_string.empty() ) { return *this; }

		std::string copy( m_string );
		m_string.erase();

		std::size_t prev = 0;
		std::size_t idx = copy.find( str.m_string, prev );
		while( idx != std::string::npos )
		{
			m_string.append( copy.substr( prev, idx - prev ) );

			prev = idx + str.m_string.length();
			idx = copy.find( str.m_string, prev );
		}
		m_string.append( copy.substr( prev ) );

		return *this;
	}

	StringA& StringA::Remove( const char* str )
	{
		std::size_t length = std::strlen( str );

		if( m_string.empty() || length == 0 ) { return *this; }

		std::string copy( m_string );
		m_string.erase();

		std::size_t prev = 0;
		std::size_t idx = copy.find( str, prev );
		while( idx != std::string::npos )
		{
			m_string.append( copy.substr( prev, idx - prev ) );

			prev = idx + length;
			idx = copy.find( str, prev );
		}
		m_string.append( copy.substr( prev ) );

		return *this;
	}

	StringA& StringA::Remove( const std::string& str )
	{
		if( m_string.empty() || str.empty() ) { return *this; }

		std::string copy( m_string );
		m_string.erase();

		std::size_t prev = 0;
		std::size_t idx = copy.find( str, prev );
		while( idx != std::string::npos )
		{
			m_string.append( copy.substr( prev, idx - prev ) );

			prev = idx + str.length();
			idx = copy.find( str, prev );
		}
		m_string.append( copy.substr( prev ) );

		return *this;
	}

	StringA& StringA::Replace( const StringA& findStr, const StringA& replaceStr )
	{
		if( m_string.empty() || findStr.m_string.empty() ) { return *this; }

		std::string copy( m_string );
		m_string.erase();

		std::size_t prev = 0;
		std::size_t idx = copy.find( findStr.m_string, prev );
		while( idx != std::string::npos )
		{
			m_string.append( copy.substr( prev, idx - prev ) );
			m_string.append( replaceStr.m_string );

			prev = idx + findStr.m_string.length();
			idx = copy.find( findStr.m_string, prev );
		}
		m_string.append( copy.substr( prev ) );

		return *this;
	}

	StringA& StringA::Replace( const char* findStr, const char* replaceStr )
	{
		std::size_t length = std::strlen( findStr );
		if( m_string.empty() || length == 0 ) { return *this; }

		std::string copy( m_string );
		m_string.erase();

		std::size_t prev = 0;
		std::size_t idx = copy.find( findStr, prev );
		while( idx != std::string::npos )
		{
			m_string.append( copy.substr( prev, idx - prev ) );
			m_string.append( replaceStr );

			prev = idx + length;
			idx = copy.find( findStr, prev );
		}
		m_string.append( copy.substr( prev ) );

		return *this;
	}

	StringA& StringA::Replace( const std::string& findStr, const std::string& replaceStr )
	{
		if( m_string.empty() || findStr.empty() ) { return *this; }

		std::string copy( m_string );
		m_string.erase();

		std::size_t prev = 0;
		std::size_t idx = copy.find( findStr, prev );
		while( idx != std::string::npos )
		{
			m_string.append( copy.substr( prev, idx - prev ) );
			m_string.append( replaceStr );

			prev = idx + findStr.length();
			idx = copy.find( findStr, prev );
		}
		m_string.append( copy.substr( prev ) );

		return *this;
	}

	bool StringA::StartWith( const StringA& findStr ) const
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

	bool StringA::StartWith( const char* findStr ) const
	{
		std::size_t length = std::strlen( findStr );
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

	bool StringA::StartWith( const std::string& findStr ) const
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

	bool StringA::EndWith( const StringA& findStr ) const
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

	bool StringA::EndWith( const char* findStr ) const
	{
		std::size_t length = std::strlen( findStr );
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

	bool StringA::EndWith( const std::string& findStr ) const
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

	StringA& StringA::Resize( s32 size )
	{
		m_string.resize( static_cast<std::size_t>( size ) );
		return *this;
	}

	s32 StringA::GetLength() const
	{
		return static_cast<s32>( m_string.length() );
	}

	bool StringA::IsEmpty() const
	{
		return m_string.empty();
	}	
}
