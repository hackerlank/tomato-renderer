#include "TomatoPCH.h"

#include "StringFormatter.h"

#include <cstdarg>

namespace Tomato
{
	StringFormatterW::StringFormatterW( const wchar* formatString, ... )
	{
		va_list marker;
		va_start( marker, formatString );

		s32 length = _vscwprintf( formatString, marker ) + 1;
		m_pBuffer = new wchar[ length ];

		vswprintf_s( m_pBuffer, length, formatString, marker );
		
		va_end( marker );
	}	

	StringFormatterW::~StringFormatterW()
	{
		DELETE_ARRAY_AND_SET_NULL( m_pBuffer );
	}

	StringFormatterA::StringFormatterA( const char* formatString, ... )
	{
		va_list marker;
		va_start( marker, formatString );

		s32 length = _vscprintf( formatString, marker ) + 1;
		m_pBuffer = new char[ length ];

		vsprintf_s( m_pBuffer, length, formatString, marker );

		va_end( marker );
	}

	StringFormatterA::~StringFormatterA()
	{
		DELETE_ARRAY_AND_SET_NULL( m_pBuffer );
	}
}