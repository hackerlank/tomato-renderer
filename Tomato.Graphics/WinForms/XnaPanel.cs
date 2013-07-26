using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;

namespace Tomato.Graphics.WinForms
{
	public class XnaPanel : XnaControl
	{
		public event Action Initializing = null;
		public event Action Drawing = null;

		public XnaPanel()
		{
			SetStyle( ControlStyles.Selectable, true );
			SetStyle( ControlStyles.UserMouse, true );
			SetStyle( ControlStyles.StandardClick, false );
			SetStyle( ControlStyles.StandardDoubleClick, false );
			SetStyle( ControlStyles.SupportsTransparentBackColor, false );
			SetStyle( ControlStyles.Opaque, true );
		}

		protected override void Initialize()
		{
			if( Initializing != null )
			{
				Initializing();
			}
		}

		protected override void Draw()
		{
			if( Drawing != null )
			{
				Drawing();
			}
		}
	}
}
