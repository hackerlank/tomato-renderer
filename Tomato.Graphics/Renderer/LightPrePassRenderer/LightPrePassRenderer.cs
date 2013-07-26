using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Tomato.Graphics.Vertex;

namespace Tomato.Graphics
{
	public class LightPrePassRenderer : Renderer
	{
		private Camera m_sceneCamera = null;

		/// <summary>
		/// Gets the main render-stage.
		/// </summary>
		[Browsable( false )]
		public RenderStage RenderStage { get; private set; }

		/// <summary>
		/// Gets the geometry render-pass.
		/// </summary>
		[Browsable( false )]
		public SceneRenderPass GeometryPass { get; private set; }

		/// <summary>
		/// Gets the occlusion render-pass.
		/// </summary>
		[Browsable( false )]
		public OcclusionPass OcclusionPass { get; private set; }

		/// <summary>
		/// Gets the lighting render-pass.
		/// </summary>
		[Browsable( false )]
		public LightingPass LightingPass { get; private set; }
		
		/// <summary>
		/// Gets the material render-pass.
		/// </summary>
		[Browsable( false )]
		public SceneRenderPass MaterialPass { get; private set; }

		[Browsable( false )]
		public GaussianBlurPass SceneBlurHPass { get; private set; }

		[Browsable( false )]
		public GaussianBlurPass SceneBlurVPass { get; private set; }

		//[Browsable( false )]
		//public FullScreenRenderPass Luminance0Pass { get; private set; }
		//[Browsable( false )]
		//public FullScreenRenderPass Luminance1Pass { get; private set; }
		//[Browsable( false )]
		//public FullScreenRenderPass Luminance2Pass { get; private set; }
		//[Browsable( false )]
		//public FullScreenRenderPass Luminance3Pass { get; private set; }

		/// <summary>
		/// Gets the post-processing render-pass.
		/// </summary>
		[Browsable( false )]
		public FinalPass FinalPass { get; private set; }

		/// <summary>
		/// Creates a LightPrePassRenderer instance.
		/// </summary>
		/// <param name="graphicsDevice"></param>
		/// <param name="applicationContents"></param>
		/// <param name="camera"></param>
		public LightPrePassRenderer( GraphicsDevice graphicsDevice, ContentManager applicationContents, Camera camera )
			: base( graphicsDevice, applicationContents )
		{
			m_sceneCamera = camera;

			// Create a render technique.
			RenderTechnique = new RenderTechnique( this, "Light Pre-Pass Renderer" );			

			// Create a render-stage.
			RenderStage = new Graphics.RenderStage( this, "Scene Render-Stage" );
			RenderTechnique.AddLastRenderStage( RenderStage );

			// Create a scene render-target.
			CreateRenderTarget( "Geometry Pass RT0", SurfaceFormat.HdrBlendable, DepthFormat.Depth24 );
			CreateRenderTarget( "Occlusion Pass RT0", SurfaceFormat.Single, DepthFormat.None );
			CreateRenderTarget( "Lighting Pass RT0", SurfaceFormat.HdrBlendable, DepthFormat.Depth24 );
			CreateRenderTarget( "Material Pass RT0", SurfaceFormat.HdrBlendable, DepthFormat.Depth24 );
			//CreateRenderTarget( "Luminance0 Pass RT0", 64, 64, SurfaceFormat.Single, DepthFormat.None );
			//CreateRenderTarget( "Luminance1 Pass RT0", 16, 16, SurfaceFormat.Single, DepthFormat.None );
			//CreateRenderTarget( "Luminance2 Pass RT0", 4, 4, SurfaceFormat.Single, DepthFormat.None );
			//CreateRenderTarget( "Luminance3 Pass RT0", 1, 1, SurfaceFormat.Single, DepthFormat.None );
			CreateRenderTarget( "SceneBlurH Pass RT0", SurfaceFormat.HdrBlendable, DepthFormat.None );
			CreateRenderTarget( "SceneBlurV Pass RT0", SurfaceFormat.HdrBlendable, DepthFormat.None );

			// Create rendering passes.
			{
				// Geometry pass.
				GeometryPass = new SceneRenderPass( this, "Geometry Pass", "DeferredLighting.GeometryPass" );
				GeometryPass.Camera = camera;
				GeometryPass.SetClearOptions( true, true, Color.Transparent, 1, 0 );
				GeometryPass.VisibleLightsProcessing += new Action<IList<Light>>( OnVisibleLightsProcessing );

				// Occlusion pass.
				OcclusionPass = new OcclusionPass( this );
				OcclusionPass.SetClearOptions( false, false );

				// Lighting pass.
				LightingPass = new LightingPass( this, camera );
				LightingPass.SetClearOptions( true, true, Color.Transparent, 1, 0 );

				// Material pass.
				MaterialPass = new SceneRenderPass( this, "Material Pass", "DeferredLighting.MaterialPass" );
				MaterialPass.Camera = camera;
				MaterialPass.SetClearOptions( true, true, Color.Transparent, 1, 0 );

				// Scene-Blur pass.
				SceneBlurHPass = new GaussianBlurPass( this, GaussianBlurPass.Direction.Horizontal );
				SceneBlurVPass = new GaussianBlurPass( this, GaussianBlurPass.Direction.Vertical );

				// Luminance computation passes.
//#warning Because of the XNA limitation, rendered scene is filtered using POINT filter, which means only 192x192 pixels are considered while evaluating the average luminance.
				//Luminance0Pass = new FullScreenRenderPass( this, "Luminance0 Pass", LoadInternalEffect( "Effects/Luminance" ), "Luminance.Luminance0" );
				//Luminance1Pass = new FullScreenRenderPass( this, "Luminance1 Pass", LoadInternalEffect( "Effects/Luminance" ), "Luminance.Luminance1" );
				//Luminance2Pass = new FullScreenRenderPass( this, "Luminance2 Pass", LoadInternalEffect( "Effects/Luminance" ), "Luminance.Luminance2" );
				//Luminance3Pass = new FullScreenRenderPass( this, "Luminance3 Pass", LoadInternalEffect( "Effects/Luminance" ), "Luminance.Luminance3" );
				//Luminance3Pass.SetClearOptions( true, true, Color.Transparent, 0, 0 );

				// Post-process pass.
				FinalPass = new FinalPass( this, m_sceneCamera );
				FinalPass.SetClearOptions( false, false, Color.Transparent, 1, 0 );
			}

			// Add render-passes.
			RenderStage.AddLastRenderPass( GeometryPass );
			RenderStage.AddLastRenderPass( OcclusionPass );
			RenderStage.AddLastRenderPass( LightingPass );
			RenderStage.AddLastRenderPass( MaterialPass );
			RenderStage.AddLastRenderPass( SceneBlurHPass );
			RenderStage.AddLastRenderPass( SceneBlurVPass );
			//RenderStage.AddLastRenderPass( Luminance0Pass );
			//RenderStage.AddLastRenderPass( Luminance1Pass );
			//RenderStage.AddLastRenderPass( Luminance2Pass );
			//RenderStage.AddLastRenderPass( Luminance3Pass );
			RenderStage.AddLastRenderPass( FinalPass );			

			FrameRenderStarting += new Action( OnFrameRenderStarting );
		}

		public float ComputeAverageLuminance()
		{
			float[] data = ReadRenderTargetData<float>( "Luminance3 Pass RT0" );
			if( data != null )
			{
				return data[ 0 ];
			}

			return -1;
		}

		private void OnVisibleLightsProcessing( IList<Light> visibleLights )
		{
			// Store the list of visible lights.
			LightingPass.SetVisibleLights( visibleLights );
		}

		/// <summary>
		/// Adds a new SceneObject.
		/// </summary>
		/// <param name="sceneObject"></param>
		public void AddScene( SceneObject sceneObject )
		{
			GeometryPass.AddSceneObject( sceneObject );
			MaterialPass.AddSceneObject( sceneObject );
		}

		/// <summary>
		/// Removes a SceneObject.
		/// </summary>
		/// <param name="sceneObject"></param>
		public void RemoveScene( SceneObject sceneObject )
		{
			GeometryPass.RemoveSceneObject( sceneObject );
			MaterialPass.RemoveSceneObject( sceneObject );
		}

		/// <summary>
		/// Clears all SceneObject(s).
		/// </summary>
		public void ClearScene()
		{
			GeometryPass.ClearSceneObjects();
			MaterialPass.ClearSceneObjects();
		}

		private void OnFrameRenderStarting()
		{
			// Set the FarClippingCorners effect parameter.
			Vector3[] farClipCorners = new Vector3[ 4 ];
			{
				float farH = 2.0f * ( float )System.Math.Tan( m_sceneCamera.FieldOfView / 2.0f ) * m_sceneCamera.FarClipPlane;
				float farW = farH * m_sceneCamera.AspectRatio;
				float farX = farW / 2.0f;
				float farY = farH / 2.0f;

				// 뷰 공간 코너
				farClipCorners[ 0 ] = new Vector3( -farX, +farY, -m_sceneCamera.FarClipPlane );
				farClipCorners[ 1 ] = new Vector3( +farX, +farY, -m_sceneCamera.FarClipPlane );
				farClipCorners[ 2 ] = new Vector3( -farX, -farY, -m_sceneCamera.FarClipPlane );
				farClipCorners[ 3 ] = new Vector3( +farX, -farY, -m_sceneCamera.FarClipPlane );
			}
			SetEffectParameterBySemantic( LightingPass.Effect, "FARCLIPCORNERS", farClipCorners );
			SetEffectParameterBySemantic( OcclusionPass.Effect, "FARCLIPCORNERS", farClipCorners );

			// Set the half texel offset.
			Vector2 halfTexelOffset = new Vector2(
				0.5f / ( float )OutputRenderTargetWidth,
				0.5f / ( float )OutputRenderTargetHeight );
			SetEffectParameterBySemantic( LightingPass.Effect, "HALFTEXOFFSET", ref halfTexelOffset );

			// Set the render-target dimension
			Vector2 renderTargetDimension = new Vector2( OutputRenderTargetWidth, OutputRenderTargetHeight );
			SetEffectParameterBySemantic( OcclusionPass.Effect, "RTDIMENSION", ref renderTargetDimension );

			// Set up the render-passes.
			SetupRenderPasses();
		}

		private void SetupRenderPasses()
		{
			// Geometry pass.
			GeometryPass.IsEnabled = true;
			GeometryPass.RenderTarget = "Geometry Pass RT0";

			// Occlusion pass.
			OcclusionPass.IsEnabled = true;
			OcclusionPass.RenderTarget = "Occlusion Pass RT0";
			OcclusionPass.Textures[ 0 ] = new TextureSampler( GeometryPass.RenderTarget0, SamplerState.PointClamp );
			OcclusionPass.Textures[ 1 ] = new TextureSampler( "Textures/random_tex", SamplerState.LinearWrap, TextureSampler.TextureType.RendererTexture );

			// Lighting pass.
			LightingPass.IsEnabled = true;
			LightingPass.RenderTarget = "Lighting Pass RT0";
			LightingPass.Textures[ 0 ] = new TextureSampler( GeometryPass.RenderTarget0, SamplerState.PointClamp );

			// Material pass.
			MaterialPass.IsEnabled = true;
			MaterialPass.RenderTarget = "Material Pass RT0";
			MaterialPass.Textures[ 5 ] = new TextureSampler( LightingPass.RenderTarget0, SamplerState.PointClamp );
			MaterialPass.Textures[ 6 ] = new TextureSampler( OcclusionPass.RenderTarget0, SamplerState.PointClamp );

			// Scene-Blur pass.
			SceneBlurHPass.IsEnabled = true;
			SceneBlurHPass.RenderTarget = "SceneBlurH Pass RT0";
			SceneBlurHPass.InputTexture = new TextureSampler( MaterialPass.RenderTarget0, SamplerState.PointClamp );

			SceneBlurVPass.IsEnabled = true;
			SceneBlurVPass.RenderTarget = "SceneBlurV Pass RT0";
			SceneBlurVPass.InputTexture = new TextureSampler( SceneBlurHPass.RenderTarget0, SamplerState.PointClamp );

			// Luminance computation passes.
			//Luminance0Pass.IsEnabled = true;
			//Luminance0Pass.RenderTarget = "Luminance0 Pass RT0";
			//Luminance0Pass.Textures[ 0 ] = new TextureSampler( MaterialPass.RenderTarget0, SamplerState.PointClamp );

			//Luminance1Pass.IsEnabled = true;
			//Luminance1Pass.RenderTarget = "Luminance1 Pass RT0";
			//Luminance1Pass.Textures[ 0 ] = new TextureSampler( Luminance0Pass.RenderTarget0, SamplerState.PointClamp );

			//Luminance2Pass.IsEnabled = true;
			//Luminance2Pass.RenderTarget = "Luminance2 Pass RT0";
			//Luminance2Pass.Textures[ 0 ] = new TextureSampler( Luminance1Pass.RenderTarget0, SamplerState.PointClamp );

			//Luminance3Pass.IsEnabled = true;
			//Luminance3Pass.RenderTarget = "Luminance3 Pass RT0";
			//Luminance3Pass.Textures[ 0 ] = new TextureSampler( Luminance2Pass.RenderTarget0, SamplerState.PointClamp );

			// Post-process pass.
			FinalPass.IsEnabled = true;
			FinalPass.RenderTarget = null;
			FinalPass.Textures[ 0 ] = new TextureSampler( MaterialPass.RenderTarget0, SamplerState.PointClamp );
			FinalPass.Textures[ 1 ] = new TextureSampler( SceneBlurVPass.RenderTarget0, SamplerState.PointClamp );
			FinalPass.Textures[ 2 ] = new TextureSampler( GeometryPass.RenderTarget0, SamplerState.PointClamp );
			//FinalPass.Textures[ 3 ] = new TextureSampler( Luminance3Pass.RenderTarget0, SamplerState.PointClamp );
		}
	}
}