using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tomato.Graphics
{
	public class SceneDescription
	{
		private Dictionary<string, SceneDescriptionEntry> m_entries = null;

		public string Name { get; set; }

		public SceneDescription()
		{
			m_entries = new Dictionary<string, SceneDescriptionEntry>();
		}

		public void Add( SceneDescriptionEntry entry )
		{
			if( Find( entry.EntryName ) != null )
			{
				throw new InvalidOperationException( "An entry with the same name already exists in the list." );
			}

			m_entries.Add( entry.EntryName, entry );
		}

		public void Remove( string entryName )
		{
			m_entries.Remove( entryName );
		}

		public SceneDescriptionEntry Find( string entryName )
		{
			SceneDescriptionEntry entry;
			if( m_entries.TryGetValue( entryName, out entry ) )
			{
				return entry;
			}
			return null;
		}

		public static SceneDescription ReadFromFile( string filePath )
		{
			SceneDescription sceneDescription = new SceneDescription();

			BinaryReader reader = new BinaryReader( File.OpenRead( filePath ) );

			// Read name.
			sceneDescription.Name = reader.ReadString();

			// Read entries.
			int entryCount = reader.ReadInt32();
			for( int i = 0 ; i < entryCount ; ++i )
			{
				// Read entry name.
				string entryName = reader.ReadString();
				
				// Read entry type.
				SceneDescriptionEntry.EntryType entryType = ( SceneDescriptionEntry.EntryType )( reader.ReadByte() );

				SceneDescriptionEntry entry = null;
				switch( entryType )
				{
					case SceneDescriptionEntry.EntryType.Camera: { entry = new SceneDescriptionCameraEntry( entryName ); } break;
					case SceneDescriptionEntry.EntryType.Light: { entry = new SceneDescriptionLightEntry( entryName ); } break;
					case SceneDescriptionEntry.EntryType.Model: { entry = new SceneDescriptionModelEntry( entryName ); } break;
					case SceneDescriptionEntry.EntryType.XnaModel: { entry = new SceneDescriptionXnaModelEntry( entryName ); } break;
				}

				if( entry == null )
				{
					throw new InvalidOperationException( "Invalid type of entry." );
				}

				// Read entry content.
				entry.ReadFromStream( reader );

				// Add to the list.
				sceneDescription.Add( entry );
			}
			
			reader.Close();

			return sceneDescription;
		}

		public void WriteToFile( string filePath )
		{
			BinaryWriter writer = new BinaryWriter( File.OpenWrite( filePath ) );

			// Write name.
			writer.Write( Name );

			// Write entries.
			writer.Write( m_entries.Count );
			foreach( var entry in m_entries )
			{
				// Write entry name.
				writer.Write( entry.Key );

				// Write entry type.
				writer.Write( ( byte )( entry.Value.Type ) );

				// Write entry content.
				entry.Value.WriteToStream( writer );
			}

			writer.Flush();
			writer.Close();
		}

		public IEnumerable<SceneDescriptionEntry> GetEntries()
		{
			return m_entries.Values;
		}
	}
}
