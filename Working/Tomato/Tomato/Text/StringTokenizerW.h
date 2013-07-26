#pragma once

namespace Tomato
{
	class StringTokenizerW : public StringTokenizerT<StringW, wchar>
	{
	public:
		StringTokenizerW( const StringW& text )
			: StringTokenizerT( text )
		{
		}
	};
}