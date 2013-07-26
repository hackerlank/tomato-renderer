using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Tomato.Graphics
{
	[DebuggerDisplay( "FullScreenRenderPass: Name={Name}" )]
	public class FullScreenRenderPass : RenderPass
	{
		private VertexBuffer m_vertexBuffer = null;
		private IndexBuffer m_indexBuffer = null;

		private Effect m_effect = null;
		private string m_effectIdentifier = null;

		private int m_lastRenderTargetWidth = 0;
		private int m_lastRenderTargetHeight = 0;

		[Browsable( false )]
		public Effect Effect
		{
			get { return m_effect; }
		}

		[Browsable( false )]
		public string EffectIdentifier
		{
			get { return m_effectIdentifier; }
			protected set { m_effectIdentifier = value; }
		}
				
		public FullScreenRenderPass( Renderer renderer, string name, Effect effect )
			: this( renderer, name, effect, null )
		{
		}

		public FullScreenRenderPass( Renderer renderer, string name, Effect effect, string effectIdentifier )
			: base( renderer, name )
		{
			// Effect
			m_effect = effect;
			m_effectIdentifier = effectIdentifier;

			// Here we don't create the vertex buffer.
			// The vertex buffer is created right before the first rendering frame.
			m_lastRenderTargetWidth = 0;
			m_lastRenderTargetHeight = 0;
			m_vertexBuffer = null;

			// Create index buffer
			m_indexBuffer = Renderer.CreateIndexBuffer<ushort>(
				string.Format( "IndexBuffer: FullScreenRenderPass[{0}]", Name ),
				IndexElementSize.SixteenBits,
				6,
				BufferUsage.WriteOnly,
				GetFullScreenQuadIndices() );
		}

		protected override void OnRender( TimeSpan elapsedTime )
		{
			// Test if the render-target dimension has changed.
			int renderTargetWidth, renderTargetHeight;
			Renderer.GetCurrentRenderTargetDimension( out renderTargetWidth, out renderTargetHeight );
			if( ( renderTargetWidth != m_lastRenderTargetWidth )
				|| ( renderTargetHeight != m_lastRenderTargetHeight ) )
			{
				// Recreate the vertex buffer.
				if( m_vertexBuffer != null )
				{
					// I just wanted to refill data of vertex buffer. I don't want to dispose it.
					// However, when calling just SetData<T>() function,
					// Exception saying 'Can't SetData() on currently set VertexBuffer has raised.
					m_vertexBuffer.Dispose();
				}

				m_vertexBuffer = Renderer.CreateVertexBuffer<VertexPositionTexture>(
					string.Format( "VertexBuffer: FullScreenRenderPass[{0}]", Name ),
					VertexPositionTexture.VertexDeclaration,
					4,
					BufferUsage.WriteOnly,
					GetFullScreenQuadVertices( renderTargetWidth, renderTargetHeight ) );
			}

			// Set additional textures.
			if( Textures != null )
			{
				Textures.SetToRenderer( Renderer );
			}

			// Apply an effect pass.
			Renderer.ApplyEffect( m_effect, m_effectIdentifier );

			// Draw.
			Renderer.Render( m_vertexBuffer, 0, m_indexBuffer, PrimitiveType.TriangleList, 4, 0, 2 );

			// Store the viewport dimension.
			m_lastRenderTargetWidth = renderTargetWidth;
			m_lastRenderTargetHeight = renderTargetHeight;
		}

		private VertexPositionTexture[] GetFullScreenQuadVertices( int width, int height )
		{
			// Compute half-texel shifting length.
			float inverseHalfWidth = 0.5f / ( float )( width );
			float inverseHalfHeight = 0.5f / ( float )( height );

			// 1 -- 2
			// |    |
			// 0 -- 3
			VertexPositionTexture[] vertices = new VertexPositionTexture[]
			{
				new VertexPositionTexture { Position = new Vector3( -1, -1, 0 ), TextureCoordinate = new Vector2( 0 + inverseHalfWidth, 1  + inverseHalfHeight ) },
				new VertexPositionTexture { Position = new Vector3( -1, +1, 0 ), TextureCoordinate = new Vector2( 0 + inverseHalfWidth, 0  + inverseHalfHeight ) },
				new VertexPositionTexture { Position = new Vector3( +1, +1, 0 ), TextureCoordinate = new Vector2( 1 + inverseHalfWidth, 0  + inverseHalfHeight ) },
				new VertexPositionTexture { Position = new Vector3( +1, -1, 0 ), TextureCoordinate = new Vector2( 1 + inverseHalfWidth, 1  + inverseHalfHeight ) }
			};

			return vertices;
		}

		private ushort[] GetFullScreenQuadIndices()
		{
			ushort[] indices = new ushort[] { 0, 1, 2, 3, 0, 2 }; // clockwise order
			return indices;
		}
	}
}
