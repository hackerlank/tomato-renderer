using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using XnaModelMeshPartContent = Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelMeshPartContent;
using TomatoMeshPartContent = Tomato.Graphics.Content.Pipeline.ModelMeshPartContent;

namespace Tomato.Graphics.Content.Pipeline
{
	public static class EffectBuilder
	{
		public static string m_effectTemplateCode = null;
		public static Dictionary<long, CompiledEffectContent> m_compiledEffectCache = null;

		static EffectBuilder()
		{
			m_compiledEffectCache = new Dictionary<long, CompiledEffectContent>();
		}

		public static CompiledEffectContent CreateEffect(
			XnaModelMeshPartContent meshPart,
			ContentProcessorContext context )
		{
			// Prepare effect template code.
			if( m_effectTemplateCode == null )
			{
				// Read effect template text from Assembly resource.
				Assembly currentAssembly = Assembly.GetExecutingAssembly();
				//string baseDirectory = Path.GetDirectoryName( currentAssembly.Location );

				// Read EffectConstant.fx
				StringBuilder effectTemplateCode = new StringBuilder();
				{
					StreamReader reader = new StreamReader( currentAssembly.GetManifestResourceStream( "Tomato.Graphics.Content.Pipeline.Effect.EffectConstants.fx" ), Encoding.Unicode );
					effectTemplateCode.Append( reader.ReadToEnd() );
					//effectTemplateCode.Append( File.ReadAllText( Path.Combine( baseDirectory, @"Effect\EffectConstants.fx" ) ) );
					effectTemplateCode.Append( "\r\n\r\n" );
				}

				// Read EffectTemplate.fx
				{
					StreamReader reader = new StreamReader( currentAssembly.GetManifestResourceStream( "Tomato.Graphics.Content.Pipeline.Effect.EffectTemplate.fx" ), Encoding.Unicode );
					effectTemplateCode.Append( reader.ReadToEnd() );
					//effectTemplateCode.Append( File.ReadAllText( Path.Combine( baseDirectory, @"Effect\EffectTemplate.fx" ) ) );
				}

				// Get code.
				m_effectTemplateCode = effectTemplateCode.ToString();
				if( string.IsNullOrWhiteSpace( m_effectTemplateCode ) )
				{
					throw new InvalidOperationException( "Effect template code has no text." );
				}
			}

			// Get material descriptor.
			MaterialDescriptor materialDescriptor = GetMaterialDescriptor( meshPart );

			// If cached effect is available, use that.
			CompiledEffectContent compiledEffectContent;
			if( m_compiledEffectCache.TryGetValue( materialDescriptor.HashValue, out compiledEffectContent ) )
			{
				return compiledEffectContent;
			}

			// Prepare effect and define macros.
			// Then compile effect with EffectProcessor.
			EffectContent effectContent = new EffectContent();
			effectContent.EffectCode = m_effectTemplateCode;
			EffectProcessor effectProcessor = new EffectProcessor();
			effectProcessor.Defines = materialDescriptor.GetEffectDefines();
			effectProcessor.DebugMode = EffectProcessorDebugMode.Auto;
			compiledEffectContent = effectProcessor.Process( effectContent, context );

			// Add to cache.
			m_compiledEffectCache[ materialDescriptor.HashValue ] = compiledEffectContent;

			return compiledEffectContent;
		}

		private static MaterialDescriptor GetMaterialDescriptor( XnaModelMeshPartContent meshPart )
		{
			return new MaterialDescriptor( meshPart );
		}
	}
}
