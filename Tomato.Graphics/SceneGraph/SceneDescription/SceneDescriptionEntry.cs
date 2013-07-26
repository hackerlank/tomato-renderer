using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Tomato.Graphics
{
	public class SceneDescriptionEntry
	{
		public enum EntryType
		{
			None,
			Model,
			XnaModel,
			Light,
			Camera
		}

		public string EntryName { get; private set; }

		public EntryType Type { get; private set; }

		public SceneDescriptionEntry( EntryType type )
		{
			EntryName = string.Empty;
			Type = type;
		}

		public SceneDescriptionEntry( string entryName, EntryType type )
		{
			EntryName = entryName;
			Type = type;
		}

		public virtual void WriteToStream( BinaryWriter writer )
		{
		}

		public virtual void ReadFromStream( BinaryReader reader )
		{
		}
	}
}