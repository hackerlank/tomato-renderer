using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
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
		public static SceneObject CreateFromSceneDescription( ContentManager contentManager, SceneDescription sceneDescription )
		{
			if( sceneDescription == null )
			{
				throw new ArgumentNullException( "sceneDescription" );
			}

			// Create a root node.
			Node rootNode = new Node( sceneDescription.Name, null );
			rootNode.LocalTransformation = Transformation.Identity;

			// Process each entries.
			foreach( var entry in sceneDescription.GetEntries() )
			{
				switch( entry.Type )
				{
					case SceneDescriptionEntry.EntryType.Camera:
						{
							SceneDescriptionCameraEntry cameraEntry = entry as SceneDescriptionCameraEntry;
							if( cameraEntry != null )
							{
								// Create a camera instance.
								Camera camera = new Camera();
								camera.Name = entry.EntryName;

								// Set view and projection parameters.
								camera.SetViewParameters( cameraEntry.EyePosition, cameraEntry.LookAtPosition, cameraEntry.UpVector );
								camera.SetProjectionParameters( cameraEntry.FieldOfView, cameraEntry.AspectRatio, cameraEntry.NearClipPlane, cameraEntry.FarClipPlane );

								// Add to the root node.
								rootNode.AddChild( camera );
							}
							else
							{
								throw new InvalidCastException( "Failed casting from SceneDescriptionEntry to SceneDescriptionCameraEntry." );
							}
						}
						break;

					case SceneDescriptionEntry.EntryType.Light:
						{
							SceneDescriptionLightEntry lightEntry = entry as SceneDescriptionLightEntry;
							if( lightEntry != null )
							{
								// Create a light instance.
								Light light = new Light( lightEntry.AttenuationType );
								light.Name = entry.EntryName;

								// Set light parameters.
								light.Position = lightEntry.Position;
								light.Direction = lightEntry.Direction;
								light.DiffuseColor = lightEntry.DiffuseColor;
								light.SpecularPower = lightEntry.SpecularPower;
								light.AttenuationDistance = lightEntry.AttenuationDistance;
								light.AttenuationDistanceExponent = lightEntry.AttenuationDistanceExponent;
								light.AttenuationInnerAngle = lightEntry.AttenuationInnerAngle;
								light.AttenuationOuterAngle = lightEntry.AttenuationOuterAngle;
								light.AttenuationAngleExponent = lightEntry.AttenuationAngleExponent;

								// Add to the root node.
								rootNode.AddChild( light );
							}
							else
							{
								throw new InvalidCastException( "Failed casting from SceneDescriptionEntry to SceneDescriptionLightEntry." );
							}
						}
						break;

					case SceneDescriptionEntry.EntryType.Model:
						{
							SceneDescriptionModelEntry modelEntry = entry as SceneDescriptionModelEntry;
							if( modelEntry != null )
							{
								// Load a Tomato model.
								TomatoModel model = contentManager.Load<TomatoModel>( modelEntry.AssetName );

								// Convert to a SceneObject instance.
								SceneObject modelObject = SceneObject.CreateFromModel( model, rootNode );
								modelObject.Name = entry.EntryName;
								modelObject.LocalTransformation = modelEntry.Transformation;

								// Add to the root node.
								rootNode.AddChild( modelObject );
							}
							else
							{
								throw new InvalidCastException( "Failed casting from SceneDescriptionEntry to SceneDescriptionModelEntry." );
							}
						}
						break;

					case SceneDescriptionEntry.EntryType.XnaModel:
						{
							SceneDescriptionXnaModelEntry modelEntry = entry as SceneDescriptionXnaModelEntry;
							if( modelEntry != null )
							{
								// Load an XNA model.
								XnaModel model = contentManager.Load<XnaModel>( modelEntry.AssetName );

								// Convert to a SceneObject instance.
								SceneObject modelObject = SceneObject.CreateFromModel( model, rootNode );
								modelObject.Name = entry.EntryName;
								modelObject.LocalTransformation = modelEntry.Transformation;

								// Add to the root node.
								rootNode.AddChild( modelObject );
							}
							else
							{
								throw new InvalidCastException( "Failed casting from SceneDescriptionEntry to SceneDescriptionXnaModelEntry." );
							}
						}
						break;
				}
			}

			return rootNode;
		}
	}
}
