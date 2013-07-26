using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Tomato.Win32;

namespace Tomato.Win32
{
	/// <summary>
	/// Provides some Win32 related functionalities.
	/// </summary>
	public static class Platform
	{
		private static DateTime s_nextProcessorCountRefreshTime = DateTime.MinValue;
		private static int s_processorCount = -1;

		/// <summary>
		/// Yield execution to another thread that is ready to run.
		/// </summary>
		public static void Yield()
		{
			NativeMethods.SwitchToThread();
		}

		
		/// <summary>
		/// </summary>
		public static bool IsSingleProcessor
		{
			get
			{
				return ( ProcessorCount == 1 );
			}
		}

		/// <summary>
		/// The number of processors available to the current process.
		/// </summary>
		/// <remarks>
		/// Original implementation: Mircrosoft Parallel Extensions library
		/// </remarks>
		public static int ProcessorCount
		{
			get
			{
				if( DateTime.UtcNow.CompareTo( s_nextProcessorCountRefreshTime ) >= 0 )
				{
					// Get processor affinity mask
					UIntPtr processAffinityMask;
					UIntPtr systemAffinityMask;
					NativeMethods.GetProcessAffinityMask( NativeMethods.GetCurrentProcess(), out processAffinityMask, out systemAffinityMask );

					// Retrieve the number of total system processor
					SYSTEM_INFO systemInfo = new SYSTEM_INFO();
					NativeMethods.GetSystemInfo( ref systemInfo );

					// Count the number of processors available to the current process
					ulong mask = ulong.MaxValue >> ( 64 - systemInfo.dwNumberOfProcessors );
					ulong bits = mask & processAffinityMask.ToUInt64();
					int processorCount = 0;
					while( bits > 0L )
					{
						processorCount++;
						bits &= ( bits - 1L );
					}
					
					// Final number of processors
					s_processorCount = processorCount;

					// Next refresh time: 30 secs
					s_nextProcessorCountRefreshTime = DateTime.UtcNow.AddMilliseconds( 30000.0 );
				}

				return s_processorCount;
			}
		}

		public static bool IsApplicationIdle
		{
			get
			{
				MSG message;
				return !NativeMethods.PeekMessage( out message, IntPtr.Zero, 0, 0, 0 );
			}
		}

		public static Rectangle GetClientRectangle( IntPtr handle )
		{
			RECT rect;
			if( !NativeMethods.GetClientRect( handle, out rect ) )
			{
				return Rectangle.Empty;
			}

			return Rectangle.FromLTRB( rect.left, rect.top, rect.right, rect.bottom );
		}

		public static Rectangle GetWindowRectangle( IntPtr handle )
		{
			RECT rect;
			if( !NativeMethods.GetWindowRect( handle, out rect ) )
			{
				return Rectangle.Empty;
			}

			return Rectangle.FromLTRB( rect.left, rect.top, rect.right, rect.bottom );
		}
	}
}

