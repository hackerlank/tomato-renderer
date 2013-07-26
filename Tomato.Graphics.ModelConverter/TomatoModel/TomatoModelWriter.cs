using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using XnaModelContent = Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelContent;
using XnaModelMeshContent = Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelMeshContent;

namespace Tomato.Graphics.ModelConverter
{
	public class TomatoModelWriter
	{
		public void WriteToFile( NodeContent nodeContent, string destinationFilePath )
		{
			BinaryWriter writer = new BinaryWriter( File.OpenWrite( destinationFilePath ) );

			Write( nodeContent, writer );

			writer.Flush();
			writer.Close();
		}

		private void Write( NodeContent nodeContent, BinaryWriter writer )
		{
			// Write name.
			writer.Write( nodeContent.Name );

			// Write transformation matrix.
			writer.Write( nodeContent.Transform.M11 );
			writer.Write( nodeContent.Transform.M12 );
			writer.Write( nodeContent.Transform.M13 );
			writer.Write( nodeContent.Transform.M14 );
			writer.Write( nodeContent.Transform.M21 );
			writer.Write( nodeContent.Transform.M22 );
			writer.Write( nodeContent.Transform.M23 );
			writer.Write( nodeContent.Transform.M24 );
			writer.Write( nodeContent.Transform.M31 );
			writer.Write( nodeContent.Transform.M32 );
			writer.Write( nodeContent.Transform.M33 );
			writer.Write( nodeContent.Transform.M34 );
			writer.Write( nodeContent.Transform.M41 );
			writer.Write( nodeContent.Transform.M42 );
			writer.Write( nodeContent.Transform.M43 );
			writer.Write( nodeContent.Transform.M44 );

			// What about OpaqueData?
			if( nodeContent.OpaqueData.Count > 0 )
			{
				throw new InvalidOperationException( "Oops!" );
			}

			MeshContent meshContent = nodeContent as MeshContent;
			if( meshContent != null )
			{
				// Indinate that mesh data is included.
				writer.Write( true );

				// Write position data.
				writer.Write( meshContent.Positions.Count );
				for( int i = 0 ; i < meshContent.Positions.Count ; ++i )
				{
					writer.Write( meshContent.Positions[ i ].X );
					writer.Write( meshContent.Positions[ i ].Y );
					writer.Write( meshContent.Positions[ i ].Z );
				}

				// Write geomety data.
				writer.Write( meshContent.Geometry.Count );
				for( int i = 0 ; i < meshContent.Geometry.Count ; ++i )
				{
					GeometryContent geometry = meshContent.Geometry[ i ];

					// Implementing here...
				}
			}
			else
			{
				// Indinate that mesh data is not included.
				writer.Write( false );
			}

			// Write children.
			writer.Write( nodeContent.Children.Count );
			for( int i = 0 ; i < nodeContent.Children.Count ; ++i )
			{
				Write( nodeContent.Children[ i ], writer );
			}
		}

		private void WriteMatrix( BinaryWriter writer, Matrix matrix )
		{
			
		}
	}
}
