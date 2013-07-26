using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics
{
	public class SceneDescriptionCameraEntry : SceneDescriptionEntry
	{
		public Vector3 EyePosition { get; set; }
		public Vector3 LookAtPosition { get; set; }
		public Vector3 UpVector { get; set; }

		public float FieldOfView { get; set; }
		public float AspectRatio { get; set; }
		public float NearClipPlane { get; set; }
		public float FarClipPlane { get; set; }

		public SceneDescriptionCameraEntry( string entryName )
			: base( entryName, EntryType.Camera )
		{
			EyePosition = Vector3.Zero;
			LookAtPosition = Vector3.Zero;
			UpVector = Vector3.Zero;

			FieldOfView = 0;
			AspectRatio = 0;
			NearClipPlane = 0;
			FarClipPlane = 0;
		}

		public SceneDescriptionCameraEntry( string entryName, Camera camera )
			: base( entryName, EntryType.Camera )
		{
			EyePosition = camera.Position;
			LookAtPosition = camera.LookAtPosition;
			UpVector = camera.UpDirection;

			FieldOfView = camera.FieldOfView;
			AspectRatio = camera.AspectRatio;
			NearClipPlane = camera.NearClipPlane;
			FarClipPlane = camera.FarClipPlane;
		}

		public override void WriteToStream( System.IO.BinaryWriter writer )
		{
			writer.Write( EyePosition.X );
			writer.Write( EyePosition.Y );
			writer.Write( EyePosition.Z );
			
			writer.Write( LookAtPosition.X );
			writer.Write( LookAtPosition.Y );
			writer.Write( LookAtPosition.Z );
			
			writer.Write( UpVector.X );
			writer.Write( UpVector.Y );
			writer.Write( UpVector.Z );
			
			writer.Write( FieldOfView );
			writer.Write( AspectRatio );
			writer.Write( NearClipPlane );
			writer.Write( FarClipPlane );
		}

		public override void ReadFromStream( System.IO.BinaryReader reader )
		{
			Vector3 value;
			value.X = reader.ReadSingle();
			value.Y = reader.ReadSingle();
			value.Z = reader.ReadSingle();
			EyePosition = value;

			value.X = reader.ReadSingle();
			value.Y = reader.ReadSingle();
			value.Z = reader.ReadSingle();
			LookAtPosition = value;

			value.X = reader.ReadSingle();
			value.Y = reader.ReadSingle();
			value.Z = reader.ReadSingle();
			UpVector = value;

			FieldOfView = reader.ReadSingle();
			AspectRatio = reader.ReadSingle();
			NearClipPlane = reader.ReadSingle();
			FarClipPlane = reader.ReadSingle();
		}
	}
}