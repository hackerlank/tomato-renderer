using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Tomato.Win32
{
	[StructLayout( LayoutKind.Sequential )]
	public struct WINDOWPLACEMENT
	{
		public int length;
		public int flags;
		public int showCmd;
		public Point ptMinPosition;
		public Point ptMaxPosition;
		public RECT rcNormalPosition;

		public static int Length
		{
			get { return Marshal.SizeOf( typeof( WINDOWPLACEMENT ) ); }
		}
	}
}
