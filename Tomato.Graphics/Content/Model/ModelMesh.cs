using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Tomato.Graphics.Content
{
	public class ModelMesh
	{
		/// <summary>
		/// Gets ModelMesh's name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets a bounding sphere.
		/// </summary>
		public BoundingSphere BoundingSphere { get; private set; }

		/// <summary>
		/// Gets a collection of MeshPart(s).
		/// </summary>
		public List<ModelMeshPart> MeshParts { get; private set; }

		private ModelMesh( string name, BoundingSphere boundingSphere )
		{
			Name = name;

			BoundingSphere = boundingSphere;

			MeshParts = new List<ModelMeshPart>();
		}

		/// <summary>
		/// Creates a ModelMesh object from ContentReader.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		public static ModelMesh ReadFrom( ContentReader reader )
		{
			// Name
			string name = reader.ReadString();

			// Bounding sphere
			Vector3 center = reader.ReadVector3();
			float radius = reader.ReadSingle();
			BoundingSphere boundingSphere = new BoundingSphere( center, radius );

			// Create a ModelMesh object.
			ModelMesh modelMesh = new ModelMesh( name, boundingSphere );

			// Read ModelMeshPart collection.
			int meshPartCount = reader.ReadInt32();
			for( int i = 0 ; i < meshPartCount ; ++i )
			{
				modelMesh.MeshParts.Add( ModelMeshPart.ReadFrom( reader ) );
			}

			return modelMesh;
		}
	}
}