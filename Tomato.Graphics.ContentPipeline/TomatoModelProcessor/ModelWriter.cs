using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace Tomato.Graphics.Content.Pipeline
{
	[ContentTypeWriter]
	public sealed class ModelWriter: ContentTypeWriter<ModelContent>
	{
		protected override void Write( ContentWriter output, ModelContent value )
		{
			value.WriteTo( output );
		}

		public override string GetRuntimeType( TargetPlatform targetPlatform )
		{
			return "Tomato.Graphics.Content.Model, Tomato.Graphics";
		}

		public override string GetRuntimeReader( TargetPlatform targetPlatform )
		{
			return "Tomato.Graphics.Content.ModelReader, Tomato.Graphics";
		}
	}
}
