using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Tomato.Win32
{
	[StructLayout( LayoutKind.Sequential )]
	public struct SYSTEM_INFO
	{
		internal int dwOemId;
		internal int dwPageSize;
		internal IntPtr lpMinimumApplicationAddress;
		internal IntPtr lpMaximumApplicationAddress;
		internal IntPtr dwActiveProcessorMask;
		internal int dwNumberOfProcessors;
		internal int dwProcessorType;
		internal int dwAllocationGranularity;
		internal short wProcessorLevel;
		internal short wProcessorRevision;
	}
}
