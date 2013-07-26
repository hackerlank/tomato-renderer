using System;
using System.Security;
using System.Runtime.InteropServices;

namespace Tomato.Win32
{
	/// <summary>
	/// A static class for providing a set of Win32 API functions.
	/// </summary>
	/// <remarks>
	/// </remarks>
	[SuppressUnmanagedCodeSecurity]
	public sealed class NativeMethods
	{
		private NativeMethods() { }

		[return: MarshalAs( UnmanagedType.Bool )]
		[DllImport( "user32.dll" )]
		public static extern bool ClientToScreen( IntPtr hWnd, out POINT point );

		[return: MarshalAs( UnmanagedType.Bool )]
		[DllImport( "user32.dll" )]
		public static extern bool GetClientRect( IntPtr hWnd, out RECT rect );

		[return: MarshalAs( UnmanagedType.Bool )]
		[DllImport( "user32.dll" )]
		public static extern bool GetWindowRect( IntPtr hWnd, out RECT rect );

		[DllImport( "user32.dll" )]
		public static extern IntPtr MonitorFromWindow( IntPtr hWnd, uint flags );

		[return: MarshalAs( UnmanagedType.Bool )]
		[DllImport( "user32.dll", CharSet = CharSet.Auto )]
		public static extern bool PeekMessage( out MSG msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags );

		[return: MarshalAs( UnmanagedType.Bool )]
		[DllImport( "kernel32" )]
		public static extern bool QueryPerformanceCounter( ref long PerformanceCount );

		[return: MarshalAs( UnmanagedType.Bool )]
		[DllImport( "kernel32" )]
		public static extern bool QueryPerformanceFrequency( ref long PerformanceFrequency );

		[DllImport( "kernel32.dll" )]
		public static extern IntPtr GetCurrentProcess();

		[DllImport( "kernel32.dll" )]
		public static extern IntPtr GetCurrentThread();

		[return: MarshalAs( UnmanagedType.Bool )]
		[DllImport( "kernel32.dll" )]
		public static extern bool GetProcessAffinityMask( IntPtr hProcess, out UIntPtr lpProcessAffinityMask, out UIntPtr lpSystemAffinityMask );

		[DllImport( "kernel32.dll" )]
		public static extern void GetSystemInfo( ref SYSTEM_INFO lpSystemInfo );

		[DllImport( "kernel32.dll" )]
		public static extern UIntPtr SetThreadAffinityMask( IntPtr hThread, IntPtr dwThreadAffinityMask );

		[DllImport( "kernel32.dll" )]
		public static extern int SwitchToThread();

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool GetWindowPlacement( IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl );

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool SetWindowPlacement( IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl );

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern uint SetWindowLong( IntPtr hWnd, int nIndex, uint dwNewLong );

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		public static extern uint GetWindowLong( IntPtr hWnd, int nIndex );

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = false )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool IsIconic( IntPtr hWnd );

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = false )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool IsZoomed( IntPtr hWnd );

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = false )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool ShowWindow( IntPtr hWnd, int nCmdShow );

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool AdjustWindowRect( ref RECT lpRect, uint dwStyle, [MarshalAs( UnmanagedType.Bool )]bool bMenu );

		[DllImport( "kernel32.dll", CharSet = CharSet.Auto, SetLastError = false )]
		public static extern uint SetThreadExecutionState( uint esFlags );

		[DllImport( "user32.dll", CharSet = CharSet.Auto, SetLastError = true )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool SetWindowPos( IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags );

		[DllImport( "user32.dll" )]
		public static extern short GetKeyState( int nVirtKey );

		[DllImport( "user32.dll" )]
		public static extern IntPtr SetCapture( IntPtr hWnd );

		[DllImport( "user32.dll" )]
		public static extern bool ReleaseCapture();
	}
}
