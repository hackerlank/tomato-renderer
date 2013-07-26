using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics
{
	partial class Renderer
	{
		public void Render(
			VertexBuffer vertexBuffer,
			int vertexOffset,
			IndexBuffer indexBuffer,
			PrimitiveType primitiveType,
			int vertexCount,
			int startIndex,
			int primitiveCount )
		{
			// Set vertex buffer.
			m_graphicsDevice.SetVertexBuffer( vertexBuffer, vertexOffset );

			// Set index buffer.
			m_graphicsDevice.Indices = indexBuffer;

			// Call Draw()
			m_graphicsDevice.DrawIndexedPrimitives( primitiveType, 0, 0, vertexCount, startIndex, primitiveCount );

			// Update statistics.
			UpdateRenderingStatistics( primitiveType, vertexCount, startIndex, primitiveCount );
		}

		private void UpdateRenderingStatistics(
			PrimitiveType primitiveType,
			int vertexCount,
			int startIndex,
			int primitiveCount )
		{
			if( ( primitiveType == PrimitiveType.TriangleList )
				|| ( primitiveType == PrimitiveType.TriangleStrip ) )
			{
				m_trianglesPerFrame += primitiveCount;
			}
		}
	}
}