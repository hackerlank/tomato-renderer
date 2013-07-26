using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Tomato.Graphics.Content
{
	/// <summary>
	/// Helper class for ContentReader classes.
	/// </summary>
	internal static class ContentReaderHelper
	{
		/// <summary>
		/// Get GraphicsDevice object from ContentReader object.
		/// </summary>
		/// <param name="contentReader"></param>
		/// <returns></returns>
		public static GraphicsDevice GetGraphicsDeviceFromContentReader( ContentReader contentReader )
		{
			IGraphicsDeviceService service = ( IGraphicsDeviceService )contentReader.ContentManager.ServiceProvider.GetService( typeof( IGraphicsDeviceService ) );
			if( service == null )
			{
				throw new InvalidOperationException( "Failed to get GraphicsDevice from ContentReader object." );
			}

			GraphicsDevice graphicsDevice = service.GraphicsDevice;
			if( graphicsDevice == null )
			{
				throw new InvalidOperationException( "Failed to get GraphicsDevice from ContentReader object." );
			}

			return graphicsDevice;
		}
	}
}
