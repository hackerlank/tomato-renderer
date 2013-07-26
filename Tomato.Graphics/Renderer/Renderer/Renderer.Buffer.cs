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
		public VertexBuffer CreateVertexBuffer( string name, VertexDeclaration vertexDeclaration, int vertexCount, BufferUsage usage )
		{
			VertexBuffer vertexBuffer = new VertexBuffer( m_graphicsDevice, vertexDeclaration, vertexCount, usage );

			vertexBuffer.Name = name;

			return vertexBuffer;
		}

		public VertexBuffer CreateVertexBuffer<T>( string name, VertexDeclaration vertexDeclaration, int vertexCount, BufferUsage usage, T[] data ) where T : struct
		{
			VertexBuffer vertexBuffer = new VertexBuffer( m_graphicsDevice, vertexDeclaration, vertexCount, usage );

			vertexBuffer.Name = name;

			vertexBuffer.SetData<T>( data );

			return vertexBuffer;
		}

		public IndexBuffer CreateIndexBuffer( string name, IndexElementSize indexElementSize, int indexCount, BufferUsage usage )
		{
			IndexBuffer indexBuffer = new IndexBuffer( m_graphicsDevice, indexElementSize, indexCount, usage );

			indexBuffer.Name = name;

			return indexBuffer;
		}

		public IndexBuffer CreateIndexBuffer<T>( string name, IndexElementSize indexElementSize, int indexCount, BufferUsage usage, T[] data ) where T : struct
		{
			IndexBuffer indexBuffer = new IndexBuffer( m_graphicsDevice, indexElementSize, indexCount, usage );

			indexBuffer.Name = name;

			indexBuffer.SetData<T>( data );

			return indexBuffer;
		}

		public void ResetBuffers()
		{
			m_graphicsDevice.Indices = null;
			m_graphicsDevice.SetVertexBuffers( null );
		}
	}
}