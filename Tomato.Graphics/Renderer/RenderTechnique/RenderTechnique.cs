using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics
{
	public class RenderTechnique
	{
		private Renderer m_renderer = null;

		private LinkedList<RenderStage> m_renderStages = null;

		[Category( "Render Technique" )]
		[DisplayName( "Name" )]
		public string Name { get; private set; }

		[Browsable( false )]
		public Renderer Renderer { get { return m_renderer; } }

		public RenderTechnique( Renderer renderer )
			: this( renderer, null )
		{
		}

		public RenderTechnique( Renderer renderer, string name )
		{
			m_renderer = renderer;

			m_renderStages = new LinkedList<RenderStage>();

			Name = name;
		}

		public void AddLastRenderStage( RenderStage renderStage )
		{
			m_renderStages.AddLast( renderStage );
		}

		public void AddFirstRenderStage( RenderStage renderStage )
		{
			m_renderStages.AddFirst( renderStage );
		}

		public void RemoveRenderStage( RenderStage renderStage )
		{
			m_renderStages.Remove( renderStage );
		}

		public void ClearRenderPasses()
		{
			m_renderStages.Clear();
		}

		public virtual void Render( TimeSpan elapsedTime )
		{
			// Execute all RenderPass(es)
			var it = m_renderStages.GetEnumerator();
			while( it.MoveNext() )
			{
				it.Current.Render( elapsedTime );
			}
		}

		public IEnumerable<RenderStage> GetRenderStages()
		{
			return m_renderStages;
		}
	}
}