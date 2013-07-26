using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Color = System.Drawing.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Tomato.Graphics.WinForms
{
	/// <summary>
	/// Base class for custom control that uses the XNA Framework GraphicsDevice to render onto itself.
	/// Derived classes can override the Initialize() and Draw() methods to add their own drawing code.
	/// </summary>
	public abstract class XnaControl : Control
	{
		// Shared graphics device service instance.
		private GraphicsDeviceService m_sharedGraphicsDeviceService;

		// Service container.
		private ServiceContainer m_services = new ServiceContainer();

		// Content manager.
		private ContentManager m_contentManager = null;

		/// <summary>
		/// Gets a GraphicsDevice that can be used to draw onto this control.
		/// </summary>
		public GraphicsDevice GraphicsDevice
		{
			get { return m_sharedGraphicsDeviceService.GraphicsDevice; }
		}

		/// <summary>
		/// Gets an IServiceProvider containing our IGraphicsDeviceService.
		/// This can be used with components such as the ContentManager,
		/// which use this service to look up the GraphicsDevice.
		/// </summary>
		public ServiceContainer Services
		{
			get { return m_services; }
		}

		/// <summary>
		/// Gets the content manager.
		/// </summary>
		public ContentManager Content
		{
			get { return m_contentManager; }
		}

		/// <summary>
		/// Initializes the control.
		/// </summary>
		protected override void OnCreateControl()
		{
			// Don't initialize the graphics device if we are running in the designer.
			if( !DesignMode )
			{
				m_sharedGraphicsDeviceService = GraphicsDeviceService.AddRef( Handle, ClientSize.Width, ClientSize.Height );

				// Register the service, so components like ContentManager can find it.
				m_services.AddService<IGraphicsDeviceService>( m_sharedGraphicsDeviceService );

				// Intialize the content manager.
				m_contentManager = new ContentManager( Services, "Content" );

				// Give derived classes a chance to initialize themselves.
				Initialize();

				// Hook the idle event to constantly redraw our animation.
				Application.Idle += delegate { Invalidate(); };
			}

			base.OnCreateControl();
		}

		/// <summary>
		/// Disposes the control.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( m_sharedGraphicsDeviceService != null )
			{
				m_sharedGraphicsDeviceService.Release( disposing );
				m_sharedGraphicsDeviceService = null;
			}

			base.Dispose( disposing );
		}

		/// <summary>
		/// Redraws the control in response to a WinForms paint message.
		/// </summary>
		protected override void OnPaint( PaintEventArgs e )
		{
			string beginDrawError = BeginDraw();
			if( string.IsNullOrEmpty( beginDrawError ) )
			{
				Draw();
				EndDraw();
			}
			else
			{
				// Paint using WinForms Paint.
				PaintUsingSystemDrawing( e.Graphics, beginDrawError );
			}
		}


		/// <summary>
		/// Attempts to begin drawing the control. 
		/// Returns an error message string if this was not possible, 
		/// which can happen if the graphics device is lost, or if we are running inside the Form designer.
		/// </summary>
		private string BeginDraw()
		{
			// If we have no graphics device, we must be running in the designer.
			if( m_sharedGraphicsDeviceService == null )
			{
				return Text + "\n\n" + GetType();
			}

			// Make sure the graphics device is big enough, and is not lost.
			string deviceResetError = HandleDeviceReset();
			if( !string.IsNullOrEmpty( deviceResetError ) )
			{
				return deviceResetError;
			}

			// Adjust the viewport to fit the current control size.
			// This is a required step because many controls can shared the device.
			Viewport viewport = new Viewport();
			viewport.X = 0;
			viewport.Y = 0;
			viewport.Width = ClientSize.Width;
			viewport.Height = ClientSize.Height;
			viewport.MinDepth = 0;
			viewport.MaxDepth = 1;
			GraphicsDevice.Viewport = viewport;

			return null;
		}


		/// <summary>
		/// Ends drawing the control. 
		/// This is called after derived classes have finished their Draw method, 
		/// and is responsible for presenting the finished image onto the screen, 
		/// using the appropriate WinForms control handle to make sure it shows up in the right place.
		/// </summary>
		private void EndDraw()
		{
			try
			{
				Rectangle sourceRectangle = new Rectangle( 0, 0, ClientSize.Width, ClientSize.Height );
				GraphicsDevice.Present( sourceRectangle, null, Handle );
			}
			catch
			{
				// Device lost will be handled at the next BeginDraw().
				// Here, we just ignore all exceptions.
			}
		}


		/// <summary>
		/// This checks the graphics device status, making sure it is big enough for drawing the current control, and that the device is not lost. 
		/// Returns an error string if the device could not be reset.
		/// </summary>
		private string HandleDeviceReset()
		{
			bool bDeviceNeedsReset = false;

			switch( GraphicsDevice.GraphicsDeviceStatus )
			{
				case GraphicsDeviceStatus.Lost:
					{
						// If the graphics device is lost, we cannot use it at all.
						return "Graphics device lost";
					}

				case GraphicsDeviceStatus.NotReset:
					{
						// If device is in the not-reset state, we should try to reset it.
						bDeviceNeedsReset = true;
					}
					break;

				default:
					{
						// If the device state is ok, check whether it is big enough.
						bDeviceNeedsReset = 
							( ClientSize.Width > GraphicsDevice.PresentationParameters.BackBufferWidth ) 
							|| ( ClientSize.Height > GraphicsDevice.PresentationParameters.BackBufferHeight );
					}
					break;
			}

			// Reset the device if necessary.
			if( bDeviceNeedsReset )
			{
				try
				{
					m_sharedGraphicsDeviceService.ResetDevice( ClientSize.Width, ClientSize.Height );
				}
				catch( Exception exception )
				{
					return string.Format( "Graphics device reset failed.\n\n{0}", exception.Message );
				}
			}

			// Successfully reset.
			return null;
		}

		protected virtual void PaintUsingSystemDrawing( System.Drawing.Graphics graphics, string text )
		{
			// Clear.
			graphics.Clear( Color.CornflowerBlue );

			// Draw message text.
			using( Brush brush = new SolidBrush( Color.Black ) )
			{
				using( StringFormat format = new StringFormat() )
				{
					format.Alignment = StringAlignment.Center;
					format.LineAlignment = StringAlignment.Center;

					graphics.DrawString( text, Font, brush, ClientRectangle, format );
				}
			}
		}

		protected override void OnPaintBackground( PaintEventArgs pevent )
		{
			// Ignore WinForms PaintBackground message.
		}

		/// <summary>
		/// Derived classes must override this to initialize their drawing code.
		/// </summary>
		protected abstract void Initialize();


		/// <summary>
		/// Derived classes must override this to draw themselves using the GraphicsDevice.
		/// </summary>
		protected abstract void Draw();
	}
}
