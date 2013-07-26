using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Tomato.Win32
{
	[StructLayout( LayoutKind.Sequential )]
	public struct MSG
	{
		public IntPtr hWnd;
		public uint message;
		public IntPtr wParam;
		public IntPtr lParam;
		public uint time;
		public Point p;
	}
}
