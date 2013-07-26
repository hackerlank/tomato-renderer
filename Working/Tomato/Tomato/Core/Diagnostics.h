#pragma once

namespace Tomato
{
#ifdef _DEBUG
	void TOMATO_API Assert( bool expression );
#else
#define Assert(expression) ((void)0)
#endif
}