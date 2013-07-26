using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics
{
	public class GaussianBlurPass : FullScreenRenderPass
	{
		public enum Direction
		{
			Horizontal,
			Vertical
		}
		
		public TextureSampler InputTexture { get; set; }

		public GaussianBlurPass( Renderer renderer, Direction direction )
			: base( renderer, "GaussianBlur Pass", renderer.LoadInternalEffect( "Effects/GaussianBlur" ), ( direction == Direction.Horizontal ) ? "GaussianBlur.Horizontal" : "GaussianBlur.Vertical" )
		{
			InputTexture = null;

			SetClearOptions( false, false );
		}

		protected override void OnRender( TimeSpan elapsedTime )
		{
			if( InputTexture != null )
			{
				// Set input texture.
				Textures[ 0 ] = InputTexture;

				// Set effect parameters.
				int width, height;
				Renderer.GetCurrentRenderTargetDimension( out width, out height );
				Vector2 inverseRTDimension = new Vector2( 1.0f / ( float )width, 1.0f / ( float )height );
				Renderer.SetEffectParameterBySemantic( Effect, "INVRTDIM", ref inverseRTDimension );

				// Go.
				base.OnRender( elapsedTime );
			}
			else
			{
				// Do nothing here.
			}
		}
	}
}
