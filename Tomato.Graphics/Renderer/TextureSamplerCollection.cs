using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics
{
	public class TextureSamplerCollection
	{
		private Dictionary<int, TextureSampler> m_textureSamplers = null;

		public TextureSampler this[ int index ]
		{
			get
			{
				return m_textureSamplers[ index ];
			}
			set
			{
				m_textureSamplers[ index ] = value;
			}
		}

		public TextureSamplerCollection()
		{
			m_textureSamplers = new Dictionary<int, TextureSampler>();
		}

		/// <summary>
		/// Set textures and samplers to the renderer.
		/// </summary>
		/// <param name="renderer"></param>
		public void SetToRenderer( Renderer renderer )
		{
			foreach( var texture in m_textureSamplers )
			{
				renderer.SetTexture( texture.Key, texture.Value );
			}
		}
	}
}
