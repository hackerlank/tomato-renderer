using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using XnaModelContent = Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelContent;
using XnaModelMeshContent = Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelMeshContent;
using TomatoModelContent = Tomato.Graphics.Content.Pipeline.ModelContent;
using TomatoModelMeshContent = Tomato.Graphics.Content.Pipeline.ModelMeshContent;

namespace Tomato.Graphics.Content.Pipeline
{
	[ContentProcessor( DisplayName = "Model - Tomato Renderer" )]
	public class TomatoModelProcessor : ContentProcessor<NodeContent, TomatoModelContent>
	{
		private bool m_bColorKeyEnabled = true;
		private Color m_colorKeyColor = new Color( 255, 0, 255, 255 );
		private MaterialProcessorDefaultEffect m_defaultEffect = MaterialProcessorDefaultEffect.BasicEffect;
		private bool m_bGenerateMipmaps = true;
		private bool m_bGenerateTangentFrames = false;
		private bool m_bPremultiplyTextureAlpha = true;
		private bool m_bPremultiplyVertexColors = true;
		private bool m_bResizeTexturesToPowerOfTwo = false;
		private float m_rotationX = 0;
		private float m_rotationY = 0;
		private float m_rotationZ = 0;
		private float m_scale = 1;
		private bool m_bSwapWindingOrder = false;
		private TextureProcessorOutputFormat m_textureFormat = TextureProcessorOutputFormat.DxtCompressed;
		//private bool m_bTextureGammaCorrection = false;

		[DisplayName( "Color Key Enabled" ), Description( "" ), DefaultValue( true )]
		public bool ColorKeyEnabled { get { return m_bColorKeyEnabled; } set { m_bColorKeyEnabled = value; } }

		[DisplayName( "Color Key Color" ), Description( "" ), DefaultValue( typeof( Color ), "255, 0, 255, 255" )]
		public Color ColorKeyColor { get { return m_colorKeyColor; } set { m_colorKeyColor = value; } }

		[DisplayName( "Default Effect" ), Description( "" ), DefaultValue( MaterialProcessorDefaultEffect.BasicEffect )]
		public virtual MaterialProcessorDefaultEffect DefaultEffect { get { return m_defaultEffect; } set { m_defaultEffect = value; } }

		[DisplayName( "Generate Mipmaps" ), Description( "" ), DefaultValue( true )]
		public virtual bool GenerateMipmaps { get { return m_bGenerateMipmaps; } set { m_bGenerateMipmaps = value; } }

		[DisplayName( "Generate Tangent Frames" ), Description( "" ), DefaultValue( false )]
		public virtual bool GenerateTangentFrames { get { return m_bGenerateTangentFrames; } set { m_bGenerateTangentFrames = value; } }

		[DisplayName( "Premultiply Texture Alpha" ), Description( "" ), DefaultValue( true )]
		public virtual bool PremultiplyTextureAlpha { get { return m_bPremultiplyTextureAlpha; } set { m_bPremultiplyTextureAlpha = value; } }

		[DisplayName( "Premultiply Vertex Colors" ), Description( "" ), DefaultValue( true )]
		public virtual bool PremultiplyVertexColors { get { return m_bPremultiplyVertexColors; } set { m_bPremultiplyVertexColors = value; } }

		[DisplayName( "Resize Textures to Power of Two" ), Description( "" ), DefaultValue( false )]
		public virtual bool ResizeTexturesToPowerOfTwo { get { return m_bResizeTexturesToPowerOfTwo; } set { m_bResizeTexturesToPowerOfTwo = value; } }

		[DisplayName( "X Axis Rotation" ), Description( "" ), DefaultValue( ( float )0.0f )]
		public virtual float RotationX { get { return m_rotationX; } set { m_rotationX = value; } }

		[DisplayName( "Y Axis Rotation" ), Description( "" ), DefaultValue( ( float )0.0f )]
		public virtual float RotationY { get { return m_rotationY; } set { m_rotationY = value; } }

		[DisplayName( "Z Axis Rotation" ), Description( "" ), DefaultValue( ( float )0.0f )]
		public virtual float RotationZ { get { return m_rotationZ; } set { m_rotationZ = value; } }

		[DisplayName( "Scale" ), Description( "" ), DefaultValue( ( float )1.0f )]
		public virtual float Scale { get { return m_scale; } set { m_scale = value; } }

		[DisplayName( "Swap Winding Order" ), Description( "" ), DefaultValue( false )]
		public virtual bool SwapWindingOrder { get { return m_bSwapWindingOrder; } set { m_bSwapWindingOrder = value; } }

		[Description( "" ), DisplayName( "Texture Format" ), DefaultValue( TextureProcessorOutputFormat.DxtCompressed )]
		public virtual TextureProcessorOutputFormat TextureFormat { get { return m_textureFormat; } set { m_textureFormat = value; } }

		//[DisplayName( "Texture Gamma-Correction" ), Description( "" ), DefaultValue( false )]
		//public virtual bool TextureGammaCorrection { get { return m_bTextureGammaCorrection; } set { m_bTextureGammaCorrection = value; } }

#if false
		[ContentProcessor( DisplayName="Texture - Tomato Renderer" )]
		public class CustomTextureProcessor : TextureProcessor
		{
			private bool m_bTextureGammaCorrection = false;

			[DisplayName( "Texture Gamma-Correction" ), Description( "" ), DefaultValue( false )]
			public bool TextureGammaCorrection { get { return m_bTextureGammaCorrection; } set { m_bTextureGammaCorrection = value; } }

			public override TextureContent Process( TextureContent input, ContentProcessorContext context )
			{
				TextureContent output = base.Process( input, context );

				if( TextureGammaCorrection )
				{
				}

				return output;
			}
		}
#endif
		
		public class CustomModelProcessor : ModelProcessor
		{
			public bool TextureGammaCorrection { get; set; }

			public CustomModelProcessor()
			{
				TextureGammaCorrection = false;
			}

			protected override MaterialContent ConvertMaterial( MaterialContent material, ContentProcessorContext context )
			{
				MaterialContent convertedMaterial = base.ConvertMaterial( material, context );

				// Check uncompiled textures.
				Dictionary<string, ExternalReference<TextureContent>> compiledTextures = new Dictionary<string,ExternalReference<TextureContent>>();
				foreach( var texture in convertedMaterial.Textures )
				{
					if( !texture.Value.Filename.EndsWith( ".xnb", StringComparison.OrdinalIgnoreCase ) )
					{
						compiledTextures.Add( texture.Key, BuildTexture( texture.Value, context ) );
					}
				}
				
				// Replace uncompiled textures.
				foreach( var compiledTexture in compiledTextures )
				{
					convertedMaterial.Textures[ compiledTexture.Key ] = compiledTexture.Value;
				}

				// Make it standard texture names.
				TomatoMaterialContent tomatoMaterial = ConvertToTomatoMaterial( convertedMaterial, context );
				if( tomatoMaterial == null )
				{
					throw new InvalidOperationException( "Failed converting material to Tomato material." );
				}

				return tomatoMaterial;
			}		

			private static TomatoMaterialContent ConvertToTomatoMaterial( MaterialContent material, ContentProcessorContext context )
			{
				TomatoMaterialContent tomatoMaterial = material as TomatoMaterialContent;
				if( tomatoMaterial != null )
				{
					return tomatoMaterial;
				}

				tomatoMaterial = new TomatoMaterialContent();

				BasicMaterialContent basicMaterial = material as BasicMaterialContent;
				if( basicMaterial != null )
				{
					tomatoMaterial.EmissiveColor = basicMaterial.EmissiveColor.HasValue ? basicMaterial.EmissiveColor.Value : Vector3.Zero;
					tomatoMaterial.DiffuseColor = basicMaterial.DiffuseColor.HasValue ? basicMaterial.DiffuseColor.Value : Vector3.Zero;
					tomatoMaterial.SpecularColor = basicMaterial.SpecularColor.HasValue ? basicMaterial.SpecularColor.Value : Vector3.Zero;
					tomatoMaterial.SpecularPower = basicMaterial.SpecularPower.HasValue ? basicMaterial.SpecularPower.Value : 0;
					tomatoMaterial.SetDiffuseTexture( basicMaterial.Texture );
				}

				AlphaTestMaterialContent alphaTestMaterial = material as AlphaTestMaterialContent;
				if( alphaTestMaterial != null )
				{
					tomatoMaterial.EmissiveColor = Vector3.Zero;
					tomatoMaterial.DiffuseColor = alphaTestMaterial.DiffuseColor.HasValue ? alphaTestMaterial.DiffuseColor.Value : Vector3.Zero;
					tomatoMaterial.SpecularColor = Vector3.Zero;
					tomatoMaterial.SpecularPower = 0;
					tomatoMaterial.SetDiffuseTexture( alphaTestMaterial.Texture );
				}

				DualTextureMaterialContent dualTextureMaterial = material as DualTextureMaterialContent;
				if( dualTextureMaterial != null )
				{
					tomatoMaterial.EmissiveColor = Vector3.Zero;
					tomatoMaterial.DiffuseColor = dualTextureMaterial.DiffuseColor.HasValue ? dualTextureMaterial.DiffuseColor.Value : Vector3.Zero;
					tomatoMaterial.SpecularColor = Vector3.Zero;
					tomatoMaterial.SpecularPower = 0;
					tomatoMaterial.SetDiffuseTexture( dualTextureMaterial.Texture );
				}

				EnvironmentMapMaterialContent environmentMapMaterial = material as EnvironmentMapMaterialContent;
				if( environmentMapMaterial != null )
				{
					tomatoMaterial.EmissiveColor = environmentMapMaterial.EmissiveColor.HasValue ? environmentMapMaterial.EmissiveColor.Value : Vector3.Zero;
					tomatoMaterial.DiffuseColor = environmentMapMaterial.DiffuseColor.HasValue ? environmentMapMaterial.DiffuseColor.Value : Vector3.Zero;
					tomatoMaterial.SpecularColor = environmentMapMaterial.EnvironmentMapSpecular.HasValue ? environmentMapMaterial.EnvironmentMapSpecular.Value : Vector3.Zero;
					tomatoMaterial.SpecularPower = environmentMapMaterial.EnvironmentMapAmount.HasValue ? environmentMapMaterial.EnvironmentMapAmount.Value : 0;
					tomatoMaterial.SetDiffuseTexture( environmentMapMaterial.Texture );
				}

				SkinnedMaterialContent skinnedMaterial = material as SkinnedMaterialContent;
				if( skinnedMaterial != null )
				{
					tomatoMaterial.EmissiveColor = skinnedMaterial.EmissiveColor.HasValue ? skinnedMaterial.EmissiveColor.Value : Vector3.Zero;
					tomatoMaterial.DiffuseColor = skinnedMaterial.DiffuseColor.HasValue ? skinnedMaterial.DiffuseColor.Value : Vector3.Zero;
					tomatoMaterial.SpecularColor = skinnedMaterial.SpecularColor.HasValue ? skinnedMaterial.SpecularColor.Value : Vector3.Zero;
					tomatoMaterial.SpecularPower = skinnedMaterial.SpecularPower.HasValue ? skinnedMaterial.SpecularPower.Value : 0;
					tomatoMaterial.SetDiffuseTexture( skinnedMaterial.Texture );
				}

				return tomatoMaterial;
			}

			private ExternalReference<TextureContent> BuildTexture( ExternalReference<TextureContent> sourceTexture, ContentProcessorContext context )
			{
				OpaqueDataDictionary processorParameters = new OpaqueDataDictionary();
				processorParameters.Add( "ColorKeyColor", ColorKeyColor );
				processorParameters.Add( "ColorKeyEnabled", ColorKeyEnabled );
				processorParameters.Add( "TextureFormat", TextureFormat );
				processorParameters.Add( "GenerateMipmaps", GenerateMipmaps );
				processorParameters.Add( "ResizeToPowerOfTwo", ResizeTexturesToPowerOfTwo );
				processorParameters.Add( "PremultiplyAlpha", PremultiplyTextureAlpha );
				//processorParameters.Add( "TextureGammaCorrection", TextureGammaCorrection );
				return context.BuildAsset<TextureContent, TextureContent>( sourceTexture, typeof( TextureProcessor ).Name, processorParameters, null, null );
			}
		}

		public TomatoModelProcessor()
		{
		}

		public override TomatoModelContent Process( NodeContent input, ContentProcessorContext context )
		{
			DateTime startTime = DateTime.Now;

			// First convert to XNA ModelContent.
			CustomModelProcessor xnaModelProcess = new CustomModelProcessor();
			{
				xnaModelProcess.ColorKeyColor = ColorKeyColor;
				xnaModelProcess.ColorKeyEnabled = ColorKeyEnabled;
				xnaModelProcess.DefaultEffect = DefaultEffect;
				xnaModelProcess.GenerateMipmaps = GenerateMipmaps;
				xnaModelProcess.GenerateTangentFrames = GenerateTangentFrames;
				xnaModelProcess.PremultiplyTextureAlpha = PremultiplyTextureAlpha;
				xnaModelProcess.PremultiplyVertexColors = PremultiplyVertexColors;
				xnaModelProcess.ResizeTexturesToPowerOfTwo = ResizeTexturesToPowerOfTwo;
				xnaModelProcess.RotationX = RotationX;
				xnaModelProcess.RotationY = RotationY;
				xnaModelProcess.RotationZ = RotationZ;
				xnaModelProcess.Scale = Scale;
				xnaModelProcess.SwapWindingOrder = SwapWindingOrder;
				xnaModelProcess.TextureFormat = TextureFormat;
				//xnaModelProcess.TextureGammaCorrection = TextureGammaCorrection;
			}
			XnaModelContent xnaModel = xnaModelProcess.Process( input, context );

			// Output time.
			TimeSpan elapsedTime = DateTime.Now - startTime;
			context.Logger.LogImportantMessage( " > Converted to XNA ModelContent. Total {0} seconds elapsed.", elapsedTime.TotalSeconds );
			
			startTime = DateTime.Now;

			// Convert XNA ModelContent to ModelContent.
			TomatoModelContent tomatoModel = ProcessModelBone( xnaModel, xnaModel.Root, null, context );

			// Output time.
			elapsedTime = DateTime.Now - startTime;
			context.Logger.LogImportantMessage( " > Converted to Tomato ModelContent. Total {0} seconds elapsed.", elapsedTime.TotalSeconds );

			return tomatoModel;
		}

		private TomatoModelContent ProcessModelBone( XnaModelContent xnaModel, ModelBoneContent xnaModelBone, TomatoModelContent parentModel, ContentProcessorContext context )
		{
			// Collect meshes that belong to the model bone.
			var modelMeshes = from mesh in xnaModel.Meshes
							  where mesh.ParentBone.Index == xnaModelBone.Index
							  select mesh;

			// Check if the bone has no children.
			if( ( xnaModelBone.Children.Count == 0 )
				&& ( modelMeshes.Count() == 0 ) )
			{
				//throw new InvalidOperationException( "ModelBone has no children." );
				return null;
			}

			if( ( modelMeshes.Count() == 1 ) && ( xnaModelBone.Children.Count == 0 ) )
			{
				// A single mesh only.
				// Convert it to Mesh object directly.
				return new TomatoModelContent( parentModel, xnaModelBone.Name, xnaModelBone.Transform, new TomatoModelMeshContent( modelMeshes.First(), context ) );
			}
			else
			{
				// Create a node containing child bones and meshes "at the same level".
				TomatoModelContent output = new TomatoModelContent( parentModel, xnaModelBone.Name, xnaModelBone.Transform, null );

				// Add bone children.
				foreach( ModelBoneContent child in xnaModelBone.Children )
				{
					TomatoModelContent childModel = ProcessModelBone( xnaModel, child, output, context );
					if( childModel != null )
					{
						output.Children.Add( childModel );
					}
				}

				// Add meshes that belong to the bone.
				foreach( XnaModelMeshContent modelMesh in modelMeshes )
				{
					output.Children.Add( new TomatoModelContent( output, modelMesh.Name, Matrix.Identity, new TomatoModelMeshContent( modelMesh, context ) ) );
				}

				return output;
			}
		}
	}
}