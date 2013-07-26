using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XnaModelMesh = Microsoft.Xna.Framework.Graphics.ModelMesh;
using XnaModelMeshPart = Microsoft.Xna.Framework.Graphics.ModelMeshPart;
using TomatoModelMesh = Tomato.Graphics.Content.ModelMesh;
using TomatoModelMeshPart = Tomato.Graphics.Content.ModelMeshPart;

namespace Tomato.Graphics
{
	[System.Diagnostics.DebuggerDisplay( "Mesh: Name={Name} SubMeshes={m_subMeshes.Length}" )]
	public class Mesh : RenderableObject
	{
		private SubMesh[] m_subMeshes = null;

		/// <summary>
		/// Constructs a Mesh object from an XNA ModelMesh object.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="parent"></param>
		/// <param name="modelMesh"></param>
		public Mesh( string name, SceneObject parent, XnaModelMesh modelMesh )
			: base( SceneObjectType.Mesh, name, parent )
		{
			m_subMeshes = new SubMesh[ modelMesh.MeshParts.Count ];
			for( int i = 0 ; i < m_subMeshes.Length ; ++i )
			{
				m_subMeshes[ i ] = new SubMesh( modelMesh.MeshParts[ i ] );
			}

			m_localBounds = modelMesh.BoundingSphere;
		}

		/// <summary>
		/// Constructs a Mesh object from a Tomato ModelMseh object.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="parent"></param>
		/// <param name="modelMesh"></param>
		public Mesh( string name, SceneObject parent, TomatoModelMesh modelMesh )
			: base( SceneObjectType.Mesh, name, parent )
		{
			m_subMeshes = new SubMesh[ modelMesh.MeshParts.Count ];
			for( int i = 0 ; i < m_subMeshes.Length ; ++i )
			{
				m_subMeshes[ i ] = new SubMesh( modelMesh.MeshParts[ i ] );
			}

			m_localBounds = modelMesh.BoundingSphere;
		}

		protected override void OnRender( Renderer renderer, Camera camera, string effectIdentifier, TextureSamplerCollection textures )
		{
			foreach( SubMesh subMesh in m_subMeshes )
			{
				subMesh.Render( renderer, WorldTransformation, camera, effectIdentifier, textures );
			}
		}

		public IEnumerable<SubMesh> GetSubMeshes()
		{
			return m_subMeshes;
		}
	}
}