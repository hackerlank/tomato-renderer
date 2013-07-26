using System;

namespace Tomato.Graphics
{
	public enum StencilOperation
	{
		Keep,
		Zero,
		Replace,
		IncrementSaturate,
		DecrementSaturate,
		Invert,
		IncrementWrap,
		DecrementWrap
	}
}