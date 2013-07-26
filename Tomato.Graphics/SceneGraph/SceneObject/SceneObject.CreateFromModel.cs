using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaModel = Microsoft.Xna.Framework.Graphics.Model;
using XnaModelBone = Microsoft.Xna.Framework.Graphics.ModelBone;
using XnaModelMesh = Microsoft.Xna.Framework.Graphics.ModelMesh;
using XnaModelMeshPart = Microsoft.Xna.Framework.Graphics.ModelMeshPart;
using TomatoModel = Tomato.Graphics.Content.Model;
using TomatoModelMesh = Tomato.Graphics.Content.ModelMesh;
using TomatoModelMeshPart = Tomato.Graphics.Content.ModelMeshPart;

namespace Tomato.Graphics
{
	partial class SceneObject
	{
		/// <summary>
		/// Creates a SceneObject from a Tomato Model object.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static SceneObject CreateFromModel( TomatoModel model )
		{
			return ConvertFromTomatoModelBone( model, null );
		}

		public static SceneObject CreateFromModel( TomatoModel model, SceneObject parent )
		{
			return ConvertFromTomatoModelBone( model, parent );
		}

		/// <summary>
		/// Creates a SceneObject from an XNA Model object.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static SceneObject CreateFromModel( XnaModel model )
		{
			return ConvertFromXnaModelBone( model, model.Root, null );
		}

		public static SceneObject CreateFromModel( XnaModel model, SceneObject parent )
		{
			return ConvertFromXnaModelBone( model, model.Root, parent );
		}

		private static SceneObject ConvertFromTomatoModelBone( TomatoModel model, SceneObject parentObject )
		{
			if( model.Mesh != null )
			{
				// This model is a Mesh object.
				Mesh mesh = new Mesh( model.Mesh.Name, parentObject, model.Mesh );
				mesh.LocalTransformation = new Transformation( model.LocalTransformation );
				return mesh;
			}
			else
			{
				// This model is a Node object.
				Node node = new Node( model.Name, parentObject );

				// Inherit local transformation.
				node.LocalTransformation = new Transformation( model.LocalTransformation );

				// Add child models.
				foreach( TomatoModel child in model.Children )
				{
					node.AddChild( ConvertFromTomatoModelBone( child, node ) );
				}

				return node;
			}
		}

		private static SceneObject ConvertFromXnaModelBone( XnaModel model, XnaModelBone modelBone, SceneObject parentObject )
		{
			// Collect meshes that belong to the model bone.
			var modelMeshes = from mesh in model.Meshes
							  where mesh.ParentBone.Index == modelBone.Index
							  select mesh;

			// Check if the bone has no children.
			if( ( modelBone.Children.Count == 0 )
				&& ( modelMeshes.Count() == 0 ) )
			{
#if false
				throw new InvalidOperationException( "ModelBone has no children." );
#else
				return null;
#endif
			}

			if( ( modelMeshes.Count() == 1 ) && ( modelBone.Children.Count == 0 ) )
			{
				// A single mesh only.
				// Convert it to Mesh object directly.
				Mesh mesh = new Mesh( modelBone.Name, parentObject, modelMeshes.First() );
				mesh.LocalTransformation = new Transformation( modelBone.Transform );
				return mesh;
			}
			else
			{
				// Create a node containing child bones and meshes "at the same level".
				Node node = new Node( modelBone.Name, parentObject );
				node.LocalTransformation = new Transformation( modelBone.Transform );

				// Add bone children.
				foreach( XnaModelBone child in modelBone.Children )
				{
					SceneObject childObject = ConvertFromXnaModelBone( model, child, node );
					if( childObject != null )
					{
						node.AddChild( childObject );
					}
				}

				// Add meshes that belong to the bone.
				foreach( XnaModelMesh modelMesh in modelMeshes )
				{
					node.AddChild( new Mesh( modelBone.Name, node, modelMesh ) );
				}

				return node;
			}
		}
	}
}
