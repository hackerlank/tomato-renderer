using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics
{
	partial class Renderer
	{
		/// <summary>
		/// Loads an effect from the application contents.
		/// </summary>
		/// <param name="effectName"></param>
		/// <returns></returns>
		public Effect LoadEffect( string effectName )
		{
			// If cached effect is available, return cached one.
			Effect effect;
			if( m_applicationEffects.TryGetValue( effectName, out effect ) )
			{
				return effect;
			}

			// Load from content manager.
			effect = m_applicationContents.Load<Effect>( effectName );
			if( effect != null )
			{
				// Cache effect.
				m_applicationEffects[ effectName ] = effect;
			}

			return effect;
		}

		/// <summary>
		/// Loads an internal effect.
		/// </summary>
		/// <param name="effectName"></param>
		/// <returns></returns>
		internal Effect LoadInternalEffect( string effectName )
		{
			// If cached effect is available, return cached one.
			Effect effect;
			if( m_rendererEffects.TryGetValue( effectName, out effect ) )
			{
				return effect;
			}

			// Load from content manager.
			effect = m_rendererContents.Load<Effect>( effectName );
			if( effect != null )
			{
				// Cache effect.
				m_rendererEffects[ effectName ] = effect;
			}

			return effect;
		}

		/// <summary>
		/// Selects the appropriate effect pass using identifer and apply it to the device.
		/// Argument identifier specifies the name of the effect pass to apply and an optional effect technique name such as
		///		DeferredLighting.GeometryPass
		///			Use the technique with the name of 'DeferredLighting' and the pass with the name of 'GeometryPass'.
		///		LightingPass
		///			Use the current technique and the pass with the name of 'LightingPass'.
		///	If identifier is null of whitespace, the first effect pass of the current effect technique is applied.
		/// </summary>
		/// <param name="effect"></param>
		/// <param name="identifier"></param>
		public void ApplyEffect( Effect effect, string identifier )
		{
			if( string.IsNullOrWhiteSpace( identifier ) )
			{
				// Use current technique and the fist pass.
				effect.CurrentTechnique.Passes[ 0 ].Apply();
			}
			else
			{
				string effectTechniqueName = null;
				string effectPassName = null;

				// Parse the effect technique name and the effect pass name.
				int index = identifier.IndexOf( '.' );
				if( index < 0 )
				{
					effectPassName = identifier;
				}
				else
				{
					effectTechniqueName = identifier.Substring( 0, index );
					effectPassName = identifier.Substring( index + 1 );
				}

				// Set the current effect technique.
				if( effectTechniqueName != null )
				{
					EffectTechnique effectTechnique = effect.Techniques[ effectTechniqueName ];
					if( effectTechnique != null )
					{
						// Set the current effect technique with that name.
						effect.CurrentTechnique = effectTechnique;
					}
					else
					{
						throw new InvalidOperationException( string.Format( "Cannot find an effect technique with the name of {0}.", effectTechniqueName ) );
					}
				}
				
				// Apply the effect pass.
				EffectPass effectPass = effect.CurrentTechnique.Passes[ effectPassName ];
				if( effectPass != null )
				{
					// Apply that pass.
					effectPass.Apply();
				}
				else
				{
					throw new InvalidOperationException( string.Format( "Cannot find an effect pass with the name of {0}.", effectPassName ) );
				}						
			}
		}

		public void SetEffectGeneralParameters( Effect effect )
		{
			// Render-target dimension
			int width, height;
			GetCurrentRenderTargetDimension( out width, out height );
			Vector4 renderTargetDimension = new Vector4(
				( float )( width ),
				( float )( height ),
				1.0f / ( float )( width ),
				1.0f / ( float )( height ) );

			SetEffectParameterBySemantic( effect, "RTDIMENSION", ref renderTargetDimension );
		}

		/// <summary>
		/// Sets transfomration parameters of effect.
		/// </summary>
		/// <param name="effect"></param>
		/// <param name="worldTransformation"></param>
		/// <param name="camera"></param>
		public void SetEffectTransformationParameters(
			Effect effect,
			Matrix worldTransformation,
			Camera camera )
		{
			// Set world, view, projection matrices.
			Matrix world = worldTransformation;
			Matrix view = camera.ViewMatrix;
			Matrix projection = camera.ProjectionMatrix;

			IEffectMatrices effectMatrices = effect as IEffectMatrices;
			if( effectMatrices != null )
			{
				effectMatrices.World = world;
				effectMatrices.View = view;
				effectMatrices.Projection = projection;
			}

			Matrix worldView;
			Matrix.Multiply( ref world, ref view, out worldView );

			Matrix viewProjection;
			Matrix.Multiply( ref view, ref projection, out viewProjection );

			Matrix worldViewProjection;
			Matrix.Multiply( ref worldView, ref projection, out worldViewProjection );

			SetEffectParameterBySemantic( effect, "WORLD", ref world );
			SetEffectParameterBySemantic( effect, "VIEW", ref view );
			SetEffectParameterBySemantic( effect, "PROJECTION", ref projection );
			SetEffectParameterBySemantic( effect, "WORLDVIEW", ref worldView );
			SetEffectParameterBySemantic( effect, "VIEWPROJECTION", ref viewProjection );
			SetEffectParameterBySemantic( effect, "WORLDVIEWPROJECTION", ref worldViewProjection );

			// Set camera projection parameters.
			Vector4 cameraProjectionParameters = new Vector4(
				camera.FieldOfView,
				camera.AspectRatio,
				camera.NearClipPlane,
				camera.FarClipPlane );
			SetEffectParameterBySemantic( effect, "PROJECTIONPARAMS", ref cameraProjectionParameters );

			// Set the view-space far-clipping plane corners.
			Vector3[] farClipCorners = new Vector3[ 4 ];
			{
				float farH = 2.0f * ( float )System.Math.Tan( camera.FieldOfView / 2.0f ) * camera.FarClipPlane;
				float farW = farH * camera.AspectRatio;
				float farX = farW / 2.0f;
				float farY = farH / 2.0f;

				// 뷰 공간 코너
				farClipCorners[ 0 ] = new Vector3( -farX, +farY, -camera.FarClipPlane );
				farClipCorners[ 1 ] = new Vector3( +farX, +farY, -camera.FarClipPlane );
				farClipCorners[ 2 ] = new Vector3( -farX, -farY, -camera.FarClipPlane );
				farClipCorners[ 3 ] = new Vector3( +farX, -farY, -camera.FarClipPlane );
			}
			SetEffectParameterBySemantic( effect, "FARCLIPCORNERS", farClipCorners );
		}

		/// <summary>
		/// Sets object material parameters of effect.
		/// </summary>
		/// <param name="effect"></param>
		/// <param name="objectMaterialState"></param>
		public void SetEffectObjectMaterialParameters( Effect effect, ObjectMaterialState objectMaterialState )
		{
			SetEffectParameterBySemantic( effect, "OBJECTEMISSIVE", ref objectMaterialState.Emissive );
			SetEffectParameterBySemantic( effect, "OBJECTDIFFUSE", ref objectMaterialState.Diffuse );
			SetEffectParameterBySemantic( effect, "OBJECTSPECULAR", ref objectMaterialState.Specular );
			SetEffectParameterBySemantic( effect, "OBJECTSHINENESS", objectMaterialState.Shineness );
		}

		public void SetEffectParameterBySemantic( Effect effect, string semanticName, float value )
		{
			EffectParameter parameter = effect.Parameters.GetParameterBySemantic( semanticName );
			if( parameter != null )
			{
				parameter.SetValue( value );
			}
		}

		public void SetEffectParameterBySemantic( Effect effect, string semanticName, int value )
		{
			EffectParameter parameter = effect.Parameters.GetParameterBySemantic( semanticName );
			if( parameter != null )
			{
				parameter.SetValue( value );
			}
		}

		public void SetEffectParameterBySemantic( Effect effect, string semanticName, bool value )
		{
			EffectParameter parameter = effect.Parameters.GetParameterBySemantic( semanticName );
			if( parameter != null )
			{
				parameter.SetValue( value );
			}
		}

		public void SetEffectParameterBySemantic( Effect effect, string semanticName, Vector2 value )
		{
			EffectParameter parameter = effect.Parameters.GetParameterBySemantic( semanticName );
			if( parameter != null )
			{
				parameter.SetValue( value );
			}
		}

		public void SetEffectParameterBySemantic( Effect effect, string semanticName, ref Vector2 value )
		{
			EffectParameter parameter = effect.Parameters.GetParameterBySemantic( semanticName );
			if( parameter != null )
			{
				parameter.SetValue( value );
			}
		}

		public void SetEffectParameterBySemantic( Effect effect, string semanticName, Vector3 value )
		{
			EffectParameter parameter = effect.Parameters.GetParameterBySemantic( semanticName );
			if( parameter != null )
			{
				parameter.SetValue( value );
			}
		}

		public void SetEffectParameterBySemantic( Effect effect, string semanticName, ref Vector3 value )
		{
			EffectParameter parameter = effect.Parameters.GetParameterBySemantic( semanticName );
			if( parameter != null )
			{
				parameter.SetValue( value );
			}
		}

		public void SetEffectParameterBySemantic( Effect effect, string semanticName, Vector3[] value )
		{
			EffectParameter parameter = effect.Parameters.GetParameterBySemantic( semanticName );
			if( parameter != null )
			{
				parameter.SetValue( value );
			}
		}

		public void SetEffectParameterBySemantic( Effect effect, string semanticName, Vector4 value )
		{
			EffectParameter parameter = effect.Parameters.GetParameterBySemantic( semanticName );
			if( parameter != null )
			{
				parameter.SetValue( value );
			}
		}

		public void SetEffectParameterBySemantic( Effect effect, string semanticName, ref Vector4 value )
		{
			EffectParameter parameter = effect.Parameters.GetParameterBySemantic( semanticName );
			if( parameter != null )
			{
				parameter.SetValue( value );
			}
		}

		public void SetEffectParameterBySemantic( Effect effect, string semanticName, Matrix value )
		{
			EffectParameter parameter = effect.Parameters.GetParameterBySemantic( semanticName );
			if( parameter != null )
			{
				parameter.SetValue( value );
			}
		}

		public void SetEffectParameterBySemantic( Effect effect, string semanticName, ref Matrix value )
		{
			EffectParameter parameter = effect.Parameters.GetParameterBySemantic( semanticName );
			if( parameter != null )
			{
				parameter.SetValue( value );
			}
		}
	}
}