using System;
using System.Runtime.InteropServices;

namespace Tomato.Win32
{
	[StructLayout( LayoutKind.Sequential )]
	public struct POINT
	{
		public int x;
		public int y;
	}
}
