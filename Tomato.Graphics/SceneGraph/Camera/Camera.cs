using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics
{
	/// <summary>
	/// Represents a camera object.
	/// Eye cameraPosition: local-space origin ( 0, 0, 0 )
	/// Look-at direction: local-space positive Z-axis
	/// UpDirection vector: local-space cameraPosition Y-axis
	/// </summary>
	public class Camera : SceneObject 
	{
		private Vector3 m_position;
		private Vector3 m_lookAtPosition;
		private Vector3 m_upVector;

		private Vector3 m_worldPosition;
		private Vector3 m_worldLookAtPosition;
		private Vector3 m_worldUpVector;

		private float m_fieldOfView;
		private float m_aspectRatio;
		private float m_nearClipPlane;
		private float m_farClipPlane;

		private bool m_bViewParametersChanged;
		private bool m_bProjectionParametersChanged;

		private Matrix m_viewMatrix;
		private Matrix m_projectionMatrix;

		/// <summary>
		/// Gets the current position of the camera.
		/// </summary>
		[Category( "Camera" )]
		[DisplayName( "Position" )]
		public Vector3 Position
		{
			get { return m_position; }
			set
			{
				if( m_position != value )
				{
					m_position = value;
					m_bViewParametersChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets the look-at target position of the camera.
		/// </summary>
		[Category( "Camera" )]
		[DisplayName( "LookAtPosition" )]
		public Vector3 LookAtPosition
		{
			get { return m_lookAtPosition; }
			set
			{
				if( m_lookAtPosition != value )
				{
					m_lookAtPosition = value;
					m_bViewParametersChanged = true;
				}
			}
		}

		/// <summary>
		/// Get the up direction vector.
		/// Returned value is normalized.
		/// </summary>
		[Category( "Camera" )]
		[DisplayName( "UpDirection" )]
		public Vector3 UpDirection
		{
			get { return m_upVector; }
			set
			{
				if( m_upVector != value )
				{
					m_upVector = value;
					m_bViewParametersChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets the forward direction vector.
		/// Returned value is normalized.
		/// </summary>
		[Category( "Camera" )]
		[DisplayName( "Forward" )]
		public Vector3 ForwardDirection
		{
			get
			{
				Vector3 forward;
				Vector3.Subtract( ref m_lookAtPosition, ref m_position, out forward );
				forward.Normalize();
				return forward;
			}
		}

		/// <summary>
		/// Gets the right direction vector.
		/// Returned value is normalized.
		/// </summary>
		[Category( "Camera" )]
		[DisplayName( "Right" )]
		public Vector3 RightDirection
		{
			get
			{
				Vector3 foward = ForwardDirection;

				Vector3 right;
				Vector3.Cross( ref foward, ref m_upVector, out right );

				return right;
			}
		}

		/// <summary>
		/// Gets the vertical field-of-view angle in radian.
		/// </summary>
		[Category( "Camera" )]
		[DisplayName( "FieldOfView" )]
		public float FieldOfView
		{
			get { return m_fieldOfView; }
			set
			{
				if( m_fieldOfView != value )
				{
					m_fieldOfView = value;
					m_bProjectionParametersChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets the ratio of viewport width and height.
		/// </summary>
		[Category( "Camera" )]
		[DisplayName( "AspectRatio" )]
		public float AspectRatio
		{
			get { return m_aspectRatio; }
			set
			{
				if( m_aspectRatio != value )
				{
					m_aspectRatio = value;
					m_bProjectionParametersChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets the distance to the near clipping plane.
		/// </summary>
		[Category( "Camera" )]
		[DisplayName( "NearClip" )]
		public float NearClipPlane
		{
			get { return m_nearClipPlane; }
			set
			{
				if( m_nearClipPlane != value )
				{
					m_nearClipPlane = value;
					m_bProjectionParametersChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets the distance to the far clipping plane.
		/// </summary>
		[Category( "Camera" )]
		[DisplayName( "FarClip" )]
		public float FarClipPlane
		{
			get { return m_farClipPlane; }
			set
			{
				if( m_farClipPlane != value )
				{
					m_farClipPlane = value;
					m_bProjectionParametersChanged = true;
				}
			}
		}

		/// <summary>
		/// Gets view transformation matrix.
		/// </summary>
		[Category( "Camera" )]
		[DisplayName( "ViewMatrix" )]
		public virtual Matrix ViewMatrix { get { return m_viewMatrix; } }

		/// <summary>
		/// Gets projection transformation matrix.
		/// </summary>
		[Category( "Camera" )]
		[DisplayName( "ProjectionMatrix" )]
		public virtual Matrix ProjectionMatrix { get { return m_projectionMatrix; } }

		[Category( "Camera" )]
		[DisplayName( "WorldPosition" )]
		public Vector3 WorldPosition { get { return m_worldPosition; } }

		[Category( "Camera" )]
		[DisplayName( "WorldLookAtPosition" )]
		public Vector3 WorldLookAtPosition { get { return m_worldLookAtPosition; } }

		[Category( "Camera" )]
		[DisplayName( "WorldUpVector" )]
		public Vector3 WorldUpVector { get { return m_worldUpVector; } }		

		/// <summary>
		/// Constructors a camera.
		/// </summary>
		public Camera()
			: base( SceneObjectType.Camera, "", null ) 
		{
			m_position = Vector3.Zero;
			m_lookAtPosition = Vector3.Zero;
			m_upVector = Vector3.Zero;

			m_worldPosition = m_position;
			m_worldLookAtPosition = m_lookAtPosition;
			m_worldUpVector = m_upVector;

			m_fieldOfView = ( float )( System.Math.PI / 4.0 );
			m_aspectRatio = 1.0f;
			m_nearClipPlane = 0.1f;
			m_farClipPlane = 100.0f;

			m_bViewParametersChanged = true;
			m_bProjectionParametersChanged = true;

			m_viewMatrix = Matrix.Identity;
			m_projectionMatrix = Matrix.Identity;
		}

		/// <summary>
		/// Sets the view parameters.
		/// </summary>
		/// <param name="cameraPosition"></param>
		/// <param name="lookAtPosition"></param>
		/// <param name="upVector"></param>
		public virtual void SetViewParameters( Vector3 cameraPosition, Vector3 lookAtPosition, Vector3 upVector )
		{
			// Set members.
			m_position = cameraPosition;
			m_lookAtPosition = lookAtPosition;
			m_upVector = upVector;
			
			m_bViewParametersChanged = true;
		}

		/// <summary>
		/// Sets the view parameters.
		/// </summary>
		/// <param name="cameraPosition"></param>
		/// <param name="lookAtPosition"></param>
		/// <param name="upVector"></param>
		public virtual void SetViewParameters( ref Vector3 cameraPosition, ref Vector3 lookAtPosition, ref Vector3 upVector )
		{
			// Set members.
			m_position = cameraPosition;
			m_lookAtPosition = lookAtPosition;
			m_upVector = upVector;

			m_bViewParametersChanged = true;
		}

		/// <summary>
		/// Sets the projection parameters.
		/// </summary>
		/// <param name="fieldOfView"></param>
		/// <param name="aspectRatio"></param>
		/// <param name="nearClipPlane"></param>
		/// <param name="farClipPlane"></param>
		public virtual void SetProjectionParameters( float fieldOfView, float aspectRatio, float nearClipPlane, float farClipPlane )
		{
			m_fieldOfView = fieldOfView;
			m_aspectRatio = aspectRatio;
			m_nearClipPlane = nearClipPlane;
			m_farClipPlane = farClipPlane;

			m_bProjectionParametersChanged = true;
		}

		protected override void OnUpdate( UpdateContext updateContext, bool bForceUpdateTransformation )
		{
			bool bRecomputeViewMatrix = false;

			// Update view matrix.
			if( m_bViewParametersChanged )
			{
				// Set-up local-space transformation.
				CreateLocalTransformation( ref m_position, ref m_lookAtPosition, ref m_upVector, out m_localTransformation );
				bRecomputeViewMatrix = true;
				bForceUpdateTransformation = true;

				m_bViewParametersChanged = false;
			}

			// Update projection matrix.
			if( m_bProjectionParametersChanged )
			{
				CreateProjectionMatrix( m_fieldOfView, m_aspectRatio, m_nearClipPlane, m_farClipPlane, out m_projectionMatrix );

				m_bProjectionParametersChanged = false;
			}

			// Update world-space transformation matrix.
			// If world-space transformation was chagned, world-space bounding volume should be also recomputed.
			bool bWorldTransformationChanged = RecomputeWorldTransformationMatrix( bForceUpdateTransformation );

			// Update world-space bounding volume.
			if( bWorldTransformationChanged )
			{
				m_localBounds.Transform( ref m_worldTransformation, out m_worldBounds );
			}

			// Update view transformation matrix.
			if( bRecomputeViewMatrix 
				|| bWorldTransformationChanged )
			{
				m_worldPosition  = m_worldTransformation.Translation;
				m_worldLookAtPosition = m_worldPosition + m_worldTransformation.Forward;
				m_worldUpVector = m_worldTransformation.Up;
				CreateViewMatrix( ref m_worldPosition, ref m_worldLookAtPosition, ref m_worldUpVector, out m_viewMatrix );
			}
		}

		protected static void CreateViewMatrix( ref Vector3 eyePosition, ref Vector3 lookAtPosition, ref Vector3 upVector, out Matrix result )
		{
			Matrix.CreateLookAt( ref eyePosition, ref lookAtPosition, ref upVector, out result );
		}

		protected static void CreateProjectionMatrix( float fieldOfView, float aspectRatio, float nearClipPlane, float farClipPlane, out Matrix result )
		{
			Matrix.CreatePerspectiveFieldOfView( fieldOfView, aspectRatio, nearClipPlane, farClipPlane, out result );
		}

		protected static void CreateLocalTransformation( ref Vector3 eyePosition, ref Vector3 lookAtPosition, ref Vector3 upVector, out Transformation result )
		{
			// Set eye cameraPosition.
			Vector3 translation = eyePosition;

			// Create forward direction vector.
			Vector3 forward;
			Vector3.Subtract( ref lookAtPosition, ref eyePosition, out forward );

			// Create rotation matrix and rotation quaternion.
			Matrix rotationMatrix = Matrix.CreateWorld( Vector3.Zero, forward, upVector );
			Quaternion rotation = Quaternion.CreateFromRotationMatrix( rotationMatrix );

			// Set local transformation.
			result = new Transformation( 1.0f, rotation, translation );
		}
	}
}