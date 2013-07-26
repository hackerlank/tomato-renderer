#pragma once

namespace Tomato
{
	class StringTokenizerA : public StringTokenizerT<StringA, char>
	{
	public:
		StringTokenizerA( const StringA& text )
			: StringTokenizerT( text )
		{
		}
	};

}