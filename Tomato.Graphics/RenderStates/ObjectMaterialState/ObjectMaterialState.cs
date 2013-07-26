using Microsoft.Xna.Framework;

namespace Tomato.Graphics
{
	public class ObjectMaterialState : RenderState
	{
		public Vector3 Emissive;
		public Vector3 Diffuse;
		public Vector3 Specular;
		public float Shineness;

		public ObjectMaterialState()
			: base( RenderStateType.ObjectMaterial )
		{
			Emissive = Vector3.Zero;
			Diffuse = Vector3.Zero;
			Specular = Vector3.Zero;
			Shineness = 0.0f;
		}

		public ObjectMaterialState( ref Vector3 emissive, ref Vector3 diffuse, ref Vector3 specular, float shineness )
			: base( RenderStateType.ObjectMaterial )
		{
			Emissive = emissive;
			Diffuse = diffuse;
			Specular = specular;
			Shineness = shineness;
		}

		public ObjectMaterialState( Vector3 emissive, Vector3 diffuse, Vector3 specular, float shineness )
			: base( RenderStateType.ObjectMaterial )
		{
			Emissive = emissive;
			Diffuse = diffuse;
			Specular = specular;
			Shineness = shineness;
		}

		public override object Clone()
		{
			return new ObjectMaterialState
			{
				Emissive = this.Emissive,
				Diffuse = this.Diffuse,
				Specular = this.Specular,
				Shineness = this.Shineness
			};
		}
	}
}