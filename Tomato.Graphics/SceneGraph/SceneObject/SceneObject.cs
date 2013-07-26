using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics
{
	/// <summary>
	/// Objects in rendering scenes are represented as SceneObject (or its derived classes) instances.
	/// </summary>
	[System.Diagnostics.DebuggerDisplay( "SceneObject: Name={Name}" )]
	public abstract partial class SceneObject : IEquatable<SceneObject>
	{
		// Unique ID value
		private Guid m_id;

		// Local-space bounding volume
		protected BoundingSphere m_localBounds = new BoundingSphere();

		// Local-space transformation
		protected Transformation m_localTransformation = Transformation.Identity;

		// Flag indicating that whether local-space transformation has changed or not.
		private bool m_bLocalTransformationChanged = true;

		// World-space transformation
		protected Matrix m_worldTransformation = Matrix.Identity;

		// World-space bounding volume
		protected BoundingSphere m_worldBounds = new BoundingSphere();

		// Render-states
		protected RenderStateSet m_renderStates = new RenderStateSet();

		/// <summary>
		/// Indicates whether local-space transformation has changed or not.
		/// This flag is reset to false at the end of each Update() call.
		/// </summary>
		protected bool IsLocalTransformationChagned { get { return m_bLocalTransformationChanged; } }

		/// <summary>
		/// Gets object's unique ID as text.
		/// </summary>
		[Browsable( false )]
		public string ID { get { return m_id.ToString(); } }

		/// <summary>
		/// Gets object's name.
		/// </summary>
		[Category( "Scene Object" )]
		[DisplayName( "Name" )]
		public string Name { get; protected set; }

		[Category( "Scene Object" )]
		[DisplayName( "Visible" )]
		public bool IsVisible { get; set; }

		/// <summary>
		/// Gets object's objectType.
		/// </summary>
		[Category( "Scene Object" )]
		[DisplayName( "ObjectType" )]
		public SceneObjectType ObjectType { get; private set; }

		/// <summary>
		/// Gets reference to parent object.
		/// This property is null if the object has no parent.
		/// </summary>
		[Browsable( false )]
		public SceneObject Parent { get; protected set; }

		/// <summary>
		/// Gets object's render-states.
		/// </summary>
		[Browsable( false )]
		public RenderStateSet RenderStates { get { return m_renderStates; } }

		/// <summary>
		/// Gets or sets local-space transformation matrix.
		/// </summary>
		[Category( "Transformation" )]
		[DisplayName( "LocalTransformation" )]
		public Transformation LocalTransformation 
		{
			get { return m_localTransformation; }
			set
			{
				// Change value only when necessary to avoid redundant computation.
				if( !m_localTransformation.Equals( value ) )
				{
					m_bLocalTransformationChanged = true;
					m_localTransformation = value;
				}
			}
		}

		/// <summary>
		/// Gets world-space bounding sphere.
		/// </summary>
		[Category( "Transformation" )]
		[DisplayName( "WorldBounds" )]
		public BoundingSphere WorldBounds { get { return m_worldBounds; } }

		/// <summary>
		/// Gets world-space transformation matrix.
		/// </summary>
		[Category( "Transformation" )]
		[DisplayName( "WorldTransformation" )]
		public Matrix WorldTransformation { get { return m_worldTransformation; } }

		protected SceneObject( SceneObjectType objectType, string name, SceneObject parent )
		{
			// Object ID
			m_id = Guid.NewGuid();

			// Object objectType
			ObjectType = objectType;

			// Object name
			// This cannot be null.
			Name = string.IsNullOrEmpty( name ) ? "" : name;

			// Reference to parent object.
			Parent = parent;

			IsVisible = true;
		}

		public void Update( UpdateContext updateContext )
		{
			Update( updateContext, false );
		}

		public void Update( UpdateContext updateContext, bool bForceUpdateTransformation )
		{
			// Call UpdateContext.OnObjectUpdating() function.
			if( updateContext != null )
			{
				updateContext.OnObjectUpdating( this );
			}

			// Call virtual function OnUpdate().
			OnUpdate( updateContext, bForceUpdateTransformation );

			// Reset local transformation changed flag.
			m_bLocalTransformationChanged = false;

			// Call UpdateContext.OnObjectUpdated() function.
			if( updateContext != null )
			{
				updateContext.OnObjectUpdated( this );
			}
		}

		protected abstract void OnUpdate( UpdateContext updateContext, bool bForceUpdateTransformation );

		/// <summary>
		/// Equality comparison. Two objects are comapred using their ID value.
		/// </summary>
		/// <param name="other"></param>
		/// <returns>Returns true when two objects have the same ID value.</returns>
		public virtual bool Equals( SceneObject other )
		{
			if( other != null )
			{
				return m_id.CompareTo( other.m_id ) == 0;
			}

			return false;
		}

		public override bool Equals( object obj )
		{
			SceneObject sceneObject = obj as SceneObject;
			if( sceneObject != null )
			{
				return Equals( sceneObject );
			}

			return false;
		}

		public override int GetHashCode()
		{
			return m_id.GetHashCode();
		}

		/// <summary>
		/// Recomputes world-space transformation matrix.
		/// </summary>
		/// <param name="bForceRecompute"></param>
		/// <returns>Returns true if the world-space transformation was recomputed.</returns>
		protected bool RecomputeWorldTransformationMatrix( bool bForceRecompute )
		{
			if( IsLocalTransformationChagned
				|| bForceRecompute )
			{
				if( Parent == null )
				{
					m_worldTransformation = m_localTransformation.AsMatrix;
				}
				else
				{
					m_worldTransformation = m_localTransformation.AsMatrix * Parent.WorldTransformation;
				}

				return true;
			}

			return false;
		}
	}
}