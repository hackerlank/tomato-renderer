using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics
{
	/// <summary>
	/// Defines a light object.
	/// Though Light is a SceneObject-derived class, Light class does not use the local-transformation.
	/// Instead Light uses the world-space transformation matrix to transform its position and direction vectors.
	/// If Light has no parent, the position and direction vectors are used without transformation.
	/// </summary>
	public class Light : SceneObject
	{
		private LightType m_lightType;

		private Vector3 m_position = Vector3.Zero;
		private Vector3 m_direction = Vector3.UnitZ;

		private Vector3 m_worldPosition = Vector3.Zero;
		private Vector3 m_worldDirection = Vector3.Zero;

		private Vector3 m_diffuseColor = Vector3.Zero;
		private float m_specularShineness = 0;

		private float m_attenuationDistance = 0;
		private float m_attenuationDistanceExponent = 1;

		private float m_attenuationInnerAngle = 0;
		private float m_attenuationOuterAngle = 0;
		private float m_attenuationAngleExponent = 1;

		//private static Vector4 s_allOne = new Vector4( 1, 1, 1, 1 );
		//private static Vector4 s_allZero = new Vector4( 0, 0, 0, 0 );

		/// <summary>
		/// Gets or sets the local-space position of light.
		/// </summary>
		[Category( "Light" )]
		[DisplayName( "Position" )]
		public Vector3 Position
		{
			get { return m_position; }
			set { m_position = value; }
		}

		/// <summary>
		/// Gets or sets the local-space direction vector of light.
		/// </summary>
		[Category( "Light" )]
		[DisplayName( "Direction" )]
		public Vector3 Direction
		{
			get { return m_direction; }
			set
			{
				// Set normalized vector.
				Vector3.Normalize( ref value, out m_direction );

#if DEBUG
				// Spot-light must have a direction.
				if( ( m_lightType == LightType.Spot )
					&& MathHelper.CompareFloatZero( m_direction.LengthSquared() ) )
				{
					throw new InvalidOperationException( "Light of DistanceAndAngle type must have a corret direction vector." );
				}
#endif
			}
		}

		/// <summary>
		/// Gets or sets the diffuse intensity(RGB) of light.
		/// </summary>
		[Category( "Light" )]
		[DisplayName( "Diffuse" )]
		public Vector3 DiffuseColor
		{
			get { return m_diffuseColor; }
			set { m_diffuseColor = value; }
		}

		/// <summary>
		/// Gets or sets the shineness factor of light.
		/// </summary>
		[Category( "Light" )]
		[DisplayName( "SpecularPower" )]
		public float SpecularPower
		{
			get { return m_specularShineness; }
			set
			{
				if( m_specularShineness < 0 )
				{
					throw new ArgumentOutOfRangeException( "Shineness value cannot be less than zero." );
				}

				m_specularShineness = value;
			}
		}

		/// <summary>
		/// Gets or sets the attenuation distance of light.
		/// At this distance from the object, light have a zero attenuation.
		/// Attenuation Equation: 
		///		Attenuation = SmoothStep( 0, [AttenuationDistance], D ) ^ [AttenuationDistanceExponent]
		/// where D is the distance between the light and the object.
		/// </summary>
		[Category( "Light Attenuation" )]
		[DisplayName( "Distance" )]
		public float AttenuationDistance
		{
			get { return m_attenuationDistance; }
			set
			{
				if( m_attenuationDistance < 0 )
				{
					throw new ArgumentOutOfRangeException( "Attenuation distance cannot be less than zero." );
				}

				m_attenuationDistance = value;
			}
		}

		/// <summary>
		/// Gets or sets the distance attenuation expoenent.
		/// Attenuation Equation: 
		///		Attenuation = SmoothStep( 0, [AttenuationDistance], D ) ^ [AttenuationDistanceExponent]
		/// where D is the distance between the light and the object.
		/// </summary>
		[Category( "Light Attenuation" )]
		[DisplayName( "DistanceExponent" )]
		public float AttenuationDistanceExponent
		{
			get { return m_attenuationDistanceExponent; }
			set
			{
				if( m_attenuationDistanceExponent < 0 )
				{
					throw new ArgumentOutOfRangeException( "Attenuation distance exponent cannot be less than zero." );
				}

				m_attenuationDistanceExponent = value;
			}
		}

		/// <summary>
		/// Gets or sets the inner angle of spot-light.
		/// </summary>
		[Category( "Light Attenuation" )]
		[DisplayName( "InnerAngle" )]
		public float AttenuationInnerAngle
		{
			get { return m_attenuationInnerAngle; }
			set
			{
				if( value < 0 )
				{
					throw new ArgumentOutOfRangeException( "Attenuation inner angle cannot be less than zero." );
				}

				m_attenuationInnerAngle = value;
			}
		}

		/// <summary>
		/// Gets or sets the outer angle of spot-light.
		/// </summary>
		[Category( "Light Attenuation" )]
		[DisplayName( "OuterAngle" )]
		public float AttenuationOuterAngle
		{
			get { return m_attenuationOuterAngle; }
			set
			{
				if( value < m_attenuationInnerAngle )
				{
					throw new ArgumentOutOfRangeException( "Attenuation outer angle cannot be less than inner angle." );
				}

				m_attenuationOuterAngle = value;
			}
		}

		/// <summary>
		/// Gets or sets the angular expoenet of spot-light.
		/// Angular Attenuation Equation:
		///		Attenuation = SmoothStep( 0, M, V ) ^ [AttenuationAngleExponent]
		///	where V and M is computed as
		///		M = cos( [AttenuationInnerAngle] ) - cos( [AttenuationOuterAngle] )
		///		V = cos( A ) - cos( [AttenuationOuterAngle] )
		///	, and A is the angle between the light direction and the light-to-object direction vector.
		/// </summary>
		[Category( "Light Attenuation" )]
		[DisplayName( "AngleExponent" )]
		public float AttenuationAngleExponent
		{
			get { return m_attenuationAngleExponent; }
			set
			{
				if( m_attenuationAngleExponent < 0 )
				{
					throw new ArgumentOutOfRangeException( "Attenuation angle exponent cannot be less than zero." );
				}

				m_attenuationAngleExponent = value;
			}
		}

		/// <summary>
		/// Gets the attenuation type of the light.
		/// </summary>
		[Category( "Light" )]
		[DisplayName( "Type" )]
		public LightType LightType { get { return m_lightType; } }

		/// <summary>
		/// Gets the world-space position of the light.
		/// </summary>
		[Category( "Light" )]
		[DisplayName( "WorldPosition" )]
		public Vector3 WorldPosition { get { return m_worldPosition; } }

		/// <summary>
		/// Gets the world-space direction of the light.
		/// </summary>
		[Category( "Light" )]
		[DisplayName( "WorldDirection" )]
		public Vector3 WorldDirection { get { return m_worldDirection; } }

		/// <summary>
		/// Creates a new instance of Light.
		/// </summary>
		/// <param name="attenuationType"></param>
		public Light( LightType attenuationType )
			: base( SceneObjectType.Light, "", null )
		{
			m_lightType = attenuationType;
		}

		/// <summary>
		/// Sets the distance attenuation factors for spot lights and point lights.
		/// </summary>
		/// <param name="distance"></param>
		/// <param name="exponent"></param>
		public void SetDistanceAttenuationFactor( float distance, float exponent )
		{
			AttenuationDistance = distance;
			AttenuationDistanceExponent = exponent;
		}

		/// <summary>
		/// Sets the angular attenuation factors for spot lights.
		/// </summary>
		/// <param name="innerAngle"></param>
		/// <param name="outerAngle"></param>
		/// <param name="exponent"></param>
		public void SetAngleAttenuationFactor( float innerAngle, float outerAngle, float exponent )
		{
			AttenuationInnerAngle = innerAngle;
			AttenuationOuterAngle = outerAngle;
			AttenuationAngleExponent = exponent;
		}

		/// <summary>
		/// Sets the diffuse color and the specular power.
		/// </summary>
		/// <param name="diffuseColor"></param>
		/// <param name="specularPower"></param>
		public void SetColor( Vector3 diffuseColor, float specularPower )
		{
			DiffuseColor = diffuseColor;
			SpecularPower = specularPower;
		}

		/// <summary>
		/// Tests if the light is visible from the view frustum or not.
		/// </summary>
		/// <param name="viewFrustum"></param>
		/// <returns></returns>
		public bool IsVisibleFrom( BoundingFrustum viewFrustum )
		{
			if( m_lightType == LightType.Directional )
			{
				// Does not attenuate. Always visible.
				return true;
			}
			else if( m_lightType == LightType.Point )
			{
				// Get bounding sphere.
				// Used attenuation distance with no scaling.
				// That means, even world-space transformation has scaling other than 1.0f, Light just ignore it.
				BoundingSphere bounds = new BoundingSphere( m_worldPosition, m_attenuationDistance );
				return ( viewFrustum.Contains( bounds ) != ContainmentType.Disjoint );
			}
			else if( m_lightType == LightType.Spot )
			{
				// Get bounding sphere.
				// Used attenuation distance with no scaling.
				// That means, even world-space transformation has scaling other than 1.0f, Light just ignore it.
				BoundingSphere bounds = new BoundingSphere( m_worldPosition, m_attenuationDistance );
				return ( viewFrustum.Contains( bounds ) != ContainmentType.Disjoint );
			}

			return false;
		}

		/// <summary>
		/// Projects the light into the camera's projection space of [0,1] and gets its bounds.
		/// Note that this computation is just an approximation of the real projection image.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void GetProjectedExtent( Camera camera, out float x, out float y, out float width, out float height )
		{
			if( m_lightType == LightType.Directional )
			{
				// Infinite type of light spans the full screen.
				x = 0;
				y = 0;
				width = 1;
				height = 1;
			}
			else
			{
				if( Vector3.Distance( m_worldPosition, camera.WorldPosition ) <= m_attenuationDistance )
				{
					// If the camera is inside the light's bounds, just draw a full-screen quad.
					x = 0;
					y = 0;
					width = 1;
					height = 1;
				}
				else
				{
#warning Need to implement here. Projecting a sphere onto an image plane?
#if true
					x = 0;
					y = 0;
					width = 1;
					height = 1;
#else
					// Get the concatenated view-project matrix of the camera.
					Matrix viewProjection = camera.ViewMatrix * camera.ProjectionMatrix;

					// Get the projected position of the light's center.
					Vector4 center = new Vector4( m_worldPosition, 1 );
					Vector4 centerProjected;
					Vector4.Transform( ref center, ref viewProjection, out centerProjected );
					centerProjected.X = ( ( centerProjected.X / centerProjected.W ) + 1.0f ) * 0.5f;
					centerProjected.Y = ( 1.0f - ( centerProjected.Y / centerProjected.W ) ) * 0.5f;
					//Vector4.Clamp( ref centerProjected, ref s_allZero, ref s_allOne, out centerProjected );

					// Get the farthest point to the right direction of the camera.
					Vector4 rightPoint = new Vector4( m_worldPosition + camera.RightDirection * m_attenuationDistance, 1 );
					// and, its projected position.
					Vector4 rightProjected;
					Vector4.Transform( ref rightPoint, ref viewProjection, out rightProjected );
					rightProjected.X = ( ( rightProjected.X / rightProjected.W ) + 1.0f ) * 0.5f;
					rightProjected.Y = ( 1.0f - ( rightProjected.Y / rightProjected.W ) ) * 0.5f;
					//Vector4.Clamp( ref rightProjected, ref s_allZero, ref s_allOne, out rightProjected );

					// Get the farthest point to the up direction of the camera.
					Vector4 upPoint = new Vector4( m_worldPosition + camera.UpDirection * m_attenuationDistance, 1 );
					// and, its projected position.
					Vector4 upProjected;
					Vector4.Transform( ref upPoint, ref viewProjection, out upProjected );
					upProjected.X = ( ( upProjected.X / upProjected.W ) + 1.0f ) * 0.5f;
					upProjected.Y = ( 1.0f - ( upProjected.Y / upProjected.W ) ) * 0.5f;
					//Vector4.Clamp( ref upProjected, ref s_allZero, ref s_allOne, out upProjected );

					// Get the half size.
					float halfWidth = rightProjected.X - centerProjected.X;
					float halfHeight = centerProjected.Y - upProjected.Y;

					// Get the rectangle.
					x = Math.Min( 1.0f, Math.Max( 0.0f, centerProjected.X - halfWidth ) );
					y = Math.Min( 1.0f, Math.Max( 0.0f, centerProjected.Y - halfHeight ) );
					width = ( halfWidth * 2.0f + x > 1.0f ) ? ( 1.0f - x ) : halfWidth * 2.0f;
					height = ( halfHeight * 2.0f + y > 1.0f ) ? ( 1.0f - y ) : halfHeight * 2.0f;
#endif
				}
			}
		}

		protected override void OnUpdate( UpdateContext updateContext, bool bForceUpdateTransformation )
		{
			// Light object does simply ignore local transformation.
			// Instead, use the parent object's world-space transformation matrix.
			// Compute world-space light direction and position.
			if( Parent != null )
			{
				Matrix parentWorldMatrix = Matrix.Identity;

				Vector3.Transform( ref m_position, ref parentWorldMatrix, out m_worldPosition );
				Vector3.TransformNormal( ref m_direction, ref parentWorldMatrix, out m_worldDirection );
			}
			else
			{
				m_worldPosition = m_position;
				m_worldDirection = m_direction;
			}
		}
	}
}