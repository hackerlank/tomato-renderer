#pragma once

namespace Tomato
{
#ifdef UNICODE
	typedef StringW String;
#else
	typedef StringA String;
#endif
}
