using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics.Vertex
{
	[Serializable, StructLayout( LayoutKind.Sequential )]
	public struct SpotLightVertex
	{
		public Vector3 Position;
		public Vector3 LightPosition;
		public Vector3 LightDirection;
		public Vector4 LightColor;
		public Vector2 LightAttenuation;
		public Vector3 LightSpotAttenuation;

		public static VertexDeclaration VertexDeclaration { get; private set; }

		static SpotLightVertex()
		{
			VertexElement[] elements = new VertexElement[]
			{ 
				new VertexElement( 0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0 ), 
				new VertexElement( 12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0 ),
				new VertexElement( 24, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 1 ),
				new VertexElement( 36, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 2 ),
				new VertexElement( 52, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 3 ),
				new VertexElement( 60, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 4 ) 
			};

			VertexDeclaration declaration = new VertexDeclaration( elements );
			declaration.Name = "VertexPositionTexture.VertexDeclaration";

			VertexDeclaration = declaration;
		}

		public SpotLightVertex(
			Vector3 position,
			Vector3 lightPosition,
			Vector3 lightDirection,
			Vector3 lightDiffuse,
			float lightSpecularPower,
			float lightAttenuationDistance,
			float lightAttenuationDistanceExponent,
			float lightAttenuationInnerAngle,
			float lightAttenuationOuterAngle,
			float lightAttenuationAngleExponent )
		{
			Position = position;
			LightPosition = lightPosition;
			LightDirection = lightDirection;
			LightColor = new Vector4( lightDiffuse, lightSpecularPower );
			LightAttenuation = new Vector2( lightAttenuationDistance, lightAttenuationDistanceExponent );
			
			LightSpotAttenuation = new Vector3( 
				(float)System.Math.Cos( lightAttenuationInnerAngle ),
				( float )System.Math.Cos( lightAttenuationOuterAngle ),
				lightAttenuationAngleExponent );
		}
	}
}