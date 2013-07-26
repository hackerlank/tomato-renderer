using System;

namespace Tomato.Graphics
{
	public class DepthStencilState : RenderState
	{
		public bool IsDepthEnabled { get; set; }
		public DepthWriteMask DepthWriteMask { get; set; }
		public CompareFunction DepthFunction { get; set; }

		public bool IsStencilEnabled { get; set; }
		public byte StencilReadMask { get; set; }
		public byte StencilWriteMask { get; set; }

		public DepthStencilOperation FrontFaceOperation { get; set; }
		public DepthStencilOperation BackFaceOperation { get; set; }

		public DepthStencilState()
			: base( RenderStateType.DepthStencil )
		{
			IsDepthEnabled = true;
			DepthWriteMask = DepthWriteMask.All;
			DepthFunction = CompareFunction.Less;

			IsStencilEnabled = false;
			StencilReadMask = 0;
			StencilWriteMask = 0;

			FrontFaceOperation = new DepthStencilOperation();
			BackFaceOperation = new DepthStencilOperation();
		}

		public override object Clone()
		{
			return new DepthStencilState
			{
				IsDepthEnabled = this.IsDepthEnabled,
				DepthWriteMask = this.DepthWriteMask,
				DepthFunction = this.DepthFunction,

				IsStencilEnabled = this.IsStencilEnabled,
				StencilReadMask = this.StencilReadMask,
				StencilWriteMask = this.StencilWriteMask,

				FrontFaceOperation = ( DepthStencilOperation )( this.FrontFaceOperation.Clone() ),
				BackFaceOperation = ( DepthStencilOperation )( this.BackFaceOperation.Clone() )
			};
		}
	}
}
