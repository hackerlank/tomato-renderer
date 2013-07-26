using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics
{
	public class SceneDescriptionModelEntry : SceneDescriptionEntry
	{
		public string AssetName { get; set; }

		public Transformation Transformation { get; set; }

		public SceneDescriptionModelEntry( string entryName )
			: base( entryName, EntryType.Model )
		{
			AssetName = "";
			Transformation = Transformation.Identity;
		}

		public SceneDescriptionModelEntry( string entryName, string assetName, Transformation transformation )
			: base( entryName, EntryType.Model )
		{
			AssetName = assetName;
			Transformation = transformation;
		}

		public SceneDescriptionModelEntry( string entryName, string assetName, ref Transformation transformation )
			: base( entryName, EntryType.Model )
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