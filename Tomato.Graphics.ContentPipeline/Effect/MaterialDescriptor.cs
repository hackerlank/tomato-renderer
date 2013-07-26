using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using XnaModelMeshPartContent = Microsoft.Xna.Framework.Content.Pipeline.Processors.ModelMeshPartContent;

namespace Tomato.Graphics.Content.Pipeline
{
	public struct MaterialDescriptor
	{
		// First 32 bits
		private static readonly BitVector32.Section m_vertexHasColor0;
		private static readonly BitVector32.Section m_vertexHasTexCoord0;
		private static readonly BitVector32.Section m_vertexHasTexCoord1;
		private static readonly BitVector32.Section m_vertexHasTexCoord2;
		private static readonly BitVector32.Section m_vertexHasNormal;
		private static readonly BitVector32.Section m_vertexHasBinormal;
		private static readonly BitVector32.Section m_vertexHasTangent;
		private static readonly BitVector32.Section m_diffuseMapEnabled;
		private static readonly BitVector32.Section m_diffuseMapAddressU;
		private static readonly BitVector32.Section m_diffuseMapAddressV;
		private static readonly BitVector32.Section m_diffuseMapMagFilter;
		private static readonly BitVector32.Section m_diffuseMapMinFilter;
		private static readonly BitVector32.Section m_diffuseMapMipFilter;

		// Bits
		private BitVector32 m_bits1;
		private BitVector32 m_bits2;

		static MaterialDescriptor()
		{
			// VERTEX_HAS_COLOR0
			m_vertexHasColor0 = BitVector32.CreateSection( 1 );

			// VERTEX_HAS_TEXCOORD0/1/2
			m_vertexHasTexCoord0 = BitVector32.CreateSection( 1, m_vertexHasColor0 );
			m_vertexHasTexCoord1 = BitVector32.CreateSection( 1, m_vertexHasTexCoord0 );
			m_vertexHasTexCoord2 = BitVector32.CreateSection( 1, m_vertexHasTexCoord1 );

			// VERTEX_HAS_NORMAL
			m_vertexHasNormal = BitVector32.CreateSection( 1, m_vertexHasTexCoord2 );
			m_vertexHasBinormal = BitVector32.CreateSection( 1, m_vertexHasNormal );
			m_vertexHasTangent = BitVector32.CreateSection( 1, m_vertexHasBinormal );

			// DIFFUSE_MAP
			m_diffuseMapEnabled = BitVector32.CreateSection( 1, m_vertexHasTangent );
			m_diffuseMapAddressU = BitVector32.CreateSection( 1, m_diffuseMapEnabled );
			m_diffuseMapAddressV = BitVector32.CreateSection( 1, m_diffuseMapAddressU );
			m_diffuseMapMagFilter = BitVector32.CreateSection( 3, m_diffuseMapAddressV );
			m_diffuseMapMinFilter = BitVector32.CreateSection( 3, m_diffuseMapMagFilter );
			m_diffuseMapMipFilter = BitVector32.CreateSection( 3, m_diffuseMapMinFilter );
		}

		public long HashValue { get { return ( long )( m_bits2.Data << 32 ) | ( long )( m_bits1.Data ); } }
		public string UniqueIdentifier { get { return string.Format( "{0:X8}{1:X8}", m_bits2.Data, m_bits1.Data ); } }

		public bool VertexHasColor0 { get { return m_bits1[ m_vertexHasColor0 ] != 0; } }

		public bool VertexHasTexCoord0 { get { return m_bits1[ m_vertexHasTexCoord0 ] != 0; } }
		public bool VertexHasTexCoord1 { get { return m_bits1[ m_vertexHasTexCoord1 ] != 0; } }
		public bool VertexHasTexCoord2 { get { return m_bits1[ m_vertexHasTexCoord2 ] != 0; } }

		public bool VertexHasNormal { get { return m_bits1[ m_vertexHasNormal ] != 0; } }
		public bool VertexHasBinormal { get { return m_bits1[ m_vertexHasBinormal ] != 0; } }
		public bool VertexHasTangent { get { return m_bits1[ m_vertexHasTangent ] != 0; } }

		public bool DiffuseMapEnabled { get { return m_bits1[ m_diffuseMapEnabled ] != 0; } }
		public int DiffuseMapAddressU { get { return m_bits1[ m_diffuseMapAddressU ]; } }
		public int DiffuseMapAddressV { get { return m_bits1[ m_diffuseMapAddressV ]; } }
		public int DiffuseMapMagFilter { get { return m_bits1[ m_diffuseMapMagFilter ]; } }
		public int DiffuseMapMinFilter { get { return m_bits1[ m_diffuseMapMinFilter ]; } }
		public int DiffuseMapMipFilter { get { return m_bits1[ m_diffuseMapMipFilter ]; } }

		public MaterialDescriptor( XnaModelMeshPartContent meshPart )
		{
			m_bits1 = new BitVector32( 0 );
			m_bits2 = new BitVector32( 0 );

			DetermineBits1( meshPart );
			DetermineBits2( meshPart );
		}

		private void DetermineBits1( XnaModelMeshPartContent meshPart )
		{
			// VERTEX_HAS_COLOR0
			m_bits1[ m_vertexHasColor0 ] = ContainsVertexChannel( meshPart, VertexElementUsage.Color, 0 ) ? 1 : 0;

			// VERTEX_HAS_TEXCOORD0/1/2
			m_bits1[ m_vertexHasTexCoord0 ] = ContainsVertexChannel( meshPart, VertexElementUsage.TextureCoordinate, 0 ) ? 1 : 0;
			m_bits1[ m_vertexHasTexCoord1 ] = ContainsVertexChannel( meshPart, VertexElementUsage.TextureCoordinate, 1 ) ? 1 : 0;
			m_bits1[ m_vertexHasTexCoord2 ] = ContainsVertexChannel( meshPart, VertexElementUsage.TextureCoordinate, 2 ) ? 1 : 0;

			// VERTEX_HAS_NORMAL
			m_bits1[ m_vertexHasNormal ] = ContainsVertexChannel( meshPart, VertexElementUsage.Normal, 0 ) ? 1 : 0;
			m_bits1[ m_vertexHasBinormal ] = ContainsVertexChannel( meshPart, VertexElementUsage.Binormal, 0 ) ? 1 : 0;
			m_bits1[ m_vertexHasTangent ] = ContainsVertexChannel( meshPart, VertexElementUsage.Tangent, 0 ) ? 1 : 0;

			// DIFFUSE_MAP
			DetermineDiffuseMapBits( meshPart );
		}

		private void DetermineBits2( XnaModelMeshPartContent meshPart )
		{
		}

		private void DetermineDiffuseMapBits( XnaModelMeshPartContent meshPart )
		{
			// Just use the first texture for now.
			if( meshPart.Material.Textures.Count > 0 )
			{
				// Enabled
				m_bits1[ m_diffuseMapEnabled ] = 1;

				// Address mode: 0 - Wrap, 1 - Clamp
				m_bits1[ m_diffuseMapAddressU ] = 0;
				m_bits1[ m_diffuseMapAddressV ] = 0;

				// Filter mode: 0 - None, 1 - Point, 2 - Linear, 3 - Anisotropic
				m_bits1[ m_diffuseMapMagFilter ] = 2;
				m_bits1[ m_diffuseMapMinFilter ] = 2;
				m_bits1[ m_diffuseMapMipFilter ] = 2;
			}
			else
			{
				// Diable all.
				m_bits1[ m_diffuseMapEnabled ] = 0;
				m_bits1[ m_diffuseMapAddressU ] = 0;
				m_bits1[ m_diffuseMapAddressV ] = 0;
				m_bits1[ m_diffuseMapMagFilter ] = 0;
				m_bits1[ m_diffuseMapMinFilter ] = 0;
				m_bits1[ m_diffuseMapMipFilter ] = 0;
			}
		}

		public string GetEffectDefines()
		{
			StringBuilder effectDefines = new StringBuilder();

			// VERTEX_HAS_COLOR0
			effectDefines.AppendFormat( "VERTEX_HAS_COLOR0={0};", VertexHasColor0 ? 1 : 0 );

			// VERTEX_HAS_TEXCOORDn
			effectDefines.AppendFormat( "VERTEX_HAS_TEXCOORD0={0};", VertexHasTexCoord0 ? 1 : 0 );
			effectDefines.AppendFormat( "VERTEX_HAS_TEXCOORD1={0};", VertexHasTexCoord1 ? 1 : 0 );
			effectDefines.AppendFormat( "VERTEX_HAS_TEXCOORD2={0};", VertexHasTexCoord2 ? 1 : 0 );

			// VERTEX_HAS_NORMAL
			effectDefines.AppendFormat( "VERTEX_HAS_NORMAL={0};", VertexHasNormal ? 1 : 0 );
			effectDefines.AppendFormat( "VERTEX_HAS_BINORMAL={0};", VertexHasBinormal ? 1 : 0 );
			effectDefines.AppendFormat( "VERTEX_HAS_TANGENT={0};", VertexHasTangent ? 1 : 0 );

			// DIFFUSE_MAP
			effectDefines.AppendFormat( "DIFFUSE_MAP_ENABLED={0};", DiffuseMapEnabled ? 1 : 0 );
			effectDefines.AppendFormat( "DIFFUSE_TEXTURE_ADDRESSU={0};", ( DiffuseMapAddressU == 0 ) ? "WRAP" : "CLAMP" );
			effectDefines.AppendFormat( "DIFFUSE_TEXTURE_ADDRESSV={0};", ( DiffuseMapAddressV == 0 ) ? "WRAP" : "CLAMP" );
			switch( DiffuseMapMagFilter )
			{
				case 0: { effectDefines.Append( "DIFFUSE_TEXTURE_MAGFILTER=NONE;" ); } break;
				case 1: { effectDefines.Append( "DIFFUSE_TEXTURE_MAGFILTER=POINT;" ); } break;
				case 2: { effectDefines.Append( "DIFFUSE_TEXTURE_MAGFILTER=LINEAR;" ); } break;
				case 3: { effectDefines.Append( "DIFFUSE_TEXTURE_MAGFILTER=ANISOTROPIC;" ); } break;
			}
			switch( DiffuseMapMinFilter )
			{
				case 0: { effectDefines.Append( "DIFFUSE_TEXTURE_MINFILTER=NONE;" ); } break;
				case 1: { effectDefines.Append( "DIFFUSE_TEXTURE_MINFILTER=POINT;" ); } break;
				case 2: { effectDefines.Append( "DIFFUSE_TEXTURE_MINFILTER=LINEAR;" ); } break;
				case 3: { effectDefines.Append( "DIFFUSE_TEXTURE_MINFILTER=ANISOTROPIC;" ); } break;
			}
			switch( DiffuseMapMipFilter )
			{
				case 0: { effectDefines.Append( "DIFFUSE_TEXTURE_MIPFILTER=NONE;" ); } break;
				case 1: { effectDefines.Append( "DIFFUSE_TEXTURE_MIPFILTER=POINT;" ); } break;
				case 2: { effectDefines.Append( "DIFFUSE_TEXTURE_MIPFILTER=LINEAR;" ); } break;
				case 3: { effectDefines.Append( "DIFFUSE_TEXTURE_MIPFILTER=ANISOTROPIC;" ); } break;
			}

			return effectDefines.ToString();
		}

		private static bool ContainsVertexChannel( XnaModelMeshPartContent meshPart, VertexElementUsage channel, int index )
		{
			foreach( VertexElement element in meshPart.VertexBuffer.VertexDeclaration.VertexElements )
			{
				if( ( element.VertexElementUsage == channel )
					&& ( element.UsageIndex == index ) )
				{
					return true;
				}
			}

			return false;
		}
	}
}
