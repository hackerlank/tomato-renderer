using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics
{
	public abstract class RenderableObject : SceneObject
	{
		public RenderableObject( SceneObjectType objectType, string name, SceneObject parent )
			: base( objectType, name, parent )
		{
		}

		protected override void OnUpdate( UpdateContext updateContext, bool bForceUpdateTransformation )
		{
			// Update world-space transformation matrix.
			// If world-space transformation was chagned, world-space bounding volume should be also recomputed.
			bool bRecomputeWorldBounds = RecomputeWorldTransformationMatrix( bForceUpdateTransformation );

			// Update world-space bounding volume.
			if( bRecomputeWorldBounds )
			{
				m_localBounds.Transform( ref m_worldTransformation, out m_worldBounds );
			}
		}

		public void Render( Renderer renderer, Camera camera, string effectIdentifier )
		{
			Render( renderer, camera, effectIdentifier, null );
		}

		public void Render( Renderer renderer, Camera camera, string effectIdentifier, TextureSamplerCollection textures )
		{
			OnRender( renderer, camera, effectIdentifier, textures );
		}

		protected abstract void OnRender( Renderer renderer, Camera camera, string effectIdentifier, TextureSamplerCollection textures );
	}
}