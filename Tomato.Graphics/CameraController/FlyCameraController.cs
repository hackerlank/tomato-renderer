using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tomato.Graphics
{
	/// <summary>
	/// 
	/// </summary>
	public class FlyCameraController : CameraController
	{
		// Local direction vectors.
		private Vector3 m_forwardDirection = -Vector3.UnitZ;
		private Vector3 m_upDirection = Vector3.UnitY;
		private Vector3 m_rightDirection = Vector3.UnitX;

		// World-space direction vectors/
		private Vector3 m_currentFowardDirection = Vector3.Zero;
		private Vector3 m_currentUpDirection = Vector3.Zero;
		private Vector3 m_currentRightDirection = -Vector3.Zero;

		// Drag starting location.
		private bool m_bDragging = false;
		private int m_dragStartLocationX = 0;
		private int m_dragStartLocationY = 0;

		// Current angle of rotating camera.
		private float m_yawAngle = 0;
		private float m_pitchAngle = 0;
		
		/// <summary>
		/// Gets the current front direction vector.
		/// </summary>
		public Vector3 ForwardDirection { get { return m_currentFowardDirection; } }

		/// <summary>
		/// Gets the current right direction vector.
		/// </summary>
		public Vector3 RightDirection { get { return m_currentRightDirection; } }

		/// <summary>
		/// Gets the current up direction vector.
		/// </summary>
		public Vector3 UpDirection { get { return m_currentUpDirection; } }

		/// <summary>
		/// Gets or sets moving speed of camera.
		/// </summary>
		public float MovementSpeed { get; set; }

		/// <summary>
		/// Gets or sets rotation speed (angle in degree) of camera.
		/// </summary>
		public float RotationSpeed { get; set; }

		/// <summary>
		/// Constructs fly camera controller.
		/// </summary>
		/// <param name="camera"></param>
		public FlyCameraController( Camera camera, GameWindow xnaGameWindow )
			: base( camera, xnaGameWindow )
		{
			m_currentFowardDirection = m_forwardDirection;
			m_currentRightDirection = m_rightDirection;
			m_currentUpDirection = m_upDirection;

			MovementSpeed = 1.0f;
			RotationSpeed = 1.0f;
		}

		public FlyCameraController( Camera camera, Control winFormsControl )
			: base( camera, winFormsControl )
		{
			m_currentFowardDirection = m_forwardDirection;
			m_currentRightDirection = m_rightDirection;
			m_currentUpDirection = m_upDirection;

			MovementSpeed = 1.0f;
			RotationSpeed = 1.0f;
		}

		protected override void OnUpdate( TimeSpan elapsedTime )
		{
			// Process wheel movement.
			int wheelDelta = GetWheelDelta();
			if( wheelDelta != 0 )
			{
				// Move the camera position along the front direction.
				float scale = ( float )( wheelDelta ) / 120.0f;
				CameraPosition += ( scale * m_currentFowardDirection * MovementSpeed * 0.25f );
			}

			// Test if dragging has started.
			if( WasButtonPressed( MouseButtons.Middle ) )
			{
				m_dragStartLocationX = GetMouseLocationX();
				m_dragStartLocationY = GetMouseLocationY();
				m_bDragging = true;
			}
			else if( WasButtonPressed( MouseButtons.Right )
				&& !IsButtonPressing( MouseButtons.Middle ) )
			{
				m_dragStartLocationX = GetMouseLocationX();
				m_dragStartLocationY = GetMouseLocationY();
				m_bDragging = true;
			}

			// Test if dragging has finished.
			if( WasButtonReleased( MouseButtons.Right )
				|| WasButtonReleased( MouseButtons.Middle ) )
			{
				m_bDragging = false;
			}

			// Process mouse dragging.
			if( m_bDragging )
			{
				int deltaX = GetMouseLocationX() - m_dragStartLocationX;
				int deltaY = GetMouseLocationY() - m_dragStartLocationY;
				if( ( deltaX != 0 ) || ( deltaY != 0 ) )
				{
					if( IsButtonPressing( MouseButtons.Middle ) )
					{
						float translationSpeed = MovementSpeed * 0.01f;

						// Move to the right direction.
						CameraPosition += m_currentRightDirection * deltaX * translationSpeed;

						// Move to the up direction.
						Vector3 upDirection;
						Vector3.Cross( ref m_currentRightDirection, ref m_currentFowardDirection, out upDirection );
						CameraPosition += upDirection * -deltaY * translationSpeed;
					}
					else if( IsButtonPressing( MouseButtons.Right ) )
					{
						// Compute rotation quaternion.
						m_yawAngle += -deltaX * ( ( float )System.Math.PI / 180.0f ) * RotationSpeed;
						m_pitchAngle += -deltaY * ( ( float )System.Math.PI / 180.0f ) * RotationSpeed;

						// Limit pitch angle to [ -pi/2, pi/2 ]
						m_pitchAngle = System.Math.Max( -MathHelper.HalfPI, m_pitchAngle );
						m_pitchAngle = System.Math.Min( MathHelper.HalfPI, m_pitchAngle );

						// Compute rotation matrix.
						Matrix rotationMatrix;
						Matrix.CreateFromYawPitchRoll( m_yawAngle, m_pitchAngle, 0, out rotationMatrix );

						// Rotate direction vectors.
						Vector3.Transform( ref m_forwardDirection, ref rotationMatrix, out m_currentFowardDirection );
						Vector3.Transform( ref m_upDirection, ref rotationMatrix, out m_currentUpDirection );
						Vector3.Transform( ref m_rightDirection, ref rotationMatrix, out m_currentRightDirection );
					}
				}
			}

			// Compute movement vector.
			Vector3 movement = Vector3.Zero;
			if( IsKeyPressing( KeyboardKeys.W ) ) { movement += m_currentFowardDirection; }
			if( IsKeyPressing( KeyboardKeys.A ) ) { movement -= m_currentRightDirection; }
			if( IsKeyPressing( KeyboardKeys.S ) ) { movement -= m_currentFowardDirection; }
			if( IsKeyPressing( KeyboardKeys.D ) ) { movement += m_currentRightDirection; }
			if( !movement.Equals( Vector3.Zero ) )
			{
				movement.Normalize();
			}

			// Get the speed of camera movement.
			float movementSpeed = MovementSpeed;
			if( IsKeyPressing( KeyboardKeys.Shift ) ) 
			{ 
				movementSpeed *= 2.0f; 
			}

			// Move the camera position.
			CameraPosition += ( movement * movementSpeed * ( float )( elapsedTime.TotalSeconds ) );
			Vector3 cameraPosition = CameraPosition;

			// Compute the look-at cameraPosition.
			Vector3 lookAtPosition;
			Vector3.Add( ref cameraPosition, ref m_currentFowardDirection, out lookAtPosition );

			// Update the camera's view parameters.
			SetCameraViewParameters( ref cameraPosition, ref lookAtPosition, ref m_currentUpDirection );
		}

		protected override void OnUpdateFinished()
		{
			//MoveCursorToCenter();
			if( m_bDragging )
			{
				MoveCursorToLocation( m_dragStartLocationX, m_dragStartLocationY );
			}
		}
	}
}
