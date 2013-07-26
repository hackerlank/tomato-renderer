using System;

namespace Tomato.Graphics
{
	public class BlendingState : RenderState
	{
		public const uint MaximumRenderTarget = 8;

		private bool[] m_bRenderTargetsBlendingEnabled = new bool[ MaximumRenderTarget ];
		private ColorWriteMask[] m_renderTargetsWriteMasks = new ColorWriteMask[ MaximumRenderTarget ];

		public bool IsAlphaTestingEnabled { get; set; }

		public BlendingMode SourceBlending { get; set; }
		public BlendingMode DestinationBlending { get; set; }
		public BlendingOperation BlendingOperation { get; set; }

		public BlendingMode SourceAlphaBlending { get; set; }
		public BlendingMode DestinationAlphaBlending { get; set; }
		public BlendingOperation AlphaBlendingOperation { get; set; }

		public bool IsAlphaToCoverageEnabled { get; set; }

		public BlendingState()
			: base( RenderStateType.Blending )
		{
			for( uint i = 0 ; i < MaximumRenderTarget ; ++i )
			{
				m_bRenderTargetsBlendingEnabled[ i ] = false;
				m_renderTargetsWriteMasks[ i ] = ColorWriteMask.All;
			}

			SourceBlending = BlendingMode.One;
			DestinationBlending = BlendingMode.Zero;
			BlendingOperation = BlendingOperation.Add;

			SourceAlphaBlending = BlendingMode.One;
			DestinationAlphaBlending = BlendingMode.Zero;
			AlphaBlendingOperation = BlendingOperation.Add;

			IsAlphaTestingEnabled = false;
		}

		public void EnableRenderTargetBlending( uint renderTargetIndex, bool bEnabled )
		{
			System.Diagnostics.Debug.Assert( renderTargetIndex < MaximumRenderTarget );

			m_bRenderTargetsBlendingEnabled[ renderTargetIndex ] = bEnabled;
		}

		public bool IsRenderTargetBlendingEnabled( uint renderTargetIndex )
		{
			System.Diagnostics.Debug.Assert( renderTargetIndex < MaximumRenderTarget );

			return m_bRenderTargetsBlendingEnabled[ renderTargetIndex ];
		}

		public void SetRenderTargetWriteMask( uint renderTargetIndex, ColorWriteMask mask )
		{
			System.Diagnostics.Debug.Assert( renderTargetIndex < MaximumRenderTarget );

			m_renderTargetsWriteMasks[ renderTargetIndex ] = mask;
		}

		public ColorWriteMask GetRenderTargetWriteMask( uint renderTargetIndex )
		{
			System.Diagnostics.Debug.Assert( renderTargetIndex < MaximumRenderTarget );

			return m_renderTargetsWriteMasks[ renderTargetIndex ];
		}

		public override object Clone()
		{
			BlendingState clone = new BlendingState
			{
				SourceBlending = this.SourceBlending,
				DestinationBlending = this.DestinationBlending,
				BlendingOperation = this.BlendingOperation,

				SourceAlphaBlending = this.SourceAlphaBlending,
				DestinationAlphaBlending = this.DestinationAlphaBlending,
				AlphaBlendingOperation = this.AlphaBlendingOperation,

				IsAlphaTestingEnabled = this.IsAlphaTestingEnabled
			};

			for( uint i = 0 ; i < MaximumRenderTarget ; ++i )
			{
				clone.m_bRenderTargetsBlendingEnabled[ i ] = this.m_bRenderTargetsBlendingEnabled[ i ];
				clone.m_renderTargetsWriteMasks[ i ] = this.m_renderTargetsWriteMasks[ i ];
			}

			return clone;
		}
	}
}
