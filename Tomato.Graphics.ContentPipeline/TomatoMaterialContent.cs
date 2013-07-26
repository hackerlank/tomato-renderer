using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace Tomato.Graphics.Content.Pipeline
{
	public class TomatoMaterialContent : MaterialContent
	{
		public Vector3 EmissiveColor { get; set; }
		public Vector3 DiffuseColor { get; set; }
		public Vector3 SpecularColor { get; set; }
		public float SpecularPower { get; set; }
		public float Alpha { get; set; }

		/// <summary>
		/// Specifies the optical density for the surface.  This is also known as index of refraction.
		/// Optical_density is the value for the optical density.  
		/// The values can range from 0.001 to 10.  A value of 1.0 means that light does not bend as it passes through an object.  
		/// Increasing the optical_density increases the amount of bending.  
		/// Glass has an index of refraction of about 1.5.  Values of less than 1.0 produce bizarre results and are not recommended.
		/// </summary>
		public float OpticalDensity { get; set; }

		/// <summary>
		/// The Tf statement specifies the transmission filter using RGB values.
		/// "r g b" are the values for the red, green, and blue components of the atmosphere.  
		/// The g and b arguments are optional.  If only r is specified, then g, and b are assumed to be equal to r.  
		/// The r g b values are normally in the range of 0.0 to 1.0.  
		/// Values outside this range increase or decrease the relectivity accordingly.
		/// </summary>
		public Vector3 TransmissionFilter { get; set; }

		public ExternalReference<TextureContent> AmbientTexture { get { return GetTexture( "Ambient" ); } }
		public ExternalReference<TextureContent> DiffuseTexture { get { return GetTexture( "Diffuse" ); } }
		public ExternalReference<TextureContent> SpecularTexture { get { return GetTexture( "Specular" ); } }
		public ExternalReference<TextureContent> AlphaTexture { get { return GetTexture( "Alpha" ); } }
		public ExternalReference<TextureContent> BumpTexture { get { return GetTexture( "Bump" ); } }

		public TomatoMaterialContent()
		{
			EmissiveColor = Vector3.Zero;
			DiffuseColor = Vector3.Zero;
			SpecularColor = Vector3.Zero;
			SpecularPower = 0;
			Alpha = 0;

			OpticalDensity = 1;
			TransmissionFilter = Vector3.Zero;
		}

		public void SetAmbientTexture( ExternalReference<TextureContent> texture )
		{
			if( texture != null )
			{
				Textures[ "Ambient" ] = texture;
			}
		}

		public void SetDiffuseTexture( ExternalReference<TextureContent> texture )
		{
			if( texture != null )
			{
				Textures[ "Diffuse" ] = texture;
			}
		}

		public void SetSpecularTexture( ExternalReference<TextureContent> texture )
		{
			if( texture != null )
			{
				Textures[ "Specular" ] = texture;
			}
		}

		public void SetAlphaTexture( ExternalReference<TextureContent> texture )
		{
			if( texture != null )
			{
				Textures[ "Alpha" ] = texture;
			}
		}

		public void SetBumpTexture( ExternalReference<TextureContent> texture )
		{
			if( texture != null )
			{
				Textures[ "Bump" ] = texture;
			}
		}
	}
}
