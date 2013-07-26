using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics.Vertex
{
	[Serializable, StructLayout( LayoutKind.Sequential )]
	public struct DirectionalLightVertex
	{
		public Vector3 Position;
		public Vector3 LightDirection;
		public Vector4 LightColor;

		public static VertexDeclaration VertexDeclaration { get; private set; }

		static DirectionalLightVertex()
		{
			VertexElement[] elements = new VertexElement[]
			{ 
				new VertexElement( 0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0 ), 
				new VertexElement( 12, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0 ),
				new VertexElement( 24, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1 ),
			};

			VertexDeclaration declaration = new VertexDeclaration( elements );
			declaration.Name = "VertexPositionTexture.VertexDeclaration";

			VertexDeclaration = declaration;
		}

		public DirectionalLightVertex( Vector3 position, Vector3 lightDirection, Vector3 lightDiffuse, float lightSpecularPower )
		{
			Position = position;
			LightDirection = lightDirection;
			LightColor = new Vector4( lightDiffuse, lightSpecularPower );
		}
	}
}