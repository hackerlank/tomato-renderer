using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics.Vertex
{
	[Serializable, StructLayout( LayoutKind.Sequential )]
	public struct PointLightVertex
	{
		public Vector3 Position;
		public Vector3 LightPosition;
		public Vector4 LightColor;
		public Vector2 LightAttenuation;

		public static VertexDeclaration VertexDeclaration { get; private set; }

		static PointLightVertex()
		{
			VertexElement[] elements = new VertexElement[]
			{ 
				new VertexElement( 0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0 ), 
				new VertexElement( 12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0 ),
				new VertexElement( 24, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1 ),
				new VertexElement( 40, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2 ) 
			};

			VertexDeclaration declaration = new VertexDeclaration( elements );
			declaration.Name = "VertexPositionTexture.VertexDeclaration";

			VertexDeclaration = declaration;
		}

		public PointLightVertex( 
			Vector3 position, 
			Vector3 lightPosition, 
			Vector3 lightDiffuse, 
			float lightSpecularPower, 
			float lightAttenuationDistance, 
			float lightAttenuationDistanceExponent )
		{
			Position = position;
			LightPosition = lightPosition;
			LightColor = new Vector4( lightDiffuse, lightSpecularPower );
			LightAttenuation = new Vector2( lightAttenuationDistance, lightAttenuationDistanceExponent );
		}
	}
}