#include "TomatoPCH.h"

#include "Diagnostics.h"

#include <cassert>

namespace Tomato
{
#ifdef _DEBUG
	void Assert( bool expression )
	{
		assert( expression );
	}
#endif
}