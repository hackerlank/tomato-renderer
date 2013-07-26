using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace Tomato.Graphics.ModelConverter
{
	public class ObjImporter
	{
		// Root node.
		private NodeContent m_rootNode;

		// Vertex data.
		private List<Vector3> m_positions;
		private List<Vector2> m_texCoords;
		private List<Vector3> m_normals;

		// The current mesh being constructed.
		private MeshBuilder m_meshBuilder;

		// Mapping from index to m_positions array to index to MeshBuilder's position array.
		private int[] m_positionMap;

		// Data index for texture coordinate vertex channel.
		private int m_textureCoordinateDataIndex;

		// Data index for normal vertex channel.
		private int m_normalDataIndex;

		// Named m_materials from all imported MTL files
		private Dictionary<String, MaterialContent> m_materials;

		// Identity of current .mtl file for reporting errors agaisnt
		private ContentIdentity m_mtlFileIdentity;

		// Current material being constructed
		private TomatoMaterialContent m_currentMaterial;

		public NodeContent Import( string filePath )
		{
			// Reset all importer state
			m_rootNode = new NodeContent();
			m_positions = new List<Vector3>();
			m_texCoords = new List<Vector2>();
			m_normals = new List<Vector3>();
			m_meshBuilder = null;
			m_materials = new Dictionary<string, MaterialContent>();

			// 
			m_rootNode.Identity = new ContentIdentity( filePath );

			try
			{
				// Loop over each tokenized line of the OBJ file.
				foreach( String[] lineTokens in GetLineTokens( filePath, m_rootNode.Identity ) )
				{
					ParseObjLine( lineTokens );
				}

				// If the file did not provide a model name (through an 'o' line),
				// then use the file name as a default
				if( m_rootNode.Name == null )
				{
					m_rootNode.Name = Path.GetFileNameWithoutExtension( filePath );
				}

				// Finish the last mesh
				FinishMesh();

				// Done with entire model!
				return m_rootNode;
			}
			catch( InvalidContentException )
			{
				// InvalidContentExceptions do not need further processing
				throw;
			}
			catch( Exception e )
			{
				// Wrap exception with content identity (includes line number)
				throw new InvalidContentException(
					"Unable to parse obj file. Exception:\n" + e.Message,
					m_rootNode.Identity, e );
			}
		}

		/// <summary>
		/// Parses and executes an individual line of an OBJ file.
		/// </summary>
		/// <param name="lineTokens">Line to parse as tokens</param>
		private void ParseObjLine( string[] lineTokens )
		{
			switch( lineTokens[ 0 ].ToLower() )
			{
				// Object
				case "o":
					{
						// Name of the model.
						m_rootNode.Name = lineTokens[ 1 ];
					}
					break;

				// Vertices
				case "v":
					{
						m_positions.Add( ParseVector3( lineTokens ) );
					}
					break;

				// Texture coordinates
				case "vt":
					{
						// The number of texture coordinate tokens can be 1, 2, or 3.
						// Assume that only 2 tokens of texturu coordinates are supported.
						Vector2 vt = ParseVector2( lineTokens );

						// Flip the second coordinate(v).
						vt.Y = 1 - vt.Y;

						m_texCoords.Add( vt );
					}
					break;

				// Normals
				case "vn":
					{
						m_normals.Add( ParseVector3( lineTokens ) );
					}
					break;

				// Groups (model meshes)
				case "g":
					{
						// Finish the current mesh.
						if( m_meshBuilder != null )
						{
							FinishMesh();
						}

						// Begin a new mesh.
						// The next token is an optional name.
						if( lineTokens.Length > 1 )
						{
							StartMesh( lineTokens[ 1 ] );
						}
						else
						{
							StartMesh( null );
						}
					}
					break;

				// Smoothing group
				case "s":
					// Ignored.
					// Just use the m_normals as specified with verticies.
					break;

				// Faces 
				case "f":
					{
						if( ( lineTokens.Length != 4 )
							&& ( lineTokens.Length != 5 ) )
						{
							//m_importerContext.Logger.LogWarning( null, m_rootNode.Identity, "N-sided polygons are not supported; Ignoring face." );
							break;
						}

						// If the builder is null, this face is outside of a group.
						// Start a new, unnamed group.
						if( m_meshBuilder == null )
						{
							StartMesh( null );
						}

						bool bTextureCoordinate = false;
						bool bNormal = false;
						int[] positionIndices = new int[ lineTokens.Length - 1 ];
						Vector2[] textureCoordinates = new Vector2[ lineTokens.Length - 1 ];
						Vector3[] normals = new Vector3[ lineTokens.Length - 1 ];
						for( int vertexIndex = 1 ; vertexIndex < lineTokens.Length ; ++vertexIndex )
						{
							// Each vertex is a set of three indices: position, texture coordinate, and normal.
							// The indices are 1-based, separated by slashes and only position is required.
							string[] indices = lineTokens[ vertexIndex ].Split( '/' );

							// Required: position.
							positionIndices[ vertexIndex - 1 ] = int.Parse( indices[ 0 ], CultureInfo.InvariantCulture ) - 1;

							// Optional: texture coordinate.
							if( indices.Length > 1 )
							{
								int textureCoordinateIndex;
								Vector2 textureCoordinate;
								if( int.TryParse( indices[ 1 ], out textureCoordinateIndex ) )
								{
									textureCoordinate = m_texCoords[ textureCoordinateIndex - 1 ];
								}
								else
								{
									textureCoordinate = Vector2.Zero;
								}

								bTextureCoordinate = true;
								textureCoordinates[ vertexIndex - 1 ] = textureCoordinate;
							}

							// Optional: normal.
							if( indices.Length > 2 )
							{
								int normalIndex;
								Vector3 normal;
								if( int.TryParse( indices[ 2 ], out normalIndex ) )
								{
									normal = m_normals[ normalIndex - 1 ];
								}
								else
								{
									normal = Vector3.Zero;
								}

								bNormal = true;
								normals[ vertexIndex - 1 ] = normal;
							}
						}

						// First triangle 0, 1, 2
						if( bTextureCoordinate ) { m_meshBuilder.SetVertexChannelData( m_textureCoordinateDataIndex, textureCoordinates[ 0 ] ); }
						if( bNormal ) { m_meshBuilder.SetVertexChannelData( m_normalDataIndex, normals[ 0 ] ); }
						m_meshBuilder.AddTriangleVertex( m_positionMap[ positionIndices[ 0 ] ] );

						if( bTextureCoordinate ) { m_meshBuilder.SetVertexChannelData( m_textureCoordinateDataIndex, textureCoordinates[ 1 ] ); }
						if( bNormal ) { m_meshBuilder.SetVertexChannelData( m_normalDataIndex, normals[ 1 ] ); }
						m_meshBuilder.AddTriangleVertex( m_positionMap[ positionIndices[ 1 ] ] );

						if( bTextureCoordinate ) { m_meshBuilder.SetVertexChannelData( m_textureCoordinateDataIndex, textureCoordinates[ 2 ] ); }
						if( bNormal ) { m_meshBuilder.SetVertexChannelData( m_normalDataIndex, normals[ 2 ] ); }
						m_meshBuilder.AddTriangleVertex( m_positionMap[ positionIndices[ 2 ] ] );

						// Second triangle 2, 3, 0
						if( positionIndices.Length > 3 )
						{
							if( bTextureCoordinate ) { m_meshBuilder.SetVertexChannelData( m_textureCoordinateDataIndex, textureCoordinates[ 2 ] ); }
							if( bNormal ) { m_meshBuilder.SetVertexChannelData( m_normalDataIndex, normals[ 2 ] ); }
							m_meshBuilder.AddTriangleVertex( m_positionMap[ positionIndices[ 2 ] ] );

							if( bTextureCoordinate ) { m_meshBuilder.SetVertexChannelData( m_textureCoordinateDataIndex, textureCoordinates[ 3 ] ); }
							if( bNormal ) { m_meshBuilder.SetVertexChannelData( m_normalDataIndex, normals[ 3 ] ); }
							m_meshBuilder.AddTriangleVertex( m_positionMap[ positionIndices[ 3 ] ] );

							if( bTextureCoordinate ) { m_meshBuilder.SetVertexChannelData( m_textureCoordinateDataIndex, textureCoordinates[ 0 ] ); }
							if( bNormal ) { m_meshBuilder.SetVertexChannelData( m_normalDataIndex, normals[ 0 ] ); }
							m_meshBuilder.AddTriangleVertex( m_positionMap[ positionIndices[ 0 ] ] );
						}
					}
					break;

				// Import a material library file.
				case "mtllib":
					{
						// Remaining tokens are relative paths to .mtl files.
						for( int i = 1 ; i < lineTokens.Length ; i++ )
						{
							string mtlFileName = lineTokens[ i ];

							// Get a full path.
							if( !Path.IsPathRooted( mtlFileName ) )
							{
								string directory = Path.GetDirectoryName( m_rootNode.Identity.SourceFilename );
								mtlFileName = Path.GetFullPath( Path.Combine( directory, mtlFileName ) );
							}

							// Import and record the new m_materials.
							ImportMaterials( mtlFileName );
						}
					}
					break;

				// Apply a material.
				case "usemtl":
					{
						// If the builder is null, OBJ most likely lacks groups.
						// Start a new, unnamed group.
						if( m_meshBuilder == null )
						{
							StartMesh( null );
						}

						// Next token is material name.
						string materialName = lineTokens[ 1 ];

						// Apply the material to the upcoming faces
						MaterialContent material;
						if( m_materials.TryGetValue( materialName, out material ) )
						{
							m_meshBuilder.SetMaterial( material );
						}
						else
						{
							throw new InvalidContentException( String.Format( "Material '{0}' not defined.", materialName ), m_rootNode.Identity );
						}
					}
					break;

				// Unsupported or invalid line types
				default:
					{
						throw new InvalidContentException( "Unsupported or invalid line type '" + lineTokens[ 0 ] + "'", m_rootNode.Identity );
					}
			}
		}


		/// <summary>
		/// Starts a new mesh and fills it with mesh mapped m_positions.
		/// </summary>
		/// <param name="name">Name of mesh.</param>
		private void StartMesh( string name )
		{
			m_meshBuilder = MeshBuilder.StartMesh( name );

			// Obj files need their winding orders swapped.
			m_meshBuilder.SwapWindingOrder = true;

			// Add additional vertex channels for texture coordinates and m_normals.
			m_textureCoordinateDataIndex = m_meshBuilder.CreateVertexChannel<Vector2>( VertexChannelNames.TextureCoordinate( 0 ) );
			m_normalDataIndex = m_meshBuilder.CreateVertexChannel<Vector3>( VertexChannelNames.Normal() );

			// Add each position to this mesh with CreatePosition.
			m_positionMap = new int[ m_positions.Count ];
			for( int i = 0 ; i < m_positions.Count ; i++ )
			{
				// m_positionsMap redirects from the original m_positions in the order they were read from file to indices returned from CreatePosition
				m_positionMap[ i ] = m_meshBuilder.CreatePosition( m_positions[ i ] );
			}
		}


		/// <summary>
		/// Finishes building a mesh and adds the resulting MeshContent or
		/// NodeContent to the root model's NodeContent.
		/// </summary>
		private void FinishMesh()
		{
			MeshContent meshContent = m_meshBuilder.FinishMesh();

			// This is a geometric mesh.
			if( meshContent.Geometry.Count > 0 )
			{
				// Add the mesh to the model
				m_rootNode.Children.Add( meshContent );
			}
			// Or, this is just a node for transformation.
			else
			{
				// Convert to a general NodeContent
				NodeContent nodeContent = new NodeContent();
				nodeContent.Name = meshContent.Name;

				// Add the transform-only node to the model
				m_rootNode.Children.Add( nodeContent );
			}

			m_meshBuilder = null;
		}

		private void AddTriangleVertex( string[] lineTokens, int vertexIndex )
		{
			// Each vertex is a set of three indices: position, texture coordinate, and normal.
			// The indices are 1-based, separated by slashes and only position is required.
			string[] indices = lineTokens[ vertexIndex ].Split( '/' );

			// Required: position.
			int positionIndex = int.Parse( indices[ 0 ], CultureInfo.InvariantCulture ) - 1;

			// Optional: texture coordinate.
			if( indices.Length > 1 )
			{
				int textureCoordinateIndex;
				Vector2 textureCoordinate;
				if( int.TryParse( indices[ 1 ], out textureCoordinateIndex ) )
				{
					textureCoordinate = m_texCoords[ textureCoordinateIndex - 1 ];
				}
				else
				{
					textureCoordinate = Vector2.Zero;
				}

				// Add to MeshBuilder.
				m_meshBuilder.SetVertexChannelData( m_textureCoordinateDataIndex, textureCoordinate );
			}

			// Optional: normal.
			if( indices.Length > 2 )
			{
				int normalIndex;
				Vector3 normal;
				if( int.TryParse( indices[ 2 ], out normalIndex ) )
				{
					normal = m_normals[ normalIndex - 1 ];
				}
				else
				{
					normal = Vector3.Zero;
				}

				// Add to MeshBuild.
				m_meshBuilder.SetVertexChannelData( m_normalDataIndex, normal );
			}

			// Add the vertex with the vertex data that was just set.
			m_meshBuilder.AddTriangleVertex( m_positionMap[ positionIndex ] );
		}


		/// <summary>
		/// Parses an .mtl file and adds all its m_materials to the m_materials collection
		/// </summary>
		/// <param name="filePath">Full path of .mtl file.</param>
		private void ImportMaterials( string filePath )
		{
			// Material library identity is tied to the file it is loaded from.
			m_mtlFileIdentity = new ContentIdentity( filePath );

			// Reset the current material
			m_currentMaterial = null;

			try
			{
				// Loop over each tokenized line of the .mtl file
				foreach( String[] lineTokens in GetLineTokens( filePath, m_mtlFileIdentity ) )
				{
					ParseMtlLine( lineTokens );
				}
			}
			catch( InvalidContentException )
			{
				// InvalidContentExceptions do not need further processing
				throw;
			}
			catch( Exception e )
			{
				// Wrap exception with content identity (includes line number)
				throw new InvalidContentException( "Unable to parse mtl file. Exception:\n" + e.Message, m_mtlFileIdentity, e );
			}

			// Finish the last material
			if( m_currentMaterial != null )
			{
				m_materials.Add( m_currentMaterial.Name, m_currentMaterial );
			}
		}


		/// <summary>
		/// Parses and executes an individual line of a .mtl file.
		/// </summary>
		/// <param name="lineTokens">Line to parse as tokens</param>
		void ParseMtlLine( string[] lineTokens )
		{
			// Switch on line type
			switch( lineTokens[ 0 ].ToLower() )
			{
				// New material
				case "newmtl":
					{
						// Finish the current material.
						if( m_currentMaterial != null )
						{
							m_materials.Add( m_currentMaterial.Name, m_currentMaterial );
						}

						// Start a new material.
						m_currentMaterial = new TomatoMaterialContent();
						m_currentMaterial.Identity = new ContentIdentity( m_mtlFileIdentity.SourceFilename );
						m_currentMaterial.Name = lineTokens[ 1 ];
					}
					break;

				// Material diffuse
				case "kd":
					{
						m_currentMaterial.DiffuseColor = ParseVector3( lineTokens );
					}
					break;

				// Diffuse texture
				case "map_kd":
					{
						// Reference a texture relative to this .mtl file
						m_currentMaterial.SetDiffuseTexture( new ExternalReference<TextureContent>( lineTokens[ 1 ], m_mtlFileIdentity ) );
					}
					break;

				// Specular texture
				case "map_ks":
					{
						// Reference a texture relative to this .mtl file
						m_currentMaterial.SetSpecularTexture( new ExternalReference<TextureContent>( lineTokens[ 1 ], m_mtlFileIdentity ) );
					}
					break;

				// Ambient texture
				case "map_ka":
					{
						// Reference a texture relative to this .mtl file
						m_currentMaterial.SetAmbientTexture( new ExternalReference<TextureContent>( lineTokens[ 1 ], m_mtlFileIdentity ) );
					}
					break;

				// Alpha texture
				case "map_d":
					{
						// Reference a texture relative to this .mtl file
						m_currentMaterial.SetAlphaTexture( new ExternalReference<TextureContent>( lineTokens[ 1 ], m_mtlFileIdentity ) );
					}
					break;

				// Bump texture
				case "map_bump":
				case "bump":
					{
						// Reference a texture relative to this .mtl file
						m_currentMaterial.SetBumpTexture( new ExternalReference<TextureContent>( lineTokens[ 1 ], m_mtlFileIdentity ) );
					}
					break;

				// Ambient color
				case "ka":
					{
						// Ignored.
					}
					break;

				// Material specular
				case "ks":
					{
						m_currentMaterial.SpecularColor = ParseVector3( lineTokens );
					}
					break;

				// Material Shineness (specular power)
				case "ns":
					{
						m_currentMaterial.SpecularPower = float.Parse( lineTokens[ 1 ], CultureInfo.InvariantCulture );
					}
					break;

				// Material emissive
				case "ke":
					{
						m_currentMaterial.EmissiveColor = ParseVector3( lineTokens );
					}
					break;

				// Material Alpha
				case "d":
				case "tr":
					{
						m_currentMaterial.Alpha = float.Parse( lineTokens[ 1 ], CultureInfo.InvariantCulture );
					}
					break;

				// Optical density
				case "ni":
					{
						m_currentMaterial.OpticalDensity = float.Parse( lineTokens[ 1 ], CultureInfo.InvariantCulture );
					}
					break;

					// Transmission Filter
				case "tf":
					{
						m_currentMaterial.TransmissionFilter = ParseVector3( lineTokens );
					}
					break;

				// Illumination mode
				//	0 = constant
				//	1 = diffuse
				//	2 = diffuse + specular
				case "illum":
					{
						m_currentMaterial.OpaqueData.Add( "IlluminationMode", int.Parse( lineTokens[ 1 ], CultureInfo.InvariantCulture ) );
					}
					break;

				// Unsupported or invalid line types
				default:
					{
						throw new InvalidContentException( "Unsupported or invalid line type '" + lineTokens[ 0 ] + "'", m_mtlFileIdentity );
					}
			}
		}


		/// <summary>
		/// Yields an array of tokens for each line in an OBJ or .mtl file.
		/// </summary>
		/// <remarks>
		/// OBJ and MTL files are text based formats of identical structure.
		/// Each line of a OBJ or .mtl file is either an instruction or a comment.
		/// Comments begin with # and are effectively ignored.
		/// Instructions are a space dilimited list of tokens. The first token is the
		/// instruction type code. The tokens which follow, are the arguments to that
		/// instruction. The number and format of arguments vary by instruction type.
		/// </remarks>
		/// <param name="filePath">Full path of file to read.</param>
		/// <param name="identity">Identity of the file being read. This is modified to
		/// include the current line number in case an exception is thrown.</param>
		/// <returns>Element 0 is the line type identifier. The remaining elements are
		/// arguments to the identifier's operation.</returns>
		private static IEnumerable<string[]> GetLineTokens( string filePath, ContentIdentity identity )
		{
			// Open the file.
			using( StreamReader reader = new StreamReader( filePath ) )
			{
				int lineNumber = 1;

				// For each line of the file
				while( !reader.EndOfStream )
				{
					// Set the line number to report in case an exception is thrown
					identity.FragmentIdentifier = lineNumber.ToString();

					// Tokenize line by splitting on 1 more more whitespace character
					string[] lineTokens = Regex.Split( reader.ReadLine().Trim(), @"\s+" );

					// Skip blank lines and comments
					if( lineTokens.Length > 0 &&
						lineTokens[ 0 ] != String.Empty &&
						!lineTokens[ 0 ].StartsWith( "#" ) )
					{
						// Pass off the tokens of this line to be processed
						yield return lineTokens;
					}

					// Done with this line!
					lineNumber++;
				}


				// Clear line number from identity
				identity.FragmentIdentifier = null;
			}
		}


		/// <summary>
		/// Parses a Vector2 from tokens of an OBJ file line.
		/// </summary>
		/// <param name="lineTokens">X and Y coordinates in lineTokens[1, 2].
		/// </param>
		private static Vector2 ParseVector2( string[] lineTokens )
		{
			return new Vector2(
				float.Parse( lineTokens[ 1 ], CultureInfo.InvariantCulture ),
				float.Parse( lineTokens[ 2 ], CultureInfo.InvariantCulture ) );
		}


		/// <summary>
		/// Parses a Vector3 from tokens of an OBJ file line.
		/// </summary>
		/// <param name="lineTokens">X,Y and Z coordinates in lineTokens[1, 3].
		/// </param>
		private static Vector3 ParseVector3( string[] lineTokens )
		{
			return new Vector3(
				float.Parse( lineTokens[ 1 ], CultureInfo.InvariantCulture ),
				float.Parse( lineTokens[ 2 ], CultureInfo.InvariantCulture ),
				float.Parse( lineTokens[ 3 ], CultureInfo.InvariantCulture ) );
		}

	}
}