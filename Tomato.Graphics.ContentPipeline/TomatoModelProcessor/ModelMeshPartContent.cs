using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using XnaModelMeshPartContent = Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelMeshPartContent;

namespace Tomato.Graphics.Content.Pipeline
{
	public sealed class ModelMeshPartContent
	{
		public int VertexCount { get; set; }
		public int PrimitiveCount { get; set; }
		public int StartIndex { get; set; }
		public int VertexStride { get; set; }
		public int VertexOffset { get; set; }
		
		public VertexBufferContent VertexBuffer { get; set; }
		public IndexCollection IndexBuffer { get; set; }
		
		public CompiledEffectContent Effect { get; set; }
		public Vector3 MaterialEmissive;
		public Vector3 MaterialDiffuse;
		public Vector3 MaterialSpecular;
		public float MaterialShineness;

		public ExternalReference<TextureContent> AmbientTexture { get; set; }
		public ExternalReference<TextureContent> DiffuseTexture { get; set; }
		public ExternalReference<TextureContent> SpecularTexture { get; set; }
		public ExternalReference<TextureContent> AlphaTexture { get; set; }
		public ExternalReference<TextureContent> BumpTexture { get; set; }

		public ModelMeshPartContent( XnaModelMeshPartContent xnaModelMeshPart, ContentProcessorContext context )
		{
			VertexCount = xnaModelMeshPart.NumVertices;
			PrimitiveCount = xnaModelMeshPart.PrimitiveCount;
			StartIndex = xnaModelMeshPart.StartIndex;
			VertexOffset = xnaModelMeshPart.VertexOffset;
			VertexStride = xnaModelMeshPart.VertexBuffer.VertexDeclaration.VertexStride.Value;

			VertexBuffer = xnaModelMeshPart.VertexBuffer;
			IndexBuffer = xnaModelMeshPart.IndexBuffer;
			Effect = EffectBuilder.CreateEffect( xnaModelMeshPart, context );

			TomatoMaterialContent objMaterial = xnaModelMeshPart.Material as TomatoMaterialContent;
			if( objMaterial != null )
			{
				MaterialEmissive = objMaterial.EmissiveColor;
				MaterialDiffuse = objMaterial.DiffuseColor;
				MaterialSpecular = objMaterial.SpecularColor;
				MaterialShineness = objMaterial.SpecularPower;
				AmbientTexture = objMaterial.AmbientTexture;
				DiffuseTexture = objMaterial.DiffuseTexture;
				SpecularTexture = objMaterial.SpecularTexture;
				AlphaTexture = objMaterial.AlphaTexture;
				BumpTexture = objMaterial.BumpTexture;
			}
			else
			{
				throw new System.InvalidOperationException( "Not supported type of MaterialContent." );
			}
		}

		public void WriteTo( ContentWriter writer )
		{
			writer.Write( VertexOffset );
			writer.Write( VertexCount );
			writer.Write( StartIndex );
			writer.Write( PrimitiveCount );
			writer.Write( VertexStride );

			writer.WriteSharedResource<VertexBufferContent>( VertexBuffer );
			writer.WriteSharedResource<IndexCollection>( IndexBuffer );
			writer.WriteSharedResource<CompiledEffectContent>( Effect );

			writer.Write( MaterialEmissive );
			writer.Write( MaterialDiffuse );
			writer.Write( MaterialSpecular );
			writer.Write( MaterialShineness );

			writer.WriteExternalReference( AmbientTexture );
			writer.WriteExternalReference( DiffuseTexture );
			writer.WriteExternalReference( SpecularTexture );
			writer.WriteExternalReference( AlphaTexture );
			writer.WriteExternalReference( BumpTexture );
		}
	}
}
