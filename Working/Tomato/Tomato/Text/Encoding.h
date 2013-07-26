#pragma once

namespace Tomato
{
	struct Encoding
	{
		enum Type
		{
			None = 0,

#ifdef _UNICODE
			// Unicode
			Default = 1200,	
#else
			// ANSI
			Default = 0,
#endif

			// ANSI
			ANSI = 0,

			// Unicode
			UTF7 = 65000,
			UTF8 = 65001,
			Unicode = 1200,				// UTF16
			UnicodeBigEndian = 1201,	// UTF16

			FORCEDWORD = 0x7FFFFFFF
		};
	};
}