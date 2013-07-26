using System;

namespace Tomato.Graphics
{
	public class RenderStateSet
	{
		private RenderState[] m_states = null;

		public RenderStateSet()
		{
			m_states = new RenderState[ ( int )( RenderStateType.Count ) ];
		}

		public void SetState( RenderState state )
		{
			m_states[ ( int )( state.Type ) ] = state;
		}

		public void SetBlendingState( BlendingState state )
		{
			m_states[ ( int )( RenderStateType.Blending ) ] = state;
		}

		public void SetRasterizerState( RasterizerState state )
		{
			m_states[ ( int )( RenderStateType.Rasterizer ) ] = state;
		}

		public void SetDepthStencilState( DepthStencilState state )
		{
			m_states[ ( int )( RenderStateType.DepthStencil ) ] = state;
		}

		public void SetObjectMaterialState( ObjectMaterialState state )
		{
			m_states[ ( int )( RenderStateType.ObjectMaterial ) ] = state;
		}

		public RenderState GetState( RenderStateType stateType )
		{
			return m_states[ ( int )( stateType ) ];
		}

		public BlendingState GetBlendingState()
		{
			return ( BlendingState )( m_states[ ( int )( RenderStateType.Blending ) ] );
		}

		public RasterizerState GetRasterizerState()
		{
			return ( RasterizerState )( m_states[ ( int )( RenderStateType.Rasterizer ) ] );
		}

		public DepthStencilState GetDepthStencilState()
		{
			return ( DepthStencilState )( m_states[ ( int )( RenderStateType.DepthStencil ) ] );
		}

		public ObjectMaterialState GetObjectMaterialState()
		{
			return ( ObjectMaterialState )( m_states[ ( int )( RenderStateType.ObjectMaterial ) ] );
		}
	}
}
