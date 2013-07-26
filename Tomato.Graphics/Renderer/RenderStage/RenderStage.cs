using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics
{
	public class RenderStage
	{
		private Renderer m_renderer = null;

		private LinkedList<RenderPass> m_renderPasses = null;

		/// <summary>
		/// Raised before rendering begins.
		/// </summary>
		public event Action<RenderStage> RenderStarting = null;

		/// <summary>
		/// Raised after rendering finished.
		/// </summary>
		public event Action<RenderStage> RenderFinished = null;

		/// <summary>
		/// Gets of sets whether this RenderStage is enabled.
		/// </summary>
		[Category( "Render Stage" )]
		[DisplayName( "Enabled" )]
		public bool IsEnabled { get; set; }

		/// <summary>
		/// Gets RenderStage's name.
		/// </summary>
		[Category( "Render Stage" )]
		[DisplayName( "Name" )]
		public string Name { get; private set; }

		[Browsable( false )]
		public Renderer Renderer { get { return m_renderer; } }

		public RenderStage( Renderer renderer )
			: this( renderer, null )
		{
		}

		public RenderStage( Renderer renderer, string name )
		{
			m_renderer = renderer;
			m_renderPasses = new LinkedList<RenderPass>();
			IsEnabled = true;
			Name = name;
		}

		public void AddLastRenderPass( RenderPass renderPass )
		{
			m_renderPasses.AddLast( renderPass );
		}

		public void AddFirstRenderPass( RenderPass renderPass )
		{
			m_renderPasses.AddFirst( renderPass );
		}

		public void RemoveRenderPass( RenderPass renderPass )
		{
			m_renderPasses.Remove( renderPass );
		}

		public void ClearRenderPasses()
		{
			m_renderPasses.Clear();
		}

		public IEnumerable<RenderPass> GetRenderPasses()
		{
			return m_renderPasses;
		}

		internal void Render( TimeSpan elapsedTime )
		{
			if( IsEnabled )
			{
				// Raise Executing event
				if( RenderStarting != null )
				{
					RenderStarting( this );
				}

				// Execute all RenderPass(es)
				var it = m_renderPasses.GetEnumerator();
				while( it.MoveNext() )
				{
					it.Current.Render( elapsedTime );
				}

				// Raise Executed event
				if( RenderFinished != null )
				{
					RenderFinished( this );
				}
			}
		}
	}
}