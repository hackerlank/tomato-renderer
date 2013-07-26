using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace Tomato.Graphics.ModelConverter
{
	public class ObjConverter
	{
		public NodeContent Convert( string sourceFilePath )
		{
			ObjImporter importer = new ObjImporter();
			return importer.Import( sourceFilePath );
		}
	}
}
