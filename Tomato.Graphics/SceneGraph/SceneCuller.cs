using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics
{
	public class SceneCuller
	{
		private BoundingFrustum m_viewFrustum = null;

		public SceneCuller()
		{
		}

		public virtual IList<RenderableObject> GetVisibleMeshes( IEnumerable<SceneObject> sceneObjects, Camera camera )
		{
			// Get the view frustum from camera.
			UpdateViewFrustum( camera );

			// Collect visible meshes.
			List<RenderableObject> visibleMeshes = new List<RenderableObject>();
			foreach( SceneObject sceneObject in sceneObjects )
			{
				GetVisibleMeshes( sceneObject, camera, visibleMeshes );
			}

			return visibleMeshes;
		}

		public virtual IList<Light> GetVisibleLights( IEnumerable<SceneObject> sceneObjects, Camera camera )
		{
			// Get the view frustum from camera.
			UpdateViewFrustum( camera );

			// Collect visible lights.
			List<Light> visibleLights = new List<Light>();
			foreach( SceneObject sceneObject in sceneObjects )
			{
				GetVisibleLights( sceneObject, camera, visibleLights );
			}

			return visibleLights;
		}

		private void UpdateViewFrustum( Camera camera )
		{
			if( camera != null )
			{
				Matrix viewProjectionMatrix = camera.ViewMatrix * camera.ProjectionMatrix;
				m_viewFrustum = new BoundingFrustum( viewProjectionMatrix );
			}
			else
			{
				m_viewFrustum = null;
			}
		}

		private void GetVisibleMeshes( SceneObject sceneObject, Camera camera, IList<RenderableObject> meshList )
		{
			if( sceneObject.IsVisible )
			{
				switch( sceneObject.ObjectType )
				{
					case SceneObjectType.Node:
						{
							Node node = sceneObject as Node;
							foreach( SceneObject child in node.Children )
							{
								GetVisibleMeshes( child, camera, meshList );
							}
						}
						break;

					case SceneObjectType.Mesh:
						{
							Mesh mesh = sceneObject as Mesh;
							if( m_viewFrustum != null )
							{
								if( m_viewFrustum.Contains( mesh.WorldBounds ) != ContainmentType.Disjoint )
								{
									meshList.Add( mesh );
								}
							}
							else
							{
								// Just add it.
								meshList.Add( mesh );
							}
						}
						break;
				}
			}
		}

		private void GetVisibleLights( SceneObject sceneObject, Camera camera, IList<Light> lightList )
		{
			if( sceneObject.IsVisible )
			{
				switch( sceneObject.ObjectType )
				{
					case SceneObjectType.Node:
						{
							Node node = sceneObject as Node;
							System.Diagnostics.Debug.Assert( node != null );

							foreach( SceneObject child in node.Children )
							{
								GetVisibleLights( child, camera, lightList );
							}
						}
						break;

					case SceneObjectType.Light:
						{
							Light light = sceneObject as Light;
							System.Diagnostics.Debug.Assert( light != null );

							if( m_viewFrustum != null )
							{
								if( light.IsVisibleFrom( m_viewFrustum ) )
								{
									lightList.Add( light );
								}
							}
							else
							{
								// Just add it.
								lightList.Add( light );
							}
						}
						break;
				}
			}
		}
	}
}