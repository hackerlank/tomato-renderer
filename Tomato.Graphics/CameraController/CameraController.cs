using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using XnaButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using XnaKeys = Microsoft.Xna.Framework.Input.Keys;
using WinFormsKeys = System.Windows.Forms.Keys;
using Point = System.Drawing.Point;

namespace Tomato.Graphics
{
	public class CameraController
	{
		private bool m_bXnaInputMode = false;
		private GameWindow m_xnaGameWindow = null;
		private Control m_winFormsControl = null;

		// XNA input states.
		private MouseState m_previousXnaMouseState = new MouseState();
		private MouseState m_currentXnaMouseState = new MouseState();
		private KeyboardState m_previousXnaKeyboardState = new KeyboardState();
		private KeyboardState m_currentXnaKeyboardState = new KeyboardState();

		// WinForms input states.
		private int m_winFormsMouseDelta = 0;
		private int m_winFormsMouseX = 0;
		private int m_winFormsMouseY = 0;
		private int m_winFormsPreviousMouseX = 0;
		private int m_winFormsPreviousMouseY = 0;
		private bool m_bWinFormsMouseLeftButtonPressed = false;
		private bool m_bWinFormsMouseLeftButtonReleased = false;
		private bool m_bWinFormsMouseLeftButtonPressing = false;
		private bool m_bWinFormsMouseRightButtonPressed = false;
		private bool m_bWinFormsMouseRightButtonReleased = false;
		private bool m_bWinFormsMouseRightButtonPressing = false;
		private bool m_bWinFormsMouseMiddleButtonPressed = false;
		private bool m_bWinFormsMouseMiddleButtonReleased = false;
		private bool m_bWinFormsMouseMiddleButtonPressing = false;
		private Dictionary<KeyboardKeys, bool> m_winFormsKeyPressing = new Dictionary<KeyboardKeys, bool>();
		private List<KeyboardKeys> m_winFormsKeyPressed = new List<KeyboardKeys>();
		private List<KeyboardKeys> m_winFormsKeyReleased = new List<KeyboardKeys>();

		private DateTime m_lastUpdateTime = DateTime.Now;

		// Current position of the camera.
		private Vector3 m_cameraPosition = Vector3.Zero;

		/// <summary>
		/// Gets the mouse state of the previous update frame.
		/// </summary>
		protected MouseState PreviousMouseState { get { return m_previousXnaMouseState; } }

		/// <summary>
		/// Gets the current position of the camera.
		/// </summary>
		public Vector3 CameraPosition 
		{ 
			get { return m_cameraPosition; }
			protected set { m_cameraPosition = value; }
		}
		
		/// <summary>
		/// Gets the Camera object bound to the controller.
		/// </summary>
		public Camera Camera { get; private set; }

		/// <summary>
		/// Gets or sets whether the controller is enabled or not.
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// Constructs camera controller.
		/// </summary>
		/// <param name="camera"></param>
		public CameraController( Camera camera, GameWindow gameWindow )
		{
			Camera = camera;

			m_bXnaInputMode = true;
			m_xnaGameWindow = gameWindow;
			m_winFormsControl = null;

			Enabled = true;
		}

		public CameraController( Camera camera, Control winFormsControl )
		{
			Camera = camera;

			m_bXnaInputMode = false;
			m_xnaGameWindow = null;
			m_winFormsControl = winFormsControl;
			{
				m_winFormsControl.KeyDown += new KeyEventHandler( OnWinFormsControlKeyDown );
				m_winFormsControl.KeyUp += new KeyEventHandler( OnWinFormsControlKeyUp );
				m_winFormsControl.MouseDown += new MouseEventHandler( OnWinFormsControlMouseDown );
				m_winFormsControl.MouseUp += new MouseEventHandler( OnWinFormsControlMouseUp );
				m_winFormsControl.MouseMove += new MouseEventHandler( OnWinFormsControlMouseMove );
				m_winFormsControl.MouseWheel += new MouseEventHandler( OnWinFormsControlMouseWheel );
			}

			Enabled = true;
		}

		public void Update()
		{
			DateTime currentTime = DateTime.Now;
			TimeSpan elapsedTime = currentTime - m_lastUpdateTime;

			if( Enabled )
			{
				if( m_bXnaInputMode )
				{
					throw new InvalidOperationException( "XNA input mode is enabled." );
				}

				// Call OnUpdate().
				OnUpdate( elapsedTime );

				// Store some states.
				m_winFormsPreviousMouseX = m_winFormsMouseX;
				m_winFormsPreviousMouseY = m_winFormsMouseY;

				// Reset input states.
				m_winFormsMouseDelta = 0;
				m_bWinFormsMouseLeftButtonPressed = false;
				m_bWinFormsMouseLeftButtonReleased = false;
				m_bWinFormsMouseMiddleButtonPressed = false;
				m_bWinFormsMouseMiddleButtonReleased = false;
				m_bWinFormsMouseRightButtonPressed = false;
				m_bWinFormsMouseRightButtonReleased = false;
				m_winFormsKeyPressed.Clear();
				m_winFormsKeyReleased.Clear();

				// Call OnUpdateFinished().
				OnUpdateFinished();
			}

			m_lastUpdateTime = currentTime;
		}

		/// <summary>
		/// Updates input states, controller state and camera view parameters.
		/// In a typical scenario, one should invoke Update() function once for each frame.
		/// </summary>
		/// <param name="keyboardState"></param>
		/// <param name="mouseState"></param>
		public void Update( ref KeyboardState keyboardState, ref MouseState mouseState )
		{
			DateTime currentTime = DateTime.Now;
			TimeSpan elapsedTime = currentTime - m_lastUpdateTime;

			if( Enabled )
			{
				if( !m_bXnaInputMode )
				{
					throw new InvalidOperationException( "XNA input mode is disabled." );
				}

				// Store the current input states.
				m_currentXnaMouseState = mouseState;
				m_currentXnaKeyboardState = keyboardState;

				// Call OnUpdate().
				OnUpdate( elapsedTime );

				// Store to the previous input states.
				m_previousXnaMouseState = mouseState;
				m_previousXnaKeyboardState = keyboardState;
				
				// Call OnUpdateFinished().
				OnUpdateFinished();
			}

			m_lastUpdateTime = currentTime;
		}

		/// <summary>
		/// Sets the projection parameters of the binding camera.
		/// </summary>
		/// <param name="verticalFieldOfView"></param>
		/// <param name="aspectRatio"></param>
		/// <param name="nearClip"></param>
		/// <param name="farClip"></param>
		public void SetCameraProjectionParameters( float verticalFieldOfView, float aspectRatio, float nearClip, float farClip )
		{
			if( Enabled )
			{
				Camera.SetProjectionParameters( verticalFieldOfView, aspectRatio, nearClip, farClip );
			}
		}

		/// <summary>
		/// Moves the mouse cursor to the center of the viewport.
		/// </summary>
		protected void MoveCursorToCenter()
		{
			if( m_bXnaInputMode )
			{
				Mouse.SetPosition( m_xnaGameWindow.ClientBounds.Width / 2, m_xnaGameWindow.ClientBounds.Height / 2 );
			}
			else
			{
				Cursor.Position = m_winFormsControl.PointToScreen( new Point( m_winFormsControl.ClientSize.Width / 2, m_winFormsControl.ClientSize.Height / 2 ) );
			}
		}

		/// <summary>
		/// Moves the mouse cursor to a specific location in the viewport.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		protected void MoveCursorToLocation( int x, int y )
		{
			if( m_bXnaInputMode )
			{
				Mouse.SetPosition( x, y );
			}
			else
			{
				Cursor.Position = m_winFormsControl.PointToScreen( new Point( x, y ) );
			}
		}

		/// <summary>
		/// Sets the view parameters of the binding camera.
		/// </summary>
		/// <param name="cameraPosition"></param>
		/// <param name="lookAtPosition"></param>
		/// <param name="upVector"></param>
		protected void SetCameraViewParameters( ref Vector3 cameraPosition, ref Vector3 lookAtPosition, ref Vector3 upVector )
		{
			if( Enabled )
			{
				Camera.SetViewParameters( ref cameraPosition, ref lookAtPosition, ref upVector );
			}
		}

		

		protected virtual void OnUpdate( TimeSpan elapsedTime )
		{
		}

		protected virtual void OnUpdateFinished()
		{
		}

		/// <summary>
		/// Tests whether the key has just pressed.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected bool WasKeyPressed( KeyboardKeys key )
		{
			if( m_bXnaInputMode )
			{
				return !m_previousXnaKeyboardState.IsKeyDown( XnaConverter.GetKey( key ) ) && m_currentXnaKeyboardState.IsKeyDown( XnaConverter.GetKey( key ) );
			}
			else
			{
				return m_winFormsKeyPressed.Contains( key );
			}
		}

		/// <summary>
		/// Tests whether the key has just released.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected bool WasKeyReleased( KeyboardKeys key )
		{
			if( m_bXnaInputMode )
			{
				return m_previousXnaKeyboardState.IsKeyDown( XnaConverter.GetKey( key ) ) && !m_currentXnaKeyboardState.IsKeyDown( XnaConverter.GetKey( key ) );
			}
			else
			{
				return m_winFormsKeyReleased.Contains( key );
			}
		}

		/// <summary>
		/// Tests whether the key is being pressed now.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected bool IsKeyPressing( KeyboardKeys key )
		{
			if( m_bXnaInputMode )
			{
				return m_currentXnaKeyboardState.IsKeyDown( XnaConverter.GetKey( key ) );
			}
			else
			{
				bool bPressing;
				if( m_winFormsKeyPressing.TryGetValue( key, out bPressing ) )
				{
					return bPressing;
				}

				return false;
			}
		}

		protected bool WasButtonPressed( MouseButtons button )
		{
			if( m_bXnaInputMode )
			{
				switch( button )
				{
					case MouseButtons.Left: return ( m_previousXnaMouseState.LeftButton == XnaButtonState.Released ) && ( m_currentXnaMouseState.LeftButton == XnaButtonState.Pressed );
					case MouseButtons.Middle: return ( m_previousXnaMouseState.MiddleButton == XnaButtonState.Released ) && ( m_currentXnaMouseState.MiddleButton == XnaButtonState.Pressed );
					case MouseButtons.Right: return ( m_previousXnaMouseState.RightButton == XnaButtonState.Released ) && ( m_currentXnaMouseState.RightButton == XnaButtonState.Pressed );
				}
				return false;
			}
			else
			{
				switch( button )
				{
					case MouseButtons.Left: return m_bWinFormsMouseLeftButtonPressed;
					case MouseButtons.Middle: return m_bWinFormsMouseMiddleButtonPressed;
					case MouseButtons.Right: return m_bWinFormsMouseRightButtonPressed;
				}
				return false;
			}
		}

		protected bool WasButtonReleased( MouseButtons button )
		{
			if( m_bXnaInputMode )
			{
				switch( button )
				{
					case MouseButtons.Left: return ( m_previousXnaMouseState.LeftButton == XnaButtonState.Pressed ) && ( m_currentXnaMouseState.LeftButton == XnaButtonState.Released );
					case MouseButtons.Middle: return ( m_previousXnaMouseState.MiddleButton == XnaButtonState.Pressed ) && ( m_currentXnaMouseState.MiddleButton == XnaButtonState.Released );
					case MouseButtons.Right: return ( m_previousXnaMouseState.RightButton == XnaButtonState.Pressed ) && ( m_currentXnaMouseState.RightButton == XnaButtonState.Released );
				}
				return false;
			}
			else
			{
				switch( button )
				{
					case MouseButtons.Left: return m_bWinFormsMouseLeftButtonReleased;
					case MouseButtons.Middle: return m_bWinFormsMouseMiddleButtonReleased;
					case MouseButtons.Right: return m_bWinFormsMouseRightButtonReleased;
				}
				return false;
			}
		}

		protected bool IsButtonPressing( MouseButtons button )
		{
			if( m_bXnaInputMode )
			{
				switch( button )
				{
					case MouseButtons.Left: return m_currentXnaMouseState.LeftButton == XnaButtonState.Pressed;
					case MouseButtons.Middle: return m_currentXnaMouseState.MiddleButton == XnaButtonState.Pressed;
					case MouseButtons.Right: return m_currentXnaMouseState.RightButton == XnaButtonState.Pressed;
				}
				return false;
			}
			else
			{
				switch( button )
				{
					case MouseButtons.Left: return m_bWinFormsMouseLeftButtonPressing;
					case MouseButtons.Middle: return m_bWinFormsMouseMiddleButtonPressing;
					case MouseButtons.Right: return m_bWinFormsMouseRightButtonPressing;
				}
				return false;
			}
		}

		protected int GetWheelDelta()
		{
			if( m_bXnaInputMode )
			{
				return m_currentXnaMouseState.ScrollWheelValue - m_previousXnaMouseState.ScrollWheelValue;
			}
			else
			{
				return m_winFormsMouseDelta;
			}
		}

		protected int GetMouseLocationX()
		{
			if( m_bXnaInputMode )
			{
				return m_currentXnaMouseState.X;
			}
			else
			{
				return m_winFormsMouseX;
			}
		}

		protected int GetMouseLocationY()
		{
			if( m_bXnaInputMode )
			{
				return m_currentXnaMouseState.Y;
			}
			else
			{
				return m_winFormsMouseY;
			}
		}

		protected int GetViewportWidth()
		{
			if( m_bXnaInputMode )
			{
				return m_xnaGameWindow.ClientBounds.Width;
			}
			else
			{
				return m_winFormsControl.ClientSize.Width;
			}
		}

		protected int GetViewportHeight()
		{
			if( m_bXnaInputMode )
			{
				return m_xnaGameWindow.ClientBounds.Height;
			}
			else
			{
				return m_winFormsControl.ClientSize.Height;
			}
		}

		private void OnWinFormsControlMouseWheel( object sender, MouseEventArgs e )
		{
			m_winFormsMouseDelta += e.Delta;
		}

		private void OnWinFormsControlMouseMove( object sender, MouseEventArgs e )
		{
			m_winFormsMouseX = e.X;
			m_winFormsMouseY = e.Y;
		}

		private void OnWinFormsControlMouseUp( object sender, MouseEventArgs e )
		{
			switch( e.Button )
			{
				case System.Windows.Forms.MouseButtons.Left:
					{
						m_bWinFormsMouseLeftButtonReleased = true;
						m_bWinFormsMouseLeftButtonPressing = false;
					}
					break;
				case System.Windows.Forms.MouseButtons.Right:
					{
						m_bWinFormsMouseRightButtonReleased = true;
						m_bWinFormsMouseRightButtonPressing = false;
					}
					break;
				case System.Windows.Forms.MouseButtons.Middle:
					{
						m_bWinFormsMouseMiddleButtonReleased = true;
						m_bWinFormsMouseMiddleButtonPressing = false;
					}
					break;
			}
		}

		private void OnWinFormsControlMouseDown( object sender, MouseEventArgs e )
		{
			switch( e.Button )
			{
				case System.Windows.Forms.MouseButtons.Left:
					{
						m_bWinFormsMouseLeftButtonPressed = true;
						m_bWinFormsMouseLeftButtonPressing = true;
					}
					break;
				case System.Windows.Forms.MouseButtons.Right:
					{
						m_bWinFormsMouseRightButtonPressed = true;
						m_bWinFormsMouseRightButtonPressing = true;
					}
					break;
				case System.Windows.Forms.MouseButtons.Middle:
					{
						m_bWinFormsMouseMiddleButtonPressed = true;
						m_bWinFormsMouseMiddleButtonPressing = true;
					}
					break;
			}
		}

		private void OnWinFormsControlKeyUp( object sender, KeyEventArgs e )
		{
			m_winFormsKeyPressing[ WinFormsConverter.GetKey( e.KeyCode ) ] = false;
		}

		private void OnWinFormsControlKeyDown( object sender, KeyEventArgs e )
		{
			m_winFormsKeyPressing[ WinFormsConverter.GetKey( e.KeyCode ) ] = true;
		}
	}
}
