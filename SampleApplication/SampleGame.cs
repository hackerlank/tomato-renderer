using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Tomato.Graphics;
using Tomato.Graphics.Content;
using XnaModel = Microsoft.Xna.Framework.Graphics.Model;
using TomatoModel = Tomato.Graphics.Content.Model;

namespace SampleApplication
{
	public class SampleGame : Microsoft.Xna.Framework.Game
	{
		private const string SampleSceneFilePath = @".\Scene.trs";

		private GraphicsDeviceManager m_graphics = null;
		private LightPrePassRenderer m_renderer = null;

		private SceneDescription m_scene = null;
		private SceneObject m_sceneRoot = null;

		private Camera m_sceneCamera = null;
		private FlyCameraController m_cameraController = null;

		private SpriteFont m_font = null;
		private SpriteBatch m_screenUI = null;

		private bool m_bCameraEnabled = true;
		private bool m_bDrawingScreenUI = true;
		private KeyboardState m_previousKeyboardState = new KeyboardState();

		public SampleGame()
		{
			Window.Title = "Tomato Renderer Sample Application";
			Window.AllowUserResizing = true;

			m_graphics = new GraphicsDeviceManager( this );
			m_graphics.PreferredBackBufferWidth = 800;
			m_graphics.PreferredBackBufferHeight = 600;
			m_graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
			m_graphics.PreferredDepthStencilFormat = DepthFormat.Depth24;
			m_graphics.PreferMultiSampling = false;
			m_graphics.ApplyChanges();

			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// Load saved scene.
			if( File.Exists( SampleSceneFilePath ) )
			{
				m_scene = SceneDescription.ReadFromFile( SampleSceneFilePath );
			}
			else
			{
				// Create a new scene.
				m_scene = new SceneDescription();
				m_scene.Name = "Sample Scene";

				// Add some models.
				m_scene.Add( new SceneDescriptionModelEntry( "Sponza", "Models/Sponza/Sponza", Transformation.Identity ) );

				m_scene.Add( new SceneDescriptionModelEntry( "Tank1", "Models/tank", Transformation.Identity ) );
				
				Transformation tank2Transformation = new Transformation( 1.0f, Quaternion.Identity, new Vector3( -5, 0, 10 ) );
				m_scene.Add( new SceneDescriptionModelEntry( "Tank2", "Models/tank", ref tank2Transformation ) );

				Transformation robot1Transformation = new Transformation( 1.0f, Quaternion.Identity, new Vector3( 5, 3, 10 ) );
				m_scene.Add( new SceneDescriptionModelEntry( "Robot1", "Models/RobotGame/Yager", ref robot1Transformation ) );

				// Add randomly distributed point lights.
				{
					// Point lights
					Random random = new Random();
					for( int i = 0 ; i < 200 ; ++i )
					{
						Vector3 position = new Vector3(
							( float )random.NextDouble() * 200.0f - 100.0f,
							( float )random.NextDouble() * 150.0f,
							( float )random.NextDouble() * 100.0f - 50.0f );
						Vector3 diffuseColor = new Vector3(
							( float )random.NextDouble(),
							( float )random.NextDouble(),
							( float )random.NextDouble() );
						float specularPower = ( float )random.NextDouble() * 10.0f;
						float attenuationDistance = 10.0f + ( float )random.NextDouble() * 190.0f;
						float attenuationDistanceExponent = 1.0f + ( float )random.NextDouble() * 99.0f;

						m_scene.Add( SceneDescriptionLightEntry.CreatePointLight( string.Format( "PointLight{0}", i ), position, diffuseColor, specularPower, attenuationDistance, attenuationDistanceExponent ) );
					}

					// Spot lights
#if false
					random = new Random( random.Next() );
					for( int i = 0 ; i < 20 ; ++i )
					{
						Vector3 position = new Vector3(
							( float )random.NextDouble() * 200.0f - 100.0f,
							( float )random.NextDouble() * 150.0f,
							( float )random.NextDouble() * 100.0f - 50.0f );
						Vector3 direction = new Vector3(
							( float )random.NextDouble() * 2.0f - 1.0f,
							( float )random.NextDouble() * 2.0f - 1.0f,
							( float )random.NextDouble() * 2.0f - 1.0f );
						Vector3 diffuseColor = new Vector3(
							( float )random.NextDouble(),
							( float )random.NextDouble(),
							( float )random.NextDouble() );
						float specularPower = ( float )random.NextDouble() * 100.0f;
						float attenuationDistance = ( float )random.NextDouble() * 200.0f;
						float attenuationDistanceExponent = 100.0f;
						float attenuationInnerAngle = ( float )random.NextDouble() * MathHelper.PiOver4;
						float attenuationOuterAngle = attenuationInnerAngle + ( float )random.NextDouble() * MathHelper.PiOver4;
						float attenuationAngleExponent = 1.0f + ( float )random.NextDouble() * 19.0f;

						m_scene.Add( 
							string.Format( "SpotLight{0}", i ), 
							SceneDescriptionLightEntry.CreateSpotLight( position, direction, diffuseColor, specularPower, attenuationDistance, attenuationDistanceExponent, attenuationInnerAngle, attenuationOuterAngle, attenuationAngleExponent ) );
					}
#endif
				}
				
				// Add a direction light.
				m_scene.Add( SceneDescriptionLightEntry.CreateDirectionalLight( "SkyLight", new Vector3( 0, -1, 0 ), new Vector3( 0.2f, 0.2f, 0.2f ), 1.0f ) );
			}

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Load a font object.
			m_font = Content.Load<SpriteFont>( "Fonts/Verdana" );
		
			// Create a screen-UI.
			m_screenUI = new SpriteBatch( GraphicsDevice );

			// Load scene objects from scene description.
			m_sceneRoot = SceneObject.CreateFromSceneDescription( Content, m_scene );

			// Set up camera.
			m_sceneCamera = new Camera();			

			// Create renderer.
			m_renderer = new LightPrePassRenderer( m_graphics.GraphicsDevice, Content, m_sceneCamera );
			m_renderer.OutputRenderTargetWidth = Window.ClientBounds.Width;
			m_renderer.OutputRenderTargetHeight = Window.ClientBounds.Height;
			Window.ClientSizeChanged += new EventHandler<EventArgs>( OnWindowClientSizeChanged );

			// Set up camera controller.
			m_cameraController = new FlyCameraController( m_sceneCamera, Window );
			m_cameraController.MovementSpeed = 20.0f;
			m_cameraController.RotationSpeed = 0.1f;
			m_cameraController.SetCameraProjectionParameters( MathHelper.PiOver4, m_renderer.OutputRenderTargetAspectRatio, 0.1f, 1000 );

			// Add scene objects.
			m_renderer.AddScene( m_sceneRoot );
		}

		private void OnWindowClientSizeChanged( object sender, EventArgs e )
		{
			m_renderer.OutputRenderTargetWidth = Window.ClientBounds.Width;
			m_renderer.OutputRenderTargetHeight = Window.ClientBounds.Height;

			m_cameraController.SetCameraProjectionParameters( MathHelper.PiOver4, m_renderer.OutputRenderTargetAspectRatio, 0.1f, 1000 );
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// Save current scenes.
			m_scene.WriteToFile( SampleSceneFilePath );

			m_renderer.ClearScene();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update( GameTime gameTime )
		{
			// Handle input.
			HandleInput( gameTime );

			// Update camera.
			m_sceneCamera.Update( new UpdateContext(), false );

			// Update the scene objects.
			m_sceneRoot.Update( new UpdateContext(), false );			

			base.Update( gameTime );
		}

		private void HandleInput( GameTime gameTime )
		{
			KeyboardState keyboard = Keyboard.GetState();
			MouseState mouse = Mouse.GetState();

			// Check for exit.
			if( IsKeyPressed( ref keyboard, Keys.Escape ) )
			{
				Exit();
			}
			// Select output color channel override function.
			else if( IsKeyPressed( ref keyboard, Keys.F5 ) )
			{
				m_renderer.FinalPass.ColorMode = FinalPass.ColorOverrideMode.RedChannelOnly;
			}
			else if( IsKeyPressed( ref keyboard, Keys.F6 ) )
			{
				m_renderer.FinalPass.ColorMode = FinalPass.ColorOverrideMode.GreenChannelOnly;
			}
			else if( IsKeyPressed( ref keyboard, Keys.F7 ) )
			{
				m_renderer.FinalPass.ColorMode = FinalPass.ColorOverrideMode.BlueChannelOnly;
			}
			else if( IsKeyPressed( ref keyboard, Keys.F8 ) )
			{
				m_renderer.FinalPass.ColorMode = FinalPass.ColorOverrideMode.AlphaChannelOnly;
			}
			else if( IsKeyPressed( ref keyboard, Keys.F9 ) )
			{
				m_renderer.FinalPass.ColorMode = FinalPass.ColorOverrideMode.Default;
			}
			else if( IsKeyPressed( ref keyboard, Keys.F10 ) )
			{
				m_bDrawingScreenUI = !m_bDrawingScreenUI;
			}
			else if( IsKeyPressed( ref keyboard, Keys.F12 ) )
			{
				m_bCameraEnabled = !m_bCameraEnabled;

				// Show or hide the cursor
				IsMouseVisible = !m_bCameraEnabled;
			}

			// Handle camera controller.
			if( m_bCameraEnabled )
			{
				m_cameraController.Update( ref keyboard, ref mouse );
			}

			// Store the keyboard state
			m_previousKeyboardState = keyboard;
		}

		private bool IsKeyPressed( ref KeyboardState currentState, Keys key )
		{
			return currentState.IsKeyDown( key ) && !m_previousKeyboardState.IsKeyDown( key );
		}

		private bool IsKeyReleased( ref KeyboardState currentState, Keys key )
		{
			return !currentState.IsKeyDown( key ) && m_previousKeyboardState.IsKeyDown( key );
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw( GameTime gameTime )
		{
			GraphicsDevice.Clear( Color.Transparent );

			m_renderer.DoFrame();

			// Draw screen-ui elements.
			if( m_bDrawingScreenUI )
			{
				DrawScreenUI( gameTime );
			}

			base.Draw( gameTime );
		}

		private void DrawScreenUI( GameTime gameTime )
		{
			float x = 10;
			float y = 10;

			m_screenUI.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend );

			// FPS
			m_screenUI.DrawString( 
			    m_font, 
			    string.Format( "FPS: {0:F1}", m_renderer.FPS ),
			    new Vector2( x, y ), 
			    Color.White );
			y += m_font.LineSpacing;

			// Camera position
			m_screenUI.DrawString(
				m_font,
				string.Format( "Camera: P({0:F2}, {1:F2}, {2:F2}) T({3:F2}, {4:F2}, {5:F2}) U({6:F2}, {7:F2}, {8:F2})", 
					m_sceneCamera.WorldPosition.X, m_sceneCamera.WorldPosition.Y, m_sceneCamera.WorldPosition.Z,
					m_sceneCamera.WorldLookAtPosition.X, m_sceneCamera.WorldLookAtPosition.Y, m_sceneCamera.WorldLookAtPosition.Z,
					m_sceneCamera.WorldUpVector.X, m_sceneCamera.WorldUpVector.Y, m_sceneCamera.WorldUpVector.Z ),
				new Vector2( x, y ),
				Color.White );
			y += m_font.LineSpacing;

			// Output color mode
			m_screenUI.DrawString(
				m_font,
				string.Format( "ColorMode: {0}", Enum.GetName( typeof( FinalPass.ColorOverrideMode ), m_renderer.FinalPass.ColorMode ) ),
				new Vector2( x, y ),
				Color.White );
			y += m_font.LineSpacing;

			// Controller mode
			m_screenUI.DrawString(
				m_font,
				string.Format( "Controller: {0} (Press F12 to toggle.)", m_bCameraEnabled ? "FlyCamera" : "Mouse" ),
				new Vector2( x, y ),
				Color.White );
			y += m_font.LineSpacing;


			m_screenUI.End();
		}

		static void Main( string[] args )
		{
			using( SampleGame game = new SampleGame() )
			{
				game.Run();
			}
		}
	}
}
