using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Tomato.Graphics.Content
{
	public sealed class Model
	{
		/// <summary>
		/// Gets Model's name.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets parent Model.
		/// If the Model has no parent, this property will return null.
		/// </summary>
		public Model Parent { get; private set; }

		/// <summary>
		/// Gets local transformation matrix.
		/// </summary>
		public Matrix LocalTransformation { get; private set; }

		/// <summary>
		/// Gets the list of child Models.
		/// </summary>
		public List<Model> Children { get; private set; }

		/// <summary>
		/// Gets ModelMesh object, if possible.
		/// </summary>
		public ModelMesh Mesh { get; private set; }

		private Model( Model parentModel, string name, ref Matrix localTransformation, ModelMesh mesh )
		{
			Parent = parentModel;

			Name = name;

			LocalTransformation = localTransformation;

			Children = new List<Model>();

			Mesh = mesh;
		}

		/// <summary>
		/// Creates a Model object from ContentReader.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="parentModel"></param>
		/// <returns></returns>
		public static Model ReadFrom( ContentReader reader, Model parentModel )
		{
			// Read name.
			string name = reader.ReadString();

			// Read transformation matrices.
			Matrix localTransformation = reader.ReadMatrix();

			// Read mesh.
			ModelMesh mesh = null;
			bool bMesh = reader.ReadBoolean();
			if( bMesh )
			{
				mesh = ModelMesh.ReadFrom( reader );
			}

			// Create Model object.
			Model model = new Model( parentModel, name, ref localTransformation, mesh );

			//  Read chilren and add them. 
			int childCount = reader.ReadInt32();
			for( int i = 0 ; i < childCount ; ++i )
			{
				model.Children.Add( ReadFrom( reader, model ) );
			}

			return model;
		}
	}
}
