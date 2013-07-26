using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics
{
	public class SceneDescriptionLightEntry : SceneDescriptionEntry
	{
		public Vector3 Position { get; set; }
		public Vector3 Direction { get; set; }

		public Vector3 DiffuseColor { get; set; }
		public float SpecularPower { get; set; }

		public LightType AttenuationType { get; set; }
		public float AttenuationDistance { get; set; }
		public float AttenuationDistanceExponent { get; set; }
		public float AttenuationInnerAngle { get; set; }
		public float AttenuationOuterAngle { get; set; }
		public float AttenuationAngleExponent { get; set; }

		public SceneDescriptionLightEntry()
			: this( string.Empty )
		{ 
		}

		public SceneDescriptionLightEntry( string entryName )
			: base( entryName, EntryType.Light )
		{
			Position = Vector3.Zero;
			Direction = Vector3.Zero;

			DiffuseColor = Vector3.Zero;
			SpecularPower = 0;

			AttenuationType = LightType.Directional;
			AttenuationDistance = 0;
			AttenuationDistanceExponent = 0;
			AttenuationInnerAngle = 0;
			AttenuationOuterAngle = 0;
			AttenuationAngleExponent = 0;
		}

		public SceneDescriptionLightEntry( string entryName, Light light )
			: base( entryName, EntryType.Light )
		{
			Position = light.Position;
			Direction = light.Direction;

			DiffuseColor = light.DiffuseColor;
			SpecularPower = light.SpecularPower;

			AttenuationType = light.LightType;
			AttenuationDistance = light.AttenuationDistance;
			AttenuationDistanceExponent = light.AttenuationDistanceExponent;
			AttenuationInnerAngle = light.AttenuationInnerAngle;
			AttenuationOuterAngle = light.AttenuationOuterAngle;
			AttenuationAngleExponent = light.AttenuationAngleExponent;
		}

		public static SceneDescriptionLightEntry CreateDirectionalLight(
			string entryName,
			Vector3 direction,
			Vector3 diffuseColor,
			float specularPower )
		{
			SceneDescriptionLightEntry entry = new SceneDescriptionLightEntry( entryName );
			{
				entry.Position = Vector3.Zero;
				entry.Direction = direction;
				entry.DiffuseColor = diffuseColor;
				entry.SpecularPower = specularPower;
				entry.AttenuationType = LightType.Directional;
				entry.AttenuationDistance = 0;
				entry.AttenuationDistanceExponent = 0;
				entry.AttenuationInnerAngle = 0;
				entry.AttenuationOuterAngle = 0;
				entry.AttenuationAngleExponent = 0;
			}
			return entry;
		}

		public static SceneDescriptionLightEntry CreatePointLight(
			string entryName,
			Vector3 position,
			Vector3 diffuseColor,
			float specularPower,
			float attenuationDistance,
			float attenuationDistanceExponent )
		{
			SceneDescriptionLightEntry entry = new SceneDescriptionLightEntry( entryName );
			{
				entry.Position = position;
				entry.Direction = Vector3.Zero;
				entry.DiffuseColor = diffuseColor;
				entry.SpecularPower = specularPower;
				entry.AttenuationType = LightType.Point;
				entry.AttenuationDistance = attenuationDistance;
				entry.AttenuationDistanceExponent = attenuationDistanceExponent;
				entry.AttenuationInnerAngle = 0;
				entry.AttenuationOuterAngle = 0;
				entry.AttenuationAngleExponent = 0;
			}
			return entry;
		}

		public static SceneDescriptionLightEntry CreateSpotLight(
			string entryName,
			Vector3 position,
			Vector3 direction,
			Vector3 diffuseColor,
			float specularPower,
			float attenuationDistance,
			float attenuationDistanceExponent,
			float attenuationInnerAngle,
			float attenuationOuterAngle,
			float attenuationAngleExponent )
		{
			SceneDescriptionLightEntry entry = new SceneDescriptionLightEntry( entryName );
			{
				entry.Position = position;
				entry.Direction = direction;
				entry.DiffuseColor = diffuseColor;
				entry.SpecularPower = specularPower;
				entry.AttenuationType = LightType.Spot;
				entry.AttenuationDistance = attenuationDistance;
				entry.AttenuationDistanceExponent = attenuationDistanceExponent;
				entry.AttenuationInnerAngle = attenuationInnerAngle;
				entry.AttenuationOuterAngle = attenuationOuterAngle;
				entry.AttenuationAngleExponent = attenuationAngleExponent;
			};
			return entry;
		}

		public override void WriteToStream( System.IO.BinaryWriter writer )
		{
			writer.Write( Position.X );
			writer.Write( Position.Y );
			writer.Write( Position.Z );

			writer.Write( Direction.X );
			writer.Write( Direction.Y );
			writer.Write( Direction.Z );

			writer.Write( DiffuseColor.X );
			writer.Write( DiffuseColor.Y );
			writer.Write( DiffuseColor.Z );

			writer.Write( SpecularPower );

			writer.Write( ( byte )( AttenuationType ) );
			writer.Write( AttenuationDistance );
			writer.Write( AttenuationDistanceExponent );
			writer.Write( AttenuationInnerAngle );
			writer.Write( AttenuationOuterAngle );
			writer.Write( AttenuationAngleExponent );					
		}

		public override void ReadFromStream( System.IO.BinaryReader reader )
		{
			Vector3 value;
			value.X = reader.ReadSingle();
			value.Y = reader.ReadSingle();
			value.Z = reader.ReadSingle();
			Position = value;

			value.X = reader.ReadSingle();
			value.Y = reader.ReadSingle();
			value.Z = reader.ReadSingle();
			Direction = value;

			value.X = reader.ReadSingle();
			value.Y = reader.ReadSingle();
			value.Z = reader.ReadSingle();
			DiffuseColor = value;

			SpecularPower = reader.ReadSingle();

			AttenuationType = ( LightType )( reader.ReadByte() );
			AttenuationDistance = reader.ReadSingle();
			AttenuationDistanceExponent = reader.ReadSingle();
			AttenuationInnerAngle = reader.ReadSingle();
			AttenuationOuterAngle = reader.ReadSingle();
			AttenuationAngleExponent = reader.ReadSingle();
		}
	}
}