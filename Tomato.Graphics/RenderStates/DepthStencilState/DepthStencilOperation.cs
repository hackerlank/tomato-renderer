using System;

namespace Tomato.Graphics
{
	public class DepthStencilOperation : ICloneable
	{
		public StencilOperation StencilFailOperation { get; set; }
		public StencilOperation StencilDepthFailOperation { get; set; }
		public StencilOperation StencilPassOperation { get; set; }
		public CompareFunction StencilFunction { get; set; }

		public DepthStencilOperation()
		{
			StencilFailOperation = StencilOperation.Keep;
			StencilDepthFailOperation = StencilOperation.Keep;
			StencilPassOperation = StencilOperation.Keep;
			StencilFunction = CompareFunction.AlwaysPass;
		}

		public object Clone()
		{
			return new DepthStencilOperation
			{
				StencilFailOperation = this.StencilFailOperation,
				StencilDepthFailOperation = this.StencilDepthFailOperation,
				StencilPassOperation = this.StencilPassOperation,
				StencilFunction = this.StencilFunction
			};
		}
	}
}