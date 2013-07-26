using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace Tomato.Graphics.Content.Pipeline
{
	public sealed class ModelContent
	{
		public string Name { get; set; }

		public ModelContent Parent { get; private set; }

		public Matrix LocalTransformation { get; set; }

		public List<ModelContent> Children { get; private set; }

		public ModelMeshContent Mesh { get; private set; }

		public ModelContent( ModelContent parent, string name, Matrix localTransformation, ModelMeshContent mesh )
		{
			Parent = parent;

			Children = new List<ModelContent>();

			LocalTransformation = localTransformation;

			Mesh = mesh;
		}

		public void WriteTo( ContentWriter writer )
		{
			// Write name.
			writer.Write( string.IsNullOrWhiteSpace( Name ) ? "" : Name );

			// Write transformation matrices.
			writer.Write( LocalTransformation );

			// Write mesh.
			if( Mesh != null )
			{
				writer.Write( true );
				Mesh.WriteTo( writer );
			}
			else
			{
				writer.Write( false );
			}

			// Write the number of children.
			writer.Write( Children.Count );

			// Write children recursively.
			foreach( ModelContent child in Children )
			{
				child.WriteTo( writer );
			}
		}
	}
}
