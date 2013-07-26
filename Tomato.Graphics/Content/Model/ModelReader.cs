using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TomatoModel = Tomato.Graphics.Content.Model;

namespace Tomato.Graphics.Content
{
	public sealed class ModelReader : ContentTypeReader<TomatoModel>
	{
		protected override TomatoModel Read( ContentReader input, TomatoModel existingInstance )
		{
			// Read data from stream.
			TomatoModel model = TomatoModel.ReadFrom( input, null );

			return model;
		}
	}
}
