using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Tomato.Graphics.Vertex;

namespace Tomato.Graphics
{
	public class OcclusionPass : FullScreenRenderPass
	{
		[Category( "SSAO" )]
		[DisplayName( "SamplingRadius" )]
		public float SSAOSamplingDistance { get; set; }

		[Category( "SSAO" )]
		[DisplayName( "Intensity" )]
		public float SSAOIntensity { get; set; }

		[Category( "SSAO" )]
		[DisplayName( "DistanceScale" )]
		public float SSAODistanceScale { get; set; }

		[Category( "SSAO" )]
		[DisplayName( "OcclusionConeWidth" )]
		public float SSAOOcclusionConeWidth { get; set; }

		public OcclusionPass( Renderer renderer )
			: base( renderer, "Occlusion Pass", renderer.LoadInternalEffect( "Effects/Occlusion" ) )
		{
			SSAOSamplingDistance = 1.1f;
			SSAOIntensity = 1.4f;
			SSAODistanceScale = 1.0f;
			SSAOOcclusionConeWidth = 0.3f;
		}

		protected override bool OnRenderStarting()
		{
			Renderer.SetEffectParameterBySemantic( Effect, "SSAOSAMPDIST", SSAOSamplingDistance );
			Renderer.SetEffectParameterBySemantic( Effect, "SSAOINT", SSAOIntensity );
			Renderer.SetEffectParameterBySemantic( Effect, "SSAODISTSCALE", SSAODistanceScale );
			Renderer.SetEffectParameterBySemantic( Effect, "SSAOCONEWIDTH", SSAOOcclusionConeWidth );

			return true;
		}
	}
}