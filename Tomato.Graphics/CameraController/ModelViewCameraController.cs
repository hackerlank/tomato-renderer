using System;
using Tomato.Win32;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tomato.Graphics
{
#if false
	public sealed class ModelViewCameraController : CameraController
	{
		private ScreenArcBall m_modelArcBall;
		private ScreenArcBall m_viewArcBall;

		private Vector3 m_modelCenter;

		private Matrix m_modelMatrix;
		private Matrix m_modelRotationMatrix;
		private Matrix m_lastModelRotationMatrix;
		private Matrix m_viewMatrix;
		private Matrix m_projectionMatrix;

		private float m_radius;
		private float m_defaultRadius;
		private float m_minimumRadius;
		private float m_maximumRadius;

		private float m_modelDistance;

		public Matrix ModelTransformation
		{
			get { return m_modelMatrix;  }
		}

		public ModelViewCameraController( Camera camera, GameWindow window )
			: this( camera, window, 5.0f )
		{ }

		public ModelViewCameraController( Camera camera, GameWindow window, float defaultRadius )
			: base( camera, window )
		{
			m_modelMatrix = Matrix.Identity;
			m_modelRotationMatrix = Matrix.Identity;
			m_lastModelRotationMatrix = Matrix.Identity;
			m_viewMatrix = Matrix.Identity;
			m_projectionMatrix = Matrix.Identity;

			m_modelCenter = new Vector3( 0, 0, 0 );

			m_modelDistance = 100;

			m_modelArcBall = new ScreenArcBall( Window.ClientBounds.Width, Window.ClientBounds.Height, defaultRadius );
			m_viewArcBall = new ScreenArcBall( Window.ClientBounds.Width, Window.ClientBounds.Height, defaultRadius );

			FreezeCursorToCenter = false;
		}

		public void Reset()
		{
		}

		protected override void OnMouseDrag( ref MouseState mouseState, CameraController.MouseButtonType mouseButton )
		{
			if( mouseButton == MouseButtonType.Right )
			{
				m_viewArcBall.UpdateRotating( mouseState.X, mouseState.Y );
			}
			else if( mouseButton == MouseButtonType.Left )
			{
				m_modelArcBall.UpdateRotating( mouseState.X, mouseState.Y );
			}
		}

		protected override void OnMousePressed( ref MouseState mouseState, CameraController.MouseButtonType mouseButton )
		{
			if( mouseButton == MouseButtonType.Right )
			{
				m_viewArcBall.BeginRotating( mouseState.X, mouseState.Y );
			}
			else if( mouseButton == MouseButtonType.Left )
			{
				m_modelArcBall.BeginRotating( mouseState.X, mouseState.Y );
			}
		}

		protected override void OnMouseReleased( ref MouseState mouseState, CameraController.MouseButtonType mouseButton )
		{
			if( mouseButton == MouseButtonType.Right )
			{
				m_viewArcBall.EndRotating();
			}
			else if( mouseButton == MouseButtonType.Left )
			{
				m_modelArcBall.EndRotating();
			}
		}

		protected override void OnMouseWheel( ref MouseState mouseState )
		{
#if false
			// Update wheel
			if( m_mouseWheelDelta != 0 )
			{
				m_radius -= m_mouseWheelDelta * m_radius * 0.1f / 120.0f;
				m_radius = System.Math.Max( m_minimumRadius, m_radius );
				m_radius = System.Math.Min( m_maximumRadius, m_radius );
				m_mouseWheelDelta = 0;
			}
#endif
		}

		protected override void OnUpdate( GameTime gameTime, ref KeyboardState keyboardState, ref MouseState mouseState )
		{
			// Inverse of view arcball's rotation matrix.
			Matrix viewRotationInverse = Matrix.Invert( m_viewArcBall.RotationMatrix );

			// Transform vectors by view rotation
			Vector3 worldUp = Vector3.Transform( Vector3.UnitY, viewRotationInverse );
			Vector3 worldForward = Vector3.Transform( Vector3.UnitZ, viewRotationInverse );

			// Transform cameraPosition delta by view rotation
			//Vector3 positionDeltaTransformed = Vector3.Transform( positionDelta, viewRotationInverse );

			// Update view matrix
			Vector3 cameraPosition = worldForward * -m_modelDistance;
			Vector3 lookAtPosition = Vector3.Zero;
			SetCameraViewParameters( ref cameraPosition, ref lookAtPosition, ref worldUp );

			// Inverse of view matrix (cancelling translation)
			Matrix viewInverse = Matrix.Invert( m_viewMatrix );
			viewInverse.M41 = viewInverse.M42 = viewInverse.M43 = 0;

			// Inverse of last model rotation matrix
			Matrix lastModelRotationInverse = Matrix.Invert( m_lastModelRotationMatrix );

			// Accumulate the delta of the arcball's rotation in view space.
			// Note that per-frame delta rotations could be problematic over long periods of time.
			Matrix modelRotation = m_modelArcBall.RotationMatrix;
			m_modelRotationMatrix *= ( m_viewMatrix * lastModelRotationInverse * modelRotation * viewInverse );
			m_lastModelRotationMatrix = modelRotation;

			// Since we're accumulating delta rotations, 
			// we need to orthonormalize the matrix to prevent eventual matrix skew.
			Vector3 xBasis = new Vector3( m_modelRotationMatrix.M11, m_modelRotationMatrix.M12, m_modelRotationMatrix.M13 );
			//Vector3 yBasis = new Vector3( m_modelRotationMatrix.M21, m_modelRotationMatrix.M22, m_modelRotationMatrix.M23 );
			Vector3 zBasis = new Vector3( m_modelRotationMatrix.M31, m_modelRotationMatrix.M32, m_modelRotationMatrix.M33 );
			xBasis.Normalize();
			Vector3 yBasis;
			Vector3.Cross( ref zBasis, ref xBasis, out yBasis );
			yBasis.Normalize();
			Vector3.Cross( ref xBasis, ref yBasis, out zBasis );
			m_modelRotationMatrix.M11 = xBasis.X; m_modelRotationMatrix.M12 = xBasis.Y; m_modelRotationMatrix.M13 = xBasis.Z;
			m_modelRotationMatrix.M21 = yBasis.X; m_modelRotationMatrix.M22 = yBasis.Y; m_modelRotationMatrix.M23 = yBasis.Z;
			m_modelRotationMatrix.M31 = zBasis.X; m_modelRotationMatrix.M32 = zBasis.Y; m_modelRotationMatrix.M33 = zBasis.Z;

			// Translate the rotation matrix to the same cameraPosition as the lookAt cameraPosition.
			m_modelRotationMatrix.M41 = m_target.X;
			m_modelRotationMatrix.M42 = m_target.Y;
			m_modelRotationMatrix.M43 = m_target.Z;

			// Translate world matrix so its at the center of the model.
			Matrix translation;
			Matrix.CreateTranslation( -m_modelCenter.X, -m_modelCenter.Y, -m_modelCenter.Z, out translation );

			// Update model matrix;
			Matrix.Multiply( ref translation, ref m_modelRotationMatrix, out m_modelMatrix );
		}
	}
#endif
}
