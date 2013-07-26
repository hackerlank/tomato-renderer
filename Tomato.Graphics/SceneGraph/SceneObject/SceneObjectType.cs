using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tomato.Graphics
{
	public enum SceneObjectType
	{
		None,

		Node,
		Mesh,
		Camera,
		Light,
		ParticleSystem
	}
}