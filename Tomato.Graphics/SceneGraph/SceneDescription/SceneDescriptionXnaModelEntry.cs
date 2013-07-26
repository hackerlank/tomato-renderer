using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics
{
	public class SceneDescriptionXnaModelEntry : SceneDescriptionEntry
	{
		public string AssetName { get; set; }

		public Transformation Transformation { get; set; }

		public SceneDescriptionXnaModelEntry( string entryName )
			: base( entryName, EntryType.XnaModel )
		{
			AssetName = "";
			Transformation = Transformation.Identity;
		}

		public SceneDescriptionXnaModelEntry( string entryName, string assetName, Transformation transformation )
			: base( entryName, EntryType.XnaModel )
		{
			AssetName = assetName;
			Transformation = transformation;
		}

		public SceneDescriptionXnaModelEntry( string entryName, string assetName, ref Transformation transformation )
			: base( entryName, EntryType.XnaModel )
		{
			AssetName = assetName;
			Transformation = transformation;
		}

		public override void WriteToStream( System.IO.BinaryWriter writer )
		{
			writer.Write( AssetName );
			Transformation.WriteToStream( writer );
		}

		public override void ReadFromStream( System.IO.BinaryReader reader )
		{
			AssetName = reader.ReadString();
			Transformation = Transformation.ReadFromStream( reader );
		}
	}
}