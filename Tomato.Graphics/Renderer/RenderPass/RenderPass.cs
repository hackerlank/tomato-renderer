using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics
{
	[DebuggerDisplay( "RenderPass: Name={Name}" )]
	public abstract class RenderPass
	{
		private Renderer m_renderer = null;
		
		private string[] m_renderTargetNames = null;

		[Browsable( false )]
		public event Action<RenderPass> RenderStarting = null;

		[Browsable( false )]
		public event Action<RenderPass> RenderFinished = null;

		[Category( "Render Pass" )]
		[DisplayName( "Enabled" )]
		public bool IsEnabled { get; set; }

		[Category( "Render Pass" )]
		[DisplayName( "Name" )]
		public string Name { get; private set; }

		[Browsable( false )]
		public Renderer Renderer { get { return m_renderer; } }

		[Category( "Clear Options" )]
		[DisplayName( "Clear Color" )]
		public bool IsClearingColor { get; set; }

		[Category( "Clear Options" )]
		[DisplayName( "Color Depth-Stencil" )]
		public bool IsClearingDepthStencil { get; set; }

		[Category( "Clear Options" )]
		[DisplayName( "Color Value" )]
		public Color ClearingColor { get; set; }

		[Category( "Clear Options" )]
		[DisplayName( "Depth Value" )]
		public float ClearingDepth { get; set; }

		[Category( "Clear Options" )]
		[DisplayName( "Stencil Value" )]
		public int ClearingStencil { get; set; }
		
		/// <summary>
		/// Gets or sets the fixed-size array of render-target names.
		/// </summary>
		[Browsable( false )]
		public string[] RenderTargets
		{
			get { return m_renderTargetNames; }
			set
			{
				if( value == null )
				{
					for( int i = 0 ; i < Renderer.MaximumRenderTargetCount ; ++i )
					{
						m_renderTargetNames[ i ] = null;
					}
				}
				else
				{
					if( ( value.Length < 1 )
						|| ( value.Length > Renderer.MaximumRenderTargetCount ) )
					{
						throw new InvalidOperationException( "Invalid number of render-targets were passed." );
					}

					for( int i = 0 ; i < value.Length ; ++i )
					{
						if( string.IsNullOrWhiteSpace( value[ i ] ) )
						{
							throw new InvalidOperationException( "The render-target name cannot be null or white-space." );
						}

						m_renderTargetNames[ i ] = value[ i ];
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets a single render-target name.
		/// Note that if you set the single render-target using this property, other render-targets are set to null.
		/// </summary>
		[Browsable( false )]
		public string RenderTarget
		{
			get 
			{
				return m_renderTargetNames[ 0 ];
			}
			set
			{
				// Set the main render-target name.
				m_renderTargetNames[ 0 ] = value;

				// Set other render-targets to null.
				for( int i = 1 ; i < Renderer.MaximumRenderTargetCount ; ++i )
				{
					m_renderTargetNames[ i ] = null;
				}
			}
		}

		/// <summary>
		/// Gets the first render-target name.
		/// </summary>
		[Category( "Render Target" )]
		[DisplayName( "RenderTarget0" )]
		public string RenderTarget0
		{
			get
			{
				return string.IsNullOrWhiteSpace( m_renderTargetNames[ 0 ] ) ? "(Not Set)" : m_renderTargetNames[ 0 ];
			}
		}

		/// <summary>
		/// Gets the second render-target name.
		/// </summary>
		[Category( "Render Target" )]
		[DisplayName( "RenderTarget1" )]
		public string RenderTarget1
		{
			get
			{
				return string.IsNullOrWhiteSpace( m_renderTargetNames[ 1 ] ) ? "(Not Set)" : m_renderTargetNames[ 1 ];
			}
		}

		/// <summary>
		/// Gets the third render-target name.
		/// </summary>
		[Category( "Render Target" )]
		[DisplayName( "RenderTarget2" )]
		public string RenderTarget2
		{
			get
			{
				return string.IsNullOrWhiteSpace( m_renderTargetNames[ 2 ] ) ? "(Not Set)" : m_renderTargetNames[ 2 ];
			}
		}

		/// <summary>
		/// Gets the fourth render-target name.
		/// </summary>
		[Category( "Render Target" )]
		[DisplayName( "RenderTarget3" )]
		public string RenderTarget3
		{
			get
			{
				return string.IsNullOrWhiteSpace( m_renderTargetNames[ 3 ] ) ? "(Not Set)" : m_renderTargetNames[ 3 ];
			}
		}

		[Browsable( false )]
		public TextureSamplerCollection Textures { get; private set; }

		public RenderPass( Renderer renderer, string name )
		{
			m_renderer = renderer;

			m_renderTargetNames = new string[ Renderer.MaximumRenderTargetCount ];
			for( int i = 0 ; i < Renderer.MaximumRenderTargetCount ; ++i )
			{
				m_renderTargetNames[ i ] = null;
			}

			IsEnabled = true;

			IsClearingColor = true;
			IsClearingDepthStencil = true;
			ClearingColor = Color.Transparent;
			ClearingDepth = 1.0f;
			ClearingStencil = 0;

			Name = name;

			Textures = new TextureSamplerCollection();
		}

		public void SetClearOptions( bool bClearColor, bool bClearDepthStencil )
		{
			SetClearOptions( bClearColor, bClearDepthStencil, Color.Transparent, 0, 0 );
		}

		public void SetClearOptions( bool bClearColor, bool bClearDepthStencil, Color clearColor, float clearDepth, int clearStencil )
		{
			IsClearingColor = bClearColor;
			IsClearingDepthStencil = bClearDepthStencil;
			ClearingColor = clearColor;
			ClearingDepth = clearDepth;
			ClearingStencil = clearStencil;
		}

		protected virtual bool OnRenderStarting()
		{
			return true;
		}

		protected abstract void OnRender( TimeSpan elapsedTime );

		internal void Render( TimeSpan elapsedTime )
		{
			if( IsEnabled )
			{
				if( !OnRenderStarting() )
				{
					return;
				}

				if( RenderStarting != null )
				{
					RenderStarting( this );
				}

				// Set render-target(s)
				if( m_renderTargetNames[ 0 ] == null )
				{
					// If the first render-target is null, use the default render-target
					Renderer.SetToDefaultRenderTarget();

					// Restore the viewport.
					// This re-setting the viewport is required because WinForms GraphicsDevice sometimes has a different dimension.
					Viewport viewport = new Viewport( 0, 0, Renderer.OutputRenderTargetWidth, Renderer.OutputRenderTargetHeight );
					Renderer.Viewport = viewport;
				}
				else
				{
					// Set render-targets.
					Renderer.SetRenderTargets( m_renderTargetNames );
				}

				// Reset all textures.
				Renderer.ResetAllTextures();

				// Clear render-target(s)
				if( IsClearingColor || IsClearingDepthStencil )
				{
					Renderer.Clear(
						IsClearingColor, 
						IsClearingDepthStencil,
						IsClearingDepthStencil,
						ClearingColor,
						ClearingDepth,
						ClearingStencil );
				}

				OnRender( elapsedTime );

				if( RenderFinished != null )
				{
					RenderFinished( this );
				}
			}
		}
	}
}

