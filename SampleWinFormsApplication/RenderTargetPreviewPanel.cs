using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Tomato.Graphics;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace SampleWinFormsApplication
{
	public partial class RenderTargetPreviewPanel : UserControl
	{
		private const int FixedHeight = 80;

		private Renderer m_renderer = null;
		private RenderTarget2D m_renderTarget = null;

		public Bitmap Image { get; private set; }

		public RenderTargetPreviewPanel( Renderer renderer, RenderTarget2D renderTarget )
		{
			InitializeComponent();

			m_renderer = renderer;
			m_renderTarget = renderTarget;

			// Pre-adjust the size.
			if( m_renderTarget != null )
			{
				int width = ( int )( System.Math.Round( ( double )( FixedHeight * renderTarget.Width ) / ( double )( renderTarget.Height ) ) );
				Size = new Size( width, FixedHeight );
			}
			else
			{
				Size = new Size( 100, 40 );
			}

			if( m_renderTarget != null )
			{
				// Set the title.
				label1.Text = m_renderTarget.Name;

				// Fill the picture box.
				XnaColor[] colors = m_renderer.ReadRenderTarget( m_renderTarget.Name );
				if( colors != null )
				{
					// Fill the bitmap data.
					Image = new Bitmap( m_renderTarget.Width, m_renderTarget.Height );
					for( int h = 0 ; h < m_renderTarget.Height ; h++ )
					{
						for( int w = 0 ; w < m_renderTarget.Width ; w++ )
						{
							Image.SetPixel(
								w, h,
								System.Drawing.Color.FromArgb(
									colors[ h * m_renderTarget.Width + w ].A,
									colors[ h * m_renderTarget.Width + w ].R,
									colors[ h * m_renderTarget.Width + w ].G,
									colors[ h * m_renderTarget.Width + w ].B ) );
						}
					}

					pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
					pictureBox1.Image = Image;
				}
			}
			else
			{
				label1.Text = "(Error)";
				Image = null;
			}
		}

		private void pictureBox1_MouseEnter( object sender, EventArgs e )
		{
		}

		private void pictureBox1_MouseLeave( object sender, EventArgs e )
		{
		}
	}
}
