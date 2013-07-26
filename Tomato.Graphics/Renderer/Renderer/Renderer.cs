using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Tomato.Graphics
{
	public partial class Renderer
	{
		public const int MaximumRenderTargetCount = 4;

		private GraphicsDevice m_graphicsDevice = null;
		private ContentManager m_applicationContents = null;
		private ContentManager m_rendererContents = null;

		private RenderTechnique m_currentRenderTechnique = null;

		private int m_lastBackBufferWidth;
		private int m_lastBackBufferHeight;

		// Render-target cache.
		private Dictionary<string, RenderTarget2D> m_renderTargets;
		private Dictionary<string, float> m_renderTargetSizingFactors;

		private Dictionary<string, Effect> m_applicationEffects;
		private Dictionary<string, Effect> m_rendererEffects;
		private Dictionary<string, Texture> m_applicationTextures;
		private Dictionary<string, Texture> m_rendererTextures;

		private DateTime m_lastFrameTime = DateTime.Now;
		private TimeSpan m_elapsedTime;
		private int m_elapsedFrames;

		private int m_samplerCount = 0;

		private int m_trianglesPerFrame = 0;

		public event Action FrameRenderStarting = null;

		public event Action FrameRenderFinished = null;

		/// <summary>
		/// Raised when the render technique has changed.
		/// </summary>
		public event Action<RenderTechnique> RenderTechniqueChanged = null;

		/// <summary>
		/// Raised when the back buffer's dimension has changed.
		/// When using WinForms GraphicsDevice, resizing the rendering control does not always raise this event because of the GraphicsDevice sharing between the controls.
		/// For example, if the size of the control after resizing gets smaller, the GraphicsDevice does not reset itself at all.
		/// </summary>
		public event Action<int, int> BackBufferSizeChanged = null;

		/// <summary>
		/// Raised when the render-target resources are automatically recreated by the renderer.
		/// </summary>
		public event Action<string> RenderTargetRecreated = null;

		/// <summary>
		/// Gets the width of the back buffer.
		/// Note that the back buffer's dimension can differ from the final output render-target's dimension.
		/// </summary>
		[Browsable( false )]
		public int BackBufferWidth { get { return m_graphicsDevice.PresentationParameters.BackBufferWidth; } }

		/// <summary>
		/// Gets the height of the back buffer.
		/// Note that the back buffer's dimension can differ from the final output render-target's dimension.
		/// </summary>
		[Browsable( false )]
		public int BackBufferHeight { get { return m_graphicsDevice.PresentationParameters.BackBufferHeight; } }

		/// <summary>
		/// Gets or sets the final output render-target's width.
		/// </summary>
		[Browsable( false )]
		public int OutputRenderTargetWidth { get; set; }

		/// <summary>
		/// Gets or sets the final output render-target's height.
		/// </summary>
		[Browsable( false )]
		public int OutputRenderTargetHeight { get; set; }

		/// <summary>
		/// Gets the aspect ratio of the final output render-target.
		/// </summary>
		[Browsable( false )]
		public float OutputRenderTargetAspectRatio
		{
			get
			{
				return ( float )OutputRenderTargetWidth / ( float )OutputRenderTargetHeight;
			}
		}

		/// <summary>
		/// Gets the FPS (frames per second).
		/// </summary>
		[Browsable( false )]
		public float FPS { get; private set; }

		/// <summary>
		/// Gets the number of triangles rendered in the last rendering frame.
		/// </summary>
		[Browsable( false )]
		public int TrianglesPerFrame { get { return m_trianglesPerFrame; } }

		/// <summary>
		/// Gets or sets the current viewport.
		/// </summary>
		[Browsable( false )]
		public Viewport Viewport 
		{ 
			get { return m_graphicsDevice.Viewport; }
			set { m_graphicsDevice.Viewport = value; }
		}

		/// <summary>
		/// Gets or sets the render-technique to use.
		/// </summary>
		[Browsable( false )]
		public RenderTechnique RenderTechnique
		{
			get { return m_currentRenderTechnique; }
			set
			{
				if( m_currentRenderTechnique != value )
				{
					m_currentRenderTechnique = value;

					// Invoke RenderTechniqueChanged event.
					if( RenderTechniqueChanged != null )
					{
						RenderTechniqueChanged( value );
					}
				}
			}
		}

		[Browsable( true )]
		public ICollection<string> RenderTargets
		{
			get { return m_renderTargets.Keys; }
		}

		/// <summary>
		/// Creates a Renderer instance.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="applicationContents"></param>
		public Renderer( GraphicsDevice graphicsDevice, ContentManager applicationContents )
		{
			m_elapsedTime = TimeSpan.Zero;
			m_elapsedFrames = 0;
			FPS = 0;

			// Set graphics device and register events
			m_graphicsDevice = graphicsDevice;
			m_graphicsDevice.DeviceLost += new EventHandler<EventArgs>( OnGraphicsDeviceLost );
			m_graphicsDevice.DeviceReset += new EventHandler<EventArgs>( OnGraphicsDeviceReset );
			m_graphicsDevice.DeviceResetting += new EventHandler<EventArgs>( OnGraphicsDeviceResetting );
			m_graphicsDevice.ResourceCreated += new EventHandler<ResourceCreatedEventArgs>( OnGraphicsResourceCreated );
			m_graphicsDevice.ResourceDestroyed += new EventHandler<ResourceDestroyedEventArgs>( OnGraphicsResourceDestroyed );

			// Set content manager.
			m_applicationContents = applicationContents;
			m_rendererContents = new ContentManager( applicationContents.ServiceProvider, "Tomato.Graphics.Content" );

			// RenderTechnique collection.
			m_currentRenderTechnique = null;

			// Resource caches.
			m_renderTargets = new Dictionary<string, RenderTarget2D>();
			m_renderTargetSizingFactors = new Dictionary<string, float>();
			m_applicationEffects = new Dictionary<string, Effect>();
			m_applicationTextures = new Dictionary<string, Texture>();
			m_rendererTextures = new Dictionary<string, Texture>();
			m_rendererEffects = new Dictionary<string, Effect>();

			// Current back-buffer size
			m_lastBackBufferWidth = m_graphicsDevice.PresentationParameters.BackBufferWidth;
			m_lastBackBufferHeight = m_graphicsDevice.PresentationParameters.BackBufferHeight;

			// Final output render-target size.
			OutputRenderTargetWidth = m_graphicsDevice.PresentationParameters.BackBufferWidth;
			OutputRenderTargetHeight = m_graphicsDevice.PresentationParameters.BackBufferHeight;

			// Update the maximum number of the texture samplers.
			DetectMaximumSamplerCount();
		}

		public void Clear(
			bool bColor,
			bool bDepth,
			bool bStencil,
			Color clearColor,
			float clearDepth,
			int clearStencil )
		{
			// Color target
			ClearOptions clearFlag = ClearOptions.Target;
			if( !bColor )
			{
				clearFlag &= ~ClearOptions.Target;
			}

			// Depth-stencil target
			bool bDepthClearEnabled = false;
			bool bStencilClearEnabled = false;
			RenderTargetBinding[] renderTargets = m_graphicsDevice.GetRenderTargets();
			if( ( renderTargets.Length == 0 )
				|| ( renderTargets[ 0 ].RenderTarget == null ) )
			{
				bDepthClearEnabled = ( m_graphicsDevice.PresentationParameters.DepthStencilFormat != DepthFormat.None );
				bStencilClearEnabled = ( m_graphicsDevice.PresentationParameters.DepthStencilFormat == DepthFormat.Depth24Stencil8 );
			}
			else
			{
				RenderTarget2D mainRenderTarget = renderTargets[ 0 ].RenderTarget as RenderTarget2D;
				if( mainRenderTarget == null )
				{
					throw new InvalidCastException( "Failed to convert RenderTarget to RenderTarget2D." );
				}

				bDepthClearEnabled = ( mainRenderTarget.DepthStencilFormat != DepthFormat.None );
				bStencilClearEnabled = ( mainRenderTarget.DepthStencilFormat == DepthFormat.Depth24Stencil8 );
			}

			if( bDepth && bDepthClearEnabled )
			{
				clearFlag |= ClearOptions.DepthBuffer;
			}

			if( bStencil && bStencilClearEnabled )
			{
				clearFlag |= ClearOptions.Stencil;
			}

			if( clearFlag != 0 )
			{
				m_graphicsDevice.Clear( clearFlag, clearColor, clearDepth, clearStencil );
			}
		}

		private void OnGraphicsDeviceResetting( object sender, EventArgs e )
		{
		}

		private void OnGraphicsDeviceReset( object sender, EventArgs e )
		{
			// Check whether back-buffer size has changed.
			if( ( m_graphicsDevice.PresentationParameters.BackBufferWidth != m_lastBackBufferWidth )
				|| ( m_graphicsDevice.PresentationParameters.BackBufferHeight != m_lastBackBufferHeight ) )
			{
				Debug.WriteLine( "Info: Back-buffer size has changed from {0}x{1} to {2}x{3}",
					m_lastBackBufferWidth, 
					m_lastBackBufferHeight,
					m_graphicsDevice.PresentationParameters.BackBufferWidth,
					m_graphicsDevice.PresentationParameters.BackBufferHeight );

				m_lastBackBufferWidth = m_graphicsDevice.PresentationParameters.BackBufferWidth;
				m_lastBackBufferHeight = m_graphicsDevice.PresentationParameters.BackBufferHeight;

				// Raise BackBufferSizeChanged event.
				if( BackBufferSizeChanged != null )
				{
					BackBufferSizeChanged(
						m_graphicsDevice.PresentationParameters.BackBufferWidth,
						m_graphicsDevice.PresentationParameters.BackBufferHeight );
				}
			}
		}

		private void OnGraphicsDeviceLost( object sender, EventArgs e )
		{
		}

		public void DoFrame()
		{
			// Get the current time and elapsed time.
			DateTime currentTime = DateTime.Now;
			TimeSpan elapsedTime = currentTime - m_lastFrameTime;

			// Reset frame statistics.
			m_trianglesPerFrame = 0;

			// Update frame rates.
			m_elapsedTime += elapsedTime;
			m_elapsedFrames++;
			if( m_elapsedTime.TotalSeconds > 1.0 )
			{
				FPS = ( float )m_elapsedFrames / ( float )m_elapsedTime.TotalSeconds;
				m_elapsedFrames = 0;
				m_elapsedTime = TimeSpan.Zero;
			}

			// Invoke FrameRenderStarting event.
			if( FrameRenderStarting != null )
			{
				FrameRenderStarting();
			}

			// Start rendering using the current technique.
			if( m_currentRenderTechnique != null )
			{
				m_currentRenderTechnique.Render( elapsedTime );
			}

			// Invoke FrameRenderFinished event.
			if( FrameRenderFinished != null )
			{
				FrameRenderFinished();
			}

			m_lastFrameTime = currentTime;
		}

		private void OnGraphicsResourceDestroyed( object sender, ResourceDestroyedEventArgs e )
		{
			Debug.WriteLine( string.Format( "Info: Resource destroyed. [Name:{0}]", e.Name ) );
		}

		private void OnGraphicsResourceCreated( object sender, ResourceCreatedEventArgs e )
		{
			Debug.WriteLine( string.Format( "Info: Resource created. [Type:{0}]", e.Resource.ToString() ) );
		}
	}
}
