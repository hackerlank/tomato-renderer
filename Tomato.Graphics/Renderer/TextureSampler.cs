using System;
using Microsoft.Xna.Framework.Graphics;
using System.ComponentModel;

namespace Tomato.Graphics
{
	public class TextureSampler
	{
		public enum TextureType
		{
			RenderTargetTexture,
			ApplicationTexture,
			RendererTexture
		}

		[Category( "Texture Sampler" )]
		[DisplayName( "TextureName" )]
		public string TextureName { get; set; }

		[Category( "Texture Sampler" )]
		[DisplayName( "SamplerState" )]
		public SamplerState SamplerState { get; set; }

		[Category( "Texture Sampler" )]
		[DisplayName( "SourceType" )]
		public TextureType SourceType { get; set; }

		public TextureSampler( string textureName, SamplerState samplerState )
			: this( textureName, samplerState, TextureType.RenderTargetTexture )
		{
		}

		public TextureSampler( string textureName, SamplerState samplerState, TextureType sourceType )
		{
			TextureName = textureName;
			SamplerState = samplerState;
			SourceType = sourceType;
		}
	}
}
