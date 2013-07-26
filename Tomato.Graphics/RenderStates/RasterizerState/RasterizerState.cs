using System;

namespace Tomato.Graphics
{
	public class RasterizerState : RenderState
	{
		public FillMode FillMode { get; set; }

		public CullMode CullMode { get; set; }

		public bool IsCounterClockwiseFrontFace { get; set; }

		public int DepthBias { get; set; }

		public float DepthBiasClamp { get; set; }

		public float SlopeScaleDepthBias { get; set; }

		public bool IsDepthClipEnabled { get; set; }

		public bool IsScissorEnabled { get; set; }

		public bool IsMultiSampleEnabled { get; set; }

		public bool IsAntiAliasedLineEnabled { get; set; }

		public RasterizerState()
			: base( RenderStateType.Rasterizer )
		{
			FillMode = FillMode.Solid;
			CullMode = CullMode.Back;
			IsCounterClockwiseFrontFace = false;
			DepthBias = 0;
			DepthBiasClamp = 0;
			SlopeScaleDepthBias = 0;
			IsDepthClipEnabled = true;
			IsScissorEnabled = false;
			IsMultiSampleEnabled = false;
			IsAntiAliasedLineEnabled = false;
		}

		public override object Clone()
		{
			return new RasterizerState
			{
				FillMode = this.FillMode,
				CullMode = this.CullMode,
				IsCounterClockwiseFrontFace = this.IsCounterClockwiseFrontFace,
				DepthBias = this.DepthBias,
				DepthBiasClamp = this.DepthBiasClamp,
				SlopeScaleDepthBias = this.SlopeScaleDepthBias,
				IsDepthClipEnabled = this.IsDepthClipEnabled,
				IsScissorEnabled = this.IsScissorEnabled,
				IsMultiSampleEnabled = this.IsMultiSampleEnabled,
				IsAntiAliasedLineEnabled = this.IsAntiAliasedLineEnabled
			};
		}
	}
}