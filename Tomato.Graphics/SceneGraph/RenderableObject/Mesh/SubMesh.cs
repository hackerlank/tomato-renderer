using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaModelMeshPart = Microsoft.Xna.Framework.Graphics.ModelMeshPart;
using TomatoModelMeshPart = Tomato.Graphics.Content.ModelMeshPart;

namespace Tomato.Graphics
{
	public class SubMesh
	{
		private int m_vertexOffset;
		private int m_vertexCount;
		private int m_primitiveCount;
		private int m_startIndex;

		private VertexBuffer m_vertexBuffer;
		private IndexBuffer m_indexBuffer;

		private Effect m_effect;
		private ObjectMaterialState m_objectMaterialState;

		private Texture m_ambientTexture;
		private Texture m_diffuseTexture;
		private Texture m_specularTexture;
		private Texture m_alphaTexture;
		private Texture m_bumpTexture;

		public SubMesh( TomatoModelMeshPart meshPart )
		{
			m_effect = meshPart.Effect;

			m_vertexBuffer = meshPart.VertexBuffer;
			m_vertexOffset = meshPart.VertexOffset;

			m_indexBuffer = meshPart.IndexBuffer;

			m_vertexCount = meshPart.VertexCount;
			m_primitiveCount = meshPart.PrimitiveCount;

			m_startIndex = meshPart.StartIndex;

			m_ambientTexture = meshPart.AmbientTexture;
			m_diffuseTexture = meshPart.DiffuseTexture;
			m_specularTexture = meshPart.SpecularTexture;
			m_alphaTexture  = meshPart.AlphaTexture;
			m_bumpTexture = meshPart.BumpTexture;

			m_objectMaterialState = ( ObjectMaterialState )( meshPart.ObjectMaterial.Clone() );
		}

		public SubMesh( XnaModelMeshPart meshPart )
		{
			m_effect = meshPart.Effect;

			m_vertexBuffer = meshPart.VertexBuffer;
			m_vertexOffset = meshPart.VertexOffset;

			m_indexBuffer = meshPart.IndexBuffer;

			m_vertexCount = meshPart.NumVertices;
			m_primitiveCount = meshPart.PrimitiveCount;

			m_startIndex = meshPart.StartIndex;

			m_ambientTexture = null;
			m_diffuseTexture = XnaConverter.GetDiffuseTexture( meshPart.Effect );
			m_specularTexture = null;
			m_alphaTexture = null;
			m_bumpTexture = null;	

			m_objectMaterialState = XnaConverter.GetObjectMaterial( meshPart.Effect );
		}

		public void Render( Renderer renderer, Matrix transformation, Camera camera, string effectIdentifier, TextureSamplerCollection textures )
		{
			// Set effect parameters.
			renderer.SetEffectGeneralParameters( m_effect );
			renderer.SetEffectTransformationParameters( m_effect, transformation, camera );
			renderer.SetEffectObjectMaterialParameters( m_effect, m_objectMaterialState );

			// Set the diffuse textures.
			if( m_diffuseTexture != null )
			{
#warning A sampler state must be passed from the content pipeline. For now, just use LineaerClamp.
				renderer.SetTexture( 0, m_diffuseTexture, SamplerState.LinearWrap );
			}

			// Set additional textures.
			if( textures != null )
			{
				textures.SetToRenderer( renderer );
			}
	
			// Apply an effect pas.
			renderer.ApplyEffect( m_effect, effectIdentifier );
		
			// Render mesh part.
			renderer.Render(
				m_vertexBuffer,
				m_vertexOffset,
				m_indexBuffer,
				PrimitiveType.TriangleList,
				m_vertexCount,
				m_startIndex,
				m_primitiveCount );
		}
	}
}