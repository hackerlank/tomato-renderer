using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Tomato.Graphics.Content
{
	public class ModelMeshPart
	{
		/// <summary>
		/// Gets the number of vertices.
		/// </summary>
		public int VertexCount { get; private set; }

		/// <summary>
		/// Gets vertex stride.
		/// </summary>
		public int VertexStride { get; private set; }

		/// <summary>
		/// Gets the number of primitives.
		/// </summary>
		public int PrimitiveCount { get; private set; }

		/// <summary>
		/// Gets the starting index.
		/// </summary>
		public int StartIndex { get; private set; }

		/// <summary>
		/// Gets a VertexBuffer object.
		/// </summary>
		public VertexBuffer VertexBuffer { get; private set; }

		/// <summary>
		/// Gets an IndexBuffer object.
		/// </summary>
		public IndexBuffer IndexBuffer { get; private set; }

		/// <summary>
		/// Gets an offset of VertexBuffer.
		/// </summary>
		public int VertexOffset { get; private set; }

		/// <summary>
		/// Gets an Effect object.
		/// </summary>
		public Effect Effect { get; private set; }

		/// <summary>
		/// Gets an ObjectMaterialState object.
		/// </summary>
		public ObjectMaterialState ObjectMaterial { get; private set; }

		/// <summary>
		/// Gets the ambient texture.
		/// </summary>
		public Texture AmbientTexture { get; private set; }

		/// <summary>
		/// Gets the diffuse texture.
		/// </summary>
		public Texture DiffuseTexture { get; private set; }

		/// <summary>
		/// Gets the specular texture.
		/// </summary>
		public Texture SpecularTexture { get; private set; }

		/// <summary>
		/// Gets the alpha texture.
		/// </summary>
		public Texture AlphaTexture { get; private set; }

		/// <summary>
		/// Gets the bump texture.
		/// </summary>
		public Texture BumpTexture { get; private set; }

		private ModelMeshPart()
		{
			VertexCount = 0;
			VertexStride = 0;
			PrimitiveCount = 0;
			StartIndex = 0;
			VertexBuffer = null;
			VertexOffset = 0;
			IndexBuffer = null;
			Effect = null;
			AmbientTexture = null;
			DiffuseTexture = null;
			SpecularTexture = null;
			AlphaTexture = null;
			BumpTexture = null;
		}

		private void OnVertexBufferRead( VertexBuffer vertexBuffer )
		{
			VertexBuffer = vertexBuffer;
		}

		private void OnIndexBufferRead( IndexBuffer indexBuffer )
		{
			IndexBuffer = indexBuffer;
		}

		private void OnEffectRead( Effect effect )
		{
			Effect = effect;
		}

		/// <summary>
		/// Creates a ModelMeshPart object from ContentReader.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		public static ModelMeshPart ReadFrom( ContentReader reader )
		{
			GraphicsDevice graphicsDevice = ContentReaderHelper.GetGraphicsDeviceFromContentReader( reader );

			ModelMeshPart modelMeshPart = new ModelMeshPart();

			modelMeshPart.VertexOffset = reader.ReadInt32();
			modelMeshPart.VertexCount = reader.ReadInt32();
			modelMeshPart.StartIndex = reader.ReadInt32();
			modelMeshPart.PrimitiveCount = reader.ReadInt32();
			modelMeshPart.VertexStride = reader.ReadInt32();

			reader.ReadSharedResource<VertexBuffer>( modelMeshPart.OnVertexBufferRead );
			reader.ReadSharedResource<IndexBuffer>( modelMeshPart.OnIndexBufferRead );
			reader.ReadSharedResource<Effect>( modelMeshPart.OnEffectRead );

			Vector3 emissive = reader.ReadVector3();
			Vector3 diffuse = reader.ReadVector3();
			Vector3 specular = reader.ReadVector3();
			float shineness = reader.ReadSingle();
			modelMeshPart.ObjectMaterial = new ObjectMaterialState( ref emissive, ref diffuse, ref specular, shineness );

			modelMeshPart.AmbientTexture = reader.ReadExternalReference<Texture>();
			modelMeshPart.DiffuseTexture = reader.ReadExternalReference<Texture>();
			modelMeshPart.SpecularTexture = reader.ReadExternalReference<Texture>();
			modelMeshPart.AlphaTexture = reader.ReadExternalReference<Texture>();
			modelMeshPart.BumpTexture = reader.ReadExternalReference<Texture>();

			return modelMeshPart;
		}
	}
}