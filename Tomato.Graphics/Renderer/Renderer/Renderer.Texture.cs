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
		public Texture LoadTexture( string textureName )
		{
			// If cached texture is available, return cached one.
			Texture texture;
			if( m_applicationTextures.TryGetValue( textureName, out texture ) )
			{
				return texture;
			}

			// Load from content manager.
			texture = m_applicationContents.Load<Texture>( textureName );
			if( texture != null )
			{
				// Cache texture.
				m_applicationTextures[ textureName ] = texture;
			}

			return texture;
		}

		internal Texture LoadInternalTexture( string textureName )
		{
			// If cached texture is available, return cached one.
			Texture texture;
			if( m_rendererTextures.TryGetValue( textureName, out texture ) )
			{
				return texture;
			}

			// Load from content manager.
			texture = m_rendererContents.Load<Texture>( textureName );
			if( texture != null )
			{
				// Cache texture.
				m_rendererTextures[ textureName ] = texture;
			}

			return texture;
		}


		/// <summary>
		/// Set the texture and the sampler to the device.
		/// </summary>
		/// <param name="samplerIndex"></param>
		/// <param name="texture"></param>
		/// <param name="sampler"></param>
		public void SetTexture( int samplerIndex, TextureSampler textureSampler )
		{
			// Load the texture.
			Texture texture = null;
			switch( textureSampler.SourceType )
			{
				case TextureSampler.TextureType.RenderTargetTexture:
					{
						texture = LoadRenderTarget( textureSampler.TextureName );
					}
					break;

				case TextureSampler.TextureType.ApplicationTexture:
					{
						texture = LoadTexture( textureSampler.TextureName );
					}
					break;

				case TextureSampler.TextureType.RendererTexture:
					{
						texture = LoadInternalTexture( textureSampler.TextureName );
					}
					break;
			}

			// Set texture and sampler state.
			if( texture != null )
			{
				m_graphicsDevice.Textures[ samplerIndex ] = texture;
				m_graphicsDevice.SamplerStates[ samplerIndex ] = textureSampler.SamplerState;
			}
		}

		public void SetTexture( int samplerIndex, Texture texture, SamplerState samplerState )
		{
			m_graphicsDevice.Textures[ samplerIndex ] = texture;
			m_graphicsDevice.SamplerStates[ samplerIndex ] = samplerState;
		}

		public void ResetAllTextures()
		{
			for( int i = 0 ; i < m_samplerCount ; ++i )
			{
				m_graphicsDevice.Textures[ i ] = null;
			}
		}

		private void DetectMaximumSamplerCount()
		{
			int count = 0;
			while( true )
			{
				try
				{
					m_graphicsDevice.Textures[ count ] = null;
					count++;
				}
				catch( Exception )
				{
					break;
				}
			}

			m_samplerCount = count;
		}
	}
}