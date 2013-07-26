using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics
{
	[System.Diagnostics.DebuggerDisplay( "Node: Name={Name} Children={m_childObjects.Count}" )]
	public class Node : SceneObject
	{
		private List<SceneObject> m_childObjects = null;

		/// <summary>
		/// Gets the list of child objects.
		/// </summary>
		[Browsable( false )]
		public IEnumerable<SceneObject> Children
		{
			get { return m_childObjects; }
		}

		/// <summary>
		/// Constructs a node.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="parent"></param>
		public Node( string name, SceneObject parent )
			: base( SceneObjectType.Node, name, parent )
		{
			m_childObjects = new List<SceneObject>();
		}

		/// <summary>
		/// Adds a child object.
		/// This function does not check for duplicate objects.
		/// </summary>
		/// <param name="childObject"></param>
		public virtual void AddChild( SceneObject childObject )
		{
			m_childObjects.Add( childObject );
		}

		/// <summary>
		/// Removes a child object.
		/// If the node does not have childObject as children, nothing happens.
		/// </summary>
		/// <param name="childObject"></param>
		public virtual void RemoveChild( SceneObject childObject )
		{
			m_childObjects.Remove( childObject );
		}

		/// <summary>
		/// Gets the child object at a specific index.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public virtual SceneObject GetChildAt( int index )
		{
			return m_childObjects[ index ];
		}

		/// <summary>
		/// Gets the number of child objects.
		/// </summary>
		/// <returns></returns>
		public virtual int GetChildCount()
		{
			return m_childObjects.Count;
		}

		protected override void OnUpdate( UpdateContext updateContext, bool bForceUpdateTransformation )
		{
			// Update world-space transformation matrix.
			// If world-space transformation was chagned, world-space bounding volume should be also recomputed.
			bool bRecomputeWorldBounds = RecomputeWorldTransformationMatrix( bForceUpdateTransformation );

			// Update child objects.
			foreach( SceneObject childObject in m_childObjects )
			{
				// If world-space transformation is chagned, children must be updated accordingly.
				childObject.Update(
					updateContext,
					bForceUpdateTransformation || IsLocalTransformationChagned );
			}

			// Update world-space bounding volume.
			if( bRecomputeWorldBounds )
			{
				// Compute world-space bounding volume of the current object.
				BoundingSphere bounds;
				m_localBounds.Transform( ref m_worldTransformation, out bounds );

				// Merge child objects' world-space bounding volume.
				foreach( SceneObject child in m_childObjects )
				{
					bounds = BoundingSphere.CreateMerged( bounds, child.WorldBounds );
				}

				m_worldBounds = bounds;
			}
		}
	}
}