using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tomato.Win32
{
	public static class Win32Macro
	{
		public static ushort LOWORD( IntPtr value )
		{
			return ( ushort )( ( ( int )value & 0xFFFF ) );
		}

		public static ushort HIWORD( IntPtr value )
		{
			return ( ushort )( ( ( int )value >> 16 ) );
		}

		public static int GET_X_LPARAM( IntPtr lParam )
		{
			return ( int )( ( short )( LOWORD( lParam ) ) );
		}

		public static int GET_Y_LPARAM( IntPtr lParam )
		{
			return ( int )( ( short )( HIWORD( lParam ) ) );
		}
	}
}
