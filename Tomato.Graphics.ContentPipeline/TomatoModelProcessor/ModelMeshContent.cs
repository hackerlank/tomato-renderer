using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using XnaModelMeshContent = Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelMeshContent;

namespace Tomato.Graphics.Content.Pipeline
{
	public sealed class ModelMeshContent
	{
		public string Name { get; set; }

		public BoundingSphere BoundingSphere { get; set; }

		public List<ModelMeshPartContent> MeshParts { get; private set; }

		public ModelMeshContent( XnaModelMeshContent xnaModelMesh, ContentProcessorContext context )
		{
			Name = xnaModelMesh.Name;

			BoundingSphere = xnaModelMesh.BoundingSphere;
			
			MeshParts = new List<ModelMeshPartContent>( xnaModelMesh.MeshParts.Count );
			for( int i = 0 ; i < xnaModelMesh.MeshParts.Count ; ++i )
			{
				MeshParts.Add( new ModelMeshPartContent( xnaModelMesh.MeshParts[ i ], context ) );
			}
		}

		public void WriteTo( ContentWriter writer )
		{
			writer.Write( string.IsNullOrWhiteSpace( Name ) ? "" : Name );
			writer.Write( BoundingSphere.Center );
			writer.Write( BoundingSphere.Radius );

			writer.Write( MeshParts.Count );
			foreach( ModelMeshPartContent meshPart in MeshParts )
			{
				meshPart.WriteTo( writer );
			}
		}
	}
}
