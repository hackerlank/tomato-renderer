using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics
{
	/// <summary>
	/// Defines the post-processing render pass.
	/// </summary>
	public class FinalPass : FullScreenRenderPass
	{
		public enum ColorOverrideMode
		{
			Default,
			RedChannelOnly,
			GreenChannelOnly,
			BlueChannelOnly,
			AlphaChannelOnly
		}

		private Camera m_sceneCamera = null;

		/// <summary>
		/// Gets or sets the tone mapping key value console variable.
		/// </summary>
		//[Category( "Tone Mapping" )]
		//[DisplayName( "KeyValue" )]
		//public float ToneMapKeyValue { get; set; }

		[Category( "Tone Mapping" )]
		[DisplayName( "ExposureAdjust" )]
		public float ToneMapExposureAdjust { get; set; }
		
		/// <summary>
		/// Gets or sets the tone mapping shoulder strength console variable.
		/// </summary>
		[Category( "Tone Mapping" )]
		[DisplayName( "ShoulderStrength" )]
		public float ToneMapShoulderStrength { get; set; }

		/// <summary>
		/// Gets or sets the tone mapping linear strength console variable.
		/// </summary>
		[Category( "Tone Mapping" )]
		[DisplayName( "LinearStrength" )]
		public float ToneMapLinearStrength { get; set; }

		/// <summary>
		/// Gets or sets the tone mapping linear angle console variable.
		/// </summary>
		[Category( "Tone Mapping" )]
		[DisplayName( "LinearAngle" )]
		public float ToneMapLinearAngle { get; set; }

		/// <summary>
		/// Gets or sets the tone mapping toe strength console variable.
		/// </summary>
		[Category( "Tone Mapping" )]
		[DisplayName( "ToeStrength" )]
		public float ToneMapToeStrength { get; set; }

		/// <summary>
		/// Gets or sets the tone mapping toe denominator console variable.
		/// </summary>
		[Category( "Tone Mapping" )]
		[DisplayName( "ToeDenominator" )]
		public float ToneMapToeDenominator { get; set; }

		/// <summary>
		/// Gets or sets the tone mapping toe numerator console variable.
		/// </summary>
		[Category( "Tone Mapping" )]
		[DisplayName( "ToeNumerator" )]
		public float ToneMapToeNumerator { get; set; }

		/// <summary>
		/// Gets or sets the tone mapping white point console variable.
		/// </summary>
		[Category( "Tone Mapping" )]
		[DisplayName( "WhiteCutoff" )]
		public float ToneMapWhitePoint { get; set; }

		[Category( "Tone Mapping" )]
		[DisplayName( "Enabled" )]
		public bool ToneMapEnabled { get; set; }

		/// <summary>
		/// Gets or sets or sets the output color overriding mode.
		/// </summary>
		[Category( "Post Process" )]
		[DisplayName( "OutputColor" )]		
		public ColorOverrideMode ColorMode { get; set; }

		[Category( "Depth of Field" )]
		[DisplayName( "FocalDistance" )]
		public float DepthOfFieldDistance { get; set; }

		[Category( "Depth of Field" )]
		[DisplayName( "FocalRange" )]
		public float DepthOfFieldRange { get; set; }

		[Category( "Depth of Field" )]
		[DisplayName( "Enabled" )]
		public bool DepthOfFieldEnabled { get; set; }

		/// <summary>
		/// Creates a PostProcessPass instance.
		/// </summary>
		/// <param name="renderer"></param>
		public FinalPass( Renderer renderer, Camera sceneCamera )
			: base( renderer, "Final Pass", renderer.LoadInternalEffect( "Effects/Final" ) )
		{
			m_sceneCamera = sceneCamera;

			// Intialize tone-mapipng parameters.
			//ToneMapKeyValue = 0.36f;
			ToneMapExposureAdjust = 10.0f;
			ToneMapShoulderStrength = 0.15f;
			ToneMapLinearStrength = 0.5f;
			ToneMapLinearAngle = 0.1f;
			ToneMapToeStrength = 0.2f;
			ToneMapToeDenominator = 0.3f;
			ToneMapToeNumerator = 0.02f;
			ToneMapWhitePoint = 11.2f;
			ToneMapEnabled = true;

			// Initialize depth-of-field parameters
			DepthOfFieldDistance = 50.0f;
			DepthOfFieldRange = 100.0f;
			DepthOfFieldEnabled = true;

			// Set output color mode.
			ColorMode = ColorOverrideMode.Default;
		}

		protected override bool OnRenderStarting()
		{
			// Determine the effect identifier.
			switch( ColorMode )
			{
				case ColorOverrideMode.Default: 
					{
						if( DepthOfFieldEnabled )
						{
							if( ToneMapEnabled )
							{
								EffectIdentifier = "Final.DoFToneMapPass";
							}
							else
							{
								EffectIdentifier = "Final.DoFPass";
							}
						}
						else
						{
							if( ToneMapEnabled )
							{
								EffectIdentifier = "Final.ToneMapPass";
							}
							else
							{
								EffectIdentifier = "Final.DefaultPass";
							}
						}
					} 
					break;
				case ColorOverrideMode.RedChannelOnly: { EffectIdentifier = "Final.RedChannelOnlyPass"; } break;
				case ColorOverrideMode.GreenChannelOnly: { EffectIdentifier = "Final.GreenChannelOnlyPass"; } break;
				case ColorOverrideMode.BlueChannelOnly: { EffectIdentifier = "Final.BlueChannelOnlyPass"; } break;
				case ColorOverrideMode.AlphaChannelOnly: { EffectIdentifier = "Final.AlphaChannelOnlyPass"; } break;
			}

			// Update the console variables.
			UpdateConsoleVariables();

			// Update effect parameters.
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
			Renderer.SetEffectParameterBySemantic( Effect, "FARCLIPCORNERS", farClipCorners );

			return true;
		}

		private void UpdateConsoleVariables()
		{
			// Tone mapping
			//Renderer.SetEffectParameterBySemantic( Effect, "FTMKV", ToneMapKeyValue );
			Renderer.SetEffectParameterBySemantic( Effect, "FTMEA", ToneMapExposureAdjust );
			Renderer.SetEffectParameterBySemantic( Effect, "FTMSS", ToneMapShoulderStrength );
			Renderer.SetEffectParameterBySemantic( Effect, "FTMLS", ToneMapLinearStrength );
			Renderer.SetEffectParameterBySemantic( Effect, "FTMLA", ToneMapLinearAngle );
			Renderer.SetEffectParameterBySemantic( Effect, "FTMTS", ToneMapToeStrength );
			Renderer.SetEffectParameterBySemantic( Effect, "FTMTD", ToneMapToeDenominator );
			Renderer.SetEffectParameterBySemantic( Effect, "FTMTN", ToneMapToeNumerator );
			Renderer.SetEffectParameterBySemantic( Effect, "FTMWP", ToneMapWhitePoint );

			// Depth-of-field
			Renderer.SetEffectParameterBySemantic( Effect, "DOFDISTANCE", DepthOfFieldDistance );
			Renderer.SetEffectParameterBySemantic( Effect, "DOFRANGE", DepthOfFieldRange );
		}

	}
}
