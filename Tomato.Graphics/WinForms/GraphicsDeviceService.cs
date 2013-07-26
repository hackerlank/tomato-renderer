using System;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics.WinForms
{
	/// <summary>
	/// Helper class responsible for creating and managing the GraphicsDevice.
	/// All GraphicsDeviceControl instances share the same GraphicsDeviceService,
	/// so even though there can be many controls, there will only ever be a single underlying GraphicsDevice. 
	/// This implements the standard IGraphicsDeviceService interface, 
	/// which provides notification events for when the device is reset or disposed.
	/// </summary>
	public class GraphicsDeviceService : IGraphicsDeviceService
	{
		// DeviceCreated event never raised because we create the device inside the constructor.
#pragma warning disable 67

		// Singleton device service instance.
		private static GraphicsDeviceService s_singletonInstance;
		private static int s_referenceCount;

		// GraphicsDevice object.
		private GraphicsDevice m_graphicsDevice;
		private PresentationParameters m_presentationParameters;

		// IGraphicsDeviceService events.
		public event EventHandler<EventArgs> DeviceCreated = null;
		public event EventHandler<EventArgs> DeviceDisposing = null;
		public event EventHandler<EventArgs> DeviceReset = null;
		public event EventHandler<EventArgs> DeviceResetting = null;

		/// <summary>
		/// Gets the current graphics device.
		/// </summary>
		public GraphicsDevice GraphicsDevice
		{
			get { return m_graphicsDevice; }
		}

		/// <summary>
		/// Constructor is private, because this is a singleton class.
		/// Client controls should use the public AddRef method instead.
		/// </summary>
		private GraphicsDeviceService( IntPtr windowHandle, int width, int height, GraphicsProfile graphicsProfile )
		{
			// Create a PresentationParameter.
			m_presentationParameters = new PresentationParameters();
			m_presentationParameters.BackBufferWidth = Math.Max( width, 1 );
			m_presentationParameters.BackBufferHeight = Math.Max( height, 1 );
			m_presentationParameters.BackBufferFormat = SurfaceFormat.Color;
			m_presentationParameters.DepthStencilFormat = DepthFormat.Depth24;
			m_presentationParameters.DeviceWindowHandle = windowHandle;
			m_presentationParameters.PresentationInterval = PresentInterval.Immediate;
			m_presentationParameters.IsFullScreen = false;

			// Create a GraphicsDevice.
			m_graphicsDevice = new GraphicsDevice( GraphicsAdapter.DefaultAdapter, graphicsProfile, m_presentationParameters );
		}

		/// <summary>
		/// Gets a reference to the singleton instance.
		/// </summary>
		public static GraphicsDeviceService AddRef( IntPtr windowHandle, int width, int height )
		{
			// Increment the reference count.
			if( Interlocked.Increment( ref s_referenceCount ) == 1 )
			{
				// This is the first control to start using the device.
				// Create the singleton instance.
				s_singletonInstance = new GraphicsDeviceService( windowHandle, width, height, GraphicsProfile.HiDef );
			}

			return s_singletonInstance;
		}

		/// <summary>
		/// Releases a reference to the singleton instance.
		/// </summary>
		public void Release( bool disposing )
		{
			// Decrement the reference count.
			if( Interlocked.Decrement( ref s_referenceCount ) == 0 )
			{
				// This is the last control to finish using the device.
				// Dispose the singleton instance.
				if( disposing )
				{
					// Invoke DeviceDisposing event.
					if( DeviceDisposing != null )
					{
						DeviceDisposing( this, EventArgs.Empty );
					}

					// Dispose the device.
					m_graphicsDevice.Dispose();
				}

				m_graphicsDevice = null;
			}
		}

		/// <summary>
		/// Resets the graphics device to whichever is bigger out of the specified resolution or its current size. 
		/// This behavior means the device will demand-grow to the largest of all its GraphicsDeviceControl clients.
		/// </summary>
		public void ResetDevice( int width, int height )
		{
			// Invoke DeviceResetting event.
			if( DeviceResetting != null )
			{
				DeviceResetting( this, EventArgs.Empty );
			}

			// Update the presentation parameters.
			m_presentationParameters.BackBufferWidth = Math.Max( m_presentationParameters.BackBufferWidth, width );
			m_presentationParameters.BackBufferHeight = Math.Max( m_presentationParameters.BackBufferHeight, height );

			// Reset the device.
			m_graphicsDevice.Reset( m_presentationParameters );

			// Invoke DeviceReset event.
			if( DeviceReset != null )
			{
				DeviceReset( this, EventArgs.Empty );
			}
		}
	}
}
