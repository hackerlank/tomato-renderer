using System;

namespace Tomato.Graphics
{
	public abstract class RenderState : IEquatable<RenderState>, ICloneable
	{
		public RenderStateType Type { get; private set; }

		internal RenderState( RenderStateType type )
		{
			Type = type;
		}

		public virtual bool Equals( RenderState other )
		{
			if( Type == other.Type )
			{
				return true;
			}

			return false;
		}

		public virtual byte[] GetStateBits()
		{
			return null;
		}

		public abstract object Clone();
	}
}
