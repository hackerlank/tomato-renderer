#pragma once

namespace Tomato
{
#ifdef UNICODE
	typedef StringTokenizerW StringTokenizer;
#else
	typedef StringTokenizerA StringTokenizer;
#endif
}