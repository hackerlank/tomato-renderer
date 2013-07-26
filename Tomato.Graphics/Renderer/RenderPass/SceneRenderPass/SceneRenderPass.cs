using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics
{
	[DebuggerDisplay( "SceneRenderPass: Name={Name}" )]
	public class SceneRenderPass : RenderPass
	{
		private List<SceneObject> m_sceneObjects = null;

		protected SceneCuller SceneCuller { get; set; }

		/// <summary>
		/// Gets or set the camera that will be used to render the scenes.
		/// </summary>
		[Browsable( false )]
		public Camera Camera { get; set; }

		/// <summary>
		/// Gets the effect identifier.
		/// </summary>
		[Browsable( false )]
		public string EffectIdentifier { get; private set; }

		/// <summary>
		/// Called after visible objects are determined by SceneCuller object.
		/// </summary>
		[Browsable( false )]		
		public event Action<IList<RenderableObject>> VisibleObjectsProcessing = null;

		/// <summary>
		/// Called after visible lights are determined by SceneCuller object.
		/// </summary>
		[Browsable( false )]		
		public event Action<IList<Light>> VisibleLightsProcessing = null;

		public SceneRenderPass( Renderer renderer, string name )
			: this( renderer, name, null )
		{
		}

		public SceneRenderPass( Renderer renderer, string name, string effectIdentifier )
			: base( renderer, name )
		{
			m_sceneObjects = new List<SceneObject>();

			EffectIdentifier = effectIdentifier;

			Camera = null;

			SceneCuller = new SceneCuller();			
		}

		public void ClearSceneObjects()
		{
			m_sceneObjects.Clear();
		}

		public void AddSceneObject( SceneObject sceneObject )
		{
			if( !m_sceneObjects.Contains( sceneObject ) )
			{
				m_sceneObjects.Add( sceneObject );
			}
		}

		public void AddSceneObjects( IEnumerable<SceneObject> sceneObjects )
		{
			foreach( var sceneObject in sceneObjects )
			{
				AddSceneObject( sceneObject );
			}
		}

		public void RemoveSceneObject( SceneObject sceneObject )
		{
			m_sceneObjects.Remove( sceneObject );
		}
		
		protected override void OnRender( TimeSpan elapsedTime )
		{
			// Get visible objects.
			var visibleObjects = SceneCuller.GetVisibleMeshes( m_sceneObjects, Camera );

			// Get visible lights.
			var visibleLights = SceneCuller.GetVisibleLights( m_sceneObjects, Camera );

			// Give chance to process visible mesh/light list here.
			HandleVisibleObjects( visibleObjects );
			HandleVisibleLights( visibleLights );

			// Render each mesh.
			foreach( RenderableObject mesh in visibleObjects )
			{
				mesh.Render( Renderer, Camera, EffectIdentifier, Textures );
			}
		}

		/// <summary>
		/// Called after visible meshes are computed at the beginning of each rendering frame.
		/// Derived classes can process visible mesh list by overriding this function.
		/// You can add, remove, or sort list.
		/// </summary>
		/// <param name="visibleObjects"></param>
		protected virtual void HandleVisibleObjects( IList<RenderableObject> visibleObjects )
		{
			// Invoke VisibleObjectsProcessing event.
			if( VisibleObjectsProcessing != null )
			{
				VisibleObjectsProcessing( visibleObjects );
			}
		}

		/// <summary>
		/// Called after visible lights are computed at the beginning of each rendering frame.
		/// Derived classes can process visible light list by overriding this function.
		/// You can add, remove, or sort list.
		/// </summary>
		/// <param name="visibleLights"></param>
		protected virtual void HandleVisibleLights( IList<Light> visibleLights )
		{
			// Invoke VisibleLightsProcessing event.
			if( VisibleLightsProcessing != null )
			{
				VisibleLightsProcessing( visibleLights );
			}
		}
	}
}
