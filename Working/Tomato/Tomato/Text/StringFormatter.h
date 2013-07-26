#pragma once

namespace Tomato
{
	class TOMATO_API StringFormatterW
	{
	public:
		StringFormatterW( const wchar* fmt, ... );
		~StringFormatterW();

	public:
		const wchar* GetString() const { return m_pBuffer; }

	private:
		wchar* m_pBuffer;
	};

	class TOMATO_API StringFormatterA
	{
	public:
		StringFormatterA( const char* fmt, ... );
		~StringFormatterA();

	public:
		const char* GetString() const { return m_pBuffer; }

	private:
		char* m_pBuffer;
	};

#ifdef UNICODE
	typedef StringFormatterW StringFormatter;
#else
	typedef StringFormatterA StringFormatter;
#endif

}