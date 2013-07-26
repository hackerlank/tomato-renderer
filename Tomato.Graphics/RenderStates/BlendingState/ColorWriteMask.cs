using System;

namespace Tomato.Graphics
{
	[Flags]
	public enum ColorWriteMask
	{
		None = 0,

		Red = 1,
		Green = 2,
		Blue = 4,
		Alpha = 8,

		All = Red | Green | Blue | Alpha
	}
}