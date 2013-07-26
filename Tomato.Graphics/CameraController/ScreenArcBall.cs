using System;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics
{
	/// <summary>
	/// Utility class for virtualizing a screen-space arc-ball.
	/// This is useful when rotating a model itself or rotating a camera around a model.
	/// </summary>
	public sealed class ScreenArcBall
	{
		private int m_windowWidth;
		private int m_windowHeight;

		private float m_radius;

		private Quaternion m_baseQuaternion;
		private Vector3 m_basePoint;
		private Vector3 m_currentPoint;

		private bool m_bRotationChanged;
		private Matrix m_rotationMatrix;
		private Quaternion m_currentQuaternion;

		/// <summary>
		/// Gets or sets the current quaternion value.
		/// </summary>
		public Quaternion CurrentQuaternion
		{
			get { return m_currentQuaternion; }
			set { m_currentQuaternion = value; }
		}

		/// <summary>
		/// Gets whether arc-ball is rotating or not.
		/// </summary>
		public bool IsRotating { get; private set; }

		/// <summary>
		/// Gets the current rotation matrix.
		/// </summary>
		public Matrix RotationMatrix
		{
			get
			{
				if( m_bRotationChanged )
				{
					Matrix.CreateFromQuaternion( ref m_currentQuaternion, out m_rotationMatrix );
					m_bRotationChanged = false;
				}

				return m_rotationMatrix;
			}
		}

		/// <summary>
		/// Constructs a ScreenArcBall object with window size.
		/// </summary>
		/// <param name="windowWidth"></param>
		/// <param name="windowHeight"></param>
		public ScreenArcBall( int windowWidth, int windowHeight )
			: this( windowWidth, windowHeight, 1.0f )
		{ }

		/// <summary>
		/// Constructs a ScreenArcBall object with window size and virtual arc-ball radius.
		/// </summary>
		/// <param name="windowWidth"></param>
		/// <param name="windowHeight"></param>
		/// <param name="radius"></param>
		public ScreenArcBall( int windowWidth, int windowHeight, float radius )
		{
			m_baseQuaternion = Quaternion.Identity;
			m_currentQuaternion = Quaternion.Identity;
			m_rotationMatrix = Matrix.Identity;

			IsRotating = false;

			m_windowWidth = windowWidth;
			m_windowHeight = windowHeight;
			m_radius = radius;

			m_basePoint = Vector3.Zero;
			m_currentPoint = Vector3.Zero;
			m_bRotationChanged = true;
		}

		/// <summary>
		/// Resets all states.
		/// </summary>
		public void Reset()
		{
			m_baseQuaternion = Quaternion.Identity;
			m_currentQuaternion = Quaternion.Identity;
			m_rotationMatrix = Matrix.Identity;
			m_bRotationChanged = true;
			IsRotating = false;
		}

		/// <summary>
		/// Changes size of the window.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void SetWindowSize( int width, int height )
		{
			SetWindowSize( width, height, 1.0f );
		}

		/// <summary>
		/// Changes size of the window and virtual arc-ball radius.
		/// </summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="radius"></param>
		public void SetWindowSize( int width, int height, float radius )
		{
			m_windowWidth = width;
			m_windowHeight = height;
			m_radius = radius;
		}

		/// <summary>
		/// Begins rotation.
		/// In a typicall scenario, when a user begins dragging, this function should be invoked with the mouse location.
		/// </summary>
		/// <param name="mouseLocationX"></param>
		/// <param name="mouseLocationY"></param>
		public void BeginRotating( int mouseLocationX, int mouseLocationY )
		{
			if( ( mouseLocationX >= 0 )
				&& ( mouseLocationX < m_windowWidth )
				&& ( mouseLocationY >= 0 )
				&& ( mouseLocationY < m_windowHeight ) )
			{
				IsRotating = true;

				m_baseQuaternion = m_currentQuaternion;
				m_basePoint = ConvertScreenPositionToArcBallPoint( mouseLocationX, mouseLocationY );
			}
		}

		/// <summary>
		/// Updates rotation.
		/// In a typical scenario, this functinon should be called when the user drags the mouse.
		/// </summary>
		/// <param name="mouseLocationX"></param>
		/// <param name="mouseLocationY"></param>
		public void UpdateRotating( int mouseLocationX, int mouseLocationY )
		{
			if( IsRotating )
			{
				m_currentPoint = ConvertScreenPositionToArcBallPoint( mouseLocationX, mouseLocationY );
				m_currentQuaternion = m_baseQuaternion * GetQuaternionFromBallPoints( ref m_basePoint, ref m_currentPoint );
				m_bRotationChanged = true;
			}
		}

		/// <summary>
		/// Finishs rotaiton.
		/// </summary>
		public void EndRotating()
		{
			IsRotating = false;
		}

		/// <summary>
		/// Converts screen coordinate to a point on the virtual arc-ball.
		/// </summary>
		/// <param name="screenX"></param>
		/// <param name="screenY"></param>
		/// <returns></returns>
		private Vector3 ConvertScreenPositionToArcBallPoint( float screenX, float screenY )
		{
			float halfWidth = ( float )m_windowWidth * 0.5f;
			float halfHeight = ( float )m_windowHeight * 0.5f;

			// Compute x, y.
			float x = -( screenX - halfWidth ) / ( m_radius * halfWidth );
			float y = ( screenY - halfHeight ) / ( m_radius * halfHeight );

			// Compute length.
			float lengthSquared = x * x + y * y;

			// Compute z.
			float z;
			if( lengthSquared > 1.0f )
			{
				float scale = 1.0f / ( float )( System.Math.Sqrt( lengthSquared ) );
				x *= scale;
				y *= scale;
				z = 0.0f;
			}
			else
			{
				z = ( float )( System.Math.Sqrt( 1.0f - lengthSquared ) );
			}

			return new Vector3( x, y, z );
		}

		/// <summary>
		/// Gets a rotation quaternion from a point to another point on virtual arc-ball.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		private Quaternion GetQuaternionFromBallPoints( ref Vector3 from, ref Vector3 to )
		{
			float dotProduct;
			Vector3.Dot( ref from, ref to, out dotProduct );

			Vector3 crossProduct;
			Vector3.Cross( ref from, ref to, out crossProduct );

			return new Quaternion( crossProduct, dotProduct );
		}
	}
}
