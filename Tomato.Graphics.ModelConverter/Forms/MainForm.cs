using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace Tomato.Graphics.ModelConverter
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void button1_Click( object sender, EventArgs e )
		{
			if( openFileDialog1.ShowDialog() == DialogResult.OK )
			{
				textBox1.Text = openFileDialog1.FileName;
			}
		}

		private void button2_Click( object sender, EventArgs e )
		{
			if( saveFileDialog1.ShowDialog() == DialogResult.OK )
			{
				textBox2.Text = saveFileDialog1.FileName;
			}
		}

		private void button3_Click( object sender, EventArgs e )
		{
			if( !string.IsNullOrWhiteSpace( textBox1.Text )
				&& !string.IsNullOrWhiteSpace( textBox2.Text ) )
			{
				// Import from source model.
				NodeContent nodeContent = null;
				switch( Path.GetExtension( textBox1.Text ).ToLower() )
				{
					case ".obj":
						{
							ObjConverter converter = new ObjConverter();
							nodeContent = converter.Convert( textBox1.Text );
						}
						break;

					case ".fbx":
						{
							FbxConverter converter = new FbxConverter();
							nodeContent = converter.Convert( textBox1.Text );
						}
						break;
				}

				// Serialize imported NodeContent to the destination file.
				if( nodeContent != null )
				{
					TomatoModelWriter writer = new TomatoModelWriter();
					writer.WriteToFile( nodeContent, textBox2.Text );
				}
			}
		}
	}
}
